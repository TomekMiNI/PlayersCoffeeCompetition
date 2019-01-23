using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sports;

namespace lastyearTASK.Controllers
{
    public class PlayerController : Controller
    {
        [HttpPost]
        public IActionResult AddPlayer([FromBody] Player p)
        {
            if (sports.PlayersStore.AddPlayer(new Player(p.Number, p.FirstName, p.Age, p.Experience)))
                return NoContent();
            else
                return StatusCode(500);
        }
        [HttpGet]
        public List<int> GetAll()
        {
            return sports.PlayersStore.GetPlayerNumbers();
        }
        [HttpGet]
        public IActionResult GetPlayer(int number)
        {
            try
            {
                return Ok(sports.PlayersStore.GetPlayer(number));
            }
            catch
            {
                return NotFound();
            }
        }
        [HttpDelete]
        public IActionResult DeletePlayer(int id)
        {
            if (sports.PlayersStore.RemovePlayer(id))
                return NoContent();
            else
                return StatusCode(500);
        }

        //rozpocznij competition
        [HttpGet]
        public IActionResult GetResults()
        {
            return Ok(sports.PlayersStore.PerformCompetition());
        }
    }

}