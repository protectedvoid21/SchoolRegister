using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SchoolRegister.Pages; 

public class RegisterModel : PageModel {
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Email address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    public void OnGet() {
        Input = new InputModel();
    }

    public async Task<IActionResult> OnPostAsync() {
        if(!ModelState.IsValid) {
            return Page();
        }

        if(await userManager.FindByNameAsync(Input.UserName) != null) {
            ModelState.AddModelError(string.Empty, "User with this username already exists.");
            return Page();
        }

        IdentityUser user = new() {
            UserName = Input.UserName,
            Email = Input.Email,
        };

        IdentityResult result = await userManager.CreateAsync(user, Input.Password);
        if (!result.Succeeded) {
            foreach (var error in result.Errors) {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }

        await signInManager.SignInAsync(user, false);
        return RedirectToAction("Index", "Home");
    }
}