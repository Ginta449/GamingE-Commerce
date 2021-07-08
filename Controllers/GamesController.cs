using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GamingE_Commerce.Data;
using GamingE_Commerce.Models;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using GamingE_Commerce.ViewModels;

namespace GamingE_Commerce.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;


        public GamesController(ApplicationDbContext context, 
            IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }


        // GET: Games
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Platforms;
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> AllGames()
        {
            var applicationDbContext = _context.Game.Include(g => g.Platform).Include(g => g.Publisher).Include(g => g.UserReview);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .Include(g => g.Platform)
                .Include(g => g.Publisher)
                .Include(g => g.UserReview)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        [Authorize(Roles = "Admin")]
        // GET: Games/Create
        public IActionResult Create()
        {
            ViewData["PlatformId"] = new SelectList(_context.Set<Platform>(), "Id", "PlatformName");
            ViewData["PublisherId"] = new SelectList(_context.Set<Publisher>(), "Id", "PublisherName");
            ViewData["UserReviewId"] = new SelectList(_context.Set<UserReviews>(), "Id", "UserRating");

            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GameName,GameDescription,GamePrice,GameReleased,GameImageName,PlatformId,PublisherId,UserReviewId,VideoLinkName,")] Game game, [Bind("ImageId,Title,ImageFile")] ImageModel imageModel)
        {
            //Images are uploaded as part of the game creation here. They are saved to the images folder in wwwroot.
            if (ModelState.IsValid)
            {
                //Save image to wwwroot/images folder
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
                string extension = Path.GetExtension(imageModel.ImageFile.FileName);
                imageModel.ImageName = fileName = fileName + DateTime.Now.ToString("yymmddssfff") + extension;
                game.GameImageName = imageModel.ImageName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await imageModel.ImageFile.CopyToAsync(fileStream);
                }

                _context.Add(imageModel);
                _context.Add(game);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(game);
        }

        [Authorize(Roles = "Admin")]
        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            ViewData["PlatformId"] = new SelectList(_context.Set<Platform>(), "Id", "Id", game.PlatformId);
            ViewData["PublisherId"] = new SelectList(_context.Set<Publisher>(), "Id", "Id", game.PublisherId);
            ViewData["UserReviewId"] = new SelectList(_context.Set<UserReviews>(), "Id", "Id", game.UserReviewId);
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GameName,GameDescription,GamePrice,GameReleased,GameImageName,PlatformId,PublisherId,UserReviewId")] Game game)
        {
            if (id != game.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.Id))
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
            ViewData["PlatformId"] = new SelectList(_context.Set<Platform>(), "Id", "Id", game.PlatformId);
            ViewData["PublisherId"] = new SelectList(_context.Set<Publisher>(), "Id", "Id", game.PublisherId);
            ViewData["UserReviewId"] = new SelectList(_context.Set<UserReviews>(), "Id", "Id", game.UserReviewId);

            return View(game);
        }


        [Authorize(Roles = "Admin")]
        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .Include(g => g.Platform)
                .Include(g => g.Publisher)
                .Include(g => g.UserReview)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Game.FindAsync(id);            
            _context.Game.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Game.Any(e => e.Id == id);
        }

        public IActionResult PlaystationGames()
        {

            var game = _context.Game
                .Include(g => g.Platform).Include(g => g.Publisher).Include(g => g.UserReview)
               .Where(g => g.Platform.PlatformName == "Playstation 5");
            return View(game);
        }

        public IActionResult XboxGames()
        {

            var game = _context.Game
                .Include(g => g.Platform).Include(g => g.Publisher).Include(g => g.UserReview)
               .Where(g => g.Platform.PlatformName == "XBOX");
            return View(game);
        }
        public IActionResult NintendoGames()
        {

            var game = _context.Game
                .Include(g => g.Platform).Include(g => g.Publisher).Include(g => g.UserReview)
               .Where(g => g.Platform.PlatformName == "Nintendo");
            return View(game);
        }
        public IActionResult PcGames()
        {

            var game = _context.Game
                .Include(g => g.Platform).Include(g => g.Publisher).Include(g => g.UserReview)
               .Where(g => g.Platform.PlatformName == "PC");
            return View(game);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string searchString)
        {
            var games = from g in _context.Game
                        select g;



            if (!String.IsNullOrEmpty(searchString))
            {
                games = games.Where(s => s.GameName.Contains(searchString));
            }



            return View(await games.ToListAsync());
        }
    }
}
