using Newtonsoft.Json;

namespace GoFish.Library.Model
{
    public class MessageSearchResult : SearchResult
    {
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
