using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.WebUI.Extensions;
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
        private IEmailSender _emailSender;
        private ICartService _cartService;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
            ,IEmailSender emailSender
            ,ICartService cartService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _cartService = cartService;
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

            //hesap email ile onaylanmadan giriş yapmaya çalışırsa
            if(!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Lütfen Hesabınızı email ile onaylayınız.");
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
                var code =await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callBackurl = Url.Action("ConfirmEmail", "Account",new
                {
                    userId = user.Id,
                    token=code
                });

                //send email
                await _emailSender.SendEmailAsync(model.Email, 
                    "Hesabınızı onaylayınız", $"Lütfen email hesabınızı onaylamak için linke <a href='http://localhost:34373{callBackurl}'>tıklayınız</a>");


                TempData.Put("message", new ResultMessage()
                {
                    Title="Hesap Onayı",
                    Message="Eposta adresinize gelen link ile hesabınızı onaylaynız.",
                    Css="warning"
                });

                return RedirectToAction("Login", "Account");
            }

            //Hata mesajını ayrıntılı olacak
            ModelState.AddModelError("", "Bilinmeyen Hata Tekrar Deneyiniz");

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            TempData.Put("message", new ResultMessage()
            {
                Title = "Oturum Kapatıldı",
                Message = "Hesabınız güvenli bir şekilde sonlandırıldı .",
                Css = "warning"
            });

            return Redirect("~/");
        }

        //Mail Onayı
        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {
            if(userId == null || token == null)
            {
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Hesap Onayı",
                    Message = "HEsap onayı için bilgileriniz yanlış .",
                    Css = "danger"
                });

                return Redirect("~/"); 
            }

            var user = await _userManager.FindByIdAsync(userId);

            if(user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);


                if (result.Succeeded)
                {

                    //kullanıcı için cart(sepet) oluşsturma
                    _cartService.InitializeCart(user.Id);

                    TempData.Put("message", new ResultMessage()
                    {
                        Title = "Hesap Onayı",
                        Message = "Hesabınız başarıyla onaylanmıştır.",
                        Css = "success"
                    });

                    return RedirectToAction("Login");
                }
            }

            TempData.Put("message", new ResultMessage()
            {
                Title = "Hesap Onayı",
                Message = "Hesabınız onaylanamadı.",
                Css = "danger"
            });


            return View();
        }

        //Şifre Unutma
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>ForgotPassword (string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                //hata mesajı 
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Forget Message",
                    Message = "Bilgileriniz Hatalı",
                    Css = "danger"
                });

                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);

            if(user == null)
            {
                //hata mesajı verilebilir   "Böyle bir kullanıcı yok diye"...
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Forget Message",
                    Message = "Girdiğiniz Eposta adresi ile ilgili kullanıcı bulunamadı",
                    Css = "danger"
                });



                return View();
            }

            var code=await _userManager.GeneratePasswordResetTokenAsync(user);

            var callBackurl = Url.Action("ResetPassword", "Account", new
            {
                
                token = code
            });

            //send email
            await _emailSender.SendEmailAsync(email,
                "Reset Password", $"Parolanızı yenilemek için Linke  <a href='http://localhost:34373{callBackurl}'>tıklayınız</a>");

            //bilgilendirme mesajı
            TempData.Put("message", new ResultMessage()
            {
                Title = "Forget Message",
                Message = "Parola Yenilenmesi için hesabınıza mail gönderildi",
                Css = "warning"
            });

            return RedirectToAction("Login","Account");
        }

        //Parola Sıfırlama
        public IActionResult ResetPassword(string token)
        {

            if( token == null)
            {
                //Hata göster
                return RedirectToAction("Index","Home");
            } 

            var model= new ResetPasswordModel { Token = token };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user == null)
            {
                //Kullanıcı yok hatası wer
                return RedirectToAction("Index", "Home");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                //Succes bildirimi göster
                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }


        //Erişim Engellendi Sayfası
        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
