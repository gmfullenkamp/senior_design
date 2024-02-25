"""
This module is for the OpenAIModel class.
"""
from langchain_community.embeddings import OpenAIEmbeddings
from langchain_community.llms import OpenAI

from api_keys import OPENAI_API_KEY
from models.base_model import BaseModel


class OpenAIModel(BaseModel):
    """
    This class allows for loading of OpenAI's model.
    """
    
    def download(self):
        raise NotImplementedError("OpenAI's langchain implementation doesn't "
                                  "have a model to download.")

    def load(self):
        """Loads the model into langchain's LLM format."""
        print("Loading OpenAI model...")
        # Load the model from openai, be sure to set the api key
        llm = OpenAI(openai_api_key=OPENAI_API_KEY)
        return llm

    def load_embeddings(self):
        """Loads the model embeddings into langchain's embeddings format."""
        # Load the model from openai, be sure to set the api key
        print("Loading OpenAI embeddings...")
        embeddings = OpenAIEmbeddings(openai_api_key=OPENAI_API_KEY)
        return embeddings
