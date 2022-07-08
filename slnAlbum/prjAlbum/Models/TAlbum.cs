using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace prjAlbum.Models
{
    public partial class TAlbum
    {
        [Display(Name = "編號")]
        public int FAlbumId { get; set; }
        [Display(Name = "分類名稱")]
        public int? FCid { get; set; }
        [Display(Name = "主題名稱")]
        [Required(ErrorMessage = "主題名稱必填")]
        public string? FTitle { get; set; }
        [Display(Name = "描述說明")]
        [Required(ErrorMessage = "描述說明必填")]
        public string? FDescription { get; set; }
        [Display(Name = "圖檔")]
        public string? FAlbum { get; set; }
        [Display(Name = "發布時間")]
        public DateTime? FReleaseTime { get; set; }
    }
}
