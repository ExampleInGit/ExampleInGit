using System.ComponentModel.DataAnnotations;
using System.Text;
using WebApp.Shared.Messages;
using WebApp.Shared.Models;

namespace WebApp.Shared.Extensions
{
    public static class Validation
    {
        public static string UserValidate(this CreateUserModel userModel)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (userModel.Name == null || userModel.Name.Length < 8)
                stringBuilder.AppendLine(Correct.UserNameIsIncorrect);

            if (!(new EmailAddressAttribute().IsValid(userModel.Email)))
                stringBuilder.AppendLine(Correct.UserEmailIsIncorrect);

            return stringBuilder.ToString();
        }
    }
}
