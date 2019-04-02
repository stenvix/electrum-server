using Newtonsoft.Json;

namespace Electrum.Identity.Messages.Commands
{
    public class SignIn
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public SignIn(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
