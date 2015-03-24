using AzureApp.Business.Models;
using AzureApp.Model;
using AzureApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureApp.Business.Services
{
    public interface IImageService
    {
        IEnumerable<ImageDTO> GetImages();
        void InsertImage(string name, byte[] fileData, string contentType);
        ImageDTO GetLatestImage();
    }

    public class ImageService : Service,IImageService
    {
        private readonly IImageContext imageContext;
        private readonly IMapper mapper;
        public ImageService(IImageContext ic, IMapper im)
        {
            this.imageContext = ic;
            this.mapper = im;
        }

        public IEnumerable<ImageDTO> GetImages()
        {
            var images = imageContext.GetImages();
            var imageDTOList = mapper.Map<Image, ImageDTO>(images);
            return imageDTOList;
        }

        public void InsertImage(string name, byte[] fileData, string contentType)
        {
            imageContext.InsertImage(name, fileData, contentType);
        }

        public ImageDTO GetLatestImage()
        {
            var latestImage = imageContext.GetLatestImage();

            return mapper.Map<Image, ImageDTO>(latestImage);            
        }        
    }


}
