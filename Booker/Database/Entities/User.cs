using System.ComponentModel.DataAnnotations;
using Booker.Database.Entities;

namespace Booker.Models;

public class User : EntityBase
{
    public User()
    {
    }

    [Key] public int UserId { get; set; }

    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Role { get; set; }
    public string Password { get; set; }


    public IList<Cart> Carts { get; set; }
}