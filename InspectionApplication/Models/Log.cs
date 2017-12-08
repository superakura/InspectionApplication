using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InspectionApplication.Models
{
    [Table("Log")]
    public class Log
    {
        [Key]
        public int LogID { get; set; }
        
        [Required]
        [StringLength(200)]
        public string LogInfo { get; set; }

        [Required]
        [StringLength(100)]
        public string LogType { get; set; }

        [Required]
        public DateTime LogInputDate { get; set; }

        public int LogInputPerson { get; set; }

        public int InspectionID { get; set; }
    }
}