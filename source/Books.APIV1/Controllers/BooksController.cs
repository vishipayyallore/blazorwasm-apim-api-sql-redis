using Books.Data;
using BooksStore.CacheDal.Interfaces;
using BooksStore.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Books.APIV1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        private readonly IBookRepository _bookRepository;
        private readonly IBookCacheRepository _bookCacheRepository;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookRepository bookRepository, IBookCacheRepository bookCacheRepository, ILogger<BooksController> logger)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));

            _bookCacheRepository = bookCacheRepository ?? throw new ArgumentNullException(nameof(bookCacheRepository));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Add a Book to the Books Table
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Book), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Book>> Post([FromBody] Book book)
        {
            await _bookRepository
                            .AddBook(book)
                            .ConfigureAwait(false);

            return Created("", book);
        }

        /// <summary>
        /// Retrieves all the books available
        /// </summary>
        /// <returns>Collection of Books</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Book>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Book>>> Get()
        {
            _logger.LogInformation("Received the Get() request.");

            var books = await _bookRepository
                            .GetAllBooks()
                            .ConfigureAwait(false);

            _ = await _bookCacheRepository.SaveOrUpdateItemToCache("NewBook", JsonSerializer.Serialize(books));

            return Ok(books);
        }

        /// <summary>
        /// Retireve a single book with the given Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Book), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Book>> Get(int id)
        {
            var book = await _bookRepository
                            .GetBookById(id)
                            .ConfigureAwait(false);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

    }

}
