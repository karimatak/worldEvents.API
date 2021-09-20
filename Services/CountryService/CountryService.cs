using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using WorldEvents.API.Data;
using WorldEvents.API.DTOs.Continent;
using WorldEvents.API.DTOs.Country;
using WorldEvents.API.Helpers;
using WorldEvents.API.Helpers.DataShaper;
using WorldEvents.API.Models;
using WorldEvents.API.Models.ModelsParametres;

namespace WorldEvents.API.Services.CountryService
{
    public class CountryService : ICountryService
    {
        private readonly WorldEventsContext _context;
        private readonly IMapper _mapper;
        private readonly IDataShaper<GetCountryDto> _dataShaper;

        public CountryService(WorldEventsContext context, IMapper mapper, IDataShaper<GetCountryDto> dataShaper)
        {
            _context = context;
            _mapper = mapper;
           _dataShaper = dataShaper;
        }

        public async Task<GetCountryDto> AddCountry(AddCountryDto newCountry)
        {
            var country = _mapper.Map<TblCountry>(newCountry);
            try
            {
                await _context.TblCountry.AddAsync(country);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return _mapper.Map<GetCountryDto>(await _context.TblCountry.Include(x => x.Continent).FirstOrDefaultAsync(x => x.CountryId == country.CountryId));
        }

        public async Task DeleteCountry(int id)
        {
            var country = await _context.TblCountry.FirstOrDefaultAsync(x => x.CountryId == id);
            _context.TblCountry.Remove(country);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedList<ExpandoObject>> GetCountries(countryParametres countryParametres)
        {
            var countries = await _context.TblCountry
                .Include(x => x.Continent).AsQueryable()
                .Skip(countryParametres.PageSize * (countryParametres.PageNumber - 1))
                .Take(countryParametres.PageSize)
                .ApplySort(countryParametres.orderBy)
                .Select(x => _mapper.Map<GetCountryDto>(x))
                .ToListAsync();

            var count = await _context.TblCountry.CountAsync();

            var shapedCountry = _dataShaper.ShapeData(countries,countryParametres.Fields).ToList();

            //return PagedList<ExpandoObject>.ToPagedList(shapedCountry, countryParametres.PageNumber,countryParametres.PageSize);
            return new PagedList<ExpandoObject>(shapedCountry, count, countryParametres.PageNumber, countryParametres.PageSize);
        }

        public async Task<GetCountryDto> GetCountryById(int id)
        {
            var country = await _context.TblCountry.Include(x => x.Continent).FirstOrDefaultAsync(c => c.CountryId == id);
            return _mapper.Map<GetCountryDto>(country);
        }

        public async Task UpdateCountry(UpdateCountryDto updateCountry)
        {
            var country = await _context.TblCountry.FirstOrDefaultAsync(x => x.CountryId == updateCountry.CountryId);
            country.CountryName = updateCountry.CountryName;
            country.ContinentId = updateCountry.ContinentId;

            _context.TblCountry.Update(country);
            await _context.SaveChangesAsync();
        }
    }
}
