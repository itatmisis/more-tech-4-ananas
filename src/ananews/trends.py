from sklearn.cluster import MiniBatchKMeans
import numpy as np

class Trends:
    def __init__(self, n_clusters=5, emb_data=None, id_data=None):
        self.n_clusters = n_clusters
        self.emb_data = emb_data
        self.id_data = id_data
        self.clusterer = MiniBatchKMeans(n_clusters=self.n_clusters).fit(self.emb_data)
        self.labels = dict(zip(self.id_data, self.clusterer.labels_))

    def get_trends_label(self, emb):
        label = self.clusterer.predict(emb)
        return label

    def update_clusters(self, emb_data, id_data):
        self.emb_data = emb_data
        self.id_data = id_data
        self.clusterer = MiniBatchKMeans(n_clusters=self.n_clusters).fit(self.emb_data)
        self.labels = dict(zip(self.id_data, self.clusterer.labels_))
