using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsORM.Models;
using Microsoft.EntityFrameworkCore;

namespace SportsORM.Controllers
{
    public class HomeController : Controller
    {

        private static Context sportsContext;

        public HomeController(Context DBContext)
        {
            sportsContext = DBContext;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.BaseballLeagues = sportsContext.Leagues
                .Where(l => l.Sport.Contains("Baseball"));
            return View();
        }

        [HttpGet("level_1")]
        public IActionResult Level1()
        {
            //...all womens' leagues
            ViewBag.AllWomensLeague = sportsContext.Leagues.Where(l => l.Name.Contains("Womens'"));

            //...all leagues where sport is any type of hockey
            // ViewBag.LeaguesQueries = sportsContext.Leagues.Where(l => l.Sport.Contains("Hockey"));       

            //...all leagues where sport is something OTHER THAN football
            // ViewBag.LeaguesQueries = sportsContext.Leagues.Where(l => l.Sport != "Football");            

            //..all leagues that call themselves "conferences"
            // ViewBag.LeaguesQueries = sportsContext.Leagues.Where(l => l.Name.Contains("Conference"));    //"conferences","conference" = None - Case sensitive and must match string

            //...all leagues in the Atlantic region
            ViewBag.LeaguesQueries = sportsContext.Leagues.Where(l => l.Name.Contains("Atlantic"));


            //...all teams based in Dallas
            // ViewBag.TeamsQueries = sportsContext.Teams.Where(t => t.Location == "Dallas");             

            //...all teams named the Raptors
            // ViewBag.TeamsQueries = sportsContext.Teams.Where(t => t.TeamName.Contains("Raptors"));     

            //...all teams whose location includes "City"
            // ViewBag.TeamsQueries = sportsContext.Teams.Where(t => t.Location.Contains("City"));        

            //..all teams whose names begin with "T"
            // ViewBag.TeamsQueries = sportsContext.Teams.Where(t => t.TeamName.StartsWith("T"));         

            //...all teams, ordered alphabetically by location
            // ViewBag.TeamsQueries = sportsContext.Teams.OrderBy(t => t.Location).ThenBy(t=>t.TeamName); 


            //ViewBag.TeamsQueries = sportsContext.Teams.Reverse().ToList();     //cannot get Reverse() to work no matter what

            //...all teams, ordered by team name in reverse alphabetical order
            ViewBag.TeamsQueries = sportsContext.Teams.Include(t => t.CurrLeague)
                                                      .OrderByDescending(t => t.TeamName)
                                                      .ThenByDescending(t => t.Location);

            //...every player with last name "Cooper"
            // ViewBag.PlayersQueries = sportsContext.Players.Where(p => p.LastName == "Cooper");            

            //...every player with first name "Joshua"
            // ViewBag.PlayersQueries = sportsContext.Players.Where(p => p.FirstName == "Joshua");           

            //...every player with last name "Cooper" EXCEPT those with "Joshua" as the first name
            // ViewBag.PlayersQueries = sportsContext.Players.Where(p => p.LastName == "Cooper" && p.FirstName != "Joshua");          

            //...all players with first name "Alexander" OR first name "Wyatt"
            ViewBag.PlayersQueries = sportsContext.Players.Include(pl => pl.CurrTeam)
                                                          .Where(p => p.FirstName == "Alexander" || p.FirstName == "Wyatt");

            // int teamCount = sportsContext.Teams.Count();
            // ViewBag.TeamCount = teamCount;
            ViewBag.TeamCount = sportsContext.Teams.Count();

            // return View();       //Used With ViewBag; commented out now so I can use my ViewModel below

            IEnumerable<Player> filteredModelPlayer = sportsContext.Players
                                                                .Where(p => p.FirstName == "Alexander" || p.FirstName == "Wyatt");
            return View(filteredModelPlayer);
        }

        [HttpGet("level_2")]
        public IActionResult Level2()
        {
            return View();
        }

        [HttpGet("level_3")]
        public IActionResult Level3()
        {
            return View();
        }

    }
}