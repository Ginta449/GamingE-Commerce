using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GamingE_Commerce.Data;
using GamingE_Commerce.Infrastructure;
using GamingE_Commerce.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GamingE_Commerce.Areas.Identity.Pages.Account.Manage
{
    //Image uploading for the profile can be found in OnPostAsync()
    //On Line 150

    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IImageUpload _imageUpload;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment hostEnvironment,
            ApplicationDbContext context,
            IImageUpload unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hostEnvironment = hostEnvironment;
            _context = context;
            _imageUpload = unitOfWork;
        }

        public string ImagePath { get; set; }

        public string Username { get; set; }

        public IFormFile ImageFile { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }


            [DataType(DataType.Text)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }


            [DataType(DataType.Text)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }


            [DataType(DataType.Text)]
            [Display(Name = "Line 1")]
            public string Address1 { get; set; }


            [DataType(DataType.Text)]
            [Display(Name = "Line 2")]
            public string Address2 { get; set; }


            [DataType(DataType.Text)]
            [Display(Name = "Town")]
            public string Town { get; set; }


            [DataType(DataType.Text)]
            [Display(Name = "County")]
            public string County { get; set; }


            [DataType(DataType.Text)]
            [Display(Name = "Eircode")]
            public string Eircode { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "ImagePath")]
            public string ImagePath { get; set; }

        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            ImagePath = user.ImagePath;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address1 = user.Address1,
                Address2 = user.Address2,
                Town = user.Town,
                County = user.County,
                Eircode = user.Eircode,
                ImagePath = user.ImagePath
            };
        }

        // GET: Index/Create
        public IActionResult AddProfileImage()
        {
            return Page();
        }
        

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            ImagePath = user.ImagePath;

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile file, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                if (file != null) //If there is an image file to be uploaded, we execute this code
                {
                    _imageUpload.UploadImage(file); //This is where the image file is uploaded. 
                    //The conversion is done with the model in the Services folder (Services>ImageUpload) 
                    //and the interface in the Infrastructure folder (Infrastructure>IImageUpload). 
                    //Profile images are saved to the uploads folder
                    var user = await _userManager.GetUserAsync(User);
                    if (user == null)
                    {
                        return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                    }

                    if (!ModelState.IsValid)
                    {
                        await LoadAsync(user);
                        return Page();
                    }

                    var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
                    if (Input.PhoneNumber != phoneNumber)
                    {
                        var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                        if (!setPhoneResult.Succeeded)
                        {
                            var userId = await _userManager.GetUserIdAsync(user);
                            throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                        }

                    }
                    if (Input.FirstName != user.FirstName)
                    {
                        user.FirstName = Input.FirstName;
                    }
                    if (Input.LastName != user.LastName)
                    {
                        user.LastName = Input.LastName;
                    }
                    if (Input.Address1 != user.Address1)
                    {
                        user.Address1 = Input.Address1;
                    }
                    if (Input.Address2 != user.Address2)
                    {
                        user.Address2 = Input.Address2;
                    }
                    if (Input.Town != user.Town)
                    {
                        user.Town = Input.Town;
                    }
                    if (Input.County != user.County)
                    {
                        user.County = Input.County;
                    }
                    if (Input.Eircode != user.Eircode)
                    {
                        user.Eircode = Input.Eircode;
                    }
                    if (Input.ImagePath != file.FileName)
                    {
                        user.ImagePath = "/uploads/" + file.FileName;
                    }

                    await _userManager.UpdateAsync(user);

                    await _signInManager.RefreshSignInAsync(user);

                    StatusMessage = "Your profile has been updated";
                }

                else //If there is no image file being uploaded. This seems like a long way of doing it, but we had errors when trying shorter codes
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user == null)
                    {
                        return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                    }

                    if (!ModelState.IsValid)
                    {
                        await LoadAsync(user);
                        return Page();
                    }

                    var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
                    if (Input.PhoneNumber != phoneNumber)
                    {
                        var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                        if (!setPhoneResult.Succeeded)
                        {
                            var userId = await _userManager.GetUserIdAsync(user);
                            throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                        }

                    }
                    if (Input.FirstName != user.FirstName)
                    {
                        user.FirstName = Input.FirstName;
                    }
                    if (Input.LastName != user.LastName)
                    {
                        user.LastName = Input.LastName;
                    }
                    if (Input.Address1 != user.Address1)
                    {
                        user.Address1 = Input.Address1;
                    }
                    if (Input.Address2 != user.Address2)
                    {
                        user.Address2 = Input.Address2;
                    }
                    if (Input.Town != user.Town)
                    {
                        user.Town = Input.Town;
                    }
                    if (Input.County != user.County)
                    {
                        user.County = Input.County;
                    }
                    if (Input.Eircode != user.Eircode)
                    {
                        user.Eircode = Input.Eircode;
                    }

                    await _userManager.UpdateAsync(user);

                    await _signInManager.RefreshSignInAsync(user);

                    StatusMessage = "Your profile has been updated";
                }
            }                
            
            return RedirectToPage();
        }
    }
}
