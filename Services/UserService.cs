using BlackBoxInc.Data;
using BlackBoxInc.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlackBoxInc.Services
{
    public class UserService : IUserService
    {
        public readonly ApplicationDbContext dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // public string CreateNewUser()
        // {
        //     var newUser = new User();
        //
        //     dbContext.Users.Add(newUser);
        //     dbContext.SaveChanges();
        //
        //     return newUser.Id;
        // }

        public List<User> GetAllUsers()
        {
            var allUsers = dbContext.Users
                .Include(u => u.Items)
                .ThenInclude(i => i.Product)
                .ToList();
            return allUsers;
        }

        public bool DeleteUser(int id)
        {
            var user = dbContext.Users.Find(id);
            if (user is null)
            {
                return false;
            }

            dbContext.Users.Remove(user);
            dbContext.SaveChanges();
            return true;
        }
    }
}
