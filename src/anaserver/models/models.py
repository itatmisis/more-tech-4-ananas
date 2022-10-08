import sqlalchemy
from sqlalchemy.dialects.postgresql import UUID
from pydantic import BaseModel

metadata = sqlalchemy.MetaData()

news_source = sqlalchemy.Table(
    "newssource",
    metadata,
    sqlalchemy.Column("id", sqlalchemy.Integer, primary_key=True),
    sqlalchemy.Column("name", sqlalchemy.String, nullable=False),
    sqlalchemy.Column("type", sqlalchemy.String, nullable=False),
    sqlalchemy.Column("isactive", sqlalchemy.Boolean, nullable=False),
)


news = sqlalchemy.Table(
    "news",
    metadata,
    sqlalchemy.Column("id", UUID, primary_key=True),
    sqlalchemy.Column("shorttext", sqlalchemy.String, nullable=False),
    sqlalchemy.Column("views", sqlalchemy.String, nullable=True),
    sqlalchemy.Column("sourceurl", sqlalchemy.String, nullable=True),
    sqlalchemy.Column("date", sqlalchemy.DateTime, nullable=True),
    sqlalchemy.Column("sourceid", sqlalchemy.ForeignKey("newssource.id")),
)

roles = sqlalchemy.Table(
    "roles",
    metadata,
    sqlalchemy.Column("id", sqlalchemy.Integer, primary_key=True),
    sqlalchemy.Column("name", sqlalchemy.String, nullable=True),
)

users = sqlalchemy.Table(
    "users",
    metadata,
    sqlalchemy.Column("userid", sqlalchemy.Integer, primary_key=True),
    sqlalchemy.Column("role", sqlalchemy.ForeignKey("roles.id")),
)


class Role(BaseModel):
    name: str


class RoleDb(BaseModel):
    id: int
    name: str


class User(BaseModel):
    userid: int
    role: int