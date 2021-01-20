using FCPC.CustomProvider;
using FCPC.Models;
using FCPC.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FCPC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IUserService _userService;

        public HomeController(ILogger<HomeController> logger, SignInManager<ApplicationUser> signInManager, IUserService userService)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userService = userService;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                _signInManager.SignOutAsync().RunSynchronously();
                return RedirectToAction("Index", "Home");
            }

            var user = _userService.GetUser(User.Identity.Name);
            if (user == null)
            {
                _signInManager.SignOutAsync().RunSynchronously();
                return RedirectToAction("Index", "Home");
            }

            if (user.Votes != null && user.Votes.Any()) // voted
            {
                return RedirectToAction("Voted");
            }

            var model = new FormModel
            {
                Id = user.UserId,
                Name = $"{user.FirstName} {user.LastName}"
            };

            //candidates
            using var db = new BloggingContext();
            var candidates = db.Cadidates.Where(x => x.Active && x.RegionId == user.RegionId).OrderBy(x => x.LastName).ThenBy(x => x.FirstName);
            var region = db.Regions.First(x => x.RegionId == user.RegionId);
            model.Candidates = candidates.ToList();

            model.Limit = region.Max;

            return View(model);
        }


        public IActionResult Submit(FormRequestModel model)
        {
            using var db = new BloggingContext();
            var user = db.Users.First(x => x.UserId == model.Id);

            string voteHtml = string.Empty;
            if (model.VoteNull) //Voto nulo
            {
                var voteNull = new Vote
                {
                    CandidateId = "0",
                    RegionId = user.RegionId,
                    UserId = model.Id,
                    Date = DateTime.Now
                };
                db.Votes.Add(voteNull);
                db.SaveChanges();

                voteHtml = "VOTO NULO";
            }
            else
            {
                if (model.Candidate == null || model.Candidate.Count == 0)
                {
                    var voteNull = new Vote
                    {
                        CandidateId = "1",
                        RegionId = user.RegionId,
                        UserId = model.Id,
                        Date = DateTime.Now
                    };
                    db.Votes.Add(voteNull);
                    db.SaveChanges();

                    voteHtml = "VOTO EN BLANCO";
                }
                else
                {
                    foreach (var candidate in model.Candidate)
                    {
                        db.Add(new Vote
                        {
                            CandidateId = candidate,
                            RegionId = user.RegionId,
                            UserId = model.Id,
                            Date = DateTime.Now
                        });
                    }

                    db.SaveChanges();


                    var candidates = db.Cadidates.Where(x => model.Candidate.Contains(x.CandidateId)).Select(x => x.FirstName + " " + x.LastName).ToList();
                    voteHtml = string.Join("<br>", candidates);
                }
            }
            
            SendEmail(user.EmailAddress, user.FirstName + " " + user.LastName, voteHtml);

            return RedirectToAction("ThankYou");
        }

        public IActionResult ThankYou()
        {
            return View();
        }

        public IActionResult Voted()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public void SendEmail(string emailAddress, string name, string vote)
        {
            try
            {
                //send email
                // create email message
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse("FCPC <santimc@gmail.com>");
                email.To.Add(MailboxAddress.Parse(emailAddress));
                email.Subject = "Votación FCPC DAC";

                var html =
                    $"<h1>Votación FCPC DAC</h1><p>Gracias {name} por participar! <br><br> Su voto ha sido registrado: <br><strong>{vote}</strong></p>";
                email.Body = new TextPart(TextFormat.Html) { Text = html };

                // send email
                using var smtp = new SmtpClient();
                //smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
                smtp.Connect("smtp.elasticemail.com", 2525, SecureSocketOptions.StartTls);
                //smtp.Authenticate("cesantiadgac@gmail.com", "FONDOD@C42");
                smtp.Authenticate("santimc@gmail.com", "6B3CF131E8B69A1BD20EBA8254BE93A69EB8");
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
