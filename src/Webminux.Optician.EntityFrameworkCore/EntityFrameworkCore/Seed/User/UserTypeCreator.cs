using System.Linq;
using Webminux.Optician.Core;

namespace Webminux.Optician.EntityFrameworkCore
{
    public class UserTypeCreator
    {
        private readonly OpticianDbContext _context;
        public UserTypeCreator(OpticianDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            if (_context.UserTypes.Any())
            {
                return;
            }
            _context.UserTypes.Add(UserType.Create(OpticianConsts.UserTypes.Employee));
            _context.UserTypes.Add(UserType.Create(OpticianConsts.UserTypes.Customer));
            _context.UserTypes.Add(UserType.Create(OpticianConsts.UserTypes.Supplier));
            _context.SaveChanges();
        }
    }
}