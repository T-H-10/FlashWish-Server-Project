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
        Task<GreetingCardDTO?> GetGreetingCardByIdAsync(int id);
        Task<GreetingCardDTO> AddGreetingCardAsync(GreetingCardDTO user);
        Task<GreetingCardDTO?> UpdateGreetingCardAsync(int id, GreetingCardDTO user);
        Task<bool> DeleteGreetingCardAsync(int id);

    }
}
