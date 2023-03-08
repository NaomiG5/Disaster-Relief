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
    public class GoodsDonationsController : Controller
    {
        private readonly Goods_Context _context;
        private readonly MDonation_Context _mcontext;
        protected string InputValue;
        decimal sTotal, total, totalA;
        public GoodsDonationsController(Goods_Context context, MDonation_Context mcontext)
        {
            _context = context;
            _mcontext = mcontext;
        }

        // GET: GoodsDonations
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


            return View(await _context.GoodsDonations.ToListAsync());
        }

        // GET: GoodsDonations/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var goodsDonations = await _context.GoodsDonations
                .FirstOrDefaultAsync(m => m.DonationID == id);
            if (goodsDonations == null)
            {
                return NotFound();
            }

            return View(goodsDonations);
        }

        // GET: GoodsDonations/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("LoggedIn") != "Yes")
            {
                return Redirect("/Users/Login");
            }

            var catList = _context.GoodsDonations.Select(x => x.Catagory).Distinct().ToList();
            ViewData["category"] = catList;
            return View();
        }

        // POST: GoodsDonations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("DonationID,DonationName,Quantity,Description,DonationDate,Catagory")] GoodsDonations goodsDonations, string DonationName, string Catagory)
        {


            if (ModelState.IsValid)
            {
                if (DonationName == null)
                {
                    goodsDonations.DonationName = "Anonymous";

                }
                if (Catagory == null)
                {

                    string category = HttpContext.Session.GetString("Category");
                    goodsDonations.Catagory = category;

                }


                _context.Add(goodsDonations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(goodsDonations);
        }

        // GET: GoodsDonations/Edit/5
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

            var goodsDonations = await _context.GoodsDonations.FindAsync(id);
            if (goodsDonations == null)
            {
                return NotFound();
            }
            return View(goodsDonations);
        }

        // POST: GoodsDonations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DonationID,DonationName,Quantity,Description,DonationDate,Catagory")] GoodsDonations goodsDonations)
        {
            if (id != goodsDonations.DonationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(goodsDonations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoodsDonationsExists(goodsDonations.DonationID))
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
            return View(goodsDonations);
        }

        // GET: GoodsDonations/Delete/5
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

            var goodsDonations = await _context.GoodsDonations
                .FirstOrDefaultAsync(m => m.DonationID == id);
            if (goodsDonations == null)
            {
                return NotFound();
            }

            return View(goodsDonations);
        }

        // POST: GoodsDonations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var goodsDonations = await _context.GoodsDonations.FindAsync(id);
            _context.GoodsDonations.Remove(goodsDonations);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GoodsDonationsExists(int id)
        {
            return _context.GoodsDonations.Any(e => e.DonationID == id);
        }

        public async Task<IActionResult> GAllocate(int? id)
        {
            if (HttpContext.Session.GetString("LoggedIn") != "Yes")
            {
                return Redirect("/Users/Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var goodsDonations = await _context.GoodsDonations.FindAsync(id);
            //To get value:
            ViewBag.Test = HttpContext.Session.GetString("Location");
            ViewBag.ID = HttpContext.Session.GetString("ID");
            HttpContext.Session.SetString("money", "yes");
            HttpContext.Session.SetString("DisID", goodsDonations.DonationID.ToString());

            if (goodsDonations == null)
            {
                return NotFound();
            }
            return View(goodsDonations);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> GAllocate([Bind("DonationID, DonationName,Quantity,Description,DonationDate,Catagory,Location,ID")] AllocatedGDonations aDonations)
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
                    if (!GoodsDonationsExists(aDonations.DonationID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }



                int DisID = Int32.Parse(HttpContext.Session.GetString("DisID"));

                var goodsDonations = await _context.GoodsDonations.FindAsync(DisID);
                _context.GoodsDonations.Remove(goodsDonations);
                await _context.SaveChangesAsync();
                return Redirect("/AllocatedGDonations/Index");
            }
            return View(aDonations);
        }
        public IActionResult PGoods()
        {
            if (HttpContext.Session.GetString("LoggedIn") != "Yes")
            {
                return Redirect("/Users/Login");
            }
            ViewBag.Test = HttpContext.Session.GetString("Location");
            ViewBag.ID = HttpContext.Session.GetString("ID");

            var catList = _context.GoodsDonations.Select(x => x.Catagory).Distinct().ToList();
            ViewData["categories"] = catList;
            HttpContext.Session.SetString("Money", "Yes");
            sTotal = _mcontext.Purchase.Where(t => t.AmountSpent >= 0).Sum(i => i.AmountSpent);
            ViewBag.sTotal = sTotal;
            total = _mcontext.MoneyDonations.Where(t => t.Amount >= 0).Sum(i => i.Amount);
            totalA = _mcontext.AllocatedMDonations.Where(t => t.Amount >= 0).Sum(i => i.Amount);
            ViewBag.Left = (total + totalA) - sTotal;


            return View();
        }


        // POST: GoodsDonations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> PGoods(string DonationName, string Catagory, decimal AmountSpent, int Quantity, string Description, Purchase purchase, [Bind("DonationID, DonationName,Quantity,Description,DonationDate,Catagory,Location,ID")] AllocatedGDonations aDonations, GoodsDonations goodsDonations)
        {

            if (AmountSpent == 0 || Quantity == 0 || Description == null)
            {
                ViewBag.MessageG = "PLEASE FILL IN ALL REQURED INFORMATION";
            }
            if (ModelState.IsValid)
            {

                if (DonationName == null)
                {
                    aDonations.DonationName = "Anonymous";

                }
                if (Catagory == null)
                {

                    string category = HttpContext.Session.GetString("Category");
                    aDonations.Catagory = category;

                }
                sTotal = _mcontext.Purchase.Where(t => t.AmountSpent >= 0).Sum(i => i.AmountSpent);
                ViewBag.sTotal = sTotal;
                total = _mcontext.MoneyDonations.Where(t => t.Amount >= 0).Sum(i => i.Amount);
                totalA = _mcontext.AllocatedMDonations.Where(t => t.Amount >= 0).Sum(i => i.Amount);
                ViewBag.Left = (total + totalA) - sTotal;
                decimal ptotal = (total + totalA) - sTotal;
                decimal amountAfter = ptotal - AmountSpent;
                if (amountAfter >= 0)
                {

                    string dID = HttpContext.Session.GetString("ID");
                    aDonations.ID = Int32.Parse(dID);
                    string location = HttpContext.Session.GetString("Location");
                    aDonations.Location = location;

                    _context.Add(aDonations);
                    await _context.SaveChangesAsync();

                    HttpContext.Session.SetString("Money", "Yes");
                    purchase.AmountSpent = AmountSpent;
                    ViewBag.Message = purchase.AmountSpent;
                    _mcontext.Add(purchase);
                    await _mcontext.SaveChangesAsync();
                    return Redirect("/Purchases/Index");

                }
                else
                {
                    HttpContext.Session.SetString("Money", "No");
                    return Redirect("/Disasters/disasterA");
                }



            }
            return Redirect("/Disasters/disasterA");
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]

        //public async Task<IActionResult> CreateP( Purchase purchase)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(purchase);
        //        await _context.SaveChangesAsync();
        //        return Redirect("/Purchase/Index");
        //    }
        //    return View(purchase);


        //}
    }
}
