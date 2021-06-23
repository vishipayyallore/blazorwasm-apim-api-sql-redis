using Books.APIV1.Interfaces;
using Books.DataV1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Books.APIV1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        private readonly IBookRepository _bookRepository;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookRepository bookRepository, ILogger<BooksController> logger)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Add a Book to the Books Table
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<bool>> Post([FromBody] Book book)
        {
            return await _bookRepository
                            .AddBook(book)
                            .ConfigureAwait(false);
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
