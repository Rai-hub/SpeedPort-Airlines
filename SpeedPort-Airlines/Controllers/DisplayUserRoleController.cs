using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpeedPort_Airlines.Models;
using SpeedPort_Airlines.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace SpeedPort_Airlines.Controllers
{
    public class DisplayUserRoleController : Controller
    {
        private readonly UserManager<SpeedPort_AirlinesUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DisplayUserRoleController(UserManager<SpeedPort_AirlinesUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var displayUserRoleViewModels = new List<DisplayUserRoleViewModel>();
            foreach (SpeedPort_AirlinesUser user in users)
            {
                var thisViewModel = new DisplayUserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = await GetUserRoles(user)
                };
                displayUserRoleViewModels.Add(thisViewModel);
            }
            return View(displayUserRoleViewModels);
        }

        private async Task<IEnumerable<string>> GetUserRoles(SpeedPort_AirlinesUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }
    }
}
