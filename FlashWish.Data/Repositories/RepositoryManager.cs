using FlashWish.Core.Entities;
using FlashWish.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Data.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITemplateRepository _templateRepository;
        private readonly IRepository<GreetingCard> _greetingCardRepository;
        private readonly IRepository<GreetingMessage> _greetingMessageRepository;
        private readonly ICategoryRepository _categoryRepository;
        public RepositoryManager(DataContext context,
                                IUserRepository userRepository,
                                IRoleRepository roleRepository,
                                ITemplateRepository templateRepository,
                                IRepository<GreetingCard> greetingCardRepository,
                                IRepository<GreetingMessage> greetingMessageRepository,
                                ICategoryRepository categoryRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _templateRepository = templateRepository;
            _greetingCardRepository = greetingCardRepository;
            _greetingMessageRepository = greetingMessageRepository;
            _categoryRepository = categoryRepository;
        }

        public IUserRepository Users => _userRepository;
        public ITemplateRepository Templates => _templateRepository;
        public IRepository<GreetingMessage> GreetingMessages => _greetingMessageRepository;
        public IRepository<GreetingCard> GreetingCards => _greetingCardRepository;
        public ICategoryRepository Categories => _categoryRepository;

        public IRoleRepository Roles => _roleRepository;

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
