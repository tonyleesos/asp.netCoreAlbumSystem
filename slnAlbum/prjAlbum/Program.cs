using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using prjAlbum.Models;
var builder = WebApplication.CreateBuilder(args);
// db �s�u�r��
var cnstr = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={builder.Environment.ContentRootPath}App_Data\\dbAlbum.mdf;Integrated Security=True;Trusted_Connection=True;";

// build mvc
builder.Services.AddMvc();
// �إ߳s�u �]�wmodel
builder.Services.AddDbContext<prjAlbum.Models.AlbumDbContext>(options => options.UseSqlServer(cnstr));
// �W�[���Ҥ覡�A�ϥ�cookie����
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    // �s��������cookie �u��g��HTTP(S)��w�Ӧs��
    option.Cookie.HttpOnly = true;
    // ���n�J�ɷ|�۰ʾɨ�n�J��
    option.LoginPath = new PathString("/Home/Login");
    // �ڵ��X�ݸ��|
    option.AccessDeniedPath = new PathString("/Home/NoAuthorization");
});
var app = builder.Build();

app.UseStaticFiles(); // �ϥιϤ��귽
//// �|������
app.UseCookiePolicy(); // �ҥ� cookie ��h�\��
app.UseAuthentication(); // �ҥΨ����ѧO
app.UseAuthorization(); // �ҥα��v�\��

app.MapControllerRoute(name: "default", pattern: "{Controller=Home}/{Action=Index}/{id?}");

app.Run();
