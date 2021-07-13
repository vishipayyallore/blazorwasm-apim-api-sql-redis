using Books.Data;
using BooksStore.Core.Interfaces;
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

        private readonly IBooksBll _booksBll;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBooksBll booksBll, ILogger<BooksController> logger)
        {
            _booksBll = booksBll ?? throw new ArgumentNullException(nameof(booksBll));

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
            _logger.LogInformation("Received the BooksController::Post() request.");

            _ = await _booksBll
                            .AddBook(book)
                            .ConfigureAwait(false);

            _logger.LogInformation("Sending output from BooksController::Post() request.");

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
            _logger.LogInformation("Received the BooksController::Get() request.");

            var books = await _booksBll
                            .GetAllBooks()
                            .ConfigureAwait(false);

            _logger.LogInformation("Sending output from BooksController::Get() request.");

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
            _logger.LogInformation("Received the BooksController::Get(id) request.");

            var book = await _booksBll
                            .GetBookById(id)
                            .ConfigureAwait(false);

            if (book == null)
            {
                return NotFound();
            }

            _logger.LogInformation("Sending output from BooksController::Get(id) request.");

            return Ok(book);
        }

    }

}
