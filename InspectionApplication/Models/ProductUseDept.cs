using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InspectionApplication.Models
{
    [Table("ProductUseDeptInfo")]
    public class ProductUseDept
    {
        [Key]
        public int ProductUseDeptInfoID { get; set; }

        [Required]
        public int ProductUseFatherDeptID { get; set; }//部门FatherID

        [Required]
        public int ProductUseDeptID { get; set; }//部门ID

        [Required]
        public int InspectionApplicationID { get; set; }//报检单ID

        [ForeignKey("ProductUseDeptID")]
        public DeptInfo DeptInfo { get; set; }

        [ForeignKey("InspectionApplicationID")]
        public InspectionApplications InspectionApplications { get; set; }
    }
}
