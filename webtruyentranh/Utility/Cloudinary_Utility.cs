using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace webtruyentranh.Utility
{
    public class Cloudinary_Utility
    {
      public static  Account account = new Account(
      "werer",
      "533727741441854",
      "mRZZhczTxF1l0Iv6mX-QJurQdJg");

        public static async Task<String> uploadavartar(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            Cloudinary _cloudinary = new Cloudinary(account);
            using (var filestream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, filestream),


                };
                uploadResult = _cloudinary.Upload(uploadParams);

                if (uploadResult.Error != null)
                    throw new Exception(uploadResult.Error.Message);
                return uploadResult.SecureUri.AbsoluteUri;




            }
        }
    }
}
