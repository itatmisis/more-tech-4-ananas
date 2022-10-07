import torch
from transformers import AutoTokenizer, AutoModel


class SbertWrapper:
    """
    NLP model for embeddings extraction
    """

    def __init__(self, multitask=True):
        """
        :param multitask: bool, default=True
        The type of the pre-trained model:
        If True, then sberbank-ai/sbert_large_mt_nlu_ru downloaded
        If False, then sberbank-ai/sbert_large_nlu_ru downloaded
        """
        if multitask:
            self.model_name = "sberbank-ai/sbert_large_mt_nlu_ru"
        else:
            self.model_name = "sberbank-ai/sbert_large_nlu_ru"

        self.device = "cuda" if torch.cuda.is_available() else "cpu"
        # Load AutoModel from huggingface model repository
        # https://huggingface.co/sberbank-ai/sbert_large_mt_nlu_ru
        # https://huggingface.co/sberbank-ai/sbert_large_nlu_ru
        self.tokenizer = AutoTokenizer.from_pretrained(self.model_name)
        self.model = AutoModel.from_pretrained(self.model_name).to(self.device)

    def get_model_output(self, encoded_input):
        """
        Compute token embeddings

        :param encoded_input: tokenized data
        :return: embeddings (last_hidden_state, pooler_output)
        """

        encoded_input = encoded_input.to(self.device)

        with torch.no_grad():
            return self.model(**encoded_input)

    def get_embedding(self, text):

        encoded_input = self.tokenizer(text, padding=True, truncation=True, max_length=64, return_tensors='pt')
        model_output = self.get_model_output(encoded_input)

        token_embeddings = model_output[0]
        attention_mask = encoded_input['attention_mask']

        input_mask_expanded = attention_mask.unsqueeze(-1).expand(token_embeddings.size()).float()
        sum_embeddings = torch.sum(token_embeddings * input_mask_expanded, 1)
        sum_mask = torch.clamp(input_mask_expanded.sum(1), min=1e-9)
        text_embedding = sum_embeddings / sum_mask
        return text_embedding


    class SummarizationWrapper:
    def __init__(self,
                 model_name="csebuetnlp/mT5_multilingual_XLSum",
                 ):
        self.whitespace_handler = lambda k: re.sub('\s+', ' ', re.sub('\n+', ' ', k.strip()))
        self.tokenizer = AutoTokenizer.from_pretrained(model_name)
        self.model = AutoModelForSeq2SeqLM.from_pretrained(model_name)

    def get_summary(self, text):
        input_ids = self.tokenizer(
            [self.whitespace_handler(text)],
            return_tensors="pt",
            padding="max_length",
            truncation=True,
            max_length=512
        )["input_ids"]

        output_ids = self.model.generate(
            input_ids=input_ids,
            max_length=84,
            no_repeat_ngram_size=2,
            num_beams=4
        )[0]

        summary = self.tokenizer.decode(
            output_ids,
            skip_special_tokens=True,
            clean_up_tokenization_spaces=False
        )
        return summary


