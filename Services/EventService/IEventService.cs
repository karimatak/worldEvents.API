using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using WorldEvents.API.DTOs.Event;
using WorldEvents.API.Helpers;
using WorldEvents.API.Models;
using WorldEvents.API.Models.ModelsParametres;

namespace WorldEvents.API.Services.EventService
{
    public interface IEventService
    {
        Task<PagedList<ExpandoObject>> GetEvents(eventParametres eventParameters);
        Task<GetEventDto> GetEventById(int id);
        Task<GetEventDto> AddEvent(AddEventDto newEvent);
        Task DeleteEvent(int id);
        Task UpdateEvent(UpdateEventDto updateEvent);
    }
}
