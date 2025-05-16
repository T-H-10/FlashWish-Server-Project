using FlashWish.Core.DTOs;
using FlashWish.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Core.IServices
{
    public interface IGreetingMessageService
    {
        Task<IEnumerable<GreetingMessageDTO>> GetAllMessagesAsync();
        Task<GreetingMessageDTO?> GetGreetingMessageByIdAsync(int id);
        Task<GreetingMessageDTO> AddGreetingMessageAsync(GreetingMessageDTO message);
        Task<GreetingMessageDTO?> UpdateGreetingMessageAsync(int id, GreetingMessageDTO message);
        //Task<bool> DeleteGreetingMessageAsync(int id);
        Task<bool> MarkMessageForDeletionAsync(int id);

        public Task<IActionResult> GenerateContentAsync(ContentRequest request);

    }
}
