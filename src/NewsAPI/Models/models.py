import sqlalchemy
from fastapi import FastAPI
from pydantic import BaseModel
import uuid

metadata = sqlalchemy.MetaData()

news_source = sqlalchemy.Table('newssource', metadata,
    sqlalchemy.Column('id', sqlalchemy.Integer(), primary_key=True),
    sqlalchemy.Column('name', sqlalchemy.String, nullable=False),
    sqlalchemy.Column('type', sqlalchemy.String, nullable=False),
    sqlalchemy.Column('isactive', sqlalchemy.Boolean, nullable=False)
)


news = sqlalchemy.Table('news', metadata,
    sqlalchemy.Column('id', uuid, primary_key=True),
    sqlalchemy.Column('shorttext', sqlalchemy.String, nullable=False),
    sqlalchemy.Column('views', sqlalchemy.String, nullable=True),
    sqlalchemy.Column('sourceurl', sqlalchemy.String, nullable=True),
    sqlalchemy.Column('date', sqlalchemy.DateTime, nullable=True),
    sqlalchemy.Column('sourceid', sqlalchemy.ForeignKey("newssource.id")))
