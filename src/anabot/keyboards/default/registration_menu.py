from aiogram.types import KeyboardButton, ReplyKeyboardMarkup

registration_menu = ReplyKeyboardMarkup(
    keyboard=[[KeyboardButton(text="/registration")]],
    resize_keyboard=True,
)

role_selection_menu = ReplyKeyboardMarkup(
    keyboard=[[KeyboardButton(text="Владелец-бизнеса👎"), KeyboardButton(text="Бухгалтер👎")]],
    resize_keyboard=True,
)

source_selection_menu = ReplyKeyboardMarkup(
    keyboard=[
        [
            KeyboardButton(text="Продолжить"),
        ],
        [
            KeyboardButton(text="/sources"),
        ],
    ],
    resize_keyboard=True,
)

time_selection_menu = ReplyKeyboardMarkup(
    keyboard=[
        [
            KeyboardButton(text="Завершить"),
        ],
        [
            KeyboardButton(text="/time"),
        ],
    ],
    resize_keyboard=True,
)
