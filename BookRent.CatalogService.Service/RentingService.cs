using Bookrent.CatalogService.Services;
using BookRent.Models;
using BookRent.ServiceClasses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRent.CatalogService.Service
{
    public class RentingService
    {
        private readonly BookRentContext _context;
        private readonly UserService _userService;
        private readonly BooksService _booksService;

        public RentingService(BookRentContext context, UserService userService, BooksService booksService)
        {
            _context = context;
            _userService = userService;
            _booksService = booksService;
        }

        public async Task<ServiceResult<bool>> RentBook(int bookId, string userLogin)
        {
            var bookCount = await _booksService.GetCountOfFree(bookId);
            if (bookCount.NotFound)
                return new ServiceResult<bool>
                {
                    NotFound = true,
                    Errors = new ServiceErrors
                    {
                        Error = "Книга с таким id не найдена.",
                        Fields = new[] { "id" }
                    }
                };

            if (bookCount.Result == 0)
                return new ServiceResult<bool>
                {
                    NotFound = true,
                    Errors = new ServiceErrors
                    {
                        Error = "Нет свободных книг",
                        Fields = new[] { "countOfFree" }
                    }
                };

            var bookRes = await _booksService.Get(bookId);
            var book = bookRes.Result;

            book.CountOfFree--;
            _context.Books.Update(book);
            _context.SaveChanges();

            return new ServiceResult<bool>
            {
                Ok = true,
                Result = true
            };
        }

        private async Task<ServiceResult<bool>> SetRentedBook(string login, int bookId, int weeks)
        {
            var userResult = await _userService.GetByLogin(login);

            if (userResult.BadRequest)
                return new ServiceResult<bool>()
                {
                    BadRequest = true,
                    Errors = userResult.Errors
                };

            var user = userResult.Result;
            RentedBook rentedBook = new RentedBook()
            {
                UsertId = user.Id,
                BookId = bookId,
                DateOfRent = DateTime.Today,
                DateOfRentEnd = DateTime.Today.AddMonths(weeks / 4).AddDays((weeks % 4) * 7)
            };

            var result = SaveRentedBook(rentedBook);
            return result.Result == false
                ? new ServiceResult<bool>()
                {
                    BadRequest = true,
                    Errors = new ServiceErrors()
                    {
                        Error = "Ошбика при сохранении данных об аренде в базу данных."
                    }
                }
                : new ServiceResult<bool>()
                {
                    Ok = true,
                    Result = true
                };
        }

        public async Task<bool> SaveRentedBook(RentedBook rentedBook)
        {
            await _context.RentedBooks.AddAsync(rentedBook);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException updateEx)
            {
                //Сохранение в Elastic
                return false;
            }
            return true;
        }
    }
}
