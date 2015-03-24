using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AzureApp.Business.Models
{
    public class ImageDTO : DTO
    {
        public int ID { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public byte[] ImageData { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ContentType { get; set; }
        
    }
}
