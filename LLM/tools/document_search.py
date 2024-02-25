from langchain.indexes import VectorstoreIndexCreator
from langchain.document_loaders import CSVLoader, UnstructuredWordDocumentLoader, \
    UnstructuredHTMLLoader, UnstructuredMarkdownLoader, PDFMinerLoader, \
    UnstructuredPowerPointLoader, TextLoader, PythonLoader
    
from models.openai_model import OpenAIModel

# Map file extensions to document loaders and their arguments
LOADER_MAPPING = {
        ".csv": (CSVLoader, {}),
        # ".docx": (Docx2txtLoader, {}),
        ".doc": (UnstructuredWordDocumentLoader, {}),
        ".docx": (UnstructuredWordDocumentLoader, {}),
        ".html": (UnstructuredHTMLLoader, {}),
        ".md": (UnstructuredMarkdownLoader, {}),
        ".pdf": (PDFMinerLoader, {}),
        ".ppt": (UnstructuredPowerPointLoader, {}),
        ".pptx": (UnstructuredPowerPointLoader, {}),
        ".txt": (TextLoader, {"encoding": "utf8"}),
        ".py": (PythonLoader, {})
        # Add more mappings for other file extensions and loaders as needed
        }


class DocumentSearchTool:
    def __init__(self, file_path) -> None:
        self.file_path = file_path
        self.index_query, self.model = self.setup()

    def load_single_document(self, file_path: str):
        """Loads the file path into a document loader."""
        ext = "." + file_path.rsplit(".", 1)[-1]
        if ext in LOADER_MAPPING:
            loader_class, loader_args = LOADER_MAPPING[ext]
            return loader_class(file_path, **loader_args)
        return ValueError(f"Unsuppoerted file extension '{ext}'.")

    def setup(self):
        """
        Sets up the vectorstore from the list of files from a repository, embeddings, and model.
        """
        # Loads the documents
        loaders = [self.load_single_document(self.file_path)]
        print(f"Loaded document '{self.file_path}'.")
        # Loads the model and embeddings
        embeddings = OpenAIModel().load_embeddings()
        llm = OpenAIModel().load()
        print("Creating embeddings...")
        index_creator = VectorstoreIndexCreator(embedding=embeddings)
        index = index_creator.from_loaders(loaders)
        return index.query_with_sources, llm

    def document_search(self, question: str):
        """Retrieves the answer given the index, llm, and question."""
        return self.index_query(question=question, llm=self.model)
