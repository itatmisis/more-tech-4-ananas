

## Парсер

Написан на c#, .net6. Парсер состоит из двух проектов: MORE_Tech.Parser  - непосредственно сервис, занимающийся парсингом и MORE_Tech.Data - проект в котором происходит все взаимодействие с базой данных. 

#### Запуск

Запустить проект MORE_Tech.Parser. Либо в папке src запустить команду ***docker-compose build && docker-compose up -d***



#### Архитектура парсера

При старте работы парсер выбирается из таблицы **source** в базе данных все записи, у который атрибут **is_active = true**. Далее для каждой записи source, в зависимости от атрибута type выбирается парсер. Сейчас у нас реализовано 2 парсера: парсер HTML  - парсит html контент с любого сайта, VK -парсер, парсит группы в  VK. Далее, выбранный парсер занимается парсингом новостей и сохранением их в БД.

#### Архитектура HTML парсера

Ключевым элементом парсера являются xml-документы, в который описываются инструкции для парсинга блоков новостей: текст, дата, вложения (изображения  и т.д.), количество просмотров. Таким образом, для того чтобы добавить новый сайт для парсинга необходимо лишь создать xml-документ. Никаких изменений в программном коде не требуется.

Пример описания документа для сайта [Клерк](https://www.klerk.ru/buh/):

```xml
<?xml version="1.0" encoding="utf-8" ?>
<instructions>
   <RootUrl>
  		<![CDATA[https://www.klerk.ru/]]>
  </RootUrl>
  <FeedUrls>
	  <item><![CDATA[\/]]></item>
	  <item><![CDATA[news\/page\/[\d]+\/]]></item>
  </FeedUrls>
  <NewsUrls>
   <item><![CDATA[buh\/news\/\d+\/]]></item>
  </NewsUrls>
  <NewsText>
  <![CDATA[//div[contains(@class, 'article__content')]]]>
  </NewsText>
   <Views attribute='count'>
  <![CDATA[//div[@class = 'article__bar']//span[@title = 'Просмотры']//core-count-format]]>
  </Views>
   <DateTime attribute='date'>
 <![CDATA[//div[@class = 'article__top']//core-date-format]]>
  </DateTime>
</instructions>
```



- RootUrl - корневой URL, в которого начинается парсинг сайта
- FeedUrls - ссылка, на которых расположены ленты с новостями. На этих страницах будут искаться ссылки на сами новости
- NewsUrls - ссылки на сами новости
- NewsText - текст новости
- Views - количество просмотров. **attribute='count'** говорит нам о том что мы берем не InnerText внутри тега, а значение его атрибута count
- DateTime - дата публикации статьи



Сформированные файла с инструкциями сохраняются в папку, путь до которой указывается в конфигурационном файле **appsettings.json** в поле **PathToXmlFiles**.


#### Архитектура VK парсера

Группы, использующиеся для получения новостей указаны в базе данных, что дает гибкость при получении новый записей и регулировании уже существующих источников. На этапе парсинга отсеиваются записи, помеченные как рекламные, закрепленные записи, записи не содержащие текста. 
При парсинге получаются текст новости, вложения и информация о взаимодействиях с ней. Возможно расширение записываемых в базу данных данных о взаимодействиях и другой сервисной информации. 

Пример создание поста из json:

```csharp
 Post temp = new Post(
                    attachments: Attachments(i),
                    num_attachments: NumAttachments(i),
                    interactions: Interactions(i),
                    text: (string)i["text"],
                    id: UInt32.Parse((string)i["id"]),
                    source_id: source_id,
                    source_url: source_url+$"{(string)i["from_id"]}_{(string)i["id"]}",
                    source_vk_id: int.Parse((string)i["from_id"]),
                    date: UInt32.Parse((string)i["date"]),
                    is_pinned: Convert.ToBoolean(Convert.ToInt16((string)i["is_pinned"])),
                    maa: Convert.ToBoolean(Convert.ToInt16((string)i["marked_as_ads"])),
                    is_reposted: reposted,
                    by_person: IsByPerson(i)
                    ); 
```

#### API

API написан на языке Python с использование веб-фреймворкм FastAPI. API позволяет получить новости для пользователя,
новости по ролям. Добавлять реалкции пользователся на новости.

```
	Ознакомится со всеми эндпоинтами API можно по ссылке: http://194.58.118.87:8001/docs
```

##### Запуск


```
git clone https://github.com/itatmisis/more-tech-4-ananas-anaserver
docker build -t API -f Dockerfile .
docker run -it -d -p 8000: 8000 API
```



