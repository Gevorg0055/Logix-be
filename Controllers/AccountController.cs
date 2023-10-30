using LogixTask.Models;
using LogixTask.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LogixTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<WebUser> _userManager;
        private readonly SignInManager<WebUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _jwtSettings;

        public AccountController(UserManager<WebUser> _userManager, SignInManager<WebUser> _signInManager, RoleManager<IdentityRole> _roleManager, IConfiguration _jwtSettings)
        {
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._roleManager = _roleManager;
            this._jwtSettings = _jwtSettings;
        }

        [HttpPost("SignUp")]
        public async  Task<IActionResult> SignUp(RegistrationViewModel registrationViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var validatedAddress = addressValidating(registrationViewModel.Address);

                    var user = new WebUser 
                    { 
                        UserName = registrationViewModel.UserName, 
                        Email = registrationViewModel.Email, 
                        PhoneNumber = registrationViewModel.PhoneNumber,
                        FirstName = registrationViewModel.FirstName,
                        LastName = registrationViewModel.LastName,
                        FullName = registrationViewModel.FirstName + " " + registrationViewModel.LastName,
                        DateOfBirth = registrationViewModel.DateOfBirth,
                        Address = validatedAddress.ToUpper()
                    };

                    var result = await _userManager.CreateAsync(user, registrationViewModel.Password);
                    if (result.Succeeded)
                    {
                        await _roleManager.CreateAsync(new IdentityRole("WebUser"));
                        await _userManager.AddToRoleAsync(user, "WebUser");
                        await _signInManager.SignInAsync(user, true);

                        var token = GenerateJwtToken(user.Email);
                        return Ok(new { Token = token });
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return BadRequest();
        }

        [HttpGet]
        public string addressValidating(string userAddress)
        {
            var splitedAddress = userAddress.Replace("#","").Replace(".", "").Split(" ");
            string resultAddress = null;
            for (int i = 0; i < splitedAddress.Length; i++)
            {
                var address = splitedAddress[i];
                switch (address)
                {
                    case "Avenue":
                        address = "AVE";
                        break;

                    case "Street":
                        address = "ST";
                        break;

                    case "Number":
                        address = "";
                        break;

                    case "Boulevard":
                        address = "BLVD";
                        break;

                    case "No":
                        address = "";
                        break;
                }

                if (resultAddress == null)
                {
                    resultAddress += address;
                }
                else
                {
                    resultAddress += " " + address;
                }
            }

            return resultAddress;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
           
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
                    if (user != null)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, isPersistent: true, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                            if (!string.IsNullOrEmpty(loginViewModel.ReturnUrl) && Url.IsLocalUrl(loginViewModel.ReturnUrl))
                            {
                                return Redirect(loginViewModel.ReturnUrl);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Dashboard");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid Password");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid UserName");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return BadRequest();
        }

        private string GenerateJwtToken(string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings["JwtSettings:SecurityKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                _jwtSettings["JwtSettings:Issuer"],
                _jwtSettings["JwtSettings:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_jwtSettings["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
