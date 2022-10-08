from aiogram.types import KeyboardButton, ReplyKeyboardMarkup

registration_menu = ReplyKeyboardMarkup(
    keyboard=[[KeyboardButton(text="Регистрация")]],
    resize_keyboard=True,
)

role_selection_menu = ReplyKeyboardMarkup(
    keyboard=[[KeyboardButton(text="Генеральный директор"), KeyboardButton(text="Бухгалтер")]],
    resize_keyboard=True,
)
