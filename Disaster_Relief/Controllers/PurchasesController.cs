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
    public class PurchasesController : Controller
    {
        private readonly MDonation_Context _context;
        private readonly Goods_Context _gcontext;
        decimal sTotal, total, totalA;

        public PurchasesController(MDonation_Context context, Goods_Context gcontext)
        {
            _context = context;
            _gcontext = gcontext;
        }

        // GET: Purchases
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
            //code attribution
            //this method was taken from stackoverflow
            //https://stackoverflow.com/questions/38931374/how-to-perform-sum-operation-in-entity-framework
            //hemantsharma
            //https://stackoverflow.com/users/5279769/hemantsharma
            //(hemantsharma, 2016)
            sTotal = _context.Purchase.Where(t => t.AmountSpent >= 0).Sum(i => i.AmountSpent);
            ViewBag.sTotal = sTotal;

            total = _context.MoneyDonations.Where(t => t.Amount >= 0).Sum(i => i.Amount);

            totalA = _context.AllocatedMDonations.Where(t => t.Amount >= 0).Sum(i => i.Amount);
            ViewBag.Left = (total + totalA) - sTotal;

            return View(await _context.Purchase.ToListAsync());
        }

        // GET: Purchases/Details/5
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

            var purchase = await _context.Purchase
                .FirstOrDefaultAsync(m => m.PurchaseID == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // GET: Purchases/Create
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
            //sTotal = _context.Purchase.Where(t => t.AmountSpent >= 0).Sum(i => i.AmountSpent);
            //ViewBag.sTotal = sTotal;
            //total = _context.MoneyDonations.Where(t => t.Amount >= 0).Sum(i => i.Amount);
            //ViewBag.Left = total - sTotal;
            return Redirect("/Disasters/disasterA");
        }

        // POST: Purchases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("PurchaseID,AmountSpent")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(purchase);
                //await _context.SaveChangesAsync();
                return Redirect("/GoodsDonations/PGoods");
            }
            return View(purchase);


        }

        // GET: Purchases/Edit/5
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

            var purchase = await _context.Purchase.FindAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }
            return View(purchase);
        }

        // POST: Purchases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PurchaseID,Amount")] Purchase purchase)
        {
            if (id != purchase.PurchaseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseExists(purchase.PurchaseID))
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
            return View(purchase);
        }

        // GET: Purchases/Delete/5
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

            var purchase = await _context.Purchase
                .FirstOrDefaultAsync(m => m.PurchaseID == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchase = await _context.Purchase.FindAsync(id);
            _context.Purchase.Remove(purchase);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseExists(int id)
        {
            return _context.Purchase.Any(e => e.PurchaseID == id);
        }
        public async Task<IActionResult> PurchaseGood(int? id)
        {
            if (HttpContext.Session.GetString("LoggedIn") != "Yes")
            {
                return Redirect("/Users/Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchase.FindAsync(id);
            //To get value:
            ViewBag.Test = HttpContext.Session.GetString("Location");
            ViewBag.ID = HttpContext.Session.GetString("ID");

            HttpContext.Session.SetString("DisID", purchase.PurchaseID.ToString());

            if (purchase == null)
            {
                return NotFound();
            }
            return View(purchase);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> PurchaseGood(int id, [Bind("DonationID,DonationName,Quantity,Description,DonationDate,Catagory")] GoodsDonations aDonations)
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
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(aDonations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseExists(aDonations.DonationID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Redirect("/GoodsDonations/Index");
            }
            return View(aDonations);
        }
    }
}
