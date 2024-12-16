using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webbandothethao.Models;
using Webbandothethao.ViewModels;
using System.Diagnostics;
using System.Security.Claims;
using System.Net;
using System.Data;

namespace QLcaulacbosinhvien.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly DataContext _context;

    public AccountController(ILogger<AccountController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    #region Login

    [HttpGet]
    public IActionResult Login(string? ReturnUrl)
    {
        ViewBag.ReturnUrl = ReturnUrl;
        return View();
    }


    // Phương thức POST để xử lý đăng nhập
    [HttpPost]
    public async Task<IActionResult> Login(tblUser model, string? ReturnUrl)
    {
        ViewBag.ReturnUrl = ReturnUrl;
        if (ModelState.IsValid)
        {
            var Userr = _context.tblUsers
                .Where(a => a.Status == true)
                .FirstOrDefault(u => u.email == model.email);


            if (Userr == null)
            {
                ModelState.AddModelError("loi", "Tài khoản không tồn tại");

            }
            else
            {

                if (Userr.password != model.password)
                {
                    ModelState.AddModelError("loi", "Sai mật khẩu.");

                }
                else
                {
                    var claims = new List<Claim> {
                                new Claim(ClaimTypes.Email, Userr.email),
                                new Claim("ID", Userr.id.ToString()),
                                new Claim(ClaimTypes.Name, Userr.name),

                                //claim - role động
                                new Claim(ClaimTypes.Role, Userr.role),

                            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(claimsPrincipal);

                    if (Userr.role == "Admin")
                    {
                        return RedirectToAction("Index", "Admin"); // Điều hướng đến trang admin
                    }
                    else if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return Redirect("/");
                    }
                }
            }
        }

        return View();
    }
    #endregion


    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // [POST] /Account/Register
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Kiểm tra email đã tồn tại chưa
            var existingUser = _context.tblUsers.SingleOrDefault(u => u.email == model.email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng.");
                return View(model);
            }
            // Tạo tài khoản mới
            var user = new tblUser
            {
                name = model.name,
                email = model.email,
                password = model.password, // Băm mật khẩu trong thực tế
                phone = model.phone,
                address = model.address,
                created_at = DateTime.Now,
                updated_at = DateTime.Now,
                role = "User",
                Status = true
            };

            _context.tblUsers.Add(user);
            await _context.SaveChangesAsync();

            // Tự động đăng nhập sau khi đăng ký
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.name),
                new Claim(ClaimTypes.Email, user.email),
                new Claim("ID", user.id.ToString()),
                new Claim(ClaimTypes.Role, user.role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(claimsPrincipal);

            return RedirectToAction("Index", "Home");
        }

        return View(model);
    }



    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}