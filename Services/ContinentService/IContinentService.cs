using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using WorldEvents.API.DTOs.Continent;
using WorldEvents.API.Helpers;
using WorldEvents.API.Models;
using WorldEvents.API.Models.ModelsParametres;

namespace WorldEvents.API.Services.ContinentService
{
    public interface IContinentService
    {
        Task<PagedList<ExpandoObject>> GetContinents(ContinentParametres continentParametres);
        Task<GetContinentDto> GetContinentById(int id);
        Task<GetContinentDto> AddContinent(AddContinentDto newContinent);
        Task DeleteContinent(int id);
        Task UpdateContinent(UpdateContinentDto updateContinent);
        Task<ContinentCountriesDto> GetContinentCountries(int id);
    }
}
