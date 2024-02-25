from abc import ABC, abstractmethod


class BaseModel(ABC):
    """Abstract class to hold Langchain models."""
    @abstractmethod
    def download(self):
        """Downloads the model if the path doesn't already exist."""

    @abstractmethod
    def load(self):
        """Loads the model into langchain's LLM format."""

    @abstractmethod
    def load_embeddings(self):
        """Loads the model embeddings into langchain's embedding format."""
