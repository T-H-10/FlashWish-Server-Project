using FlashWish.Core.IRepositories;
using FlashWish.Core.IServices;
using FlashWish.Core;
using FlashWish.Data.Repositories;
using FlashWish.Data;
using FlashWish.Service.Services;

namespace FlashWish.Api
{
    public static class ExtensionDependency
    {
        public static void AddDependency(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITemplateService, TemplateService>();
            services.AddScoped<IGreetingMessageService, GreetingMessageService>();
            services.AddScoped<IGreetingCardService, GreetingCardService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAuthService, AuthService>();


            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped(typeof(ITemplateRepository), typeof(TemplateRepository));
            services.AddScoped(typeof(ICategoryRepository),typeof(CategoryRepository));

            services.AddControllers();
            services.AddSingleton<DataContext>();

            services.AddAutoMapper(typeof(MappingProfile), typeof(MappingPostProfile));
            //services.AddDbContext<DataContext>(option =>
            //{
            //    option.UseSqlServer("Data Source = תהילה-הרשלר\\SQLEXPRESS; Inital Catalog = FlashWish1; Integrated Security = true; ");
            //});
        }

    }
}
