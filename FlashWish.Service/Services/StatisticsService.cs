using FlashWish.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Service.Services
{
    public class StatisticsService
    {
        private readonly IRepositoryManager _repositoryManager;

        public StatisticsService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }
        public async Task<object> GetDashBoardStatistics()
        {
            var usersCount = await _repositoryManager.Users.GetCountAsync();
            var greetingsCount = await _repositoryManager.GreetingCards.GetCountAsync();
            var templatesCount = await _repositoryManager.Templates.GetCountAsync();

            return new
            {
                UsersCount = usersCount,
                GreetingsCount = greetingsCount,
                TemplatesCount = templatesCount
            };
        }
    }
}
