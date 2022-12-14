from typing import List

from pydantic import BaseModel


class NewsEmbeddingBase(BaseModel):
    embedding: List[float]
    cluster_id: int


class NewsEmbedding(NewsEmbeddingBase):
    id: int

    class Config:
        orm_mode = True
