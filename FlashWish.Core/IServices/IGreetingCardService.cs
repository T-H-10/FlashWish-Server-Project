using FlashWish.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Core.IServices
{
    public interface IGreetingCardService
    {
        Task<IEnumerable<GreetingCardDTO>> GetAllGreetingCardsAsync();
        Task<IEnumerable<GreetingCardDTO>> GetMyGreetingCardsAsync(int userId);
        Task<GreetingCardDTO?> GetGreetingCardByIdAsync(int id);
        Task<GreetingCardDTO> AddGreetingCardAsync(GreetingCardDTO card);
        Task<GreetingCardDTO?> UpdateGreetingCardAsync(int id, GreetingCardDTO card);
        Task<bool> DeleteGreetingCardAsync(int id);

    }
}
