using FlashWish.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Core.IRepository
{
    public interface IRepositoryManager
    {
        IUserRepository Users { get; }
        ITemplateRepository Templates { get; }
        IRepository<GreetingMessage> GreetingMessages { get; }
        IRepository<GreetingCard> GreetingCards { get; }
        ICategoryRepository Categories { get; }
        Task SaveAsync();
    }
}
