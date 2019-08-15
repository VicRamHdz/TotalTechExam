using System;
using System.Threading.Tasks;
using TotalTech.Framework;
using TotalTech.Models;

namespace TotalTech.Services
{
    public class UserService : BaseService
    {
        public async Task<ResponseResult<TokenResponseResult>> Login(string email, string password)
        {
            var endpoint = "https://reqres.in/api/login";
            var p = new
            {
                email = email,
                password = password
            };
            var result = await PostAsync<object, TokenResponseResult>(endpoint, p);
            return result;
        }
    }
}
