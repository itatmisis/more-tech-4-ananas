from sqlalchemy import Column
from sqlalchemy import String
from sqlalchemy import Integer
from fastapi_asyncalchemy.db.base import Base

class NewsSource(Base):
    __tablename__ = "cities"

    id = Column(Integer, autoincrement=True, primary_key=True, index=True)
    name = Column(String, unique=True)
    population = Column(Integer)

news_source = Table('newssource', metadata,
    Column('id', Integer(), primary_key=True),
    Column('name', String, nullable=False),
    Column('type', String, nullable=False),
    Column('isactive', Boolean, nullable=False)
)

news = Table('news', metadata,
    Column('id', uuid, primary_key=True),
    Column('shorttext', String, nullable=False),
    Column('views', String, nullable=True),
    Column('sourceurl', String, nullable=True),
    Column('date', datetime, nullable=True),
    Column('sourceid', ForeignKey("newssource.id")))
