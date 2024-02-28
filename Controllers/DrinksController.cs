using FastFoodAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FastFoodAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrinksController : ControllerBase
    {

        private FastFoodTacoDbContext _dbCtx = new FastFoodTacoDbContext();

        [HttpGet()]
        public IActionResult GetAllDrinks(string? SortByCost = null)
        {

            List<Drink> result = _dbCtx.Drinks.ToList();
            if (SortByCost.ToLower().Contains("asc"))
            {
                result = result.OrderBy(d => d.Cost).ToList();
            } 
            else if (SortByCost.ToLower().Contains("des"))
            {
                result = result.OrderByDescending(d => d.Cost).ToList();
            }
                
            return Ok(result);
        }

        [HttpGet("GetDrinkById/{id}")]
        public IActionResult GetDrinkById(int id)
        {
            Drink result = _dbCtx.Drinks.FirstOrDefault(t => t.Id == id);

            if (result == null)
            {
                return NotFound("Drink not found");
            }
            return Ok(result);
        }

        [HttpPost()]
        public IActionResult AddDrink([FromBody] Drink newDrink)
        {
            if (GetDrinkById(newDrink.Id) == null)
            {
                _dbCtx.Drinks.Add(newDrink);
                _dbCtx.SaveChanges();
                return Created($"/api/Drinks/{newDrink.Id}", newDrink);
            }
            else
            {
                return Ok("Drink already created.");
            }

        }

        [HttpPut("{id}")]
        public IActionResult UpdateDrink([FromBody] Drink fancierDrink, int id)
        {
            if (id != fancierDrink.Id) { return BadRequest("Please enter valid id"); }
            if (!_dbCtx.Drinks.Any(d => d.Id == id)) { return NotFound("Drink not found"); }
            _dbCtx.Drinks.Update(fancierDrink);
            _dbCtx.SaveChanges();
            return NoContent();
        }

    }
}
