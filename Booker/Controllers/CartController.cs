using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Booker.Models;
using System.Security.Claims;
using Booker.Database.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Booker.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _context.Carts
                .Where(c => c.UserId == int.Parse(userId))
                .Include(c => c.Book)
                .ToListAsync();

            return View(cartItems);
        }

        // POST: /Cart/Buy
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Buy(int id, int quantity)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(int.Parse(userId));
            if (user == null)
            {
                return NotFound();
            }

            var cartItem = await _context.Carts
                .Where(c => c.UserId == user.UserId && c.BookId == id)
                .FirstOrDefaultAsync();

            if (cartItem != null)
            {
                // If the book is already in the cart, update the quantity
                cartItem.Amount += quantity;
                _context.Update(cartItem);
            }
            else
            {
                // If the book is not yet in the cart, add it with the specified quantity
                cartItem = new Cart
                {
                    UserId = user.UserId,
                    BookId = id,
                    Amount = quantity
                };
                _context.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Cart/Checkout
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _context.Carts
                .Where(c => c.UserId == int.Parse(userId))
                .Include(c => c.Book)
                .ToListAsync();

            return View(cartItems);
        }

        // POST: /Cart/UpdateQuantity
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartItem = await _context.Carts.FindAsync(Int32.Parse(userId),cartItemId);
            if (cartItem == null)
            {
                return NotFound();
            }

            cartItem.Amount = quantity;
            _context.Update(cartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: /Cart/RemoveFromCart/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var cartItem = await _context.Carts.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: /Cart/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> CheckoutConfirmed()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _context.Carts
                .Where(c => c.UserId == int.Parse(userId))
                .ToListAsync();

            // Perform checkout logic (e.g., create order, process payment, etc.)

            // Clear the cart after checkout
            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home"); // Redirect to home page or order confirmation page
        }
    }
    
    
}
