using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Booker.Models;
using Microsoft.AspNetCore.Authorization;

namespace Booker.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var books = _context.Books.ToList();
        return View(books);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateBook(BookModel bookModel)
    {
        if (ModelState.IsValid)
        {
            var book = new Book
            {
                Title = bookModel.Title,
                Author = bookModel.Author,
                Genre = bookModel.Genre,
                PublicationDate = bookModel.PublicationDate
            };

            if (bookModel.CoverPhoto != null && bookModel.CoverPhoto.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await bookModel.CoverPhoto.CopyToAsync(memoryStream);
                    book.ImageData = memoryStream.ToArray();
                }
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(bookModel);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Edit(int id)
    {
        var book = _context.Books.Find(id);
        if (book == null)
        {
            return NotFound();
        }

        var bookModel = new BookModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Genre = book.Genre,
            PublicationDate = book.PublicationDate
        };

        return View(bookModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, BookModel bookModel)
    {
        if (id != bookModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            book.Title = bookModel.Title;
            book.Author = bookModel.Author;
            book.Genre = bookModel.Genre;
            book.PublicationDate = bookModel.PublicationDate;

            if (bookModel.CoverPhoto != null && bookModel.CoverPhoto.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await bookModel.CoverPhoto.CopyToAsync(memoryStream);
                    book.ImageData = memoryStream.ToArray();
                }
            }

            _context.Update(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(bookModel);
    }


    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var book = _context.Books.Find(id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteConfirmed(int id)
    {
        try
        {
            var book = _context.Books.Find(id);
            if (book == null)
            {
                _logger.LogError($"Book with ID {id} not found.");
                return NotFound();
            }

            _context.Books.Remove(book);
            _context.SaveChanges();
            _logger.LogInformation($"Book with ID {id} deleted successfully.");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting book with ID {id}: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
}