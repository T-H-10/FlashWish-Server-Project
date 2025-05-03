namespace FlashWish.Api.PostModels
{
    public class UserPostModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
    //public enum UserRole { Admin, Editor};
    public class UserRolePostModel: UserPostModel
    {
        public string Role { get; set; }
    }
}
