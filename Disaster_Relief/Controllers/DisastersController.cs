using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Disaster_Relief.Models;
using Microsoft.AspNetCore.Http;

namespace Disaster_Relief.Controllers
{
    public class DisastersController : Controller
    {
        private readonly Disasters_Context _context;

        public DisastersController(Disasters_Context context)
        {
            _context = context;
        }

        // GET: Disasters
        public async Task<IActionResult> Index()
        {
            //code attribution
            //this method was taken from benjii.me
            //https://benjii.me/2016/07/using-sessions-and-httpcontext-in-aspnetcore-and-mvc-core/
            //Ben Cull
            //https://bencull.com/
            //(Cull, 2016)
            if (HttpContext.Session.GetString("LoggedIn") != "Yes")
            {
                //code attribution
                //this method was taken DotNetTricks
                //https://www.dotnettricks.com/learn/mvc/return-view-vs-return-redirecttoaction-vs-return-redirect-vs-return-redirecttoroute
                //Shailendra Chauhan
                //https://www.dotnettricks.com/mentors/shailendra-chauhan
                //(Chauhan, 2022)
                return Redirect("/Users/Login");
            }
            return View(await _context.Disaster.ToListAsync());
        }

        // GET: Disasters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetString("LoggedIn") != "Yes")
            {
                return Redirect("/Users/Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var disaster = await _context.Disaster
                .FirstOrDefaultAsync(m => m.ID == id);
            if (disaster == null)
            {
                return NotFound();
            }

            return View(disaster);
        }

        // GET: Disasters/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("LoggedIn") != "Yes")
            {
                return Redirect("/Users/Login");
            }
            return View();
        }

        // POST: Disasters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Type,Description,Location,SDate,EDate,AidNeeded")] Disaster disaster)
        {
            if (ModelState.IsValid)
            {
                _context.Add(disaster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DisasterL));
            }
            return View(disaster);
        }

        // GET: Disasters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("LoggedIn") != "Yes")
            {
                return Redirect("/Users/Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var disaster = await _context.Disaster.FindAsync(id);
            if (disaster == null)
            {
                return NotFound();
            }
            return View(disaster);
        }

        // POST: Disasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Type,Description,Location,SDate,EDate,AidNeeded")] Disaster disaster)
        {
            if (id != disaster.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(disaster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DisasterExists(disaster.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(DisasterL));
            }
            return View(disaster);
        }

        // GET: Disasters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("LoggedIn") != "Yes")
            {
                return Redirect("/Users/Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var disaster = await _context.Disaster
                .FirstOrDefaultAsync(m => m.ID == id);
            if (disaster == null)
            {
                return NotFound();
            }

            return View(disaster);
        }

        // POST: Disasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var disaster = await _context.Disaster.FindAsync(id);
            _context.Disaster.Remove(disaster);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DisasterL));
        }

        private bool DisasterExists(int id)
        {
            return _context.Disaster.Any(e => e.ID == id);
        }


        public async Task<IActionResult> DisasterL()
        {
            if (HttpContext.Session.GetString("LoggedIn") != "Yes")
            {
                return Redirect("/Users/Login");
            }
            return View(await _context.Disaster.ToListAsync());
        }
        public async Task<IActionResult> Allocate(int? id)
        {
            var disaster = await _context.Disaster
               .FirstOrDefaultAsync(m => m.ID == id);
            HttpContext.Session.SetString("Location", disaster.Location);
            HttpContext.Session.SetString("ID", disaster.ID.ToString());
            return Redirect("/GoodsDonations/Index");
        }
        public async Task<IActionResult> AllocateM(int? id)
        {
            var disaster = await _context.Disaster
               .FirstOrDefaultAsync(m => m.ID == id);
            HttpContext.Session.SetString("Location", disaster.Location);
            HttpContext.Session.SetString("ID", disaster.ID.ToString());
            return Redirect("/MoneyDonations/Index");


        }
        public async Task<IActionResult> disasterA(Disaster disa)
        {
            if (HttpContext.Session.GetString("LoggedIn") != "Yes")
            {
                return Redirect("/Users/Login");
            }
            if (HttpContext.Session.GetString("Money") == "No")
            {
                ViewBag.MessageP = "NOT ENOUGH MONEY FOR PURCHASE!!!";
            }
            ViewBag.Message = HttpContext.Session.GetString("ID");
            //code attribution
            //this method was taken stackoverflow
            //https://stackoverflow.com/questions/68987850/how-to-only-display-items-created-by-the-current-logged-in-user-only-in-asp-net
            //Rena
            //https://stackoverflow.com/users/11398810/rena
            //(Rena, 2021)
            var model = await _context.Disaster.Where(a => a.SDate <= DateTime.Today && a.EDate >= DateTime.Today)
                           .ToListAsync();

            return View(model);
        }
        public async Task<IActionResult> disasterP(int? id)
        {
            var disaster = await _context.Disaster
               .FirstOrDefaultAsync(m => m.ID == id);
            HttpContext.Session.SetString("Location", disaster.Location);
            HttpContext.Session.SetString("ID", disaster.ID.ToString());
            return Redirect("/GoodsDonations/PGoods");
        }
    }
}
