from aiogram import executor

import anabot.handlers as handlers
from anabot.loader import dp
from anabot.middlewares import setup_middleware
from anabot.utils.notify_admins import on_startup_notify
from anabot.utils.set_bot_commands import set_default_commands


async def on_startup(dispatcher):
    # Устанавливаем дефолтные команды
    await set_default_commands(dispatcher)

    # Уведомляет про запуск
    await on_startup_notify(dispatcher)


def start():
    setup_middleware(dp)
    handlers.setup_handlers(dp)
    executor.start_polling(dp, on_startup=on_startup)


if __name__ == "__main__":
    start()
