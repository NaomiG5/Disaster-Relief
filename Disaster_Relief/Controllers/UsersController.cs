using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Disaster_Relief.Models;
using WebsiteExampleMain.Models;
using Microsoft.AspNetCore.Http;

namespace Disaster_Relief.Controllers
{
    public class UsersController : Controller
    {
        private readonly User_Context _context;

        public UsersController(User_Context context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.Email == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Password, string conPassword,
            string Email, string Username, [Bind("Email,Username,Password")] Users users)
        {
            if (ModelState.IsValid)
            {
                if (Password == null || conPassword == null || Email == null || Username == null)
                {
                    ViewBag.Error = "Please fill in all fields";

                }
                else
                {
                    if (UsersExists(users.Email))
                    {
                        ViewBag.Error = "Email is already assigned to another user";
                    }
                    else
                    {
                        if (!Email.Contains("@") || !Email.Contains("."))
                        {
                            ViewBag.Error = "Please make sure that your Email is valid eg. It must contain an @ or .";

                        }
                        else
                        {
                            if (Password != conPassword)
                            {
                                ViewBag.Error = "Please make sure that the Password and the confirm password are the same";

                            }
                            else
                            {

                                users.Password = EncodePassword(Password);
                                _context.Add(users);
                                await _context.SaveChangesAsync();
                                return RedirectToAction(nameof(Login));

                            }

                        }
                    }



                }


            }
            return View(users);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Email,Username,Password")] Users users)
        {
            if (id != users.Email)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.Email))
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
            return View(users);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.Email == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var users = await _context.Users.FindAsync(id);
            _context.Users.Remove(users);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(string id)
        {
            return _context.Users.Any(e => e.Email == id);
        }
        public IActionResult Login()
        {
            return View();
        }
        //[HttpPost]
        [HttpPost]
        public IActionResult Login(string Email, string Password, Users users)
        {
            //code attribution
            //this method was taken from Stack Overflow
            //https://stackoverflow.com/questions/26290237/asp-net-mvc-get-textbox-value-in-view
            //doncadavona
            //https://stackoverflow.com/users/3936053/doncadavona

            if (Email == null || Password == null)

            {
                ViewBag.Error = "Please enter all the fields";

                return View();


            }
            if (!UsersExists(users.Email))
            {
                ViewBag.Error = "User does not exist please register ";
                return View();
            }
            else
            {
                //code attribution
                //this method was taken from C#Corner
                //https://www.c-sharpcorner.com/blogs/how-to-encrypt-or-decrypt-password-using-asp-net-with-c-sharp1
                //Amit Sanandiya
                //https://www.c-sharpcorner.com/members/amit-sanandiya

                // Code taken from (Sanandiya, 2022)
                string decrypted = EncodePassword(Password);


                //code attribution
                //this Code was taken from Stack Overflow
                //https://stackoverflow.com/questions/8791540/multiple-where-clauses-with-linq-extension-methods
                //David
                //https://stackoverflow.com/users/328193/david
                var Valid = _context.Users.Where(a => a.Email.Equals(Email) && a.Password.Equals(decrypted)).Count();



                if (Valid > 0)
                {
                    //code attribution
                    //this method was taken from benjii.me
                    //https://benjii.me/2016/07/using-sessions-and-httpcontext-in-aspnetcore-and-mvc-core/
                    //Ben Cull
                    //https://bencull.com/
                    //(Cull, 2016)
                    HttpContext.Session.SetString("LoggedIn", "Yes");

                    ViewBag.Error = "SUCESSFULLY LOGGED IN ";

                    //code attribution
                    //this method was taken DotNetTricks
                    //https://www.dotnettricks.com/learn/mvc/return-view-vs-return-redirecttoaction-vs-return-redirect-vs-return-redirecttoroute
                    //Shailendra Chauhan
                    //https://www.dotnettricks.com/mentors/shailendra-chauhan
                    //(Chauhan, 2022)
                    return Redirect("/Home/Index");
                }

                else
                {
                    HttpContext.Session.SetString("LoggedIn", "No");
                    ViewBag.Error = "WRONG PASSWORD.";
                    return View();
                }
            }

        }
        public IActionResult LogOf()
        {
            HttpContext.Session.SetString("LoggedIn", "No");
            return Redirect("/Home/Index");
        }


        //code attribution
        //this method was taken from C#Corner
        //https://www.c-sharpcorner.com/blogs/how-to-encrypt-or-decrypt-password-using-asp-net-with-c-sharp1
        //Amit Sanandiya
        //https://www.c-sharpcorner.com/members/amit-sanandiya
        public static string EncodePassword(string password)
        {
            try
            {
                byte[] encodeData = new byte[password.Length];
                encodeData = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encodeData);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error when  encoding" + ex.Message);
            }
        }
    }
}
