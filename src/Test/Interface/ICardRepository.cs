using System.Collections.Generic;
using System.Threading.Tasks;
using Test.Models;

namespace Test.interfaces
{
    public interface ICardRepository
    {

        /// <summary>
        /// Add card.   
        /// </summary>
        /// <param name="path"></param>
        /// <param name="account"></param>        
        Task AddCardAsync(Book book);

        /// <summary>
        /// Delete card.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="account"></param>        
        Task<bool> DeleteCardAsync(string id);

        /// <summary>
        /// Edit card.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="account"></param>        
        Task<bool> EditCardAsync(string id, Book book);

        /// <summary>
        /// Get cards.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="account"></param>        
        Task<List<Book>> GetCardsAsync();

        /// <summary>
        /// Get card by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Book> GetCardByIdAsync(string id);
    }
}
