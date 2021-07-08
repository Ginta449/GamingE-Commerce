using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GamingE_Commerce.Models;
using GamingE_Commerce.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using GamingE_Commerce.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GamingE_Commerce.Controllers
{
    public class ShoppingCartController : Controller
    {
        UserManager<ApplicationUser> _userManager;

        private readonly IGameRepository _gameRepository;
        private readonly ShoppingCart _shoppingCart;
        private readonly ApplicationDbContext _Context;

        public ShoppingCartController(ShoppingCart shoppingCart, IGameRepository gameRepository, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _gameRepository = gameRepository;
            _shoppingCart = shoppingCart;
            this._Context = context;
            this._userManager = userManager;
        }

        public ViewResult Index()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };

            return View(shoppingCartViewModel);
        }

        public RedirectToActionResult AddToShoppingCart(int Id)
        {
            var selectedGame = _gameRepository.AllGames.FirstOrDefault(m => m.Id == Id);

            if (selectedGame != null)
            {

                _shoppingCart.AddToCart(selectedGame, 1);
            }
            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveFromShoppingCart(int Id)
        {
            var selectedGame = _gameRepository.AllGames.FirstOrDefault(m => m.Id == Id);

            if (selectedGame != null)
            {
                _shoppingCart.RemoveFromCart(selectedGame);
            }
            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveAllFromShoppingCart()
        {
            _shoppingCart.ClearCart();
            return RedirectToAction("Index", "Games");
        }

        [Authorize]
        public ActionResult Checkout()
        {
            ClaimsPrincipal currentUser = User;
            var user = _userManager.GetUserAsync(User).Result;
            ViewBag.User = user;

            //ViewBag.CurrentUser = new SelectList(_Context.ApplicationUsers, "Id", "FirstName", "LastName", "Address1", "Address2", "Town", "County", "Eircode", "PhoneNumber", "Email");

            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };

            return View(shoppingCartViewModel);
        }


        public async Task<IActionResult> ProcessOrder([Bind("FirstName,LastName,Address1,Address2,County,Town,Eircode")] Order Order)
        {

            _Context.Orders.Add(Order);
            await _Context.SaveChangesAsync();
            RemoveAllFromShoppingCart();
            return View("OrderSuccess");
        }

        //public async Task<IActionResult> ProcessOrder(FormCollection frmCollection)
        //{
        //    List<ShoppingCart> lstCart = (List<ShoppingCart>)Session[strCart];

        //    await _Context.SaveChangesAsync();
        //    return View("OrderSuccess");
        //}

    }
}

