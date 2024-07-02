using Booker.Database.Entities;

namespace Booker.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Genre { get; set; }
    public DateTime PublicationDate { get; set; }

    public byte[] ImageData { get; set; }

    public IList<Cart> Carts { get; set; }
}