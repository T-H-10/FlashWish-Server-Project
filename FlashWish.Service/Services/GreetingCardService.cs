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
    public class GreetingCardService : IGreetingCardService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinartService;
        public GreetingCardService(IRepositoryManager repositoryManager, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _cloudinartService = cloudinaryService;
        }
        public async Task<GreetingCardDTO> AddGreetingCardAsync(GreetingCardDTO greetingCardDto)//---
        {
            var template = await _repositoryManager.Templates.GetByIdAsync(greetingCardDto.TemplateID);
            if (template == null)
            {
                //throw new Exception("לא נמצא רקע תואם לבקשה.");
                return null;
            }

            if (template.MarkedForDeletion)
            {
                return null;
                //throw new Exception("אין אפשרות ליצור כרטיס ברכה מתבנית המסומנת למחיקה.");
            }
            var message = await _repositoryManager.GreetingMessages.GetByIdAsync(greetingCardDto.TextID);
            if(message == null || message.MarkedForDeletion)
            {
                return null;
            }
            var greetingCardToAdd = _mapper.Map<GreetingCard>(greetingCardDto);
            greetingCardToAdd.CreatedAt = DateTime.UtcNow;
            greetingCardToAdd.UpdatedAt = DateTime.UtcNow;
            //if (greetingCardToAdd != null)
            //{
            await _repositoryManager.GreetingCards.AddAsync(greetingCardToAdd);
            await _repositoryManager.SaveAsync();
            return _mapper.Map<GreetingCardDTO>(greetingCardToAdd);
            //}
            //return null;
        }

        public async Task<bool> DeleteGreetingCardAsync(int id)
        {
            // שלב 1 – מציאת הכרטיס
            var card = await _repositoryManager.GreetingCards.GetByIdAsync(id);
            if (card == null)
            {
                return false;
            }

            int templateId = card.TemplateID;
            int messageId = card.TextID;
            // שלב 2 – מחיקת הכרטיס
            await _repositoryManager.GreetingCards.DeleteAsync(card);
            await _repositoryManager.SaveAsync();

            // שלב 3 – בדיקה אם יש עדיין כרטיסים שמשתמשים בתבנית הזו
            var stillInTemplateUse = await _repositoryManager.GreetingCards
                .ExistsAsync(c => c.TemplateID == templateId);

            if (!stillInTemplateUse)
            {
                // שלב 4 – קבלת התבנית
                var template = await _repositoryManager.Templates.GetByIdAsync(templateId);

                // שלב 5 – האם התבנית מסומנת למחיקה?
                if (template?.MarkedForDeletion == true)
                {
                    // שלב 6 – מחיקת התמונה מהענן
                    bool imageDeleted = await _cloudinartService.DeleteImageAsync(template.ImageURL);
                    Console.WriteLine("the image " + template.TemplateID + " deleted? " + imageDeleted);
                    // שלב 7 – מחיקת התבנית עצמה
                    await _repositoryManager.Templates.DeleteAsync(template);
                    await _repositoryManager.SaveAsync();
                }
            }

            var stillInMessageUse= await _repositoryManager.GreetingCards
                .ExistsAsync(c=>c.TextID == messageId);
            if (!stillInMessageUse)
            {
                var message = await _repositoryManager.GreetingMessages.GetByIdAsync(messageId);
                if(message?.MarkedForDeletion == true)
                {
                    await _repositoryManager.GreetingMessages.DeleteAsync(message);
                    await _repositoryManager.SaveAsync();
                }
            }
            return true;
        }



        public async Task<IEnumerable<GreetingCardDTO>> GetAllGreetingCardsAsync()
        {
            var greetingCards = await _repositoryManager.GreetingCards.GetAllAsync();
            return _mapper.Map<IEnumerable<GreetingCardDTO>>(greetingCards);
        }

        public async Task<IEnumerable<GreetingCardDTO>> GetMyGreetingCardsAsync(int userId)
        {
            var greetingCards = await _repositoryManager.GreetingCards.GetAllAsync(card => card.UserID == userId);
            return _mapper.Map<IEnumerable<GreetingCardDTO>>(greetingCards);
        }
        public async Task<GreetingCardDTO?> GetGreetingCardByIdAsync(int id)
        {
            var greetingCard = await _repositoryManager.GreetingCards.GetByIdAsync(id);
            if (greetingCard == null) { return null; }
            return _mapper.Map<GreetingCardDTO>(greetingCard);
        }


        public async Task<GreetingCardDTO?> UpdateGreetingCardAsync(int id, GreetingCardDTO greetingCard)
        {
            if (greetingCard == null)
            {
                return null;
            }
            var greetingCardToUpdate = _mapper.Map<GreetingCard>(greetingCard);
            greetingCardToUpdate.UpdatedAt = DateTime.UtcNow;
            await _repositoryManager.GreetingCards.UpdateAsync(id, greetingCardToUpdate);
            await _repositoryManager.SaveAsync();
            return _mapper.Map<GreetingCardDTO?>(greetingCardToUpdate);
        }
    }
}
