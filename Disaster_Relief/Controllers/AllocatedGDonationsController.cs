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
    public class AllocatedGDonationsController : Controller
    {
        private readonly Goods_Context _context;

        public AllocatedGDonationsController(Goods_Context context)
        {
            _context = context;
        }

        // GET: AllocatedGDonations
        public async Task<IActionResult> Index()
        {
            //check if user is logged in
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

            return View(await _context.AllocatedGDonations.ToListAsync());
        }

        // GET: AllocatedGDonations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //check if user is logged in
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
            if (id == null)
            {
                return NotFound();
            }

            var allocatedGDonations = await _context.AllocatedGDonations
                .FirstOrDefaultAsync(m => m.DonationID == id);
            if (allocatedGDonations == null)
            {
                return NotFound();
            }

            return View(allocatedGDonations);
        }

        // GET: AllocatedGDonations/Create
        public IActionResult Create()
        {
            //check if user is logged in
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
            return View();
        }

        // POST: AllocatedGDonations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DonationID,DonationName,Quantity,Description,DonationDate,Catagory,Location")] AllocatedGDonations allocatedGDonations)
        {
            if (ModelState.IsValid)
            {
                _context.Add(allocatedGDonations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(allocatedGDonations);
        }

        // GET: AllocatedGDonations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //check if user is logged in
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
            if (id == null)
            {
                return NotFound();
            }

            var allocatedGDonations = await _context.AllocatedGDonations.FindAsync(id);
            if (allocatedGDonations == null)
            {
                return NotFound();
            }
            return View(allocatedGDonations);
        }

        // POST: AllocatedGDonations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DonationID,DonationName,Quantity,Description,DonationDate,Catagory,Location")] AllocatedGDonations allocatedGDonations)
        {
            if (id != allocatedGDonations.DonationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(allocatedGDonations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AllocatedGDonationsExists(allocatedGDonations.DonationID))
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
            return View(allocatedGDonations);
        }

        // GET: AllocatedGDonations/Delete/5
        public async Task<IActionResult> Delete(int? id)

        {
            //check if user is logged in
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
            if (id == null)
            {
                return NotFound();
            }

            var allocatedGDonations = await _context.AllocatedGDonations
                .FirstOrDefaultAsync(m => m.DonationID == id);
            if (allocatedGDonations == null)
            {
                return NotFound();
            }

            return View(allocatedGDonations);
        }

        // POST: AllocatedGDonations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var allocatedGDonations = await _context.AllocatedGDonations.FindAsync(id);
            _context.AllocatedGDonations.Remove(allocatedGDonations);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AllocatedGDonationsExists(int id)
        {
            return _context.AllocatedGDonations.Any(e => e.DonationID == id);
        }
    }
}
