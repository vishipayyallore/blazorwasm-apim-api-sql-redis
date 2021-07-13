using Books.Data;
using BooksStore.Core.Common;
using BooksStore.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace BooksStore.Bll
{

    public class BooksBll : IBooksBll
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookCacheRepository _bookCacheRepository;
        private readonly ILogger<BooksBll> _logger;

        public BooksBll(IBookRepository bookRepository, IBookCacheRepository bookCacheRepository, ILogger<BooksBll> logger)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));

            _bookCacheRepository = bookCacheRepository ?? throw new ArgumentNullException(nameof(bookCacheRepository));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Book> AddBook(Book book)
        {
            _logger.LogInformation("Received the BooksBll::AddBook() request.");

            var newBook = await _bookRepository
                            .AddBook(book)
                            .ConfigureAwait(false);

            await RemoveAllBooksDataFromCache(Constants.RedisCacheStore.AllBooksKey);

            _logger.LogInformation("Sending output from BooksBll::AddBook() request.");

            return newBook;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            IEnumerable<Book> books;

            _logger.LogInformation("Received the BooksBll::GetAllBooks() request.");

            var booksFromCache = await _bookCacheRepository
                    .RetrieveItemFromCache(Constants.RedisCacheStore.AllBooksKey);

            if (!string.IsNullOrEmpty(booksFromCache))
            {
                books = JsonSerializer.Deserialize<IEnumerable<Book>>(booksFromCache);
            }
            else
            {
                books = await _bookRepository
                            .GetAllBooks()
                            .ConfigureAwait(false);

                _ = await _bookCacheRepository.SaveOrUpdateItemToCache(
                        Constants.RedisCacheStore.AllBooksKey,
                        JsonSerializer.Serialize(books));
            }

            _logger.LogInformation("Sending output from BooksBll::GetAllBooks() request.");

            return books;
        }

        public async Task<Book> GetBookById(int id)
        {
            _logger.LogInformation("Received the BooksBll::GetBookById(id) request.");

            var book = await _bookRepository
                            .GetBookById(id)
                            .ConfigureAwait(false);

            _logger.LogInformation("Sending output from BooksBll::GetBookById(id) request.");

            return book;
        }

        private async Task RemoveAllBooksDataFromCache(string redisCacheKey)
        {
            await _bookCacheRepository.DeleteItemFromCache(redisCacheKey);
        }
    }

}
