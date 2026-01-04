using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PuppetCat.AspNetCore.Core;
using PuppetCat.AspNetCore.Mvc;
using PuppetCat.Sample.Data;
using PuppetCat.Sample.Repository;
using PuppetCat.Sample.WebLogic;


namespace PuppetCat.Sample.API.Controllers
{
    /// <summary>
    /// 用户类
    /// </summary>
    [Route("User")]
    public class UserController : BaseController
    {
        private readonly UserRepository _userRepository;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="userRepository">User repository</param>
        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAll")]
        [ProducesResponseType(typeof(ResponseDefault<List<ApiUserGetAllResponse>>), 200)]
        public JsonResult GetAll([FromBody]RequestNoData request)
        {
            List<User> list = _userRepository.LoadListAll();

            List<ApiUserGetAllResponse> listRes = EntityUtils.CopyToList<User, ApiUserGetAllResponse>(list);

            return CreateResult<List<ApiUserGetAllResponse>>(ResponseStatusCode.OK, string.Empty, listRes);
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("Add")]
        //[ProducesResponseType(typeof(ResponseNoData), 200)]
        //public JsonResult Add([FromBody] ResponseDefault<ApiUserAddRequest> request)
        //{
        //    //Stopwatch sw = new Stopwatch();
        //    //sw.Start();
        //    User user = new User();
        //    user = EntityUtils.CopyToModel<ApiUserAddRequest, User>(request.data);
        //    _userRepository.Save(user);

        //    //sw.Stop();
        //    //_requestLog.Add("APIExcuseTime", sw.ElapsedMilliseconds.ToString());

        //    return CreateResult<ResponseNoData>(ResponseStatusCode.OK);
        //}
    }
}
