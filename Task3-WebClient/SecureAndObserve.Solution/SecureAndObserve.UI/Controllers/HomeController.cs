using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureAndObserve.Core.Domain.IdentityEntities;
using SecureAndObserve.Core.DTO;
using SecureAndObserve.Core.Enums;
using SecureAndObserve.Core.ServiceContracts;
using SecureAndObserve.Core.Services;
using SecureAndObserve.Infrastructure.DbContext;
using SecureAndObserve.UI.Filters;

namespace SecureAndObserve.UI.Controllers
{
    [Route("[controller]")]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IGuardExstensionsService _guardExstensionsService;

        private readonly ApplicationDbContext _context;

        public HomeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, ApplicationDbContext context, IGuardExstensionsService guardExstensionsService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _guardExstensionsService = guardExstensionsService;
        }

        [HttpGet]
        [Route("Login")]
        [Route("/")]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        [TypeFilter(typeof(AuthFilter))]
        public async Task<IActionResult> Login(LoginDTO loginDTO, string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View(loginDTO);
            }
            ApplicationUser? user = await _context.Users.FirstOrDefaultAsync(x => x.UserNickname == loginDTO.Nickname);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (user != null)
                    {
                        //Owner
                        if (await _userManager.IsInRoleAsync(user, UserTypeOptions.Owner.ToString()))
                        {
                            return RedirectToAction("Index", "Owner", new { area = "Owner" });
                        }
                        //Guard
                        if (await _userManager.IsInRoleAsync(user, UserTypeOptions.Guard.ToString()))
                        {
                            return RedirectToAction("Index", "Guard", new { area = "Guard" });
                        }
                        //Admin
                        if (await _userManager.IsInRoleAsync(user, UserTypeOptions.Admin.ToString()))
                        {
                            return RedirectToAction("Territories", "Admin", new { area = "Admin" });
                        }
                    }

                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return LocalRedirect(ReturnUrl);
                    }
                }
            }

            ModelState.AddModelError("Login", "Invalid nickname or password");
            return View(loginDTO);
        }

        [HttpGet]
        [Route("Register")]
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View(registerDTO);
            }

            ApplicationUser user = new ApplicationUser() { Email = registerDTO.Email, PhoneNumber = registerDTO.Phone, UserName = registerDTO.PersonName, UserSurname = registerDTO.PersonSurname, UserNickname = registerDTO.PersonNickname };
            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded)
            {
                //Check status of radio button
                if (registerDTO.UserType.ToString() == UserTypeOptions.Guard.ToString())
                {
                    //Create 'Guard' role
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.Guard.ToString()) is null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole() { Name = UserTypeOptions.Guard.ToString() };
                        await _roleManager.CreateAsync(applicationRole);
                    }

                    //Add the new user into 'Guard' role

                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Guard.ToString());

                    //Adding the GuardExstentions

                    GuardExstensionsAddRequest guardExstensionsAddRequest = new GuardExstensionsAddRequest();
                    string registerUserId = _context.Users.Where(x => x.UserNickname == registerDTO.PersonNickname)
                                .Select(x => x.Id)
                                .FirstOrDefault()
                                .ToString();
                    Guid registerUserIdGuid = new Guid(registerUserId);
                    guardExstensionsAddRequest.UserId = registerUserIdGuid;
                    string? corporalRankId = _context.Ranks.Where(x => x.Name == "Corporal").Select(x => x.Id).FirstOrDefault().ToString();
                    Guid corporalRankGuid = new Guid(corporalRankId);
                    guardExstensionsAddRequest.RankId = corporalRankGuid;

                    GuardExstensionsResponse guardExstensionsResponse = await _guardExstensionsService.AddGuardExstensions(guardExstensionsAddRequest);

                }
                if (registerDTO.UserType.ToString() == UserTypeOptions.Owner.ToString())
                {
                    //Create 'Owner' role
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.Owner.ToString()) is null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole() { Name = UserTypeOptions.Owner.ToString() };
                        await _roleManager.CreateAsync(applicationRole);
                    }

                    //Add the new user into 'Owner' role

                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Owner.ToString());
                }
                if (registerDTO.UserType.ToString() == UserTypeOptions.Owner.ToString())
                {
                    //Create 'Owner' role
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.Owner.ToString()) is null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole() { Name = UserTypeOptions.Owner.ToString() };
                        await _roleManager.CreateAsync(applicationRole);
                    }

                    //Add the new user into 'Owner' role

                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Owner.ToString());
                }
                if (registerDTO.UserType.ToString() == UserTypeOptions.Admin.ToString())
                {
                    //Create 'Admin' role
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.Admin.ToString()) is null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole() { Name = UserTypeOptions.Admin.ToString() };
                        await _roleManager.CreateAsync(applicationRole);
                    }

                    //Add the new user into 'Admin' role

                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Admin.ToString());
                }

                //Sign in
                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction(nameof(HomeController.Login), "Home");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }

                return View(registerDTO);
            }
        }
    }
}
