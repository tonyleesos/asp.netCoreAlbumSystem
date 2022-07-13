using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using prjAlbum.Models;

namespace prjAlbum.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private AlbumDbContext _context;
        private string _path;
        public AdminController(AlbumDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _path = $"{hostEnvironment.WebRootPath}\\Album";
        }
        public IActionResult Index()
        {
            var categories = _context.TCategories.OrderByDescending(x => x.FCid).ToList();
            return View(categories);
        }
        public IActionResult CategoryDelete(int Cid)
        {
            var category = _context.TCategories.FirstOrDefault(x => x.FCid == Cid); // 取的分類
            var albums = _context.TAlbums.Where(x=>x.FCid == Cid); // 該分類所有照片
            // 刪除圖檔
            foreach(var album in albums)
            {
                System.IO.File.Delete($"{_path}\\{album.FAlbum}");
            }
            // 刪除該分類與該分類的所有照片
            _context.TAlbums.RemoveRange(albums);
            _context.TCategories.Remove(category);
            _context.SaveChanges();
            TempData["success"] = "相簿分類刪除成功";
            return RedirectToAction("Index");
        }
        public IActionResult CategoryCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CategoryCreate(TCategory category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.TCategories.Add(category);
                    _context.SaveChanges();
                    TempData["success"] = "相簿分類新增成功";
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    TempData["error"] = "相簿分類新增失敗";
                }
            }
            return View(category);
        }
        public IActionResult CategoryEdit(int Cid)
        {
            var category = _context.TCategories.FirstOrDefault(x => x.FCid == Cid); // 取的分類
            return View(category);
        }
        [HttpPost]
        public IActionResult CategoryEdit(TCategory categoryData)
        {
            if (ModelState.IsValid)
            {
                try
                {                   
                    var category = _context.TCategories.Find(categoryData.FCid);
                    category.FCname = categoryData.FCname;
                    _context.SaveChanges();
                    TempData["success"] = "相簿分類修改成功";
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    TempData["error"] = "相簿分類無法修改，請重新檢視修改資料";
                }
            }
            return View(categoryData);
        }
        public IActionResult AlbumCategory(int Cid = 1)
        {
            ViewBag.CategoryName = _context.TCategories.FirstOrDefault(x => x.FCid == Cid).FCname; // 取得分類名稱
            var albums = _context.TAlbums.Where(x => x.FCid == Cid).OrderByDescending(y => y.FReleaseTime).ToList();
            return View(albums);
        }
        public IActionResult AlbumDelete(int AlbumId)
        {
            var album = _context.TAlbums.Where(x => x.FAlbumId == AlbumId).FirstOrDefault(); // 該分類所有照片
            // 刪除圖檔
            System.IO.File.Delete($"{_path}\\{album.FAlbum}");
            
            // 刪除該分類與該分類的所有照片
            _context.TAlbums.RemoveRange(album);
            _context.SaveChanges();
            TempData["success"] = "照片刪除成功";
            return RedirectToAction("AlbumCategory",new {Cid = album.FCid});
        }
        public IActionResult MemberList()
        {           
            var members = _context.TMembers.Where(x => x.FRole == "Member").ToList();
            return View(members);
        }
        public IActionResult MemberCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult MemberCreate(TMember member)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    member.FRole = "Member";
                    _context.TMembers.Add(member);
                    _context.SaveChanges();
                    TempData["success"] = "會員新增成功";
                    return RedirectToAction("MemberList");

                }
                catch (Exception ex)
                {
                    TempData["error"] = "會員新增失敗，帳號可能重複";
                }
            }
            return View(member);
        }
        public IActionResult MemberEdit(string Uid)
        {
            var member = _context.TMembers.FirstOrDefault(x=>x.FUid == Uid);
            return View(member);
        }
        [HttpPost]
        public IActionResult MemberEdit(TMember memberData)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var member = _context.TMembers.Find(memberData.FUid);
                    member.FPwd = memberData.FPwd;
                    member.FName = memberData.FName;
                    member.FMail = member.FMail;
                    _context.SaveChanges();
                    TempData["success"] = "會員修改成功";
                    return RedirectToAction("MemberList");

                }
                catch (Exception ex)
                {
                    TempData["error"] = "會員修改失敗，請重新檢視修改資料";
                }
            }
            return View(memberData);
        }
        public IActionResult MemberDelete(string Uid)
        {
            var member = _context.TMembers.Where(x => x.FUid == Uid).FirstOrDefault(); // 該分類所有照片
            _context.TMembers.RemoveRange(member);
            _context.SaveChanges();
            TempData["success"] = "會員刪除成功";
            return RedirectToAction("MemberList");
        }
    }
}
