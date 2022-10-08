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
    sqlalchemy.Column('id', sqlalchemy.String, primary_key=True),
    sqlalchemy.Column('shorttext', sqlalchemy.String, nullable=False),
    sqlalchemy.Column('views', sqlalchemy.String, nullable=True),
    sqlalchemy.Column('sourceurl', sqlalchemy.String, nullable=True),
    sqlalchemy.Column('date', sqlalchemy.DateTime, nullable=True),
    sqlalchemy.Column('sourceid', sqlalchemy.ForeignKey("newssource.id")))

roles = sqlalchemy.Table('roles', metadata,
    sqlalchemy.Column('id', sqlalchemy.String, primary_key=True),
    sqlalchemy.Column('name', sqlalchemy.String, nullable=True))

users = sqlalchemy.Table('users', metadata,
    sqlalchemy.Column('userid', sqlalchemy.Integer, primary_key=True),
    sqlalchemy.Column('role', sqlalchemy.ForeignKey("roles.id")))

reaction = sqlalchemy.Table('reaction', metadata,
    sqlalchemy.Column('id', sqlalchemy.Integer),
    sqlalchemy.Column('name', sqlalchemy.Text))

user_reaction = sqlalchemy.Table('userreactions', metadata,
     sqlalchemy.Column('userid', sqlalchemy.ForeignKey('users.id')),
     sqlalchemy.Column('newsid', sqlalchemy.ForeignKey('news.id')),
     sqlalchemy.Column('reaction', sqlalchemy.ForeignKey('reaction.id')))

class Role(BaseModel):
    name: str

class RoleDb(BaseModel):
    id: int
    name: str

class User(BaseModel):
    userid: int
    role: int

class UserReaction(BaseModel):
    userid: int
    newsid: str
    reaction: int

class Digest(BaseModel):
    userid: int
    type: int
