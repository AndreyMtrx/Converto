using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Converto.Data.Models
{
    public class Conversion
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ConversionGuid { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string FromFormat { get; set; }
        [Required]
        public string ToFormat { get; set; }
        public DateTime ConversionDate { get; set; }
    }
}
