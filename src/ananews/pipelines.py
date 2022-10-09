import os
from typing import Tuple, Union

import joblib
import numpy as np
from sklearn.decomposition import TruncatedSVD

from ananews.nlp import BertWrapper


class ItemPipeline:
    def __init__(
        self,
        classifier: Union[LogisticRegression, Union[os.PathLike, str]],
        trunc_svd: Union[TruncatedSVD, Union[os.PathLike, str]],
        nlp_model: BertWrapper,
    ):
        super().__init__()
        if isinstance(trunc_svd, str) or isinstance(trunc_svd, os.PathLike):
            self.trunc_svd = joblib.load(trunc_svd)
        else:
            self.trunc_svd = trunc_svd
        self.nlp_model = nlp_model

    def forward(
        self, news_text: str, news_id: int, news_views: int, news_source_id: int
    ) -> Tuple[np.ndarray, Tuple[int, int, int]]:
        """
        :param news_text: текст новости
        :param news_id: id новости
        :param news_views: количество просмотров новости
        :param news_source_id: id источника новости
        :return: векторное представление новости, (id новости, id источника, предсказанная роль)
        """
        news_emb = self.get_embedding(news_text)
        return news_emb, (news_id, news_source_id)

    def get_embedding(self, text: str) -> np.ndarray:
        sbert_emb = self.nlp_model.get_embedding(text)
        svd_emb = self.trunc_svd.transform(sbert_emb)
        return svd_emb

    @staticmethod
    def save_model(model, path_to_model: Union[os.PathLike, str]):
        joblib.dump(model, path_to_model)

    @staticmethod
    def load_model(path_to_model: Union[os.PathLike, str]) -> "ItemPipeline":
        model = joblib.load(path_to_model)
        return model
