using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingE_Commerce.Infrastructure
{
    public interface IImageUpload
    {
        //This is an interface for the model ImageUpload in services folder.
        //It is used to upload an image to the user profile in Areas/Identity/Pages/Account/Manage/Index.cshtml
        void UploadImage(IFormFile file)
        {

        }
    }
}
