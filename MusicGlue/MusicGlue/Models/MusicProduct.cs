namespace MusicGlue.Models
{
    public class MusicProduct
    {
        public int Id { get; set; }
        public double Price { get; set; }

        public ProductDescription Description { get; set; }
    }
}
