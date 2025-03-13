using AutoMapper;
using FlashWish.Api.PostModels;
using FlashWish.Core.DTOs;

namespace FlashWish.Api
{
    public class MappingPostProfile:Profile
    {
        public MappingPostProfile()
        {
            CreateMap<UserPostModel, UserDTO>();
            CreateMap<TemplatePostModel, TemplateDTO>();
            CreateMap<GreetingCardPostModel, GreetingCardDTO>();
            CreateMap<GreetingMessagePostModel, GreetingMessageDTO>();
            CreateMap<CategoryPostModel, CategoryDTO>();
        }
    }
}
