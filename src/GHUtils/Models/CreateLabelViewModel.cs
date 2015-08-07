using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GHUtils.Models
{
    public class CreateLabelViewModel
    {
        [Required]
        public string Repositories { get; set; }
        [Required]
        public string Label { get; set; }
        [Required]
        public string Color { get; set; }
    }
}
