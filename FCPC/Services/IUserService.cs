namespace FCPC.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);

        User GetUser(string username);
    }
}
