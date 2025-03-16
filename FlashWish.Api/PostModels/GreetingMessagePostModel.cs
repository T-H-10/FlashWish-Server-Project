namespace FlashWish.Api.PostModels
{
    public class GreetingMessagePostModel
    {
        public int CategoryID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int UserID { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
