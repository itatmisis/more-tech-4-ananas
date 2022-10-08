from aiogram import types
from aiogram.dispatcher.filters.builtin import CommandStart
from anabot.keyboards.default import registration_menu
from anabot.loader import dp


@dp.message_handler(CommandStart())
async def bot_start(message: types.Message):
    await message.answer(
        f"Привет, {message.from_user.full_name}!\n\n"
        f"Я - бот Pineapples News\n\n"
        f"Я буду рекомендовать тебе новости в зависимости от "
        f"твоих предпочтений\n"
        f"Для того, чтобы я понял, какие новости тебе "
        f"интересны - тебе надо указать данные о себе\n"
        f"Для этого просто напиши:\nРегистрация",
        reply_markup=registration_menu.registration_menu,
    )
