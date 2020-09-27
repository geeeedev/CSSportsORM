﻿using System;
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
                                                        .OrderByDescending(t => t.TeamId)
                                                        .ThenByDescending(t => t.TeamName);


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
            //...all teams in the Atlantic Soccer Conference (league) 
            // sportsContext.Teams.Include(t => t.CurrLeague).Where(l => l.Name == "Atlantic Soccer Conference")    !!!WRONG: go through League to filter and which contains the object of List<Team>  
            ViewBag.LeaguesQueries = sportsContext.Leagues.Where(l => l.Name == "Atlantic Soccer Conference"
                                                                    || l.Name == "Atlantic Amateur Field Hockey League")
                                                            .Include(l => l.Teams);
            //Note that we are expecting IEnum of <League>, not <Team> (Team will error out)
            IEnumerable<League> modelIEnumLeague = sportsContext.Leagues.Where(l => l.Name == "Atlantic Soccer Conference"
                                                                            || l.Name == "Atlantic Amateur Field Hockey League")
                                                                        .Include(l => l.Teams);
            // return View();                   //ViewBag use this
            // return View(modelIEnumLeague);   //Model use this  //dont forget to drop the IEnum model to View, otherwise will get NullReferenceException for the Model object in cshtml!

            //...all (current) players on the Boston Penguins
            ViewBag.TeamQueries = sportsContext.Teams.Where(t => t.Location == "Boston" && t.TeamName == "Penguins")
                                                        .Include(t => t.CurrPlayers);
            IEnumerable<Team> modelIEnumTeam = sportsContext.Teams.Where(t => t.Location == "Boston" && t.TeamName == "Penguins")
                                                                    .Include(t => t.CurrPlayers);
            // return View();                   //ViewBag use this
            // return View(modelIEnumTeam);     //Model use this  //dont forget to drop the IEnum model to View, otherwise will get NullReferenceException for the Model object in cshtml!

            //...all teams in the International Collegiate Baseball Conference
            // IEnumerable<League> modelIEnum2 = sportsContext.Leagues.Where( l => l.Name == "International Collegiate Baseball Conference")
            //                                                              .Include(l => l.Teams);
            // return View(modelIEnum2);

            //...all teams in the American Conference of Amateur Football
            // IEnumerable<League> modelIEnum2 = sportsContext.Leagues.Where( l => l.Name == "International Collegiate Baseball Conference"
            //                                                                        || l.Name == "American Conference of Amateur Football")
            //                                                              .Include(l => l.Teams);
            // return View(modelIEnum2);

            //...all football teams
            // IEnumerable<League> modelIEnum2 = sportsContext.Leagues.Where( l => l.Sport.Contains("Football"))
            //                                                        .Include(l => l.Teams);
            // return View(modelIEnum2);


            //...all teams with a (current) player named "Sophia"
            // IEnumerable<Player> modelIEnum3  = sportsContext.Players.Where( p => p.FirstName == "Sophia")
            //                                                         .Include(p => p.CurrTeam);

            // return View(modelIEnum3);

            //...everyone with the last name "Flores" who DOESN'T (currently) play for the Raptors (hint: think about ways you can filter data on the cshtml side)
            // ViewBag.IEnum4 = sportsContext.Players.Where(p => p.LastName == "Flores")       //more difficult to do this with ViewBag - makes more sense to do with Model View
            //                                                         .Include( p => p.CurrTeam);  
            // IEnumerable<Player> modelIEnum4 = sportsContext.Players.Where(p => p.LastName == "Flores")
            //                                                         .Include( p => p.CurrTeam);
            // return View(modelIEnum4);

            //...all current players with the Manitoba Tiger-Cats (Team)
            // IEnumerable<Team> modelIEnum5 = sportsContext.Teams.Where(t => (t.Location == "Manitoba" || t.Location =="Montreal") && (t.TeamName == "Tiger-Cats" || t.TeamName == "Wild"))
            //                                                     .Include(t => t.CurrPlayers);
            // return View(modelIEnum5);

            //...all teams that have had 12 or more players
            IEnumerable<Team> modelIEnum6 = sportsContext.Teams.Where(t => t.CurrPlayers.Count > 5)
                                                                .Include(t => t.CurrPlayers)  //needed only for showing .CurrPlayers.Count in cshtml
                                                                .OrderByDescending(t => t.CurrPlayers.Count);
            return View(modelIEnum6);


            // return View(modelIEnum);   
            // return View(modelIEnumTeam);        //Model use this  //dont forget to drop the IEnum model to View, otherwise will get NullReferenceException for the Model object in cshtml!


        }

        [HttpGet("level_3")]
        public IActionResult Level3()
        {
            //...all teams, past and present, that Alexander Bailey has played with
            // IEnumerable<Player> IEmum1 = sportsContext.Players.Where(p => p.FirstName == "Alexander" || p.FirstName =="Isabella" && p.LastName == "Bailey" || p.LastName == "Lewis")  //produces diff result - "FoundMany || NotFound || FoundMany"
            // IEnumerable<Player> IEnum1 = sportsContext.Players.Where(p => (p.FirstName == "Alexander" || p.FirstName =="Lucas") && (p.LastName == "Bailey" || p.LastName == "Lewis"))
            //                                                     .Include(p => p.CurrTeam)
            //                                                     .Include(p => p.PastTeams)
            //                                                     .ThenInclude( pt => pt.PastTeamOfPlayer);
            // return View(IEnum1);

            //...all players, past and present, with the Manitoba Tiger-Cats
            // IEnumerable<Team> IEnum2 = sportsContext.Teams.Where(t => t.Location == "Manitoba" && t.TeamName == "Tiger-Cats")
            //                                                 .Include(t => t.CurrPlayers)
            //                                                 .Include(t => t.PastPlayers)
            //                                                 .ThenInclude(pp => pp.PastPlayerOnTeam);
            // return View(IEnum2);

            //...all players who were formerly (but aren't currently) with the Wichita Vikings
            // IEnumerable<Team> IEnum3 = sportsContext.Teams.Where(t => t.Location == "Wichita" && t.TeamName == "Vikings")
            //                                                 // .Include(t => t.CurrPlayers)
            //                                                 .Include(t => t.PastPlayers)
            //                                                 .ThenInclude(pp => pp.PastPlayerOnTeam);
            // return View(IEnum3);

            //...every team that Emily Sanchez played for before she joined the Indiana Royals
            // IEnumerable<Player> IEnum4 = sportsContext.Players.Where(p => p.FirstName == "Emily" && p.LastName =="Sanchez")
            //                                                     // .Include(p => p.CurrTeam)
            //                                                     .Include(p => p.PastTeams)
            //                                                     .ThenInclude( pt => pt.PastTeamOfPlayer );
            
            // return View(IEnum4);

            //...everyone named "Levi" who has ever played in the Atlantic Federation of Amateur Baseball Players


            //...all players, sorted by the number of teams they've played for
            IEnumerable<Player> IEnum6 = sportsContext.Players//.Include(p => p.CurrTeam)
                                                                .Include(p => p.PastTeams)
                                                                .ThenInclude( pt => pt.PastTeamOfPlayer )
                                                                .OrderByDescending( p => p.PastTeams.Count)
                                                                .ThenBy(p => p.PlayerId);
                                                                
                                                                
            
            return View(IEnum6);


            // return View();
        }

    }
}