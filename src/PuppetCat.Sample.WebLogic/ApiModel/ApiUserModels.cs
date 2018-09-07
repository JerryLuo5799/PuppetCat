using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PuppetCat.Sample.WebLogic
{
    #region GetAll


    public class ApiUserGetAllResponse
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required]
        [Description("用户Id")]
        public uint Id { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户邮件
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 用户手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    #endregion

    #region Add

    public class ApiUserAddRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required]
        [Description("用户Id")]
        public uint Id { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        [Required]
        [Description("用户姓名")]
        public string Name { get; set; }
        /// <summary>
        /// 用户邮件
        /// </summary>
        [Required]
        [Description("用户邮件")]
        public string Email { get; set; }
        /// <summary>
        /// 用户手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    #endregion

}
