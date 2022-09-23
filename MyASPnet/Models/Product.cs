using System.ComponentModel.DataAnnotations;

namespace MyASPnet.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string Img { get; set; }
        public string Desc { get; set; }

        public Product()
        {

        }
    }
}
