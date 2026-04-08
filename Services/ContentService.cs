using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Text.Json;
using DevPath.Models;

namespace DevPath.Services
{
    public class ContentService
    {
        public static TopicContent Load(string fileName)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Content", fileName);

            if (!File.Exists(path)) return new TopicContent 
            {
                Blocks = new List<ContentBlock>()
            };

            var json = File.ReadAllText(path);

            return JsonSerializer.Deserialize<TopicContent>(json,new JsonSerializerOptions 
            {
                PropertyNameCaseInsensitive = true
            }) ?? new TopicContent { Blocks = new List<ContentBlock>() };
        }
    }


}