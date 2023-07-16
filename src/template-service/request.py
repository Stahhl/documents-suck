from imports import *
from pydantic import BaseModel
from enum import Enum

class DocumentData(BaseModel):
    email: str = "foo@bar.com"
    telephone: str = "555-555 55 55"

class RequestModel(BaseModel):
    document: DocumentData 

class Item(BaseModel):
    document: DocumentData
    template: dict

class FExtension(str, Enum):
    html = "html"
    latex = "latex"
    pdf = "pdf"
    zip = "zip"