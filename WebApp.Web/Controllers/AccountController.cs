using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Infrastructure.Interfaces;
using WebApp.Shared.Enums;
using WebApp.Shared.Extensions;
using WebApp.Shared.Messages;
using WebApp.Shared.Models;

namespace WebApp.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Account
        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<UserModel>> Get()
        {
            return await _userService.GetAllAsync();
        }

        // GET api/Account/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<UserModel?> Get(int id)
        {
            return await _userService.GetByIdAsync(id);
        }

        // POST api/Account
        [HttpPost]
        public async Task<CreateUserResponseModel> Post(CreateUserModel userModel)
        {
            string isCorrect = userModel.UserValidate();
            var response = new CreateUserResponseModel();

            if (!string.IsNullOrEmpty(isCorrect))
            {
                response.Message = isCorrect;
                return response;
            }

            response.User = await _userService.AddAsync(userModel, RoleType.User);
            response.Message = Correct.Success;

            return response;
        }

        // PUT api/Account
        [Authorize]
        [HttpPut]
        public async Task Put([FromBody] UserModel userModel)
        {
            await _userService.UpdateAsync(userModel);
        }

        // DELETE api/Account/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async void Delete(int id)
        {
            await _userService.DeleteAsync(id);
        }
    }
}
