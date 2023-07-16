from imports import *
from pydantic import BaseModel
from enum import Enum

class DocumentData(BaseModel):
    email: str = "foo@bar.com"
    telephone: str = "555-555 55 55"

class RequestModel(BaseModel):
    document: DocumentData 

class AnyRequestModel(RequestModel):
    template: dict

class FExtension(str, Enum):
    pdf = "pdf"
    zip = "zip"