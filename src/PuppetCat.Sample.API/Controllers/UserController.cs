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
    /// <summary>
    /// 用户类
    /// </summary>
    [Route("User")]
    public class UserController : BaseController
    {
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAll")]
        [ProducesResponseType(typeof(ResponseDefault<List<ApiUserGetAllResponse>>), 200)]
        public JsonResult GetAll([FromBody] RequestNoData request)
        {
            List<User> list = UserRepository.Intance.LoadListAll();

            List<ApiUserGetAllResponse> listRes = EntityUtils.CopyToList<User, ApiUserGetAllResponse>(list);

            return CreateResult<List<ApiUserGetAllResponse>>(ResponseStatusCode.OK, string.Empty, listRes);
        }

    }
}
