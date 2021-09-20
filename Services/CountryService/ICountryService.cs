using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using WorldEvents.API.DTOs.Country;
using WorldEvents.API.Helpers;
using WorldEvents.API.Models;
using WorldEvents.API.Models.ModelsParametres;

namespace WorldEvents.API.Services.CountryService
{
   public interface ICountryService
    {
        Task<PagedList<ExpandoObject>> GetCountries(countryParametres countryParametres);
        Task<GetCountryDto> GetCountryById(int id);
        Task<GetCountryDto> AddCountry(AddCountryDto newCountry);
        Task DeleteCountry(int id);
        Task UpdateCountry(UpdateCountryDto updateCountry);
    }
}
