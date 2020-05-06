using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameCatalogueAPI.Models;
using GameCatalogueAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameCatalogueAPI.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly DataService _dataService;
        private readonly string collection = "Wishlist";

        // Constructer initilizes DataService through dependancy injection
        public WishlistController(DataService dataService) => _dataService = dataService;


        // Retrieves all wishlist items for a user by using the User ID to find them
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Wishlist>>> GetWishlist(string id)
        {
            var wishlists = await _dataService.GetMultipleByID<Wishlist>(id, collection);
            if (wishlists != null && wishlists.Count() != 0)
                return Ok(wishlists);
            else
                return NotFound();
        }

        // Posts a new wishlist item
        [HttpPost]
        public async Task<ActionResult> PostWishlist([FromBody]Wishlist wishlistItem)
        {
            try
            {
                await _dataService.InsertAsync(wishlistItem, collection);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        // Deletes a wishlist item based on its unique ID
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(string id)
        {
            var deleted = await _dataService.DeleteAsync<Wishlist>(id, collection);

            if (deleted)
                return Ok();
            else
                return NotFound();
        }
    }
}