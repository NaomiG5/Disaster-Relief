using Disaster_Relief.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Disaster_Relief.Controllers
{
    public class HomeController : Controller
    {
        private readonly MDonation_Context _context;
        private readonly Goods_Context _gcontext;
        decimal  total, totalA, totalG, totalGA, totalAgoods, totalAmoney;
        private readonly HomeController _logger;
        private readonly Disasters_Context _dcontext;

        public HomeController( MDonation_Context context, Goods_Context gcontext, Disasters_Context dcontext)
        {
            _context = context;
            _dcontext = dcontext;
            _gcontext = gcontext;
        }

        public async Task<IActionResult> Index()
        {
            //code attribution
            //this method was taken from stackoverflow
            //https://stackoverflow.com/questions/38931374/how-to-perform-sum-operation-in-entity-framework
            //hemantsharma
            //https://stackoverflow.com/users/5279769/hemantsharma
            //(hemantsharma, 2016)

            Calculate();

            //int goods = ViewBag.DisID;
            var model = await _dcontext.Disaster.Where(a => a.SDate <= DateTime.Today && a.EDate >= DateTime.Today)
               .ToListAsync();
            getDisaster();
            return View(model);

        }
        public void Calculate()
        {
            total = _context.MoneyDonations.Where(t => t.Amount >= 0).Sum(i => i.Amount);

            totalA = _context.AllocatedMDonations.Where(t => t.Amount >= 0).Sum(i => i.Amount);
            ViewBag.mTotal = (total + totalA);

            totalG = _gcontext.AllocatedGDonations.Where(t => t.Quantity >= 0).Sum(i => i.Quantity);
            totalGA = _gcontext.GoodsDonations.Where(t => t.Quantity >= 0).Sum(i => i.Quantity);
            ViewBag.gTotal = (totalG + totalGA);
              
        }

        public void getDisaster()
        {
            var model =_dcontext.Disaster.Where(a => a.SDate <= DateTime.Today && a.EDate >= DateTime.Today).ToList();
            ViewBag.Disaster = model;

            var agoods = _gcontext.GoodsDonations.Where(t => t.DonationID >= 0).ToList();
            ViewBag.Goods = agoods;

            var goods = _gcontext.AllocatedGDonations.Where(t => t.ID >= 0).ToList();
                ViewBag.Goods = goods;

                var mgoods = _context.AllocatedMDonations.Where(t => t.ID >= 0).ToList();
                ViewBag.Money = mgoods;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
