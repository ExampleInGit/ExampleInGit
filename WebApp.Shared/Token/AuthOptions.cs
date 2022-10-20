using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApp.Shared.Token
{
    public class AuthOptions
    {
        public string Issuer { get; set; } // token issuer
        public string Audience { get; set; } // token consumer
        public string Key { get; set; } // key for hashing
        public int Lifetime { get; set; } // token life time - minutes
        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
