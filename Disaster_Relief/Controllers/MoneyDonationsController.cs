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
    public class MoneyDonationsController : Controller
    {
        private readonly MDonation_Context _context;

        public MoneyDonationsController(MDonation_Context context)
        {
            _context = context;
        }

        // GET: MoneyDonations
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

            //ViewBag.Total = total;
            return View(await _context.MoneyDonations.ToListAsync());

        }

        // GET: MoneyDonations/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var moneyDonations = await _context.MoneyDonations
                .FirstOrDefaultAsync(m => m.DonationID == id);
            if (moneyDonations == null)
            {
                return NotFound();
            }

            return View(moneyDonations);
        }

        // GET: MoneyDonations/Create
        public IActionResult Create()
        {
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

        // POST: MoneyDonations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DonationID,DonationName,Amount,DonationDate")] MoneyDonations moneyDonations, string DonationName)
        {
            if (ModelState.IsValid)
            {

                if (DonationName == null)
                {
                    moneyDonations.DonationName = "Anonymous";
                    ViewBag.MessageDon = moneyDonations.DonationName;
                }

                _context.Add(moneyDonations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));


            }
            return View(moneyDonations);
        }

        // GET: MoneyDonations/Edit/5
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

            var moneyDonations = await _context.MoneyDonations.FindAsync(id);
            if (moneyDonations == null)
            {
                return NotFound();
            }
            return View(moneyDonations);
        }

        // POST: MoneyDonations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DonationID,DonationName,Amount,DonationDate")] MoneyDonations moneyDonations)
        {
            if (id != moneyDonations.DonationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(moneyDonations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MoneyDonationsExists(moneyDonations.DonationID))
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
            return View(moneyDonations);
        }

        // GET: MoneyDonations/Delete/5
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

            var moneyDonations = await _context.MoneyDonations
                .FirstOrDefaultAsync(m => m.DonationID == id);
            if (moneyDonations == null)
            {
                return NotFound();
            }

            return View(moneyDonations);
        }

        // POST: MoneyDonations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var moneyDonations = await _context.MoneyDonations.FindAsync(id);
            _context.MoneyDonations.Remove(moneyDonations);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MoneyDonationsExists(int id)
        {
            return _context.MoneyDonations.Any(e => e.DonationID == id);
        }
        public async Task<IActionResult> MAllocate(int? id)
        {
            if (HttpContext.Session.GetString("LoggedIn") != "Yes")
            {
                return Redirect("/Users/Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var moneyDonations = await _context.MoneyDonations.FindAsync(id);
            //To get value:
            ViewBag.Test = HttpContext.Session.GetString("Location");
            ViewBag.ID = HttpContext.Session.GetString("ID");

            HttpContext.Session.SetString("DisID", moneyDonations.DonationID.ToString());

            if (moneyDonations == null)
            {
                return NotFound();
            }
            return View(moneyDonations);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> MAllocate(int id, [Bind("DonationID,DonationName,Amount,DonationDate")] AllocatedMDonations aDonations)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    string dID = HttpContext.Session.GetString("ID");
                    aDonations.ID = Int32.Parse(dID);
                    string location = HttpContext.Session.GetString("Location");
                    aDonations.Location = location;

                    _context.Add(aDonations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MoneyDonationsExists(aDonations.DonationID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }



                int DisID = Int32.Parse(HttpContext.Session.GetString("DisID"));

                var moneyDonations = await _context.MoneyDonations.FindAsync(DisID);
                _context.MoneyDonations.Remove(moneyDonations);
                await _context.SaveChangesAsync();
                return Redirect("/AllocatedMDonations/Index");
            }
            return View(aDonations);
        }


    }
}
