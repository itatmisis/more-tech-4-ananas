from nlp import SbertWrapper
from sklearn.linear_model import LogisticRegression
from sklearn.decomposition import TruncatedSVD
import numpy as np
import pickle


class ItemPipeline:
    def __init__(
            self,
            classifier=LogisticRegression(),
            trunc_svd=TruncatedSVD,
            emb_dim=256,
            sbert=SbertWrapper()
    ):
        super().__init__()
        self.classifier = classifier
        self.emb_dim = emb_dim
        self.sbert = sbert
        self.trunc_svd = trunc_svd

    def forward(self, news_text, news_id=0, news_views=0, news_source_id=0):

        news_emb = self.get_embedding(news_text)
        news_features = np.array(list(news_emb) + [news_views, news_source_id])
        news_role = self.classifier.predict_proba(news_features)[:, 1]
        return news_emb, [news_id, news_source_id, news_role]
   

    def get_embedding(self, text):

        sbert_emb = self.sbert.get_embedding(text)
        svd_emb = self.trunc_svd(n_components=self.emb_dim).fit_transform(sbert_emb)
        return svd_emb

    
    def save_model(self, path_to_model):
        with open(path_to_model, "wb") as f:
            pickle.dump(self, f)

            
    @staticmethod
    def load_model(path_to_model):
        if not path_to_model.endswith("pkl"):
            raise ValueError("Model extension must be .pkl")

        with open(f"{path_to_model}", "rb") as f:
            model = pickle.load(file=f)

        return model
