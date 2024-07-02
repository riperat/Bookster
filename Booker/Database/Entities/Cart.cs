using Booker.Models;

namespace Booker.Database.Entities
{
    public class Cart
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int Amount { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}