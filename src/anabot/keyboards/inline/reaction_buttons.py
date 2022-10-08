from aiogram.types import InlineKeyboardButton, InlineKeyboardMarkup
from keyboards.inline.callback_datas import continue_callback, reaction_callback


def create_reaction_keyboard(link, article_number, news_id, reaction=None, continue_button=False):
    if reaction is None:
        like_text = "👍"
        dislike_text = "👎"
    else:
        like_text = "✅ " * int(reaction) + "👍"
        dislike_text = "✅ " * int(not reaction) + "👎"
    reaction_keyboard = InlineKeyboardMarkup(
        inline_keyboard=[
            [
                InlineKeyboardButton(text="Читать статью", url=link),
            ],
            [
                InlineKeyboardButton(
                    text=like_text,
                    callback_data=reaction_callback.new(
                        reaction="like",
                        article_number=article_number,
                        news_id=news_id,
                        continue_button=int(continue_button),
                        link=link,
                    ),
                ),
                InlineKeyboardButton(
                    text=dislike_text,
                    callback_data=reaction_callback.new(
                        reaction="dislike",
                        article_number=article_number,
                        news_id=news_id,
                        continue_button=int(continue_button),
                        link=link,
                    ),
                ),
            ],
        ]
    )
    if continue_button:
        reaction_keyboard.add(
            InlineKeyboardButton(
                text="Продолжить",
                callback_data=continue_callback.new(
                    reaction=str(reaction), article_number=article_number, news_id=news_id, link=link
                ),
            )
        )
    return reaction_keyboard
