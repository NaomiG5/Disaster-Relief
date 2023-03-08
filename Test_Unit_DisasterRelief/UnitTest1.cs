using Disaster_Relief;
using Disaster_Relief.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using WebsiteExampleMain.Models;

namespace Test_Unit_DisasterRelief
{
    [TestClass]
    public class UnitTest1
    {
            // to have the same Configuration object as in Startup
            private IConfigurationRoot _configuration;

            // represents database's configuration
            private DbContextOptions<User_Context> _User;

            private DbContextOptions<Disasters_Context> _dis;

            private DbContextOptions<Goods_Context> _goods;

            private DbContextOptions<MDonation_Context> _mDonation;

        decimal total, totalA,  totalAmoney;
        int totalg, totalgA, totalAgoods;

        //code attribution
        //this method was taken from CodeJourney
        //https://www.codejourney.net/entity-framework-core-database-initialization-with-unit-test/
        //Dawid Sibiński
        //https://www.codejourney.net/about/
        //(Sibiński, 2016)
        public UnitTest1()
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

                _configuration = builder.Build();
                _User = new DbContextOptionsBuilder<User_Context>()
                    .UseSqlServer(_configuration.GetConnectionString("online"))
                    .Options;

                _dis = new DbContextOptionsBuilder<Disasters_Context>()
                       .UseSqlServer(_configuration.GetConnectionString("online"))
                       .Options;

                _goods = new DbContextOptionsBuilder<Goods_Context>()
                       .UseSqlServer(_configuration.GetConnectionString("online"))
                       .Options;

                _mDonation = new DbContextOptionsBuilder<MDonation_Context>()
                           .UseSqlServer(_configuration.GetConnectionString("online"))
                           .Options;
        }

        #region Test Linq
        public decimal calcTotalMoney()
        {
            using (var context = new MDonation_Context(_mDonation))
            {
                total = context.MoneyDonations.Where(t => t.Amount >= 0).Sum(i => i.Amount);
                totalA = context.AllocatedMDonations.Where(t => t.Amount >= 0).Sum(i => i.Amount);
                totalAmoney = (total + totalA);
                return totalAmoney;
            }

        }
        [TestMethod]
        public void TestGetTotalMDonation()
        {

            decimal Actual = calcTotalMoney();
            decimal expected = 820;
            Assert.AreEqual(expected, Actual);
        }

        public int calcTotalGoods()
        {
            using (var context = new Goods_Context(_goods))
            {
                totalg = context.GoodsDonations.Where(t => t.Quantity >= 0).Sum(i => i.Quantity);
                totalgA = context.AllocatedGDonations.Where(t => t.Quantity >= 0).Sum(i => i.Quantity);
                totalAgoods = totalg + totalgA;
                return totalAgoods;
            }

        }
        [TestMethod]
        public void TestGetTotalGDonation()
        {

            int Actual = calcTotalGoods();
            int expected = 10;
            Assert.AreEqual(expected, Actual);
        }

        public int getActiveCount()
        {
            using (var context = new Disasters_Context(_dis))
            {
                totalg = context.Disaster.Where(a => a.SDate <= DateTime.Today && a.EDate >= DateTime.Today).Count();
                return totalg;
            }

        }
        [TestMethod]
        public void TestGetActiveDisaster()
        {

            int Actual = getActiveCount();
            int expected = 2;
            Assert.AreEqual(expected, Actual);
        }
        #endregion

        //code attribution
        //this method was taken from CodeJourney
        //https://www.codejourney.net/entity-framework-core-database-initialization-with-unit-test/
        //Dawid Sibiński
        //https://www.codejourney.net/about/
        //(Sibiński, 2016)
        #region Test Database

        #region Test Users
        [TestMethod]
        public void TestUserTable()
        {
            using (var context = new User_Context(_User))
            {
                context.Database.EnsureCreated();

                var user = new Users()
                {
                    Email = "test1@gmail.com",
                    Username = "Test1",
                    Password = "bmFvbWkyMDIw"
                };

                context.Users.AddRange(user);
                context.SaveChanges();
            }
        }
        #endregion

        #region Test disaster
        [TestMethod]
        public void TestDisasterTable()
        {
            using (var context = new Disasters_Context(_dis))
            {
                context.Database.EnsureCreated();

                var disaster = new Disaster()
                {

                    Type = "Fire",
                    Description = "Fire has displaced 20 people",
                    Location = "Port Elizabeth",
                    SDate = new DateTime(2022, 11, 01),
                    EDate = new DateTime(2022, 11, 02),
                    AidNeeded = "Food, Water, Clothes, Blankets",
                };

                context.Disaster.AddRange(disaster);
                context.SaveChanges();
            }
        }
        #endregion

        #region Test Goods Donation
        [TestMethod]
        public void TestGoodsTable()
        {
            using (var context = new Goods_Context(_goods))
            {
                context.Database.EnsureCreated();

                var goods = new GoodsDonations()
                {

                    DonationName = "Fred",
                    Quantity = 1,
                    Description = "1 can of meat",
                    DonationDate = new DateTime(2022, 11, 10),
                    Catagory = "Canned Food"
                };

                context.GoodsDonations.AddRange(goods);
                context.SaveChanges();
            }
        }
        #endregion

        #region Test Allocate Goods
        [TestMethod]
        public void TestAllocateGoodsTable()
        {
            using (var context = new Goods_Context(_goods))
            {
                context.Database.EnsureCreated();

                var agoods = new AllocatedGDonations()
                {

                    DonationName = "Anonymous",
                    Quantity = 1,
                    Description = "1 can of corn",
                    DonationDate = new DateTime(2022, 11, 11),
                    Catagory = "Canned Food",
                    Location = "Port Elizabeth",
                    ID = 3
                };

                context.AllocatedGDonations.AddRange(agoods);
                context.SaveChanges();
            }
        }
        #endregion

        #region Test Money Donation
        [TestMethod]
        public void TestMoneyTable()
        {
            using (var context = new MDonation_Context(_mDonation))
            {
                context.Database.EnsureCreated();

                var goods = new MoneyDonations()
                {

                    DonationName = "Anonymous",
                    Amount = 20,
                    DonationDate = new DateTime(2022, 11, 11)
                };

                context.MoneyDonations.AddRange(goods);
                context.SaveChanges();
            }
        }
        #endregion

        #region Test Allocate Goods
        [TestMethod]
        public void TestAllocateMoneyTable()
        {
            using (var context = new MDonation_Context(_mDonation))
            {
                context.Database.EnsureCreated();

                var goods = new AllocatedMDonations()
                {

                    DonationName = "Anonymous",
                    Amount = 20,
                    DonationDate = new DateTime(2022, 11, 11),
                    Location = "Port Elizabeth",
                    ID = 3
                };

                context.AllocatedMDonations.AddRange(goods);
                context.SaveChanges();
            }
        }
        #endregion

        #region Test Purchase
        [TestMethod]
        public void TestPurchaseTable()
        {
            using (var context = new MDonation_Context(_mDonation))
            {
                context.Database.EnsureCreated();

                var goods = new Purchase()
                {
                    AmountSpent = 25
                };

                context.Purchase.AddRange(goods);
                context.SaveChanges();
            }
        }
        #endregion
        #endregion


    }
}
