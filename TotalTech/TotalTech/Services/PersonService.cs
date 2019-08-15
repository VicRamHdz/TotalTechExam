using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TotalTech.Framework;
using TotalTech.Models;

namespace TotalTech.Services
{
    public class PersonService : BaseService
	{
        public async Task<ResponseResult<PersonsResponse>> Get(int rowlimit)
        {
            var endpoint = $"https://randomuser.me/api?results={rowlimit}";
            var result = await GetAsync<PersonsResponse>(endpoint);
            return result;
        }
    }
}
