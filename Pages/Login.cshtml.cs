using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SchoolRegister.Pages; 

public class LoginModel : PageModel {
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
    }

    public LoginModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }
        
    public void OnGet() {
        Input = new();
    }

    public async Task<IActionResult> OnPostAsync() {
        if(!ModelState.IsValid) {
            return Page();
        }

        var user = await userManager.FindByNameAsync(Input.UserName);
        if(user == null) {
            return Page();
        }

        var result = await signInManager.PasswordSignInAsync(user, Input.Password, false, false);
        if(result.Succeeded) {
            return RedirectToAction("Index", "Home");
        }
        return Page();
    }
}