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
    public class AllocatedMDonationsController : Controller
    {
        private readonly MDonation_Context _context;

        public AllocatedMDonationsController(MDonation_Context context)
        {
            _context = context;
        }

        // GET: AllocatedMDonations
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
            return View(await _context.AllocatedMDonations.ToListAsync());
        }

        // GET: AllocatedMDonations/Details/5
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

            var allocatedMDonations = await _context.AllocatedMDonations
                .FirstOrDefaultAsync(m => m.DonationID == id);
            if (allocatedMDonations == null)
            {
                return NotFound();
            }

            return View(allocatedMDonations);
        }

        // GET: AllocatedMDonations/Create
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

        // POST: AllocatedMDonations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DonationID,DonationName,Amount,DonationDate,Location,ID")] AllocatedMDonations allocatedMDonations)
        {
            if (ModelState.IsValid)
            {
                _context.Add(allocatedMDonations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(allocatedMDonations);
        }

        // GET: AllocatedMDonations/Edit/5
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

            var allocatedMDonations = await _context.AllocatedMDonations.FindAsync(id);
            if (allocatedMDonations == null)
            {
                return NotFound();
            }
            return View(allocatedMDonations);
        }

        // POST: AllocatedMDonations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DonationID,DonationName,Amount,DonationDate,Location,ID")] AllocatedMDonations allocatedMDonations)
        {
            if (id != allocatedMDonations.DonationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(allocatedMDonations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AllocatedMDonationsExists(allocatedMDonations.DonationID))
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
            return View(allocatedMDonations);
        }

        // GET: AllocatedMDonations/Delete/5
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

            var allocatedMDonations = await _context.AllocatedMDonations
                .FirstOrDefaultAsync(m => m.DonationID == id);
            if (allocatedMDonations == null)
            {
                return NotFound();
            }

            return View(allocatedMDonations);
        }

        // POST: AllocatedMDonations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var allocatedMDonations = await _context.AllocatedMDonations.FindAsync(id);
            _context.AllocatedMDonations.Remove(allocatedMDonations);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AllocatedMDonationsExists(int id)
        {
            return _context.AllocatedMDonations.Any(e => e.DonationID == id);
        }
    }
}
