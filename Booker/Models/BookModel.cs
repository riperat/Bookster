using System.ComponentModel.DataAnnotations;

namespace Booker.Models;

public class BookModel
{
    public int Id { get; set; }

    [StringLength(100, MinimumLength = 5)]
    [Required(ErrorMessage = "Please enter the title of your book")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Please enter the author name")]
    public string Author { get; set; }

    [Required(ErrorMessage = "Please enter the Genre")]
    public string Genre { get; set; }

    public DateTime PublicationDate { get; set; }

    [Display(Name = "Choose the cover photo of your book")]
    [Microsoft.Build.Framework.Required]
    public IFormFile CoverPhoto { get; set; }

}