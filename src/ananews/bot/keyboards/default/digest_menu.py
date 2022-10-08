from aiogram.types import ReplyKeyboardMarkup, KeyboardButton


digest_menu = ReplyKeyboardMarkup(
    keyboard=[
        [
            KeyboardButton(text="Дневной дайджест"),
        ],
        [
            KeyboardButton(text="Недельный дайджест"),
            KeyboardButton(text="Месячный дайджест"),
        ]
    ],
    resize_keyboard=True,
)