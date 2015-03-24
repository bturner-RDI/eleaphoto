using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureApp.Model
{
    public interface IImageContext
    {
        IEnumerable<Image> GetImages();
        void InsertImage(string name, byte[] fileData, string contentType);
        Image GetLatestImage();
    }

    public class ImageContext : DbContext, IImageContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Image>().Property(x => x.ImageData).HasColumnName("Image");
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Image> Images { get; set; }

        public IEnumerable<Image> GetImages()
        {
            var images = from x in Images
                         select x;

            return images.AsEnumerable();
        }

        public void InsertImage(string name, byte[] imageData, string contentType)
        {
            using (var db = this)
            {
                Image image = new Image();
                image.Name = name;
                image.ImageData = imageData;
                image.ContentType = contentType;
                image.CreateDate = DateTime.Now;

                Images.Add(image);

                db.SaveChanges();
            }
        }

        public Image GetLatestImage()
        {
            var image = (from x in Images
                          orderby x.CreateDate descending
                             select x).FirstOrDefault();

            return image;
        }
    }
}
