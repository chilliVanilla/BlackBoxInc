using BlackBoxInc.Models.Entities;

namespace BlackBoxInc.Services
{
    public interface IUserService
    {

        public List<User> GetAllUsers();
        // public string CreateNewUser();
        public bool DeleteUser(int id);
    }
}
