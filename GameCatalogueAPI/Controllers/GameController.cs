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
    public class GameController : ControllerBase
    {
        private readonly DataService _dataService;
        private readonly string collection = "Game";

        // Constructer initilizes DataService through dependancy injection
        public GameController(DataService dataService) => _dataService = dataService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetAllGames()
        {
            var item = await _dataService.GetAllAsync<Game>(collection);
            if (item != null && item.Count() != 0)
                return Ok(item);
            else
                return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGameById(string id)
        {
            var item = await _dataService.GetRecordByIdAsync<Game>(collection, id);

            if (item == null)
                return NotFound();
            else
                return Ok(item);
        }

    }
}