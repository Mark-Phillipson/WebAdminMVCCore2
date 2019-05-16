using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    [RequireHttps]
    [Authorize(Roles = "Admin")]
    public class AspNetUsersController : Controller
    {
        private readonly IdentityDbContext _context;

        public AspNetUsersController(IdentityDbContext context)
        {
            _context = context;
        }

        // GET: AspNetUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.AspNetUsers.Include(u => u.AspNetUserRoles).ToListAsync());
        }

        public async Task<IActionResult> UsersAndRoles()
        {
            return View(await _context.Views_Assigned_Roles.OrderBy(v => v.Username).ThenBy(v => v.Role).ToListAsync());
        }

        [AllowAnonymous]
        public IActionResult AddRoles()
        {
            if (User.Identity.Name!="MPhillipson0@Gmail.com")
            {
                return NotFound();
            }

            var roleId = _context.AspNetRoles.Where(r => r.Name == "Developer").FirstOrDefault()?.Id;


            if (roleId== null )
            {
                AspNetRole aspNetRole = new AspNetRole();
                aspNetRole.Id = Guid.NewGuid().ToString();
                aspNetRole.ConcurrencyStamp = Guid.NewGuid().ToString();
                //aspNetRole.CreatedDate = DateTime.Now;
                aspNetRole.Name = "Developer";
                aspNetRole.NormalizedName = "DEVELOPER";
                //aspNetRole.Description = "Developer Application Role";
                _context.Add(aspNetRole);
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }

            }

            roleId = _context.AspNetRoles.Where(r => r.Name == "Admin").FirstOrDefault()?.Id;
            if (roleId== null )
            {
                var aspNetRole = new AspNetRole();
                aspNetRole.Id = Guid.NewGuid().ToString();
                aspNetRole.ConcurrencyStamp = Guid.NewGuid().ToString();
                //aspNetRole.CreatedDate = DateTime.Now;
                aspNetRole.Name = "Admin";
                aspNetRole.NormalizedName = "ADMIN";
                //aspNetRole.Description = "Admin Application Role";
                _context.Add(aspNetRole);
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }

            }
            var userId = _context.AspNetUsers.Where(u => u.Email == User.Identity.Name).FirstOrDefault()?.Id;
            roleId = _context.AspNetRoles.Where(r => r.Name == "Developer").FirstOrDefault()?.Id;

            if (userId== null )
            {
                return NotFound();
            }

            AspNetUserRole aspNetUserRole = new AspNetUserRole();
            aspNetUserRole.UserId = userId;
            aspNetUserRole.RoleId = roleId;
            _context.Add(aspNetUserRole);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            roleId = _context.AspNetRoles.Where(r => r.Name == "Admin").FirstOrDefault()?.Id;

            if (userId == null)
            {
                return NotFound();
            }

            aspNetUserRole = new AspNetUserRole();
            aspNetUserRole.UserId = userId;
            aspNetUserRole.RoleId = roleId;
            _context.Add(aspNetUserRole);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        // GET: AspNetUsers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //AspNetUserRole ur = new AspNetUserRole();
            var aspNetUser = await _context.AspNetUsers
                .Include("AspNetUserRoles.Role")
                .Include(u => u.AspNetUserLogins)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (aspNetUser == null)
            {
                return NotFound();
            }
            return View(aspNetUser);
        }
        // GET: AspNetUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AspNetUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccessFailedCount,ConcurrencyStamp,Email,EmailConfirmed,LockoutEnabled,LockoutEnd,Name,NormalizedEmail,NormalizedUserName,PasswordHash,PhoneNumber,PhoneNumberConfirmed,SecurityStamp,TwoFactorEnabled,UserName")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aspNetUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aspNetUser);
        }

        // GET: AspNetUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUser = await _context.AspNetUsers.SingleOrDefaultAsync(m => m.Id == id);
            if (aspNetUser == null)
            {
                return NotFound();
            }
            return View(aspNetUser);
        }

        // POST: AspNetUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,AccessFailedCount,ConcurrencyStamp,Email,EmailConfirmed,LockoutEnabled,LockoutEnd,Name,NormalizedEmail,NormalizedUserName,PasswordHash,PhoneNumber,PhoneNumberConfirmed,SecurityStamp,TwoFactorEnabled,UserName")] AspNetUser aspNetUser)
        {
            if (id != aspNetUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aspNetUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspNetUserExists(aspNetUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(aspNetUser);
        }

        // GET: AspNetUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUser = await _context.AspNetUsers
                .SingleOrDefaultAsync(m => m.Id == id);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            return View(aspNetUser);
        }

        // POST: AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var aspNetUser = await _context.AspNetUsers.SingleOrDefaultAsync(m => m.Id == id);
            _context.AspNetUsers.Remove(aspNetUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AspNetUserExists(string id)
        {
            return _context.AspNetUsers.Any(e => e.Id == id);
        }
    }
}
