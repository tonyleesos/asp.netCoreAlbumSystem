using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace prjAlbum.Models
{
    public partial class TCategory
    {
        [Display(Name = "類別編號")]
        public int FCid { get; set; }
        [Display(Name = "類別名稱")]
        [Required(ErrorMessage = "類別名稱必填")]
        public string? FCname { get; set; }
    }
}
