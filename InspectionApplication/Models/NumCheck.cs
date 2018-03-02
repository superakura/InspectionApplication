using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InspectionApplication.Models
{
    [Table("NumCheck")]
    public class NumCheck
    {
        [Key]
        public int Id { get; set; }

        public int InspectionApplicationID { get; set; }//报检单ID

        [StringLength(200)]
        public string InspectionNum { get; set; }//报检单编号

        [StringLength(200)]
        public string TypeOneString { get; set; }//大类编号--文本存储

        [StringLength(200)]
        public string TypeTwoString { get; set; }//小类编号--文本存储

        public int TypeOneInt { get; set; }//大类编号--数字存储

        public int TypeTwoInt { get; set; }//小类编号--数字存储
    }
}