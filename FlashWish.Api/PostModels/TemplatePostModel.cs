namespace FlashWish.Api.PostModels
{
    public class TemplatePostModelPost
    {
        public string TemplateName { get; set; }
        //public string TemplateDescription { get; set; }
        public int CategoryID { get; set; }
        public int UserID { get; set; }
        public IFormFile ImageFile { get; set; }
        //public DateTime UpdatedAt { get; set; }

    }

    public class TemplatePostModelPut
    {
        public string TemplateName { get; set; }
        //public string TemplateDescription { get; set; }
        public int CategoryID { get; set; }
        public int UserID { get; set; }
    }
}
