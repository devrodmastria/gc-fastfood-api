using FastFoodAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FastFoodAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TacosController : ControllerBase
    {

        private FastFoodTacoDbContext _dbCtx = new FastFoodTacoDbContext();

        [HttpGet()]
        public IActionResult GetAllTacos(bool? softShell = null)
        {

            List<Taco> result = _dbCtx.Tacos.ToList();
            if (softShell != null)
            {
                result = result.Where(t => t.SoftShell == softShell).ToList();
            }
            return Ok(result);
        }

        [HttpGet("GetTacoById/{id}")]
        public IActionResult GetTacoById(int id)
        {
            Taco result = _dbCtx.Tacos.FirstOrDefault(t => t.Id == id);

            if (result == null)
            {
                return NotFound("Taco not found");
            }
            return Ok(result);
        }

        [HttpPost()]
        public IActionResult AddTaco([FromBody] Taco newTaco)
        {
            _dbCtx.Tacos.Add(newTaco);
            _dbCtx.SaveChanges();
            return Created($"/api/Tacos/{newTaco.Id}", newTaco);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTaco(int id)
        {
            Taco taco  = _dbCtx.Tacos.Find(id);
            if (taco == null)
            {
                return NotFound();
            }
            _dbCtx.Tacos.Remove(taco);
            _dbCtx.SaveChanges();
            return NoContent();
            //return Ok("Taco deleted successfully") //my personal preference for good UX
        }

    }
}
