from aiogram import types
from aiogram.dispatcher.filters import Text
from anabot.keyboards.inline.callback_datas import continue_callback, reaction_callback
from anabot.keyboards.inline.reaction_buttons import create_reaction_keyboard
from anabot.loader import dp


@dp.message_handler(Text(equals=["Дневной дайджест"]), state=None)
async def send_news(message: types.Message):
    await message.answer("<b>Article Title 1</b>", reply_markup=create_reaction_keyboard("www.google.com", 1, 929751))
    await message.answer("<b>Article Title 2</b>", reply_markup=create_reaction_keyboard("www.google.com", 2, 929752))
    await message.answer(
        "<b>Article Title 3</b>",
        reply_markup=create_reaction_keyboard("www.google.com", 3, 929753, continue_button=True),
    )
    print("Запрос дневного дайджеста")


@dp.callback_query_handler(reaction_callback.filter(reaction=["like", "dislike"]))
async def save_reaction(call: types.CallbackQuery, callback_data: dict):
    await call.answer(cache_time=15)
    reaction_bool = True if callback_data["reaction"] == "like" else False
    await call.message.edit_reply_markup(
        create_reaction_keyboard(
            callback_data["link"],
            int(callback_data["article_number"]),
            int(callback_data["news_id"]),
            reaction=reaction_bool,
            continue_button=bool(int(callback_data["continue_button"])),
        )
    )
    print(callback_data)
    # save like/dislike


# @dp.callback_query_handler(view_callback.filter(article_number=['1', '2', '3']))
# async def save_view(call: types.CallbackQuery, callback_data: dict):
#     # await call.answer(url="https://www.google.com/", cache_time=15)
#     # await call.answer(text=text, show_alert=True)
#     print(callback_data)
#     # save view


@dp.callback_query_handler(continue_callback.filter(article_number=["1", "2", "3"]))
async def continue_news(call: types.CallbackQuery, callback_data: dict):
    await call.answer(cache_time=15)
    if callback_data["reaction"] == "None":
        reaction = None
    elif callback_data["reaction"] == "True":
        reaction = True
    else:
        reaction = False
    await call.message.edit_reply_markup(
        create_reaction_keyboard(
            callback_data["link"],
            int(callback_data["article_number"]),
            int(callback_data["news_id"]),
            reaction=reaction,
        )
    )
    await call.message.answer(
        "<b>Article Title 4</b>", reply_markup=create_reaction_keyboard("www.google.com", 1, news_id=1111)
    )
    await call.message.answer(
        "<b>Article Title 5</b>", reply_markup=create_reaction_keyboard("www.google.com", 2, news_id=1111)
    )
    await call.message.answer(
        "<b>Article Title 6</b>",
        reply_markup=create_reaction_keyboard("www.google.com", 3, news_id=1111, continue_button=True),
    )
    print(callback_data)
