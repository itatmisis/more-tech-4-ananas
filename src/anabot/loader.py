from aiogram import Bot, Dispatcher, types
from aiogram.contrib.fsm_storage.memory import MemoryStorage
from anabot.data import config
import anabot.handlers as handlers
from anabot.middlewares import setup_middleware

bot = Bot(token=config.BOT_TOKEN, parse_mode=types.ParseMode.HTML)
storage = MemoryStorage()
dp = Dispatcher(bot, storage=storage)
setup_middleware(dp)
handlers.setup_handlers(dp)
