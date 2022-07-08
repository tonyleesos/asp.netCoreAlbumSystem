using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace prjAlbum.Models
{
    public partial class TMember
    {
        [Display(Name = "帳號")]
        [Required(ErrorMessage = "帳號必填")]
        public string FUid { get; set; } = null!;
        [Display(Name = "密碼")]
        [Required(ErrorMessage = "密碼必填")]
        public string? FPwd { get; set; }
        [Display(Name = "會員姓名")]
        [Required(ErrorMessage = "會員姓名必填")]
        public string? FName { get; set; }
        [Display(Name = "信箱")]
        [Required(ErrorMessage = "信箱必填")]
        [EmailAddress(ErrorMessage = "必須符合信箱格式")]
        public string? FMail { get; set; }
        [Display(Name = "角色")]
        public string? FRole { get; set; }
    }
}
