using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InspectionApplication.Models
{
    [Table("Logs")]
    public class Logs
    {
        [Key]
        public int LogsID { get; set; }

        public int InputPersonID { get; set; }

        [Required]
        public DateTime InputDate { get; set; }

        [Required]
        [StringLength(500)]
        public string LogInfo { get; set; }
    }
}
