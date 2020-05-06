using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameCatalogueAPI.Models;
using GameCatalogueAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameCatalogueAPI.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataService _dataService;
        private readonly string collection = "User";

        // Constructer initilizes DataService through dependancy injection
        public UserController(DataService dataService) => _dataService = dataService;

        // Gets user information based on their login details
        [HttpGet("user={uName}&pwrd={password}")]
        public async Task<ActionResult<User>> GetUser(string uName, string password)
        {
            var user = await _dataService.GetUserAsync(uName, password);

            if (user == null)
                return BadRequest();
            else
                return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> PostUser([FromBody]User user)
        {
            try
            {
                await _dataService.InsertAsync(user, collection);

                return Ok();
            }
            catch
            {
                return BadRequest();            
            }
        }

        [HttpPut]
        public async Task<ActionResult> PutUser([FromBody]User user)
        {
            try
            {
                await _dataService.UpdateAsync(user.Id, user, collection);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }

        }

    }
}