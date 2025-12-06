namespace Library.Management.Demo.Dtos
{
    public class Bookdto
    {

        public string Title { get; set; } = null!;

        public string Author { get; set; }

        public string Category { get; set; }

        public string Publisher { get; set; }

        public int? PublishedYear { get; set; }

        public int Quantity { get; set; }

        public List<int> BookEditions { get; set; } = new List<int>();
        public List<string> Libraries { get; set; } = new List<string>();
        public List<string> Reviews { get; set; } = new List<string>();

    }
}
