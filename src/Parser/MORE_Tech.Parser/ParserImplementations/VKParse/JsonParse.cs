using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;


namespace MORE_Tech.Parser.ParserImplementations.VKParse
{
    public class JsonParse
    {
        JObject Json { get; set; }
        public JsonParse(string response)
        {
            Json = JObject.Parse(response);
        }

        public Post[] MakePosts(UInt32 source_id, string source_url)
        {
            Post[] posts = new Post[0];
            foreach (var i in Json["response"]["items"])
            {
                bool reposted = false;
                if (i["copy_history"] != null)
                {
                    reposted = true;
                }
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
                    ); ;
                if(!temp.IsPinned() && !temp.IsByPerson() && !temp.IsAd() && !temp.IsReposted() && temp.GetText().Length!=0)
                    posts = posts.Append(temp).ToArray();
            }
            return posts;
        }

        private List<List<string>> Attachments(JToken token)
        {
            {
                List<List<string>> attachments = new List<List<string>>() { new List<string>(), new List<string>()};
                if (NumAttachments(token) != 0)
                {
                    foreach (var i in token["attachments"])
                    {
                        if ((string)i["type"] == "link")
                        {
                            if ((string)i["link"]["description"] != "Playlist")
                            {
                                attachments[1].Add((string)i["link"]["url"]);
                            }
                        }
                        else if ((string)i["type"] == "photo")
                        {
                            string url = (string)i["photo"]["sizes"].Last()["url"];
                            if(url != null)
                            {
                                attachments[0].Add(url);
                            }

                        }
                    }
                }
                return attachments;
            }
        }
        private int NumAttachments(JToken token)
        {
            try
            {
                return token["attachments"].Count();
            }
            catch
            {
                return 0;
            }
        }
        private Dictionary<string, UInt32> Interactions(JToken token)
        {
            {
                Dictionary<string, UInt32> interactions = new Dictionary<string, UInt32>();
                string[] list = {"likes", "views", "reposts", "comments"};
                foreach (var i in list)
                {
                    interactions.Add(i, UInt32.Parse((string)token[i]["count"]));
                }
                return interactions;
            }
        }

        private bool IsByPerson(JToken token)
        {
            return int.TryParse((string)token["signer_id"], out int _);
        }
    }
}
