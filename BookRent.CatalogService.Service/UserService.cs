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
    public class UserService
    {
        private readonly BookRentContext _context;

        public UserService(BookRentContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<User>> GetByLogin(string login)
        {
            var res = await _context.Users.FirstOrDefaultAsync(u => u.UserLogin == login);
            return res == null
                ? new ServiceResult<User>
                {
                    BadRequest = true,
                    Errors = new ServiceErrors()
                    {
                        Fields = new[] { login },
                        Error = "Не существует User с таким Login"
                    }
                }
                : new ServiceResult<User>
                {
                    Ok = true,
                    Result = res
                };
        }
    }
}
