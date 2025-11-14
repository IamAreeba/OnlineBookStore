using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OnlineBookStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<IActionResult> AddItem(int bookId, int qty = 1, int redirect = 0)
        {
            var cartCount = await _cartRepository.AddItem(bookId, qty);
            if (redirect == 0)
            {
                return Ok(cartCount);
            }
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> RemoveItem(int bookId)
        {
            var cartCount = await _cartRepository.RemoveItem(bookId); 
            return RedirectToAction("GetUserCart");
        } 

        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepository.GetUserCart();   
            return View(cart);
        }


        // We are going to use this method as API type of method so that we can call this method from API
        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartCount = await _cartRepository.GetCartItemCount();
            return Ok(cartCount);
        }


        public async Task<IActionResult> Checkout()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutModel model)
        {
            // If DoCheckout fails, the exact exception is thrown automatically
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            bool isCheckedOut = await _cartRepository.DoCheckout(model);
            if (!isCheckedOut)
            {
                return RedirectToAction(nameof(OrderFailure));
            }

            return RedirectToAction(nameof(OrderSuccess));
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }

        public IActionResult OrderFailure()
        {
            return View();
        }


    }
} 
