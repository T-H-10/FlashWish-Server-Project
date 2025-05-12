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
            var now = DateTime.UtcNow;
            var lastMonth=now.AddMonths(-1);
            var twoMonthsAgo = now.AddMonths(-2);


            var usersCount = await _repositoryManager.Users.GetCountAsync();
            var greetingsCount = await _repositoryManager.GreetingCards.GetCountAsync();
            var templatesCount = await _repositoryManager.Templates.GetCountAsync();
            var greetingMessagesCount = await _repositoryManager.GreetingMessages.GetCountAsync();
            var categoriesCount = await _repositoryManager.Categories.GetCountAsync();

            var currentUsers=await _repositoryManager.Users.GetCountAsync(u=>
                u.CreatedAt >= lastMonth && u.CreatedAt <= now);
            var previousUsers = await _repositoryManager.Users.GetCountAsync(u =>
                u.CreatedAt >= twoMonthsAgo && u.CreatedAt < lastMonth);

            var currentGreetings = await _repositoryManager.GreetingCards.GetCountAsync(g =>
                g.CreatedAt >= lastMonth && g.CreatedAt <= now);
            var previousGreetings = await _repositoryManager.GreetingCards.GetCountAsync(g =>
                g.CreatedAt >= twoMonthsAgo && g.CreatedAt < lastMonth);

            var currentTemplates = await _repositoryManager.Templates.GetCountAsync(t =>
                t.CreatedAt >= lastMonth && t.CreatedAt <= now);
            var previousTemplates = await _repositoryManager.Templates.GetCountAsync(t =>
                t.CreatedAt >= twoMonthsAgo && t.CreatedAt < lastMonth);

            var currentMessages = await _repositoryManager.GreetingMessages.GetCountAsync(m =>
                m.CreatedAt >= lastMonth && m.CreatedAt <= now);
            var previousMessages = await _repositoryManager.GreetingMessages.GetCountAsync(m =>
                m.CreatedAt >= twoMonthsAgo && m.CreatedAt < lastMonth);

            var currentCategories = await _repositoryManager.Categories.GetCountAsync(c =>
             c.CreatedAt >= lastMonth && c.CreatedAt <= now);
            var previousCategories = await _repositoryManager.Categories.GetCountAsync(c =>
                c.CreatedAt >= twoMonthsAgo && c.CreatedAt < lastMonth);

            double GetGrowthRate(int current, int previous)
            {
                if (previous > 0)
                    return ((double)(current - previous) / previous) * 100;
                return current > 0 ? 100 : 0;
            }

            return new
            {
                UsersCount = usersCount,
                GreetingsCount = greetingsCount,
                TemplatesCount = templatesCount,
                GreetingMessagesCount= greetingMessagesCount,
                CategoriesCount= categoriesCount,

                UsersGrowthRate = GetGrowthRate(currentUsers, previousUsers),
                GreetingsGrowthRate = GetGrowthRate(currentGreetings, previousGreetings),
                TemplatesGrowthRate = GetGrowthRate(currentTemplates, previousTemplates),
                GreetingMessagesGrowthRate = GetGrowthRate(currentMessages, previousMessages),
                CategoriesGrowthRate=GetGrowthRate(currentCategories, previousCategories)
    };
        }
    }
}
