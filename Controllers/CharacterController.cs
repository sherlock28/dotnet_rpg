using Microsoft.AspNetCore.Mvc;
using dotnet_rpg.Models;
using System.Collections.Generic;

namespace dotnet_rpg.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharacterController : ControllerBase
    {
        private static List<Character> characters = new List<Character> {
            new Character(),
            new Character { Id = 2, Name = "Sam", HitPoints = 105, Strength = 12, Defence = 12, Intelligence = 12, Class = RpgClass.Knight }
        };

        [HttpGet]
        public ActionResult<List<Character>> Get()
        {
            return Ok(characters);
        }
    }
}