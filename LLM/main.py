from tools.document_search import DocumentSearchTool
from openai import OpenAI
import traceback
from transformers import pipeline
import datetime

# Initialize the OpenAI client outside of the function to avoid re-initializing it on every call
client = OpenAI(api_key="<insert_openai_api_token>")

VERBOSE = True
MAX_ITERATIONS = 3
FILE_PATH = r"C:\Users\gmful\Downloads\senior_design\results\SnakeGameLog_20240408_213935.txt"

IS_CSHARP_CODE = False
if IS_CSHARP_CODE:
    base_query = "Here is the c sharp code that is used to run, play, and train an ml-agent on the game Snake. "\
            "What changes would you suggest making to this code in order to make the game more difficult, "\
            "while also updating the ml-agent to learn this new feature?"
else:
    base_query = "Here is a text file containing the states of a Snake game as it was played. "\
            "What suggestions would you make to the player to increase their efficiency and score?"

document_search_tool = DocumentSearchTool(file_path=FILE_PATH)
document_search_results = document_search_tool.document_search(question=base_query)
enhanced_query = f"{base_query}\n\n{document_search_results}"

def generate_code(user_prompt, max_tokens=100, model_choice='openai'):
    """
    Generate Python code based on the user prompt using the selected model.
    """
    instruction = ("Please write Python code that fulfills the following instructions. "
                   "Ignore documentation and ignore generating example uses. "
                   "Focus on completing the task within the token limit.\n\n")
    full_prompt = instruction + user_prompt
    if model_choice == 'openai':
        response = client.chat.completions.create(
            model="gpt-3.5-turbo",
            messages=[{"role": "user", "content": full_prompt}],
            max_tokens=max_tokens,
            temperature=0.7
        )
        generated_code = response.choices[0].message.content
        return generated_code
    elif model_choice == 'huggingface':
        codegen = pipeline("text-generation", model='your_huggingface_model_name', tokenizer='your_huggingface_model_name')
        response = codegen(full_prompt, max_length=max_tokens + len(user_prompt), num_return_sequences=1)
        generated_code = response[0]['generated_text'].strip()
        return generated_code

def write_to_file(content):
    """
    Writes the given content to a .py file named with the current date and time.
    """
    timestamp = datetime.datetime.now().strftime("%Y%m%d_%H%M%S")
    file_name = f"generated_code_{timestamp}.py"
    with open(file_name, "w") as file:
        file.write(content)
    print(f"Generated code saved to {file_name}")
    return file_name

def calculate_cost(num_tokens):
    token_price = 0.002
    cost = num_tokens * token_price
    return cost

def test_python_file(python_file_path):
    """
    Executes the specified Python file and captures any exceptions.
    """
    try:
        with open(python_file_path, "r") as file:
            code = file.read()
            exec(code, globals())
        return "The file executed successfully."
    except Exception as e:
        return f"An exception occurred:\n{traceback.format_exc()}"

max_tokens = 1000
model_choice = 'openai'

generated_code = generate_code(enhanced_query, max_tokens, model_choice)
file_name = write_to_file(generated_code)
cost = calculate_cost(max_tokens)  # Assuming the whole token limit was used for pricing
print(f"This interaction cost ${cost:.2f}")

# Execute the generated Python file and handle exceptions
execution_result = test_python_file(file_name)
print(execution_result)
