using BlackBoxInc.Models.Entities;

namespace BlackBoxInc.Services
{
    public interface IUserService
    {

        public List<User> GetAllUsers();
        public int CreateNewUser();
        public bool DeleteUser(int id);
    }
}
