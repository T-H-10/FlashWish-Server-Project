namespace FlashWish.Api.PostModels
{
    public class TemplatePostModel
    {
        public string TemplateName { get; set; }
        public string TemplateDescription { get; set; }
        public int CategoryID { get; set; }
        public int UserID { get; set; }
        public string ImageURL { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
