using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Interface;
using Project.models.DTOs;
using Project.models;
using System.Security.Claims;

namespace Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
    public class CartController : ControllerBase{

        [HttpPost("finish")]
        public async Task<ActionResult<Purchase>> FinishOrder()
        {
            try
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdStr))
                {
                    _logger.LogError("User identifier claim is missing");
                    return Unauthorized("User identifier is missing");
                }
                int userId = int.Parse(userIdStr);
                _logger.LogInformation($"Finishing order for user with Id: {userId}");

                // נניח שיש לך IPurchaseService ב-DI (אם לא, יש להוסיף)
                var purchaseService = (HttpContext.RequestServices.GetService(typeof(IPurchaseService)) as IPurchaseService);
                if (purchaseService == null)
                {
                    _logger.LogError("IPurchaseService not available");
                    return StatusCode(500, "Internal server error");
                }

                var purchase = await purchaseService.ConvertCartToPurchase(userId);
                return Ok(purchase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while finishing order");
                return StatusCode(500, "Internal server error");
            }
        }
    
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<GiftCart>> GetCart()
        {
            try
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdStr))
                {
                    _logger.LogError("User identifier claim is missing");
                    return Unauthorized("User identifier is missing");
                }
                int userId = int.Parse(userIdStr);
                _logger.LogInformation($"Getting cart for user with Id: {userId}");
                var cart = await _cartService.GetCartByUserIdAsync(userId);
                // תמיד נחזיר מערך (ריק אם אין עגלה)
                return Ok(cart?.GiftCartItems ?? new List<GiftCartItem>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while getting cart");
                return StatusCode(500, "Internal server error");
            }
        }

        // Endpoint להחזרת כל העגלות (למנהל/בדיקות)
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<GiftCart>>> GetAllCarts()
        {
            try
            {
                var carts = await _cartService.GetAllCartsAsync();
                return Ok(carts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while getting all carts");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// מוסיף מתנה לעגלה של המשתמש ומחזיר את העגלה העדכנית
        /// </summary>
        /// <param name="cartDto">GiftCartItemDto (giftId, quantity)</param>
        /// <returns>רשימת פריטי עגלה עדכנית</returns>
        [HttpPost]
        [HttpPost("add")]
        public async Task<ActionResult<IEnumerable<GiftCartItem>>> AddToCart([FromBody] GiftCartItemDto cartDto)
        {
            try
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdStr))
                {
                    _logger.LogError("User identifier claim is missing");
                    return Unauthorized("User identifier is missing");
                }
                if (cartDto == null || cartDto.GiftId <= 0 || cartDto.Quantity <= 0)
                {
                    return BadRequest("יש לספק מזהה מתנה וכמות חוקית");
                }
                int userId = int.Parse(userIdStr);
                _logger.LogInformation($"Adding item to cart: GiftId={cartDto.GiftId}, Quantity={cartDto.Quantity}");

                await _cartService.AddToCartAsync(userId, cartDto);
                // נחזיר תמיד את כל העגלה העדכנית
                var cart = await _cartService.GetCartByUserIdAsync(userId);
                return Ok(cart?.GiftCartItems ?? new List<GiftCartItem>());
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid input for adding to cart");
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Cannot add raffled gift to cart");
                return Conflict("לא ניתן להוסיף לעגלה מתנה שכבר הוגרלה");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Gift not found when adding to cart");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while adding item to cart");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// מסיר פריט מהעגלה של המשתמש ומחזיר את העגלה העדכנית
        /// </summary>
        /// <param name="itemId">מזהה פריט עגלה</param>
        /// <returns>רשימת פריטי עגלה עדכנית</returns>
        [HttpDelete("{itemId}")]
        public async Task<ActionResult<IEnumerable<GiftCartItem>>> DeleteCartItem(int itemId)
        {
            try
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdStr))
                {
                    _logger.LogError("User identifier claim is missing");
                    return Unauthorized("User identifier is missing");
                }
                int userId = int.Parse(userIdStr);
                _logger.LogInformation($"Deleting CartItem with Id: {itemId} for user Id: {userId}");

                await _cartService.DeleteFromCart(itemId);
                var cart = await _cartService.GetCartByUserIdAsync(userId);
                return Ok(cart?.GiftCartItems ?? new List<GiftCartItem>());
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid cart item id");
                return BadRequest("מזהה פריט לא חוקי");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Cart item not found");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while deleting item from cart");
                return StatusCode(500, "שגיאה פנימית בשרת");
            }
        }
        /// <summary>
        /// דוגמת שימוש ב-API (Swagger/Docs)
        /// </summary>
        /// <remarks>
        /// דוגמה להוספת מתנה לעגלה:
        /// POST /api/cart/add
        /// {
        ///   "giftId": 1,
        ///   "quantity": 2
        /// }
        /// תשובה:
        /// [
        ///   { "id": 1, "giftId": 1, "quantity": 2, "price": 50, "giftName": "בובה" }, ...
        /// ]
        /// </remarks>
    }
}

