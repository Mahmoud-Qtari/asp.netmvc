
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChallengerCore.Models
{
    public class UploadFile
    {
        [Required]
        [Display(Name ="File Name")]
        public string Name { get; set; }
    }
}