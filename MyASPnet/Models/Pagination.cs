namespace MyASPnet.Models
{
    public class Pagination
    {
        public List<Product> list { get; set; }
        public int pageTotal { get; set; }
        public Pagination() { }
        public Pagination(List<Product> l, int total)
        {
            this.list = l;
            pageTotal = total;
        }
    }
}
