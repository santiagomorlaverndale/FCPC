using System.Linq;

namespace FCPC.Services
{
    public class UserService : IUserService
    {
        public User Authenticate(string username, string password)
        {
            using var db = new BloggingContext();
            var user = db.Users.FirstOrDefault(x => x.UserId == username && x.UserId == password);
            return user;
        }

        public User GetUser(string username)
        {
            using var db = new BloggingContext();
            var user = db.Users.FirstOrDefault(x => x.UserId == username);

            if (user != null)
            {
                user.Votes = db.Votes.Where(x => x.UserId == username).ToList();
            }

            return user;
        }
    }
}
