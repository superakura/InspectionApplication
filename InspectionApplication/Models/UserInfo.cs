using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InspectionApplication.Models
{
    [Table("UserInfo")]
    public class UserInfo
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(20)]
        public string UserNum { get; set; }//用户的员工编号

        [Required]
        [StringLength(20)]
        public string UserName { get; set; }//用户姓名

        [Required]
        [StringLength(100)]
        public string UserPassword { get; set; }//用户密码

        [StringLength(50)]
        public string UserPhone { get; set; }//用户联系方式

        //用户角色
        //1、报检单申请
        //2、报检单审批
        //3、系统管理员
        [Required]
        [StringLength(30)]
        public string UserRole { get; set; }

        [StringLength(200)]
        public string UserRemark { get; set; }

        //用户状态
        //0：正常
        //1：删除
        [Required]
        public int UserState { get; set; }

        //用户部门ID外键
        [Required]
        public int UserDeptID { get; set; }

        [ForeignKey("UserDeptID")]
        public DeptInfo DeptInfo { get; set; }
    }
}
