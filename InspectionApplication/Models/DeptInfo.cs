using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InspectionApplication.Models
{
    [Table("DeptInfo")]
    public class DeptInfo
    {
        [Key]
        public int DeptID { get; set; }

        [Required]
        [StringLength(30)]
        public string DeptName { get; set; }

        public int DeptFatherID { get; set; }

        //部门状态
        //0：正常
        //1：删除
        public int DeptState { get; set; }

        [StringLength(200)]
        public string DeptRemark { get; set; }

        public int DeptOrder { get; set; }
    }
}