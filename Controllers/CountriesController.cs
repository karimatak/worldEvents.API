using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WorldEvents.API.DTOs.Country;
using WorldEvents.API.Models.ModelsParametres;
using WorldEvents.API.Services.CountryService;

namespace WorldEvents.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCountries([FromQuery]countryParametres countryParametres)
        {
            var countries = await _countryService.GetCountries(countryParametres);
            if (countries == null)
            {
                return NoContent();
            }
            var metadata = new
            {
                countries.TotalCount,
                countries.PageSize,
                countries.CurrentPage,
                countries.HasNext,
                countries.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(countries);
        }
        
        
        
        
        [HttpGet("{id}/Continent",Name = "GetContinentOfCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetContinentOfCountry(int id)
        {
            var country = await _countryService.GetCountryById(id);
            if (country == null)
            {
                return NotFound();
            }
            var continent = country.continent;
            //return Ok(country);
            return Ok(continent);

        }


        [HttpGet("{id}", Name = "GetCountryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCountryById(int id)
        {
            var country = await _countryService.GetCountryById(id);
            if (country == null)
            {
                return NotFound();
            }
            return Ok(country);

        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCountry(AddCountryDto newCountry)
        {
            if (newCountry == null)
            {
                return BadRequest();
            }
            var country = await _countryService.AddCountry(newCountry);
            return CreatedAtRoute(nameof(GetCountryById), new { Id = country.CountryId }, country);
        }
        
        
        
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _countryService.GetCountryById(id);
            if (country == null)
            {
                return NotFound();
            }
            await _countryService.DeleteCountry(id);
            return NoContent();
        }
        
        
        
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCountry(UpdateCountryDto updateCountry)
        {
            var country = await _countryService.GetCountryById(updateCountry.CountryId);
            if (country == null)
            {
                return NotFound();
            }
            await _countryService.UpdateCountry(updateCountry);
            return NoContent();
        }
    }
}
