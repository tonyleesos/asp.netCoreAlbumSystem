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

    }
}
