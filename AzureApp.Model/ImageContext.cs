using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

using Npgsql;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.Infrastructure;
using System.Data.Common;

namespace AzureApp.Model
{
    public interface IImageContext
    {
        IEnumerable<Image> GetImages();
        void InsertImage(string name, byte[] fileData, string contentType);
        Image GetLatestImage();
    }

    [DbConfigurationType(typeof(ElephantSqlDbConfiguration))]
    public class PostgresImageContext : DbContext, IImageContext
    {
        //Create DBSets
        public PostgresImageContext()
                : base(new Npgsql.NpgsqlConnection(PostgresImageContext.GetConnString()), true)
        {
            
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Build the model
            modelBuilder.Entity<Image>().Property(x => x.ImageData).HasColumnName("Image");
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        private static string GetConnString()
        {
            var uriString = "postgres://ldwfypsi:Aj7rEEY9-FGPiUNIpueqrxyt0M892oJZ@babar.elephantsql.com:5432/ldwfypsi";
            var uri = new Uri(uriString);
            var db = uri.AbsolutePath.Trim('/');
            var user = uri.UserInfo.Split(':')[0];
            var passwd = uri.UserInfo.Split(':')[1];
            var port = uri.Port > 0 ? uri.Port : 5432;
            var connStr = string.Format("Server={0};Database={1};User Id={2};Password={3};Port={4}",
                uri.Host, db, user, passwd, port);
            return connStr;
        }
    }

    internal sealed class ElephantSqlDbConfiguration : DbConfiguration
    {
        private const string ManifestToken = @"9.2.8";
        public ElephantSqlDbConfiguration()
        {
            this.AddDependencyResolver(new SingletonDependencyResolver<ManifestTokenService>(new ManifestTokenService()));
        }
        private sealed class ManifestTokenService : IManifestTokenResolver
        {
            private static readonly IManifestTokenResolver DefaultManifestTokenResolver
              = new DefaultManifestTokenResolver();
            public string ResolveManifestToken(DbConnection connection)
            {
                if (connection is NpgsqlConnection)
                {
                    return ManifestToken;
                }
                return DefaultManifestTokenResolver.ResolveManifestToken(connection);
            }
        }
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
