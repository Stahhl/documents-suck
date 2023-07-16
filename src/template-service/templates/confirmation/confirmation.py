from imports import *

class TemplateData(BaseModel):
    name: str
    condition: bool

class Confirmation(RequestModel):
    template: TemplateData