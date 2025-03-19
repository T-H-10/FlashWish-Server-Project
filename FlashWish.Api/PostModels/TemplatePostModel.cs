namespace FlashWish.Api.PostModels
{
    public class TemplatePostModel
    {
        public string TemplateName { get; set; }
        //public string TemplateDescription { get; set; }
        public string CategoryID { get; set; }
        public string UserID { get; set; }
        public IFormFile ImageFile { get; set; }
        //public DateTime UpdatedAt { get; set; }

    }
}
