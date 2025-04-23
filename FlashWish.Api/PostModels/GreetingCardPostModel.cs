namespace FlashWish.Api.PostModels
{
    public class GreetingCardPostModel
    {
        public int UserID { get; set; }
        public int TemplateID { get; set; }
        public int TextID { get; set; }
        public int CategoryID { get; set; }
        public string CanvasStyle { get; set; }
        //public DateTime UpdatedAt { get; set; }

    }
}
