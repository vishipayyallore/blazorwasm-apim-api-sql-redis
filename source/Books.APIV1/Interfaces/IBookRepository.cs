using Books.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Books.APIV1.Interfaces
{

    public interface IBookRepository
    {
        Task<bool> AddBook(Book video);

        Task<IEnumerable<Book>> GetAllBooks();

        Task<Book> GetBookById(int id);

        //Task<bool> UpdateBook(Book video);

        //Task<bool> DeleteBook(int id);
    }

}
