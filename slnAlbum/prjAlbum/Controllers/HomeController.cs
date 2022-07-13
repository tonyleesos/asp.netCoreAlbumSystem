using Microsoft.AspNetCore.Mvc;
using prjAlbum.Models;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace prjAlbum.Controllers
{
    public class HomeController : Controller
    {
        private AlbumDbContext _context;
        private string _path;
        public HomeController(AlbumDbContext context,IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _path = $"{hostEnvironment.WebRootPath}\\Album";
        }


        public IActionResult Index()
        {
            var hostAlbums = _context.TAlbums.OrderByDescending(x => x.FReleaseTime).Take(12).ToList();
            return View(hostAlbums);
        }

        public IActionResult AlbumCategory(int Cid=1)
        {
            ViewBag.CategoryName = _context.TCategories.FirstOrDefault(x => x.FCid == Cid).FCname;
            var albums = _context.TAlbums.Where(x => x.FCid == Cid).OrderByDescending(x => x.FReleaseTime).ToList();
            return View(albums);
        }

        public IActionResult AlbumUpload()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AlbumUpload(TAlbum album,IFormFile formFile)
        {
            TempData["error"] = "資料無法上傳，請記得上傳照片並檢視資料";
            if (ModelState.IsValid)
            {
                if(formFile != null)
                {
                    if(formFile.Length > 0)
                    {
                        // 照片上傳
                        string fileName = $"{Guid.NewGuid().ToString()}.jpg";
                        string savePath = $"{_path}\\{fileName}";
                        using(var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                        // 照片記錄寫入
                        album.FAlbum = fileName;
                        album.FReleaseTime = DateTime.Now;
                        _context.TAlbums.Add(album);
                        _context.SaveChanges();
                        TempData["success"] = "照片上傳成功";
                        return RedirectToAction("AlbumCategory",new {Cid = album.FCid});
                    }
                }
            }
            return View(album);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string uid, string pwd)
        {

            var member = _context.TMembers.FirstOrDefault(x=>x.FUid == uid && x.FPwd == pwd);
            if (member != null)
            {
                // 建立身分宣告
                IList<Claim> claims = new List<Claim>{
                    new Claim(ClaimTypes.Name, member.FUid),
                    new Claim(ClaimTypes.Role, member.FRole)
                };

                // 建立身分識別物件 指定帳號與角色
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true };
                // 進行登入動作 帶入身分識別物件
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                TempData["success"] = "登入成功!";
                return RedirectToAction("Index", "Member", member.FRole);
            }
            else
            {
                TempData["error"] = "帳密錯誤,請重新檢查";
                return View();
            }
        }
        // 會員登出
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
        public IActionResult NoAuthorization()
        {
            return View();
        }
    }
}
