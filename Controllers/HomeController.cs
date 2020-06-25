using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dojodachi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace dojodachi.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("Status") == null)
            {
                HttpContext.Session.SetString("Status", "Vibing");
                HttpContext.Session.SetInt32("Meals", 3);
                HttpContext.Session.SetInt32("Happiness", 20);
                HttpContext.Session.SetInt32("Fullness", 20);
                HttpContext.Session.SetInt32("Energy", 50);

                TempData["Image"] = "yodahatch.gif";
                TempData["Message"] = "Baby Yoda has hatched, congratulations! Use the buttons to take care of him.";
            }
            else if(HttpContext.Session.GetInt32("Fullness") <= 0 || HttpContext.Session.GetInt32("Happiness") <= 0 )
            {
                HttpContext.Session.SetString("Status", "Lose");
                TempData["Image"] = "yodagif.gif";
                TempData["Message"] = "You have mistreated Baby Yoda and now he ded. Your prison sentence is 30 years.";
            }
            else if(HttpContext.Session.GetInt32("Fullness") >= 100 && HttpContext.Session.GetInt32("Happiness") >= 100 && HttpContext.Session.GetInt32("Energy") >= 100)
            {
                HttpContext.Session.SetString("Status", "Win");
                TempData["Image"] = "yodahappy.gif";
                TempData["Message"] = "You have treated Baby Yoda great! You win!!!";
            }
            ViewBag.status = HttpContext.Session.GetString("Status");
            ViewBag.fullness = HttpContext.Session.GetInt32("Fullness");
            ViewBag.happiness = HttpContext.Session.GetInt32("Happiness");
            ViewBag.energy = HttpContext.Session.GetInt32("Energy");
            ViewBag.meals = HttpContext.Session.GetInt32("Meals");
            ViewBag.image = TempData["Image"];
            ViewBag.message = TempData["Message"];
            return View("Index");
        }
        [HttpGet("feed")]
        public IActionResult Feed()
        {
            int? meals = HttpContext.Session.GetInt32("Meals");
            if(meals <=0)
            {
                TempData["Image"] ="yodasad.gif";
                TempData["Message"] = "Uh oh, you don't have enough meals to feed Baby Yoda!";
            }
            else{
                meals--;
                HttpContext.Session.SetInt32("Meals", (int) meals);
                int? fullness = HttpContext.Session.GetInt32("Fullness");
                Random rand = new Random();
                int luck = rand.Next(1,5);
                if( luck == 2)
                {
                    TempData["Image"] ="yodafierce.png";
                    TempData["Message"] = "Baby Yoda ate 1 meal but he did not like it one bit! No increase in fullness!";
                }
                else
                {
                    int randfullness = rand.Next(5, 11);
                    fullness += randfullness;
                    HttpContext.Session.SetInt32("Fullness", (int) fullness);

                    TempData["Image"] ="yodahappy.png";
                    TempData["Message"] = $"Baby Yoda ate 1 meal and gained {randfullness} fullness!";
                }
            }
            return RedirectToAction("Index");

        }
        [HttpGet("play")]
        public IActionResult Play()
        {
            int? energy = HttpContext.Session.GetInt32("Energy");
            if(energy <= 0)
            {
              TempData["Image"] ="yodawakey.png";
              TempData["Message"] = "Uh oh, Baby Yoda doesn't have enough energy to play right now!";
            }
            else
            {
                energy -=5;
                HttpContext.Session.SetInt32("Energy", (int) energy);
                int? happiness = HttpContext.Session.GetInt32("Happiness");
                Random rand = new Random();
                int luck = rand.Next(0,100);
                if (luck <= 25)
                {
                    TempData["Image"] ="yodaunhappy.png";
                    TempData["Message"] = "You played with Baby Yoda but he didn't like it!! He lost 5 energy, no increase in happiness!";
                }
                else
                {
                int randhappiness = rand.Next(5, 11);
                happiness += randhappiness;
                HttpContext.Session.SetInt32("Happiness", (int) happiness);

                TempData["Image"] ="yodahappy.png";
                TempData["Message"] = $"You played with Baby Yoda and he gained {randhappiness} happiness!";
                }
            }
            return RedirectToAction("Index");

        }
        [HttpGet("work")]
        public IActionResult Work()
        {
            int? energy = HttpContext.Session.GetInt32("Energy");
            if(energy <= 0)
            {
              TempData["Image"] ="yodaunhappy.png";
              TempData["Message"] = "Uh oh, Baby Yoda doesn't have enough energy to work right now!";
            }
            else{
                energy -=5;
                HttpContext.Session.SetInt32("Energy", (int) energy);
                int? meals = HttpContext.Session.GetInt32("Meals");
                Random rand = new Random();
                int randmeals = rand.Next(1,4);
                meals += randmeals;
                HttpContext.Session.SetInt32("Meals", (int) meals);

                TempData["Image"] ="yodatries.gif";
                TempData["Message"] = $"Baby Yoda worked and he gained {randmeals} meals but lost 5 energy!";
                

            }
            return RedirectToAction("Index");

        }
        [HttpGet("rest")]
        public IActionResult Rest()
        {
            int? energy = HttpContext.Session.GetInt32("Energy");
            energy +=15;
            HttpContext.Session.SetInt32("Energy", (int) energy);
            int? fullness = HttpContext.Session.GetInt32("Fullness");
            fullness -=5;
            HttpContext.Session.SetInt32("Fullness", (int) fullness);
            int? happiness = HttpContext.Session.GetInt32("Happiness");
            happiness -=5;
            HttpContext.Session.SetInt32("Happiness", (int) happiness);
            TempData["Image"] ="yodawakey.gif";
            TempData["Message"] = "You put Baby Yoda in bed to nap and he gained 15 energy, but lost 5 fullness and happiness!";
            return RedirectToAction("Index");
        }
        [HttpGet("reset")]
        public IActionResult Reset()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        [HttpGet("love")]
        public IActionResult Love()
        {
            int? happiness = HttpContext.Session.GetInt32("Happiness");
            happiness +=10;
            HttpContext.Session.SetInt32("Happiness", (int) happiness);
            TempData["Image"] ="lovehim.gif";
            TempData["Message"] = $"You gave Baby Yoda some love!!! He gained 10 happiness.";
            return RedirectToAction("Index");
        }
        [HttpGet("punish")]
        public IActionResult Punish()
        {
            int? happiness = HttpContext.Session.GetInt32("Happiness");
            happiness -=10;
            HttpContext.Session.SetInt32("Happiness", (int) happiness);
            TempData["Image"] ="yodamad.gif";
            TempData["Message"] = "Baby Yoda was being bad so you scolded him. He lost 10 happiness.";
            return RedirectToAction("Index");
        }
        [HttpGet("walk")]
        public IActionResult Walk()
        {
            int? happiness = HttpContext.Session.GetInt32("Happiness");
            happiness +=5;
            HttpContext.Session.SetInt32("Happiness", (int) happiness);
            int? energy = HttpContext.Session.GetInt32("Energy");
            energy -=15;
            HttpContext.Session.SetInt32("Energy", (int) energy);
            TempData["Image"] ="yodawalk.gif";
            TempData["Message"] = "You took Baby Yoda on a walk! He gained 5 happiness but lost 15 energy.";
            return RedirectToAction("Index");
        }
        [HttpGet("fight")]
        public IActionResult Fight()
        {
            int? meals = HttpContext.Session.GetInt32("Meals");
            meals +=2;
            HttpContext.Session.SetInt32("Meals", (int) meals);
            TempData["Image"] ="yodafight.png";
            TempData["Message"] = "You challenged Baby Yoda and he kicked yo ass. He got free meals.";
            return RedirectToAction("Index");
        }
        [HttpGet("socialize")]
        public IActionResult Socialize()
        {
            int? happiness = HttpContext.Session.GetInt32("Happiness");
            happiness +=5;
            HttpContext.Session.SetInt32("Happiness", (int) happiness);
            TempData["Image"] ="friend.png";
            TempData["Message"] = "You let Baby Yoda have his eewok friend over! He gained 6 happiness.";
            return RedirectToAction("Index");
        }
        [HttpGet("reward")]
        public IActionResult Treats()
        {
            int? fullness = HttpContext.Session.GetInt32("Fullness");
            if(fullness >= 75)
            {
                TempData["Image"] ="yodafat.png";
                TempData["Message"] = "Baby Yoda is cut off!!! He has had too many treats.";
    
                

            }
            else{
                fullness +=5;
                HttpContext.Session.SetInt32("Fullness", (int) fullness);
                TempData["Image"] ="yodasurprise.png";
                TempData["Message"] = "You gave Baby Yoda a treat!! He gained 5 fullness.";
            }
            
        
    
            return RedirectToAction("Index");
        }

        [HttpGet("funeral")]
        public IActionResult Funeral()
        {
            return View("funeral");

        }

       

        
    }
}
