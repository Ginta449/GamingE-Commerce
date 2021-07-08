using GamingE_Commerce.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GamingE_Commerce.Services
{
    //This is a service for uploading images to the user profile
    //It works with its interface in the Services folder
    //and the user profile at Areas/Identiy/Pages/Account/Manage/Index.cshtml
    public class ImageUpload : IImageUpload
    {
        private IWebHostEnvironment _hostingEnvironment;

        public ImageUpload(IWebHostEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
        }

        public async void UploadImage(IFormFile file)
        {
            long totalBytes = file.Length;
            string filename = file.FileName.Trim('"');
            filename = EnsureFileName(filename);
            byte[] buffer = new byte[16 * 1024];
            using (FileStream output = File.Create(GetpathAndFileName(filename)))
            {
                using (Stream input = file.OpenReadStream())
                {

                    int readBytes;
                    while ((readBytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        await output.WriteAsync(buffer, 0, readBytes);
                        totalBytes += readBytes;
                    }
                }
            }

        }

        private string EnsureFileName(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);
            return filename;
        }

        private string GetpathAndFileName(string filename)
        {
            string path = _hostingEnvironment.WebRootPath + "/uploads/"; //!!!!!!!!!! This is where we specify where the uploaded images are saved
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path + filename;
        }
    }
}
