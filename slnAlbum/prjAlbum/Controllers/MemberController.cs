using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using prjAlbum.Models;

namespace prjAlbum.Controllers
{
    [Authorize(Roles = "Member")]
    public class MemberController : Controller
    {
        private AlbumDbContext _context;
        private string _path;
        public MemberController(AlbumDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _path = $"{hostEnvironment.WebRootPath}\\Album";
        }
        public IActionResult Index()
        {
            var categories = _context.TCategories.OrderByDescending(x => x.FCid).ToList();
            return View(categories);
        }
        public IActionResult AlbumCategory(int Cid = 1)
        {
            ViewBag.CategoryName = _context.TCategories.FirstOrDefault(x=>x.FCid == Cid).FCname;
            var albums = _context.TAlbums.Where(x => x.FCid == Cid).OrderByDescending(x => x.FReleaseTime).ToList();
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
            return RedirectToAction("AlbumCategory", new { Cid = album.FCid });
        }
    }
}
