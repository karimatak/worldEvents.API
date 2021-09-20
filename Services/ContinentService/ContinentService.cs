using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WorldEvents.API.Data;
using WorldEvents.API.DTOs.Continent;
using WorldEvents.API.DTOs.Country;
using WorldEvents.API.Helpers;
using WorldEvents.API.Helpers.DataShaper;
using WorldEvents.API.Models;
using WorldEvents.API.Models.ModelsParametres;

namespace WorldEvents.API.Services.ContinentService
{
    public class ContinentService : IContinentService
    {
        private readonly WorldEventsContext _context;
        private readonly IMapper _mapper;
        private readonly IDataShaper<GetContinentDto> _dataShaper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ContinentService(WorldEventsContext context, IMapper mapper, IDataShaper<GetContinentDto> dataShaper, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _dataShaper = dataShaper;
            this._webHostEnvironment = webHostEnvironment;
        }

        public async Task<GetContinentDto> AddContinent(AddContinentDto newContinent)
        {
            var continent = _mapper.Map<TblContinent>(newContinent);
            try
            {
                continent.FilePath = await uploadImage(newContinent.FilePath);
                
                await _context.TblContinent.AddAsync(continent);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return _mapper.Map<GetContinentDto>(continent);
        }

        public async Task DeleteContinent(int id)
        {
            var continent = await _context.TblContinent.FirstOrDefaultAsync(x => x.ContinentId == id);
            var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
            var filePath = Path.Combine(uploadFolder, continent.FilePath);
            try
            {
                System.IO.File.Delete(filePath);
                _context.TblContinent.Remove(continent);
            }
            catch (Exception)
            {
                throw new Exception();
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task<GetContinentDto> GetContinentById(int id)
        {
            var continent = await _context.TblContinent.FirstOrDefaultAsync(c => c.ContinentId == id);
            return _mapper.Map<GetContinentDto>(continent);
        }

        public async Task<ContinentCountriesDto> GetContinentCountries(int id)
        {
            var countries = await _context.TblCountry
               .Where(x => x.ContinentId == id)
               .Select(x => _mapper.Map<GetCountriesForContinentDto>(x))
               .ToListAsync();

            var continent = await _context.TblContinent
                .FirstOrDefaultAsync(x => x.ContinentId == id);

            var continentDto = _mapper.Map<ContinentCountriesDto>(continent);
            if (continentDto != null)
            {
                continentDto.Countries.AddRange(countries);
            }

            return continentDto;
        }

        public async Task<PagedList<ExpandoObject>> GetContinents(ContinentParametres continentParametres)
        {
            var uploadFolder = Path.Combine(new Uri(@"https://localhost:5001").AbsoluteUri, "Images");
            var allContinents = _context.TblContinent.AsQueryable();

            if (!String.IsNullOrEmpty(continentParametres.ContinentName))
            {
                allContinents = allContinents.Where(c => c.ContinentName.Contains(continentParametres.ContinentName));
            }
                var continents = await allContinents
                .Skip(continentParametres.PageSize * (continentParametres.PageNumber - 1))
                .Take(continentParametres.PageSize)
                .ApplySort(continentParametres.orderBy)
                .Select(x => _mapper.Map<GetContinentDto>(x))
                .ToListAsync();

            foreach (var continent in continents)
            {
                if (continent.FilePath != null)
                {
                    continent.FilePath = new Uri(Path.Combine(uploadFolder, continent.FilePath)).AbsoluteUri;
                }
            }

            var count = await _context.TblContinent.CountAsync();

            var shapedContinents = _dataShaper.ShapeData(continents, continentParametres.Fields).ToList();

            //return PagedList<ExpandoObject>.ToPagedList(shapedContinents, continentParametres.PageNumber,continentParametres.PageSize);
            return new PagedList<ExpandoObject>(shapedContinents, count, continentParametres.PageNumber, continentParametres.PageSize);
        }

        public async Task UpdateContinent(UpdateContinentDto updateContinent)
        {
            var continent = await _context.TblContinent.FirstOrDefaultAsync(x => x.ContinentId == updateContinent.ContinentId);
            continent.ContinentName = updateContinent.ContinentName;
            continent.Summary = updateContinent.Summary;
            if (updateContinent.FilePath != null)
            {
                if (!String.IsNullOrEmpty(continent.FilePath))
                {
                    var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    var fileToDelete = Path.Combine(uploadFolder, continent.FilePath);
                    System.IO.File.Delete(fileToDelete);
                }
                

                continent.FilePath = await uploadImage(updateContinent.FilePath);
            }
            _context.TblContinent.Update(continent);
            await _context.SaveChangesAsync();
        }

        private async Task<string> uploadImage(IFormFile file)
        {
            if (file.Length > 0)
            {
                var supportedTypes = new[] { "image/png", "image/jpeg", "image/gif" };
                if (supportedTypes.Contains(file.ContentType) && file.Length < 1500000)
                {
                    var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

                    var filePath = Path.Combine(uploadFolder, uniqueFileName);


                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return uniqueFileName;
                }
            }
            return null;
        }
    }
}
