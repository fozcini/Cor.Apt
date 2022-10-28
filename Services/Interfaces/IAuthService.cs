using System.Collections.Generic;
using Cor.Apt.Models;

namespace Cor.Apt.Services.Interfaces
{
    public interface IAuthService
    {
        bool Authenticate(AuthenticateModel authenticateModel);
        bool UserIsValid(List<string> rl);
        bool LogOut();
    }
}