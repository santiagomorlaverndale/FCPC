using System.Collections.Generic;
using System.Linq;
using FCPC.CustomProvider;
using FCPC.Models;
using FCPC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FCPC.Controllers
{
    [AllowAnonymous]
    [Route("api/account")]
    public class AccountApiController : Controller
    {
        private IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public AccountApiController(IUserService userService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public JsonResult CreateUser(string firstName, string lastName, string id, string email)
        {
            using var db = new BloggingContext();
            var user = new User
            {
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName,
                UserId = id,
                Active = true
            };
            // Create
            db.Add(user);
            db.SaveChanges();

            return Json(user);
        }

        public JsonResult CreateCandidate(string firstName, string lastName, string id)
        {
            using var db = new BloggingContext();
            var candidate = new Candidate()
            {
                FirstName = firstName,
                LastName = lastName,
                CandidateId = id,
                Active = true
            };
            // Create
            db.Add(candidate);
            db.SaveChanges();

            return Json(candidate);
        }

        public JsonResult FlushDB()
        {
            using var db = new BloggingContext();
            db.Database.EnsureDeleted();

            db.Database.EnsureCreated();
            return Json(null);
        }


        [Route("votes")]
        public JsonResult GetVotes()
        {
            using var db = new BloggingContext();
            var votes = db.Votes.OrderBy(x => x.Date);
            var users = db.Users;
            var candidates = db.Cadidates;

            var x = 1;

            var winner = votes.GroupBy(d => d.CandidateId)
                .Select(
                    g => new
                    {
                        Key = g.Key,
                        Value = g.Count()
                    });

            var results = new List<VoteItem>();
            foreach (var item in winner)
            {
                var vote = new VoteItem
                {
                    Candidate = candidates.First(x => x.CandidateId == item.Key),
                    Vote = item.Value

                };
                vote.Label = vote.Candidate.FirstName + " " + vote.Candidate.LastName;
                results.Add(vote);
            }

            var list = results.OrderByDescending(x => x.Vote).ToList();
            return Json(list);
        }
    }
}
