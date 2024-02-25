from tools.document_search import DocumentSearchTool

VERBOSE = True
MAX_ITERATIONS = 3
FILE_PATH = ""  # Input the file path you want the LLM to help analyze
IS_CSHARP_CODE = False  # Boolean for whether to analyze the c# code or the Snake .txt game states

if IS_CSHARP_CODE:
    query = "Here is the c sharp code that is used to run, play, and train an ml-agent on the game Snake. " \
            "What changes would you suggest making to this code in order to make the game more difficult, " \
            "while also updating the ml-agent to learn this new feature?"
else:
    query = "Here is a text file containing the states of a Snake game as it was played. " \
            "What suggestions would you make to the player to increase there efficiency and score?"

document_search_tool = DocumentSearchTool(file_path=FILE_PATH)
print(document_search_tool.document_search())
