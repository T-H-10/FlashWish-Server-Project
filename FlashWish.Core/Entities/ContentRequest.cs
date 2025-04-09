using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FlashWish.Core.Entities
{
    public class ContentRequest
    {
        [JsonPropertyName("prompt")]
        public string Prompt { get; set; } = string.Empty;

        [JsonPropertyName("style")]
        public string Style { get; set; } = "ידידותי";

        [JsonPropertyName("rhyming")]
        public bool Rhyming { get; set; } = false;

        [JsonPropertyName("length")]
        public string Length { get; set; } = "קצר";

        [JsonPropertyName("recipientGender")]
        public string RecipientGender { get; set; } = "לא מוגדר";

        [JsonPropertyName("importantWords")]
        public List<string> ImportantWords { get; set; } = new List<string>();
    }
}
