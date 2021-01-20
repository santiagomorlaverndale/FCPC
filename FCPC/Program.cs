using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace FCPC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateRegions();

            CreateUsers();

            CreateCandidates();
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        public static void CreateRegions()
        {
            using (var db = new BloggingContext())
            {
                db.Add(new Region
                {
                    RegionId = 1,
                    Name = "Circuito 1",
                    Max = 6
                });

                db.Add(new Region
                {
                    RegionId = 2,
                    Name = "Circuito 2",
                    Max = 4
                });

                db.Add(new Region
                {
                    RegionId = 3,
                    Name = "Circuito 3",
                    Max = 3
                });

                db.Add(new Region
                {
                    RegionId = 4,
                    Name = "Circuito 4",
                    Max = 2
                });

                db.Add(new Region
                {
                    RegionId = 5,
                    Name = "Circuito 5",
                    Max = 1
                });

                db.Add(new Region
                {
                    RegionId = 6,
                    Name = "Circuito 6",
                    Max = 1
                });

                db.SaveChanges();
            }

        }

        public static void CreateUsers()
        {
            using (var db = new BloggingContext())
            {
                // Create
                Console.WriteLine("Inserting a new user");


                //Region 1
                db.Add(new User
                {
                    EmailAddress = "crisvas77@hotmail.com",
                    FirstName = "Circuito1",
                    LastName = "Demo",
                    UserId = "1111111111",
                    RegionId = 1,
                    Active = true
                });

                //Region 2
                db.Add(new User
                {
                    EmailAddress = "crisvas77@hotmail.com",
                    FirstName = "Circuito2",
                    LastName = "Demo",
                    UserId = "2222222222",
                    RegionId = 2,
                    Active = true
                });

                //Region 3
                db.Add(new User
                {
                    EmailAddress = "crisvas77@hotmail.com",
                    FirstName = "Circuito3",
                    LastName = "Demo",
                    UserId = "3333333333",
                    RegionId = 3,
                    Active = true
                });

                //Region 4
                db.Add(new User
                {
                    EmailAddress = "crisvas77@hotmail.com",
                    FirstName = "Circuito4",
                    LastName = "Demo",
                    UserId = "4444444444",
                    RegionId = 4,
                    Active = true
                });

                //Region 5
                db.Add(new User
                {
                    EmailAddress = "crisvas77@hotmail.com",
                    FirstName = "Circuito5",
                    LastName = "Demo",
                    UserId = "5555555555",
                    RegionId = 5,
                    Active = true
                });

                //Region 6
                db.Add(new User
                {
                    EmailAddress = "crisvas77@hotmail.com",
                    FirstName = "Circuito6",
                    LastName = "Demo",
                    UserId = "6666666666",
                    RegionId = 6,
                    Active = true
                });

                db.SaveChanges();
            }

        }


        public static void CreateCandidates()
        {
            using (var db = new BloggingContext())
            {
                // Create
                Console.WriteLine("Inserting a new user");

                db.Add(new Candidate
                {
                    FirstName = "BLANCO",
                    LastName = "",
                    Active = true,
                    CandidateId = "1",
                    RegionId = 0,
                    Photo = ""
                });

                db.Add(new Candidate
                {
                    FirstName = "NULO",
                    LastName = "",
                    Active = true,
                    CandidateId = "0",
                    RegionId = 0,
                    Photo = ""
                });

                db.Add(new Candidate
                {
                    FirstName = "EGMA ROSARIO",
                    LastName = "JADAN LOZANO",
                    Active = true,
                    CandidateId = "0604155630",
                    RegionId = 2,
                    Photo = "/img/candidatos/EGMA-JADAN.png"
                });

                db.Add(new Candidate
                {
                    FirstName = "HOMERO ANTONIO",
                    LastName = "ANDRADE BARBA",
                    Active = true,
                    CandidateId = "0909767170",
                    RegionId = 2,
                    Photo = "/img/candidatos/HOMERO-ANDRADE.png"
                });

                db.Add(new Candidate
                {
                    FirstName = "JUAN CARLOS",
                    LastName = "DOMINGUEZ ESPIN",
                    Active = true,
                    CandidateId = "1712690112",
                    RegionId = 1,
                    Photo = "/img/candidatos/JUAN-CARLOS-DOMINGUEZ.png"
                });

                db.Add(new Candidate
                {
                    FirstName = "RAMIRO GUSTAVO",
                    LastName = "GUERRERO JIMENEZ",
                    Active = true,
                    CandidateId = "1102406756",
                    RegionId = 3,
                    Photo = "/img/candidatos/RAMIRO-GUERRERO.png"
                });

                db.Add(new Candidate
                {
                    FirstName = "ALEJANDRO ANDRES",
                    LastName = "CORONADO MARTINEZ",
                    Active = true,
                    CandidateId = "0909586232",
                    RegionId = 2,
                    Photo = "/img/candidatos/ALEJANDRO-CORONADO.png"
                });
                
                db.Add(new Candidate
                {
                    FirstName = "HERNAN MAURICIO",
                    LastName = "PALLO DE LA CUEVA",
                    Active = true,
                    CandidateId = "1707603534",
                    RegionId = 1,
                    Photo = "/img/candidatos/HERNAN-PALLO.png"
                });
                
                db.Add(new Candidate
                {
                    FirstName = "DAVID ALBERTO",
                    LastName = "BRIONES ALCIVAR",
                    Active = true,
                    CandidateId = "0912638863",
                    RegionId = 2,
                    Photo = "/img/candidatos/DAVID-BRIONES.png"
                });

                db.Add(new Candidate
                {
                    FirstName = "LUIS CESAREO",
                    LastName = "TARIRA VELIZ",
                    Active = true,
                    CandidateId = "0910014612",
                    RegionId = 2,
                    Photo = "/img/candidatos/LUIS-TARIRA.png"
                });

                db.Add(new Candidate
                {
                    FirstName = "RAUL IVAN",
                    LastName = "YASELGA MANCHENO",
                    Active = true,
                    CandidateId = "1707313456",
                    RegionId = 5,
                    Photo = "/img/candidatos/RAUL-YASELGA.png"
                });

                db.Add(new Candidate
                {
                    FirstName = "ANTONIO JOSE",
                    LastName = "ANDRADE CUESTA",
                    Active = true,
                    CandidateId = "0801168907",
                    RegionId = 4,
                    Photo = "/img/candidatos/ANTONIO-ANDRADE.png"
                });

                db.Add(new Candidate
                {
                    FirstName = "EDWIN SANTIAGO",
                    LastName = "CUNALATA CORDOVA",
                    Active = true,
                    CandidateId = "1803340247",
                    RegionId = 3,
                    Photo = "/img/candidatos/EDWIN-CUNALATA.png"
                });

                db.Add(new Candidate
                {
                    FirstName = "MARIA FERNANDA",
                    LastName = "JUMBO CUMBICOS",
                    Active = true,
                    CandidateId = "1717715336",
                    RegionId = 1,
                    Photo = "/img/candidatos/MA-FERNANDA-JUMBO.png"
                });

                db.Add(new Candidate
                {
                    FirstName = "FREDDY OSWALDO",
                    LastName = "VALENZUELA DOMINGUEZ",
                    Active = true,
                    CandidateId = "0913145025",
                    RegionId = 2,
                    Photo = "/img/candidatos/FREDDY-VALENZUELA.png"
                });

                db.Add(new Candidate
                {
                    FirstName = "JORGE EDISON",
                    LastName = "FERNANDEZ GONZALEZ",
                    Active = true,
                    CandidateId = "0400979514",
                    RegionId = 1,
                    Photo = "/img/candidatos/JORGE-FERNANDEZ.png"
                });

                db.Add(new Candidate
                {
                    FirstName = "MARIO GONZALO",
                    LastName = "SOLORZANO SOLORZANO",
                    Active = true,
                    CandidateId = "1600194409",
                    RegionId = 3,
                    Photo = "/img/candidatos/MARIO-SOLORZANO.png"
                });

                db.Add(new Candidate
                {
                    FirstName = "RODRIGO FERNANDO",
                    LastName = "RIVERA PEREZ",
                    Active = true,
                    CandidateId = "0102366119",
                    RegionId = 6,
                    Photo = "/img/candidatos/RODRIGO-RIVERA.png"
                });


                db.SaveChanges();
            }

        }

    }
}
