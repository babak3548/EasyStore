using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer.EF;
using DataLayer.Contract;
using Utility;

namespace AdminPanel.Controllers
{
    public class UsersController : BaseController
    {
        private readonly OnlineShopping _context;

        public UsersController(OnlineShopping context)
        {
            _context = context;
        }
        public async Task<IActionResult> LoginView()
        {
            //var onlineShopping = _context.User.Include(u => u.FkRoleNavigation);
            return View();
        }
        public async Task<IActionResult> Login(string mobile, string password)
        {
            var _entity =await _context.User.FirstOrDefaultAsync(u=>u.Mobile== mobile);
            if (_entity != null && _entity.Password == password.MD5Hash() && _entity.Ative != false && _entity.FkRole == 1)//باید ادمین باشد
            {
              var  uc = new UserContract();
                uc.Mobile = _entity.Mobile;
                uc.Name = _entity.Name ;
               
                uc.FK_Role = _entity.FkRole;
                uc.Ative = _entity.Ative;
                CurrentUserContract = uc;
               return RedirectToAction("Index","Home");
            }
            else
            {
                return RedirectToAction("LoginView");
            }
                
        }
        public async Task<IActionResult> LogOut()
        {
           
            CurrentUserContract = null;
            return RedirectToAction("LoginView");
        }
        // GET: Users
        public async Task<IActionResult> Index()
        {
            var onlineShopping = _context.User.Include(u => u.FkRoleNavigation);
            return View(await onlineShopping.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.FkRoleNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["FkRole"] = new SelectList(_context.Role, "Id", "Id");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Mobile,RegisterDate,FkRole,IpComputerCreator,CurrentInvoice,Ative,AtivationCode,Password,TempPassword,IpComputerLast,Family,Tel,CityName,Address,FkProvince,IpComputer")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkRole"] = new SelectList(_context.Role, "Id", "Id", user.FkRole);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["FkRole"] = new SelectList(_context.Role, "Id", "Id", user.FkRole);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Mobile,RegisterDate,FkRole,IpComputerCreator,CurrentInvoice,Ative,AtivationCode,Password,TempPassword,IpComputerLast,Family,Tel,CityName,Address,FkProvince,IpComputer")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (user.Password.Length<12)
                    {
                        user.Password = user.Password.MD5Hash();
                    }
                  
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            ViewData["FkRole"] = new SelectList(_context.Role, "Id", "Id", user.FkRole);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.FkRoleNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
