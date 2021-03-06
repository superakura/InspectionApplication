﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InspectionApplicationDomain.Entities
{
    [Table("ProductUseDeptInfo")]
    public class ProductUseDept
    {
        [Key]
        public int ProductUseDeptInfoID { get; set; }

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
