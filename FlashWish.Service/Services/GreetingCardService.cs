using AutoMapper;
using FlashWish.Core.DTOs;
using FlashWish.Core.Entities;
using FlashWish.Core.IRepositories;
using FlashWish.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Service.Services
{
    public class GreetingCardService: IGreetingCardService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public GreetingCardService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }
        public async Task<GreetingCardDTO> AddGreetingCardAsync(GreetingCardDTO greetingCard)//---
        {
            var greetingCardToAdd = _mapper.Map<GreetingCard>(greetingCard);
            if (greetingCardToAdd != null)
            {
                await _repositoryManager.GreetingCards.AddAsync(greetingCardToAdd);
                await _repositoryManager.SaveAsync();
                return _mapper.Map<GreetingCardDTO>(greetingCardToAdd);
            }
            return null;
        }

        public async Task<bool> DeleteGreetingCardAsync(int id)
        {
            var greetingCardDTO = await _repositoryManager.GreetingCards.GetByIdAsync(id);
            if (greetingCardDTO == null)
            {
                return false;
            }
            var greetingCardToDelete = _mapper.Map<GreetingCard>(greetingCardDTO);
            if (greetingCardToDelete == null)
            {
                return false;
            }
            await _repositoryManager.GreetingCards.DeleteAsync(greetingCardToDelete);
            await _repositoryManager.SaveAsync();
            return true;
        }


        public async Task<IEnumerable<GreetingCardDTO>> GetAllGreetingCardsAsync()
        {
            var greetingCards = await _repositoryManager.GreetingCards.GetAllAsync();
            return _mapper.Map<IEnumerable<GreetingCardDTO>>(greetingCards);
        }


        public async Task<GreetingCardDTO?> GetGreetingCardByIdAsync(int id)
        {
            var greetingCard = await _repositoryManager.GreetingCards.GetByIdAsync(id);
            if(greetingCard == null) { return null; }
            Console.WriteLine(greetingCard);
            //Console.WriteLine(typeof(greetingCard));
            return _mapper.Map<GreetingCardDTO>(greetingCard);
        }


        public async Task<GreetingCardDTO?> UpdateGreetingCardAsync(int id, GreetingCardDTO greetingCard)
        {
            if (greetingCard == null)
            {
                return null;
            }
            var greetingCardToUpdate = _mapper.Map<GreetingCard>(greetingCard);
            await _repositoryManager.GreetingCards.UpdateAsync(id, greetingCardToUpdate);
            await _repositoryManager.SaveAsync();
            return _mapper.Map<GreetingCardDTO?>(greetingCardToUpdate);
        }
    }
}
