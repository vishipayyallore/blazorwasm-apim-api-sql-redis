using Books.Data;
using BooksStore.Core.Configuration;
using BooksStore.Core.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BooksStore.SqlDal
{

    public class BookRepository : IBookRepository
    {
        private readonly IDataStoreSettings _dataStoreSettings;
        private readonly ILogger<BookRepository> _logger;

        public BookRepository(IDataStoreSettings dataStoreSettings, ILogger<BookRepository> logger)
        {
            _dataStoreSettings = dataStoreSettings ?? throw new ArgumentNullException(nameof(dataStoreSettings));
            
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Book> AddBook(Book book)
        {
            _logger.LogInformation("Request Received for BookRepository::AddBook()");

            using (var conn = new SqlConnection(_dataStoreSettings.SqlServerConnectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("PictureUrl", book.PictureUrl, DbType.String);
                parameters.Add("Title", book.Title, DbType.String);
                parameters.Add("Author", book.Author, DbType.String);
                parameters.Add("IsActive", book.IsActive, DbType.Boolean);
                parameters.Add("ISBN", book.ISBN, DbType.String);
                parameters.Add("Pages", book.Pages, DbType.Int32);
                parameters.Add("Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                // Stored procedure method
                await conn.ExecuteAsync("[dbo].[usp_add_book]", parameters,
                                commandType: CommandType.StoredProcedure)
                                .ConfigureAwait(false);

                book.Id = parameters.Get<int>("Id");
            }

            _logger.LogInformation("Returning output from BookRepository::AddBook()");

            return book;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            _logger.LogInformation("Request Received for BookRepository::GetAllBooks()");
            
            IEnumerable<Book> books;

            using (var conn = new SqlConnection(_dataStoreSettings.SqlServerConnectionString))
            {
                books = await conn.QueryAsync<Book>("[dbo].[usp_get_all_books]",
                                commandType: CommandType.StoredProcedure)
                                .ConfigureAwait(false);
            }

            _logger.LogInformation("Returning output from BookRepository::GetAllBooks()");

            return books;
        }

        public async Task<Book> GetBookById(int id)
        {
            _logger.LogInformation("Request Received for BookRepository::GetBookById()");

            Book book = new();

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);

            using (var conn = new SqlConnection(_dataStoreSettings.SqlServerConnectionString))
            {
                book = await conn.QueryFirstOrDefaultAsync<Book>("[dbo].[usp_get_book_by_id]", parameters,
                                    commandType: CommandType.StoredProcedure)
                                    .ConfigureAwait(false);
            }

            _logger.LogInformation("Returning output from BookRepository::GetBookById()");

            return book;
        }

    }

}
