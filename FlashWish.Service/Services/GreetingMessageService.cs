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
    public class GreetingMessageService : IGreetingMessageService
    {
        public readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        public GreetingMessageService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }
        public async Task<GreetingMessageDTO> AddGreetingMessageAsync(GreetingMessageDTO message)
        {
            var messageToAdd = _mapper.Map<GreetingMessage>(message);
            if (messageToAdd == null)
            {
                return null;
            }
            messageToAdd.CreatedAt = DateTime.UtcNow;
            messageToAdd.UpdatedAt = DateTime.UtcNow;
            await _repositoryManager.GreetingMessages.AddAsync(messageToAdd);
            await _repositoryManager.SaveAsync();
            return _mapper.Map<GreetingMessageDTO>(messageToAdd);
        }

        //public async Task<bool> DeleteGreetingMessageAsync(int id)
        //{
        //    var messageDTO = await _repositoryManager.GreetingMessages.GetByIdAsync(id);
        //    if (messageDTO == null)
        //    {
        //        return false;
        //    }
        //    var messageToDelete = _mapper.Map<GreetingMessage>(messageDTO);
        //    if (messageToDelete == null)
        //    {
        //        return false;
        //    }
        //    await _repositoryManager.GreetingMessages.DeleteAsync(messageToDelete);
        //    await _repositoryManager.SaveAsync();
        //    return true;
        //}

        public async Task<bool> MarkMessageForDeletionAsync(int id)
        {
            var message = await _repositoryManager.GreetingMessages.GetByIdAsync(id);
            if (message == null)
            {
                return false;
            }
            message.MarkedForDeletion = true;
            await _repositoryManager.GreetingMessages.UpdateAsync(id, message);
            await _repositoryManager.SaveAsync();

            var cardsUsingMessage = await _repositoryManager.GreetingCards.GetAllAsync(card => card.TextID == id);
            if (!cardsUsingMessage.Any())
            {
                await _repositoryManager.GreetingMessages.DeleteAsync(message);
                await _repositoryManager.SaveAsync();
            }
            return true;
        }

        public async Task<IEnumerable<GreetingMessageDTO>> GetAllMessagesAsync()
        {
            var messages = await _repositoryManager.GreetingMessages.GetAllAsync();
            return _mapper.Map<IEnumerable<GreetingMessageDTO>>(messages);

        }

        public async Task<GreetingMessageDTO?> GetGreetingMessageByIdAsync(int id)
        {
            var message = await _repositoryManager.GreetingMessages.GetByIdAsync(id);
            return _mapper.Map<GreetingMessageDTO>(message);

        }

        public async Task<GreetingMessageDTO?> UpdateGreetingMessageAsync(int id, GreetingMessageDTO message)
        {
            if (message == null)
            {
                return null;
            }
            var messageToUpdate = _mapper.Map<GreetingMessage>(message);
            await _repositoryManager.GreetingMessages.UpdateAsync(id, messageToUpdate);
            await _repositoryManager.SaveAsync();
            return _mapper.Map<GreetingMessageDTO?>(messageToUpdate);
        }
    }
}
