using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ChartJSCore.Helpers;
using FCPC.CustomProvider;
using FCPC.Models;
using FCPC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ChartJSCore.Models;
using ClosedXML.Excel;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace FCPC.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public ReportController(IUserService userService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            
            using var db = new BloggingContext();
            var votes = db.Votes.OrderBy(x => x.Date);
            var users = db.Users;
            var candidates = db.Cadidates;
            var regions = db.Regions;


            var final = new GeneralModel();
            final.Results = new List<ResultsModel>();

            foreach (var region in regions)
            {
                var model = new ResultsModel();
                model.Region = region;

                var v = votes.Where(x => x.RegionId == region.RegionId);
                
                var list = new List<ResultItem>();
                foreach (var vote in v)
                {
                    var result = new ResultItem
                    {
                        Vote = vote,
                        User = users.First(x => x.UserId == vote.UserId)
                    };

                    if (!string.IsNullOrEmpty(vote.CandidateId))
                    {
                        result.Candidate = candidates.First(x => x.CandidateId == vote.CandidateId);
                    }
                    else
                    {
                        result.Candidate = candidates.First(x => x.CandidateId == null);
                    }


                    list.Add(result);
                }
                model.Results = list;

                var winner = v.GroupBy(d => d.CandidateId)
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
                        Vote = item.Value
                    };

                    if (!string.IsNullOrEmpty(item.Key))
                    {
                        vote.Candidate = candidates.First(x => x.CandidateId == item.Key);
                    }
                    else
                    {
                        vote.Candidate = candidates.First(x => x.CandidateId == null);
                    }
                    

                    results.Add(vote);
                }
                model.Votes = results.OrderByDescending(x => x.Vote).ToList();


                var chart = new Chart();

                chart.Type = Enums.ChartType.Pie;

                ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();
                data.Labels = model.Votes.Select(x =>  x.Candidate.FirstName + " " + x.Candidate.LastName).ToList();

                var listDouble = new List<double?>();
                foreach (var vote in model.Votes)
                {
                    listDouble.Add(vote.Vote);
                }

                PieDataset dataset = new PieDataset()
                {
                    Label = "Results",
                    Data = listDouble,
                    BackgroundColor = new List<ChartColor>() {
                    ChartColor.FromHexString("#FF6384"),
                    ChartColor.FromHexString("#36A2EB"),
                    ChartColor.FromHexString("#FFCE56"),
                    ChartColor.FromHexString("#FF3384"),
                    ChartColor.FromHexString("#3633EB"),
                    ChartColor.FromHexString("#FF3356")
                },
                    HoverBackgroundColor = new List<ChartColor>() {
                    ChartColor.FromHexString("#FF6380"),
                    ChartColor.FromHexString("#36A2E0"),
                    ChartColor.FromHexString("#FFCE50"),
                    ChartColor.FromHexString("#FF3380"),
                    ChartColor.FromHexString("#3633E0"),
                    ChartColor.FromHexString("#FF3350")
                },
                };

                data.Datasets = new List<Dataset>();
                data.Datasets.Add(dataset);
                chart.Data = data;

                ViewData[$"chart{region.RegionId}"] = chart;


                final.Results.Add(model);
            }
            

            return View(final);
        }

        public IActionResult Csv()
        {
            using var db = new BloggingContext();
            var regions = db.Regions;
            
            var users = db.Users;
            var candidates = db.Cadidates;

            //var builder = new StringBuilder();
            //builder.AppendLine("Id,Nombre,Apellido,Voto,Hora");
            //foreach (var vote in votes)
            //{
            //    var user = users.First(x => x.UserId == vote.UserId);
            //    var cand = candidates.First(x => x.CandidateId == vote.CandidateId);

            //    builder.AppendLine($"{user.UserId},{user.FirstName},{user.LastName},{cand.FirstName} {cand.LastName},{vote.Date.ToString()}");
            //}

            //return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "users.csv");


            using (var workbook = new XLWorkbook())
            {

                foreach (var region in regions)
                {
                    var votes = db.Votes.Where(x => x.RegionId==region.RegionId).OrderBy(x => x.Date);

                    var worksheet = workbook.Worksheets.Add("Region " + region.Name);
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "Documento";
                    worksheet.Cell(currentRow, 2).Value = "Nombre";
                    worksheet.Cell(currentRow, 3).Value = "Voto";
                    worksheet.Cell(currentRow, 4).Value = "Fecha";
                    foreach (var vote in votes)
                    {
                            var user = users.First(x => x.UserId == vote.UserId);
                            var cand = candidates.First(x => x.CandidateId == vote.CandidateId);

                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = user.UserId;
                        worksheet.Cell(currentRow, 2).Value = user.FirstName + " " + user.LastName;
                        worksheet.Cell(currentRow, 3).Value = cand.FirstName + " " + cand.LastName;
                        worksheet.Cell(currentRow, 4).Value = vote.Date;
                    }


                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "reporte.xlsx");
                }
            }

        }

    }
}
