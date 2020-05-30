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
    public class PlayedController : ControllerBase
    {
        private readonly DataService _dataService;
        private readonly string collection = "Played";

        // Constructer initilizes DataService through dependancy injection
        public PlayedController(DataService dataService) => _dataService = dataService;

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Played>>> GetPlayed(string id)
        {
            var played = await _dataService.GetMultipleByID<Played>(id, collection);
            if (played != null && played.Count() != 0)
                return Ok(played);
            else
                return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> PostPlayed(Played playedItem)
        {
            try
            {
                await _dataService.InsertAsync(playedItem, collection);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(string id)
        {
            var deleted = await _dataService.DeleteAsync<Played>(id, collection);
            if (deleted)
                return Ok();
            else
                return NotFound();
        }
        

    }
}