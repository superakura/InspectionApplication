﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InspectionApplicationDomain.Entities
{
    [Table("InspectionApplications")]
    public class InspectionApplications
    {
        [Key]
        public int InspectionApplicationID { get; set; }

        [Required]
        [StringLength(100)]
        [Index(IsUnique =true)]
        public string InspectionApplicationNum { get; set; }//报检单编号

        [Required]
        [StringLength(200)]
        public string ProductName { get; set; }//产品名称

        [Required]
        [StringLength(200)]
        public string ProductFactory { get; set; }//生产厂家

        [Required]
        [StringLength(200)]
        public string ProductDealer { get; set; }//经销商

        [Required]
        [StringLength(200)]
        public string ProductType { get; set; }//产品批号、型号

        [Required]
        [StringLength(200)] 
        public string ProductPackingType { get; set; }//包装规格

        [Required]
        [StringLength(50)]
        public string ProductCount { get; set; }//到货数量

        [Required]
        public DateTime ArrivalDate { get; set; }//到货日期--用户填写

        [Required]
        public DateTime InspectionDate { get; set; }//报检日期时间，数据来源--用户提交报检单的系统时间

        [StringLength(100)]
        public string InspectionDeptName { get; set; }//报检单位名称

        public int InspectionDeptID { get; set; }//报检单位ID

        [StringLength(100)]
        public string InspectionPersonName { get; set; }//报检单位负责人姓名

        public int InspectionPersonID { get; set; }//报检单位负责人ID

        [Required]
        [StringLength(20)]
        public string DisposePersonName { get; set; }//质检处理报检单人员姓名

        public int DisposePersonID { get; set; }//质检处理报检单人员ID

        public DateTime DisposeDate { get; set; }//质检处理报检单日期

        [Required]
        [StringLength(20)]
        public string InspectionApplicationState { get; set; }//报检单状态

        [StringLength(200)]
        public string DisposeRemark { get; set; }//质检报检单处理意见
    }
}
