using Books.Data;
using Books.DataServices;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Books.WebV1.Pages.BookPages
{

    public partial class Add
    {
        [Inject]
        private IBookDataService BookDataService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private readonly Random _random = new();

        public Book NewBook { get; set; } = new Book();

        protected async Task SaveBook()
        {
            NewBook.PictureUrl = $"/images/books/Book{_random.Next(1, 10)}.jpg";

            await BookDataService.AddBook(NewBook);

            NavigationManager.NavigateTo("listbooks");
        }

        protected void Home()
        {
            NavigationManager.NavigateTo("listbooks");
        }

    }

}
