using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TodoListApp.WebApp.Models.ViewModels;

namespace TodoListApp.WebApp.Controllers;

/// <summary>
/// Controller for managing user accounts.
/// </summary>
[Authorize]
[Route("account")]
public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly IConfiguration configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountController"/> class.
    /// </summary>
    /// <param name="userManager">The user manager.</param>
    /// <param name="signInManager">The sign-in manager.</param>
    /// <param name="configuration">The configuration.</param>
    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.configuration = configuration;
    }

    /// <summary>
    /// GET: /account/register.
    /// </summary>
    /// <returns>The register view.</returns>
    [HttpGet]
    [Route("register")]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return this.View();
    }

    /// <summary>
    /// POST: /account/register.
    /// </summary>
    /// <param name="model">The register view model.</param>
    /// <returns>The result of the registration.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (this.ModelState.IsValid)
        {
            var user = new IdentityUser { Email = model.Email, UserName = model.Username };
            var result = await this.userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return this.RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return this.BadRequest(this.ModelState);
    }

    /// <summary>
    /// GET: /account/login.
    /// </summary>
    /// <returns>The login view.</returns>
    [HttpGet]
    [Route("login")]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return this.View();
    }

    /// <summary>
    /// POST: /account/login.
    /// </summary>
    /// <param name="model">The login view model.</param>
    /// <returns>The result of the login.</returns>
    [HttpPost]
    [Route("login")]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (this.ModelState.IsValid)
        {
            var result = await this.signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

            if (result.Succeeded)
            {
                var user = await this.userManager.FindByNameAsync(model.Username);
                var token = this.GenerateJwtToken(user);

                this.Response.Cookies.Append("Token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                });

                return this.RedirectToAction("Index", "TodoList");
            }

            this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        return this.View(model);
    }

    /// <summary>
    /// GET: /account/logout.
    /// </summary>
    /// <returns>The result of the logout.</returns>
    [HttpGet]
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        await this.signInManager.SignOutAsync();

        this.Response.Cookies.Delete("Token");

        return this.RedirectToAction(nameof(this.Login), "Account");
    }

    private string GenerateJwtToken(IdentityUser user)
    {
        var claims = new ClaimsIdentity(new Claim[]
        {
                new (ClaimTypes.NameIdentifier, user.Id),
                new (ClaimTypes.Name, user.UserName),
        });

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: this.configuration["Jwt:Issuer"],
            audience: this.configuration["Jwt:Audience"],
            claims: claims.Claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
