using BookRent.Models;
using BookRent.ServiceClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using System.Text.Json;

namespace Bookrent.CatalogService.Services
{
    public class BooksService
    {
        private readonly BookRentContext _context;
        private readonly RedisCache _redisCache;

        public BooksService(BookRentContext context, RedisCache redisCache)
        {
            _context = context;
            _redisCache = redisCache;
        }

        public async Task<ServiceResult<ICollection<Book>>> GetAll()
        {
            List<Book> books = await _context.Books.Include(b => b.Publisher).
                Include(b => b.Authors).Include(b => b.Categories).ToListAsync();
            
            if (books is null)
                return new ServiceResult<ICollection<Book>>
                {
                    NotFound = true,
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

        public async Task<ServiceResult<ICollection<Book>>> GetDailyCache()
        {
            var cacheKey = (DateTime.Today.Day + DateTime.Today.Month.ToString()).ToString();

            var cachedData = await _redisCache.GetAsync(cacheKey);

            if (cachedData == null)
            {
                return new ServiceResult<ICollection<Book>>
                {
                    NotFound = true,
                    Errors = new ServiceErrors
                    {
                        Error = "Кэш не содержит списка книг с таким ключом"
                    }
                };
            }

            var books = JsonSerializer.Deserialize<List<Book>>(cachedData);
            return new ServiceResult<ICollection<Book>>
            {
                Ok = true,
                Result = books
            };
        }

        public async Task<ServiceResult<int>> GetCountOfFree(string strId)
        {
            int id;
            if (!int.TryParse(strId, out id))
                return new ServiceResult<int>
                {
                    BadRequest = true,
                    Errors = new ServiceErrors
                    {
                        Fields = new[] { "id" },
                        Error = "id не соответствует формату."

                    }
                };
            return await GetCountOfFree(id);
        }

        public async Task<ServiceResult<int>> GetCountOfFree(int id)
        {
            var book = await _context.Books.
                FirstOrDefaultAsync(x => x.Id == id);

            return book is not null 
                ? new ServiceResult<int>
                {
                    Ok = true,
                    Result = book.CountOfFree
                }
                : new ServiceResult<int>
                {
                    NotFound = true,
                    Errors = new ServiceErrors
                    {
                        Fields = new[] { "id" },
                        Error = "Книга с таким id не найдена."
                    }
                };
        }

        public async Task<ServiceResult<Book>> Get(string strId)
        {
            int id;
            if (!int.TryParse(strId, out id))
                return new ServiceResult<Book>
                {
                    BadRequest = true,
                    Errors = new ServiceErrors
                    {
                        Fields = new[] { "id" },
                        Error = "id не соответствует формату."

                    }
                };
            return await Get(id);
        }

        public async Task<ServiceResult<Book>> Get(int id)
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
