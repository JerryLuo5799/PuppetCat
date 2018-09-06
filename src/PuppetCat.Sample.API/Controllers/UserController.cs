using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PuppetCat.AspNetCore.Core;
using PuppetCat.AspNetCore.Mvc;
using PuppetCat.Sample.Data;
using PuppetCat.Sample.Repository;
using PuppetCat.Sample.WebLogic;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PuppetCat.Sample.API.Controllers
{
    public class UserController : BaseController
    {
        [HttpPost]
        public JsonResult GetAll([FromBody] RequestNoData request)
        {
            List<User> list = UserRepository.Intance.LoadListAll();

            List<ApiUserGetAllResponse> listRes = EntityUtils.CopyToList<User, ApiUserGetAllResponse>(list);

            return CreateResult<List<ApiUserGetAllResponse>>(ResponseStatusCode.OK, string.Empty, listRes);
        }

    }
}
