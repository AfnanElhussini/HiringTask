using HiringTask.DTO;
using HiringTask.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HiringTask.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public UserController(UserManager<User> UserManager, SignInManager<User> SignInManager)
        {
            this._userManager = UserManager;    
            this._signInManager = SignInManager;    
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var AppUser = new User()
            {
                Email = model.Email,
                UserName = model.UserName,
                ArabicName = model.ArabicName,
                EnglishName = model.EnglishName,
            };

            IdentityResult result = await _userManager.CreateAsync(AppUser, model.Password);
            if(result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Errors);
            }   
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var AppUser = await _userManager.FindByEmailAsync(model.Email);
            if(AppUser != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(AppUser, model.Password, false);
                if(result.Succeeded)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest("Password is incorrect");
                }
            }
            else
            {
                return BadRequest("Email is incorrect");
            }
        }
        [HttpPost]  
        public async Task<IActionResult> Logout()
        {
             await _signInManager.SignOutAsync();

            
            return Ok("Done");
           
          
        }


    }
}
