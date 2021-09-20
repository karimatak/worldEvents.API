using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WorldEvents.API.DTOs.Continent;
using WorldEvents.API.Models.ModelsParametres;
using WorldEvents.API.Services.ContinentService;

namespace WorldEvents.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContinentsController : ControllerBase
    {
        private readonly IContinentService _continentService;

        public ContinentsController(IContinentService continentService)
        {
            _continentService = continentService;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetContinents([FromQuery]ContinentParametres continentParametres)
        {
            var continents = await _continentService.GetContinents(continentParametres);
            if (continents == null)
            {
                return NoContent();
            }
            
            var metadata = new
            {
                continents.TotalCount,
                continents.PageSize,
                continents.CurrentPage,
                continents.HasNext,
                continents.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(continents);
        }
        [HttpGet("{id}",Name = "GetContinentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetContinentById(int id)
        {
            var continent = await _continentService.GetContinentById(id);
            if (continent == null)
            {
                return NotFound();
            }
            return Ok(continent);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddContinent([FromForm]AddContinentDto newContinent)
        {
            if (newContinent == null)
            {
                return BadRequest();
            }
            var continent = await _continentService.AddContinent(newContinent);
            return CreatedAtRoute(nameof(GetContinentById), new { Id = continent.ContinentId }, continent);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteContinent(int id)
        {
            var continent = await _continentService.GetContinentById(id);
            if (continent == null)
            {
                return NotFound();
            }

            await _continentService.DeleteContinent(id);
            return NoContent();
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateContinent([FromForm]UpdateContinentDto updateContinent)
        {
            var continent = await _continentService.GetContinentById(updateContinent.ContinentId);
            if (continent == null)
            {
                return NotFound();
            }
            await _continentService.UpdateContinent(updateContinent);
            return NoContent();
        }
        
        
        
        [HttpGet("{ContinentId}/Countries")]
        public async Task<IActionResult> GetContinentCountries(int ContinentId)
        {
            var ContinentCountries = await _continentService.GetContinentCountries(ContinentId);

            if (ContinentCountries == null)
            {
                return NotFound();
            }
            return Ok(ContinentCountries);
        }
    }
}
