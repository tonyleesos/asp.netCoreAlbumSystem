using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using prjAlbum.Models;
var builder = WebApplication.CreateBuilder(args);
// db 連線字串
var cnstr = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={builder.Environment.ContentRootPath}App_Data\\dbAlbum.mdf;Integrated Security=True;Trusted_Connection=True;";

// build mvc
builder.Services.AddMvc();
// 建立連線 設定model
builder.Services.AddDbContext<prjAlbum.Models.AlbumDbContext>(options => options.UseSqlServer(cnstr));
// 增加驗證方式，使用cookie驗證
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    // 瀏覽器限制cookie 只能經由HTTP(S)協定來存取
    option.Cookie.HttpOnly = true;
    // 未登入時會自動導到登入頁
    option.LoginPath = new PathString("/Home/Login");
    // 拒絕訪問路徑
    option.AccessDeniedPath = new PathString("/Home/NoAuthorization");
});
var app = builder.Build();

app.UseStaticFiles(); // 使用圖片資源
//// 會員驗證
app.UseCookiePolicy(); // 啟用 cookie 原則功能
app.UseAuthentication(); // 啟用身分識別
app.UseAuthorization(); // 啟用授權功能

app.MapControllerRoute(name: "default", pattern: "{Controller=Home}/{Action=Index}/{id?}");

app.Run();
