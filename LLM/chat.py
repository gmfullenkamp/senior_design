"""
This script is for chatting with a model that has access to memory and tools.
"""
import re
from typing import List, Union
from langchain.memory import ConversationBufferWindowMemory
from langchain.agents import Tool, AgentExecutor, LLMSingleActionAgent, AgentOutputParser
from langchain.prompts import StringPromptTemplate
from langchain import LLMChain
from langchain.schema import AgentAction, AgentFinish

from utils.get_custom_objects import get_custom_objects
# pylint: disable=wildcard-import
from configs.chat_opbd import *
if MODE == "Speech":
    from utils import voice_recognition

# Define which tools the agent can use to answer user queries
print(f"Initializing tools: {TOOL_SETTINGS}")
tools = get_custom_objects(settings=TOOL_SETTINGS)

# Set up the base template and force_words for the model output
TEMPLATE_WITH_HISTORY = """Answer the following questions as best you can.
You have access to the following tools:

{tools}

Use the following as examples:
----------
Question: How many samples are in the database?
Thought: I don't know the answer to this question. I'll use the PandasDatabase tool to find the answer.
Action: PandasDatabaseTool
Action Input: How many samples are in the database?
Observation: There are 9999 samples in the database.
Thought: I now know the final answer
Final Answer: There are 9999 samples in the database.
----------

Now begin to do this structured reasoning on your own!

Previous conversation history:
{history}

----------
Question: {input}
{agent_scratchpad}"""

# Load the langchain llm
llm = get_custom_objects(settings=MODEL_SETTINGS)[0].load()


# Set up a prompt template
class CustomPromptTemplate(StringPromptTemplate):
    """
    A custom prompt template built off of langchain.
    """
    # The template to use
    template: str
    # The list of tools available
    tools: List[Tool]

    def format(self, **kwargs) -> str:
        # Get the intermediate steps (AgentAction, Observation tuples)
        # Format them in a particular way
        intermediate_steps = kwargs.pop("intermediate_steps")
        thoughts = ""
        for action, observation in intermediate_steps:
            thoughts += action.log
            thoughts += f"\nObservation: {observation}\nThought: "
        # Set the agent_scratchpad variable to that value
        kwargs["agent_scratchpad"] = thoughts
        # Create a tools variable from the list of tools provided
        kwargs["tools"] = "\n".join([f"{tool.name}: {tool.description}" for tool in self.tools])
        # Create a list of tool names for the tools provided
        kwargs["tool_names"] = ", ".join([tool.name for tool in self.tools])
        return self.template.format(**kwargs)


prompt_with_history = CustomPromptTemplate(
    template=TEMPLATE_WITH_HISTORY,
    tools=tools,
    # This omits the `agent_scratchpad`, `tools`, and `tool_names` variables because those are
    # generated dynamically. This includes the `intermediate_steps` variable because that is needed
    input_variables=["input", "intermediate_steps", "history"]
)


class CustomOutputParser(AgentOutputParser):
    """
    A custom output parser built off of langchain and molded to work with the
    custom prompt template class.
    """
    def parse(self, text: str) -> Union[AgentAction, AgentFinish]:
        # Check if agent should finish
        if "Final Answer:" in text:
            return AgentFinish(
                # Return values is generally always a dictionary with a single `output` key
                # It is not recommended to try anything else at the moment :)
                return_values={"output": text.split("Final Answer:")[-1]
                                                   .split("----------")[0].strip()},
                log=text,
            )
        # Parse out the action and action input
        regex = r"Action\s*\d*\s*:(.*?)\nAction\s*\d*\s*Input\s*\d*\s*:[\s]*(.*)"
        match = re.search(regex, text, re.DOTALL)
        if not match:
            raise ValueError(f"Could not parse LLM output: `{text}`")
        action = match.group(1).strip()
        action_input = match.group(2)
        # Return the action and action input
        return AgentAction(tool=action,
                           tool_input=action_input.strip(" ").strip('"'),
                           log=text)


output_parser = CustomOutputParser()

# LLM chain consisting of the LLM and a prompt
llm_chain = LLMChain(llm=llm, prompt=prompt_with_history)
agent = LLMSingleActionAgent(llm_chain=llm_chain,
                             output_parser=output_parser,
                             stop=["\nObservation:"],
                             allowed_tools=TOOL_SETTINGS.keys())

memory = ConversationBufferWindowMemory(k=K)

agent_executor = AgentExecutor.from_agent_and_tools(agent=agent,
                                                    tools=tools,
                                                    verbose=VERBOSE,
                                                    memory=memory,
                                                    max_iterations=MAX_ITERATIONS)

# Chat functions both modes
def speech_chat():
    """Runs the chat-bot in speech mode"""
    try:
        while True:
            recognizer = voice_recognition.sr.Recognizer()
            mic = voice_recognition.sr.Microphone(device_index=0)
            # listen to the user's speech and convert it to text
            with mic as source:
                print("Listening...")
                audio = recognizer.listen(source)
            try:
                user_input = recognizer.recognize_google(audio)
                # display user input
                print(f"User: {user_input}")
            except voice_recognition.sr.UnknownValueError:
                voice_recognition.speak("I'm sorry, I didn't understand that.")
                print("I'm sorry, I didn't understand that.")
                continue
            output = agent_executor.run(user_input)
            voice_recognition.speak(output)
            print(output)
    except AssertionError:
        print("Chat-bot: Sorry, unable to detect your microphone! Switching to text mode...")
        text_chat()

def text_chat():
    """Runs the chat-bot in text mode"""
    while True:
        user_input = input("User: ")
        output = agent_executor.run(user_input)
        print(f"Chat-bot: {output}")

# Chat time
if MODE == "Speech":
    speech_chat()
elif MODE == "Text":
    text_chat()
