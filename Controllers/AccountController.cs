using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.Models;
using SchoolRegister.Models.ViewModels;

namespace SchoolRegister.Controllers;

[Authorize(Roles = "Admin")]
public class AccountController : Controller {
    private readonly UserManager<AppUser> userManager;
    private readonly SignInManager<AppUser> signInManager;
    private readonly RoleManager<IdentityRole<int>> roleManager;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole<int>> roleManager) {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.roleManager = roleManager;
    }

    public async Task<IActionResult> Logout() {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    #region Users

    public IActionResult UserList() {
        IEnumerable<AppUser> users = userManager.Users.AsEnumerable();
        return View(users);
    }

    /*[HttpGet]
    public async Task<IActionResult> CreateUser() {
        var userModel = new CreateUserViewModel {
            RoleList = new()
        };

        IAsyncEnumerable<IdentityRole> roles = roleManager.Roles.AsAsyncEnumerable();
        await foreach (var role in roles) {
            userModel.RoleList.Add(new RoleUserViewModel {
                Id = role.Id,
                Name = role.Name
            });
        }

        return View(userModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserViewModel userModel) {
        if (!ModelState.IsValid) {
            return View(userModel);
        }

        if (await userManager.Users.AnyAsync(u => u.UserName == userModel.UserName)) {
            ModelState.AddModelError("", "User with this username already exists");
            return View(userModel);
        }

        var user = new IdentityUser {
            UserName = userModel.UserName,
        };
        IdentityResult result = await userManager.CreateAsync(user, userModel.Password);
        if (result.Succeeded) {
            return RedirectToAction("UserList");
        }

        foreach (var error in result.Errors) {
            ModelState.AddModelError("", error.Description);
        }
        return View(userModel);
    }*/

    #endregion

    #region Roles

    public async Task<IActionResult> RoleList() {
        List<RoleViewModel> roleModelList = new();
        var roleList = roleManager.Roles.AsAsyncEnumerable();
        await foreach(var role in roleList) {
            var usersInRole = await userManager.GetUsersInRoleAsync(role.Name);
            roleModelList.Add(new RoleViewModel {
                Id = role.Id,
                Name = role.Name,
                UserCount = usersInRole.Count,
            });
        }

        return View(roleModelList);
    }

    [HttpGet]
    public IActionResult CreateRole() => View();

    [HttpPost]
    public async Task<IActionResult> CreateRole(CreateRoleViewModel roleModel) {
        if (!ModelState.IsValid) {
            return View(roleModel);
        }

        if (await roleManager.RoleExistsAsync(roleModel.Name)) {
            ModelState.AddModelError("", "Role with this name already exists");
            return View(roleModel);
        }

        IdentityRole<int> role = new IdentityRole<int> {
            Name = roleModel.Name
        };
        await roleManager.CreateAsync(role);
        return RedirectToAction("RoleList");
    }

    [HttpGet]
    public async Task<IActionResult> EditRole(string roleId) {
        IdentityRole<int> role = await roleManager.FindByIdAsync(roleId);
        return View(role);
    }

    [HttpPost]
    public async Task<IActionResult> EditRole(IdentityRole<int> role) {
        if (!ModelState.IsValid) {
            return View(role);
        }
        if(await roleManager.RoleExistsAsync(role.Name)) {
            ModelState.AddModelError("", "Role with this name already exists");
            return View(role);
        }

        await roleManager.UpdateAsync(role);
        return RedirectToAction("RoleList");
    }

    public async Task<IActionResult> DeleteRole(string roleId) {
        IdentityRole<int> role = await roleManager.FindByIdAsync(roleId);
        await roleManager.DeleteAsync(role);
        return RedirectToAction("RoleList");
    }
    
    #endregion
}