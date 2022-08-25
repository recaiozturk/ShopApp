using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Models;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {

        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //Kullanıcı girişi
        public IActionResult Login(string returnUrl=null)
        {
            return View(new LoginModel()
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            //return url boş ise ana dizine yönlendirsin
            

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user =await _userManager.FindByEmailAsync(model.Email);

            if(user == null)
            {
                ModelState.AddModelError("", "Bu email ile daha önce hesap oluşturulmamış.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl ?? "~/");
                 

            }

            ModelState.AddModelError("", "Email ve ya Parola Yanlış");

            return View(model);
        }

        //Kullanıcı Oluşturma
        public IActionResult Register()
        {
            return View(new RegisterModel());
        }

        [HttpPost]
        public async  Task<IActionResult> Register(RegisterModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser
            {
                FullName = model.FullName,
                UserName = model.UserName,
                Email = model.Email

            };

            var result=await _userManager.CreateAsync(user,model.Password);

            if (result.Succeeded)
            {
                //generate token
                //send email
                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError("", "Bilinmeyen Hata Tekrar Deneyiniz");

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }


    }
}
