using SmartMed.Models;

namespace SmartMed.Interfaces;

public interface IUserService
{
    User SignIn();
    void SignUp();
    void SignOut();
    User GetCurrentUser();
}