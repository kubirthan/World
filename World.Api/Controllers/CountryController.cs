using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using World.Api.Data;
using World.Api.Models;

namespace World.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public CountryController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext; 
        }

        [HttpGet]
        public ActionResult<IEnumerable<Country>> GetAll()
        {
            var countries =  _dbContext.Countries.ToList();

            if(countries == null)
            {
                return NoContent();
            }

            return Ok(countries);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Country> GetById(int id)
        {
            var country = _dbContext.Countries.Find(id);

            if(country == null)
            {
                return NoContent();
            }

            return Ok(country);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Country> Update(int id ,[FromBody]Country country)
        {
            if(country == null || id != country.Id)
            {
                return BadRequest();
            }

            var countryFromdb = _dbContext.Countries.Find(id);

            if(countryFromdb == null)
            {
                return NotFound();
            }

            countryFromdb.Name = country.Name;
            countryFromdb.ShortName = country.ShortName;
            countryFromdb.CountryCode = country.CountryCode;

            _dbContext.Countries.Update(country);
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeleteById(int id)
        {
            var country = _dbContext.Countries.Find(id);
            _dbContext.Countries.Remove(country);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<Country> Create([FromBody]Country country)
        {
            var result = _dbContext.Countries.AsQueryable().Where(x=>x.Name.ToLower().Trim() == country.Name.ToLower().Trim()).Any();

            if (result)
            {
                return Conflict("country already Exists in database");
            }

            _dbContext.Countries.Add(country);
            _dbContext.SaveChanges();
            return CreatedAtAction("GetById", new {id = country.Id},country);
        }
    }
}
