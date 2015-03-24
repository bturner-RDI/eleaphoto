using AzureApp.Business.Models;
using AzureApp.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using AzureApp.Utilities;

namespace AzureApp.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageService imageService;
        public ImageController(IImageService iis)
        {
            imageService = iis;
        }
        // GET: Image
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetLatestImage()
            {
            var imageDTO = imageService.GetLatestImage();

            if (imageDTO != null)
            {
                var resizedImage = ImageUtil.ResizeImage(imageDTO.ImageData, 500, 200);

                var jsonObject = new
                {
                    ContentType = imageDTO.ContentType,
                    Base64 = Convert.ToBase64String(resizedImage)
                };

                return Json(jsonObject, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InsertImage(ImageDTO image)
        {
            if (Request.Files.Count == 1)
            {
                var fileFromRequest = Request.Files[0];

                byte[] fileData = new byte[fileFromRequest.ContentLength];
                fileFromRequest.InputStream.Read(fileData, 0, fileFromRequest.ContentLength);
                image.ImageData = fileData;
                image.ContentType = fileFromRequest.ContentType;
                image.Name = image.Name + Path.GetExtension(fileFromRequest.FileName);

                imageService.InsertImage(image.Name, image.ImageData, image.ContentType);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}