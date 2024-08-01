using BookRent.Models;
using BookRent.ServiceClasses;
using Microsoft.EntityFrameworkCore;

namespace Bookrent.CatalogService.Services
{
    public class BooksService
    {
        private readonly BookRentContext _context;

        public BooksService(BookRentContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<ICollection<Book>>> getAll()
        {
            List<Book> books = await _context.Books.Include(b => b.Publisher).
                Include(b => b.Authors).Include(b => b.Categories).ToListAsync();
            
            if (books is null)
                return new ServiceResult<ICollection<Book>>
                {
                    Ok = false,
                    Errors = new ServiceErrors
                    {
                        Error = "Возвращаемый список пуст."
                    }
                };

            return new ServiceResult<ICollection<Book>>
            {
                Ok = true,
                Result = books
            };
        }

        public async Task<ServiceResult<Book>> get(string strId)
        {
            int id;
            if (!int.TryParse(strId, out id))
                return new ServiceResult<Book>
                {
                    Ok = false,
                    BadRequest = true,
                    Errors = new ServiceErrors
                    {
                        Fields = new[] { "id" },
                        Error = "id не соответствует формату."

                    }
                };
            return await get(id);
        }

        public async Task<ServiceResult<Book>> get(int id)
        {
            var book =  await _context.Books.
                FirstOrDefaultAsync(b => b.Id == id);

            return book is not null
                ? new ServiceResult<Book>
                {
                    Ok = true,
                    Result = book
                }
                : new ServiceResult<Book>
                {
                    Ok = false,
                    NotFound = true,
                    Errors = new ServiceErrors
                    {
                        Fields = new[] { "id" },
                        Error = "Книга с таким id не найдена."
                    }
                };
        }
    }
}
