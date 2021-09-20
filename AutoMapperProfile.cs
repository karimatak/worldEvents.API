using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldEvents.API.DTOs.Category;
using WorldEvents.API.DTOs.Continent;
using WorldEvents.API.DTOs.Country;
using WorldEvents.API.DTOs.Event;
using WorldEvents.API.Models;

namespace WorldEvents.API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<GetCategoryDto, TblCategory>().ReverseMap();
            CreateMap<AddCategoryDto, TblCategory>().ReverseMap();
            CreateMap<UpdateCategoryDto, TblCategory>().ReverseMap();

            CreateMap<GetContinentDto, TblContinent>().ReverseMap();
            CreateMap<AddContinentDto, TblContinent>().ReverseMap();
            CreateMap<UpdateContinentDto, TblContinent>().ReverseMap();
            CreateMap<ContinentCountriesDto, TblContinent>().ReverseMap();


            CreateMap<TblCountry, GetCountryDto>().ReverseMap();
            CreateMap<TblCountry, AddCountryDto>().ReverseMap();
            CreateMap<TblCountry, UpdateCountryDto>().ReverseMap();
            CreateMap<TblCountry, GetCountriesForContinentDto>().ReverseMap();

            CreateMap<TblEvent, GetEventDto>().ReverseMap();
            CreateMap<TblEvent, AddEventDto>().ReverseMap();
            CreateMap<TblEvent, UpdateEventDto>().ReverseMap();
        }
    }
}
