namespace FlashWish.Api.PostModels
{
    public class GreetingCardPostModel
    {
        public int userID { get; set; }
        public int TemplateID { get; set; }
        public int TextID { get; set; }
        public int CategoryID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
