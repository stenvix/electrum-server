using Newtonsoft.Json;

namespace Electrum.Api.Messages.Commands
{
    public class SignIn
    {
        public string Email { get; set; }
        public string Password { get; set; }

        [JsonConstructor]
        public SignIn(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
