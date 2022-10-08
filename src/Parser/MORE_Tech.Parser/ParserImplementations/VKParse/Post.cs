using System;
using System.Collections.Generic;
using MORE_Tech.Data.Models;
namespace MORE_Tech.Parser.ParserImplementations.VKParse
{
    public class Post
    {
        readonly List<Attachments> attachments;
        readonly int num_attachments;
        readonly Dictionary<string, UInt32> interactions;
        readonly string text;
        readonly UInt32 id;
        readonly UInt32 source_id;
        readonly int source_vk_id;
        readonly string source_url;
        readonly DateTimeOffset date;
        readonly bool is_pinned = false;
        readonly bool is_repost = false;
        readonly bool maa = false;
        readonly bool by_person = false;
        //readonly float str;
        public Post(List<List<string>> attachments, 
            int num_attachments,
            Dictionary<string, UInt32> interactions, 
            string text, 
            string source_url, 
            UInt32 id,
            UInt32 source_id,
            int source_vk_id,
            UInt32 date,
            bool is_pinned,
            bool maa,
            bool is_reposted,
            bool by_person)
        {
            this.maa = maa;
            this.is_pinned = is_pinned;
            this.num_attachments = num_attachments;
            this.id = id;
            this.text = text;
            this.source_id = source_id;
            this.source_url = source_url;
            this.interactions = interactions;
            this.attachments = Attachment(attachments);
            this.is_repost = is_reposted;
            this.by_person = by_person;
            this.source_vk_id = source_vk_id;
            this.date = DateTimeOffset.FromUnixTimeSeconds(date);
        }
        public List<Attachments> GetAttachments() => attachments;
        public Dictionary<string, UInt32> GetInteractions()=> interactions;
        public string GetText() => text;
        public int GetNumAttachments() => num_attachments;
        public UInt32 GetGroup() => source_id;
        public UInt32 GetId() => id;
        public int GetVKId() => source_vk_id;
        public DateTimeOffset GetDate() => date;
        public UInt32 GetSource() => source_id;
        public bool IsPinned() => is_pinned;
        public bool IsAd() => maa;
        public bool IsByPerson() => by_person;
        public bool IsReposted() => is_repost;

        public News News()
        {
            return new News("", this.text, this.source_url, (int)this.interactions["views"], this.date, (int)this.source_id);
        }
        public List<Attachments> Attachment(List<List<string>> strings)
        {

            List<Attachments> attachments = new List<Attachments>();
            if (num_attachments != 0)
            {
                for (int i = 0; i < strings.Count; i++)
                {
                    for (int j = 0; j < strings[i].Count; j++)
                    {
                        attachments.Add(new Attachments()
                        {
                            Url = strings[i][j],
                            NewsId = News().Id,
                            Type = (Data.Models.Enums.AttachmentTypes)i
                        });
                    }
                }
            }
            return attachments;
        }
    }
}
