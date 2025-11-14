using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineBookStore.Models;

namespace OnlineBookStore.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(ApplicationDbContext db, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> AddItem(int bookId, int qty)
        {
            var userId = GetUserId();
            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("user is not logged-in");
                }

                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId
                    };
                    _db.ShoppingCarts.Add(cart);
                }
                await _db.SaveChangesAsync();

                // Cart Detail section
                // Check if the item is already present in the cart
                // chec
                var cartItem = await _db.CartDetails.FirstOrDefaultAsync(a => a.ShoppingCartId == cart.Id && a.BookId == bookId);
                if (cartItem is not null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    var book = _db.Books.Find(bookId);
                    cartItem = new CartDetail
                    {
                        BookId = bookId,
                        ShoppingCartId = cart.Id,
                        Quantity = qty,
                        // In orderDetai we need the unit price but tht was not there in cartDetail so i added the fiedl in the cartDetail 
                        UnitPrice = book.Price

                    };
                    _db.CartDetails.Add(cartItem);
                }
                _db.SaveChanges();
                transaction.Commit();

            }

            catch (Exception)
            {

            }

            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<int> RemoveItem(int bookId)
        {
            var userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("user is not logged-in");
                }

                var cart = await GetCart(userId);
                if (cart is null)
                {
                    throw new Exception("Invalid Cart");
                }
                await _db.SaveChangesAsync();

                // Cart Detail section
                // Check if the item is already present in the cart
                // chec
                var cartItem = await _db.CartDetails.FirstOrDefaultAsync(a => a.ShoppingCartId == cart.Id && a.BookId == bookId);
                if (cartItem is null)
                {
                    throw new Exception("No items in cart");
                }
                else if (cartItem.Quantity == 1)
                {
                    _db.CartDetails.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity = cartItem.Quantity - 1;
                }

                _db.SaveChanges();
            }

            catch (Exception)
            {

            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<ShoppingCart> GetUserCart()
        {
            // I am getting records from multiple tables. 
            // Structure Shopping Cart -> multiple CartDetails -> 1 CartDetails have Book -> That Book have Author and Genre    
            // So to get record from all tables i have to join all tables using inner join query

            var userId = GetUserId();
            if (userId == null)
            {
                throw new Exception("Invalid UserId");
            }
            var shoppingCart = await _db.ShoppingCarts
                                  .Include(a => a.CartDetails)
                                  .ThenInclude(a => a.Book)
                                  .ThenInclude(a => a.Stock)
                                  .Include(a => a.CartDetails)
                                  .ThenInclude(a => a.Book)
                                  .ThenInclude(a => a.Genre)
                                  .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;

        }

        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _db.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }


        // This method tells how many items are available in cart
        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            // Writing LINQ query to get count of items in cart
            var data = await (from cart in _db.ShoppingCarts
                              join CartDetail in _db.CartDetails
                              on cart.Id equals CartDetail.ShoppingCartId
                              where cart.UserId == userId // Updated Line
                              select new { CartDetail.Id }
                             ).ToListAsync();
            return data.Count;
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(principal);
            return userId;
        }

        public async Task<bool> DoCheckout(CheckoutModel model)
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                // logic: Move data from cartDetail to order and orderDetail then we will remove cart detail
                // entry -> order, orderDetail
                // remove -> cartDetail data
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedAccessException("User is not logged-in");
                }
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    throw new UnauthorizedAccessException("Invalid Cart");
                }

                var cartDetail = _db.CartDetails.Where(a => a.ShoppingCartId == cart.Id).ToList();
                if (cartDetail.Count == 0)
                { 
                    throw new Exception("No items in cart to checkout");
                }

                var pendingRecord = _db.orderStatuses.FirstOrDefault(s => s.StatusName == "Pending");

                if(pendingRecord is null)
                {
                    throw new Exception("Order status does not have pending status");
                }

                var order = new Order
                {
                    UserId = userId,
                    CreateDate = DateTime.Now,
                    Name = model.Name,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber,
                    PaymentMethod = model.PaymentMethod,
                    Address = model.Address,
                    IsPaid = false,
                    OrderStatusId = pendingRecord.Id, // Pending
                };
                _db.Orders.Add(order);
                _db.SaveChanges();
                foreach (var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        BookId = item.BookId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                    };
                    _db.OrderDetails.Add(orderDetail);
                }

                // removing the cartdetails
                _db.CartDetails.RemoveRange(cartDetail);

                _db.SaveChanges();
                transaction.Commit();
                return true;

            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}


