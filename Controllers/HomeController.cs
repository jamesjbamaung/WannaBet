using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WannaBet.Models;
//added these line
using Microsoft.AspNetCore.Http;
using System.Text;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Globalization;


namespace WannaBet.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            Console.WriteLine("/1/1/1/1/1/1/1/1/1/1/1");
            return View();
        }
        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            System.Console.WriteLine("----------------7---------------");
            if (ModelState.IsValid)
            {
                if (dbContext.Users.Any(u => u.Email == user.Email))
                {
                    // Manually add a ModelState error to the Email field, with provided
                    // error message
                    ModelState.AddModelError("Email", "Email already in use!");

                    // You may consider returning to the View at this point
                    return View("Index");
                }
                // User newUser = new User()
                // {
                //     FirstName = Request.Form["FirstName"],
                //     LastName = Request.Form["LastName"],
                //     Email = Request.Form["Email"],
                //     Password = Request.Form["Password"],
                // };
                // return View(newUser);
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                dbContext.Add(user);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("userInSession", user.UserId);
                Console.WriteLine("/./././././././././././././././././.");
                Console.WriteLine("redirecting to account page from register method");
                return RedirectToAction("Account", new { id = user.UserId });
            }
            else
            {
                return View("Index");
            }
        }
        [HttpPost("login")]
        public IActionResult Login(LoginUser userSubmission)
        {
            if (ModelState.IsValid)
            {
                // If inital ModelState is valid, query for a user with provided email
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.Email);
                // If no user exists with provided email
                if (userInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Index");
                }

                // Initialize hasher object
                var hasher = new PasswordHasher<LoginUser>();

                // varify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);

                // result can be compared to 0 for failure
                if (result == 0)
                {
                    // handle failure (this should be similar to how "existing email" is handled)
                    ModelState.AddModelError("Password", "Invalid Email/Password");
                    return View("Index");
                }

                HttpContext.Session.SetInt32("userInSession", userInDb.UserId);
                // int id = userInDb.UserId;
                // return View("Account");
                return RedirectToAction("Account", new { id = userInDb.UserId });
            }
            else
            {
                return View("Index");
            }

        }
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }
        [HttpGet("account/{id}")]
        public IActionResult Account(int id)
        {
            if (HttpContext.Session.GetInt32("userInSession") == null)
            {
                return View("Index");
            }
            List<Bet> AllBets = dbContext.Bets.Include(a => a.listOfParticipants).ThenInclude(b => b.Better).OrderByDescending(c => c.ClosingDate).ToList();
            ViewBag.AllBets = AllBets;
            ViewBag.id = HttpContext.Session.GetInt32("userInSession");

            ViewBag.User = dbContext.Users.FirstOrDefault(a => a.UserId == HttpContext.Session.GetInt32("userInSession"));
            return View();
        }
        [HttpGet("betform")]
        public IActionResult BetForm()
        {
            if (HttpContext.Session.GetInt32("userInSession") == null)
            {
                return View("Index");
            }
            User userInSession = dbContext.Users.FirstOrDefault(user => user.UserId == HttpContext.Session.GetInt32("userInSession"));
            ViewBag.userInSession = userInSession;

            ViewBag.user = HttpContext.Session.GetInt32("userInSession");

            return View();
        }
        [HttpGet("bet/{bid}")]
        public IActionResult BetInfo(int bid)
        {
            ViewBag.userId = HttpContext.Session.GetInt32("userInSession");
            ViewBag.thisBet = dbContext.Bets.Include(a => a.listOfParticipants).ThenInclude(b => b.Better).FirstOrDefault(i => i.BetId == bid);
            ViewBag.listOfMessages = dbContext.Bets.Include(c => c.listOfMessages).ThenInclude(d => d.User).ToList();
            return View();
        }
        [HttpPost("addbet")]
        public IActionResult AddBet(Bet bet)
        {

            Bet newBet = bet;
            newBet.User = dbContext.Users.FirstOrDefault(u => u.UserId == bet.UserId);
            dbContext.Add(newBet);
            dbContext.SaveChanges();
            return RedirectToAction("Account", new { id = HttpContext.Session.GetInt32("userInSession") });

        }
        [HttpGet("reservebet/{bid}/{uid}")]
        public IActionResult ReserveBet(int bid, int uid)
        {
            Bet getBet = dbContext.Bets.Include(a => a.listOfParticipants).ThenInclude(b => b.Better).FirstOrDefault(c => c.BetId == bid);
            User better = dbContext.Users.FirstOrDefault(d => d.UserId == uid);
            User taker = dbContext.Users.FirstOrDefault(e => e.UserId == HttpContext.Session.GetInt32("userInSession"));


            Reserve newRes = new Reserve();
            newRes.BetId = getBet.BetId;
            newRes.Bet = getBet;

            newRes.BetterId = better.UserId;
            newRes.Better = better;

            newRes.TakerId = taker.UserId;
            newRes.Taker = taker;

            dbContext.Add(newRes);
            dbContext.SaveChanges();


            return RedirectToAction("Account", new { id = HttpContext.Session.GetInt32("userInSession") });
        }
        [HttpPost("addmessage")]
        public IActionResult AddMessage(Message mess)
        {
            User thisUser = dbContext.Users.FirstOrDefault(a => a.UserId == mess.UserId);
            Bet thisBet = dbContext.Bets.FirstOrDefault(b => b.BetId == mess.BetId);
            mess.User = thisUser;
            mess.Bet = thisBet;
            dbContext.Add(mess);
            dbContext.SaveChanges();
            
            return RedirectToAction("BetInfo", new { bid = mess.BetId });
        }
        [HttpGet("deletemessage/{mid}")]
        public IActionResult DeleteMessage(int mid)
        {
            System.Console.WriteLine(mid);
            Message delete = dbContext.Messages.FirstOrDefault(a => a.MessageId == mid);
            int id = delete.BetId;
            dbContext.Remove(delete);
            dbContext.SaveChanges();
            System.Console.WriteLine("1-1-1-1-1-1-1-1-1-1-1-1-1");
            System.Console.WriteLine("deleted message");
            return RedirectToAction("BetInfo", new { bid = id });
        }
        [HttpGet("userinfo/{uid}")]
        public IActionResult UserInfo(int uid)
        {
            ViewBag.user = dbContext.Users.Include(a => a.listOfFollows).ThenInclude(b => b.Follower).FirstOrDefault(c => c.UserId == uid);
            ViewBag.userBets = dbContext.Users.Include(a => a.BetterBets).ThenInclude(b => b.Bet).FirstOrDefault(c => c.UserId == uid);
            ViewBag.userInSession = dbContext.Users.FirstOrDefault(b => b.UserId == HttpContext.Session.GetInt32("userInSession"));
            return View();
        }
        [HttpPost("addfollow")]
        public IActionResult AddFollow(Follow fol)
        {
            System.Console.WriteLine(fol.FollowerId);
            System.Console.WriteLine(fol.FollowedId);
            User follower = dbContext.Users.FirstOrDefault(a => a.UserId == fol.FollowerId);
            User followed = dbContext.Users.FirstOrDefault(b => b.UserId == fol.FollowedId);
            fol.Follower = follower;
            fol.Followed = followed;
            dbContext.Add(fol);
            dbContext.SaveChanges();
            Console.WriteLine("/././././././././././././././././././././.");
            Console.WriteLine("added a follow");
            return RedirectToAction("UserInfo", new { uid = fol.FollowedId });
        }
        [HttpGet("unfollow/{fid}")]
        public IActionResult UnFollow(int fid)
        {
            Follow delete = dbContext.Follows.FirstOrDefault(a => a.FollowId == fid);
            int num = delete.FollowedId;
            dbContext.Remove(delete);
            dbContext.SaveChanges();
            return RedirectToAction("UserInfo", new { uid = num });

        }
        [HttpGet("addfavorite/{bid}/{uid}")]
        public IActionResult AddFavorite(int bid, int uid)
        {
            Console.WriteLine("/./././././././././././././././././");
            Favorite newFav = new Favorite();
            User thisUser = dbContext.Users.FirstOrDefault(a => a.UserId == uid);
            Bet thisBet = dbContext.Bets.FirstOrDefault(b => b.BetId == bid);
            newFav.User = thisUser;
            newFav.UserId = thisUser.UserId;
            newFav.Bet = thisBet;
            newFav.BetId = thisBet.BetId;
            dbContext.Add(newFav);
            dbContext.SaveChanges();
            Console.WriteLine("-/-/-/-/-/-/-/-/-/-/-/-/-/--/-/-/-/-/");

            return RedirectToAction("BetInfo", new { bid = newFav.BetId });
        }
        [HttpGet("deletebet/{bid}")]
        public IActionResult DeleteBet(int bid)
        {
            Bet canceled = dbContext.Bets.FirstOrDefault(a => a.BetId == bid);
            dbContext.Remove(canceled);
            dbContext.SaveChanges();
            return RedirectToAction("Account", new { id = HttpContext.Session.GetInt32("userInSession") });
        }
        [HttpGet("cancelreserve/{rid}")]
        public IActionResult CancelReserve(int rid)
        {
            Reserve canceled = dbContext.Reserves.FirstOrDefault(a => a.ReserveId == rid);
            dbContext.Remove(canceled);
            dbContext.SaveChanges();
            return RedirectToAction("Account", new { id = HttpContext.Session.GetInt32("userInSession") });
        }
    }
}
