using Logis.AspNet.Models;
using Logis.AspNet.ViewModel.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Logis.AspNet.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager,SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
           _userManager = userManager;
            _signInManager = signInManager;
           _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task< IActionResult> Register(RegisterVm register)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            User user = new User()
            {
                Name = register.Name,
                Email = register.Email,
                Surname = register.Surname,
                UserName = register.Username,
            };
            var result= await  _userManager.CreateAsync(user);
            if(!result.Succeeded)
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError("",item.Description);
                }
            }
          await   _signInManager.SignInAsync(user, isPersistent: false);
            
            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task< IActionResult> Login(LoginVm login)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            User user;
            if(login.UsernameorEmail.Contains("@"))
            {
                user=await _userManager.FindByEmailAsync(login.UsernameorEmail);
            }
            else
            {
                user=await _userManager.FindByNameAsync(login.UsernameorEmail);
            }
            if (user == null)
            {
                ModelState.AddModelError("", "username or password is failed");
            }
            var result=await _signInManager.CheckPasswordSignInAsync(user,login.Password,true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "please,try again");
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "username or password is failed");
            }
            await _signInManager.SignInAsync(user, login.RememberMe);
            return RedirectToAction("Index", "Home");
        }
        public async Task< IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
