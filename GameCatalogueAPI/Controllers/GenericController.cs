using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameCatalogueAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameCatalogueAPI.Controllers
{
    // This class is my attempt at making Wishlist and Played controllers generic so they can both use the same controller
    // To manage both the Wishlist and Played tables in the database

    [Route("/[controller]")]
    [ApiController]
    public class GenericController : ControllerBase
    {
        private readonly DataService _dataService;

        // Constructer initilizes DataService through dependancy injection
        public GenericController(DataService dataService) => _dataService = dataService;


        // By specifying the collection/table your trying to use it can return the appropriate item

        // Gets all records from a collection
        [HttpGet("{collection}")]
        public async Task<ActionResult<T>> GetItem<T>(string collection)
        {
            var item = await _dataService.GetAllAsync<T>(collection);
            if (item != null && item.Count() != 0)
                return Ok(item);
            else
                return NotFound();
        }

        // Gets a single record based on ID
        [HttpGet("{collection}&single={id}")]
        public async Task<ActionResult<T>> GetSingleItem<T>(string collection, string id)
        {
            var item = await _dataService.GetRecordByIdAsync<T>(collection, id);

            if (item == null)
                return NotFound();
            else
                return Ok(item);
        }

        
        // Gets multiple records based on ID (Used for wishlist/ Played)
        [HttpGet("{collection}&multiple={id}")]
        public async Task<ActionResult<IEnumerable<T>>> GetItems<T>(string collection, string id)
        {
            var item = await _dataService.GetMultipleByID<T>(id, collection);
            if (item != null && item.Count() != 0)
                return Ok(item);
            else
                return NotFound();
        }

        // Posts an item to a collection
        [HttpPost("{collection}")]
        public async Task<ActionResult> PostItem<T>([FromBody]T item, string collection)
        {
            try
            {
                await _dataService.InsertAsync(item, collection);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        // Deletes an item from a collection based on id
        [HttpDelete("{collection}&{id}")]
        public async Task<ActionResult> DeleteItem<T>(string collection, string id)
        {
            var deleted = await _dataService.DeleteAsync<T>(id, collection);

            if (deleted)
                return Ok();
            else
                return NotFound();
        }
 
    }
}