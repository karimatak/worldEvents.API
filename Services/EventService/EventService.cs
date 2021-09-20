using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using WorldEvents.API.Data;
using WorldEvents.API.DTOs.Event;
using WorldEvents.API.Helpers;
using WorldEvents.API.Helpers.DataShaper;
using WorldEvents.API.Models;
using WorldEvents.API.Models.ModelsParametres;

namespace WorldEvents.API.Services.EventService
{
    public class EventService : IEventService
    {
        private readonly WorldEventsContext _context;
        private readonly IMapper _mapper;
        private readonly IDataShaper<GetEventDto> _dataShaper;

        public EventService(WorldEventsContext context, IMapper mapper, IDataShaper<GetEventDto> dataShaper)
        {
            _context = context;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }

        public async Task<GetEventDto> AddEvent(AddEventDto newEvent)
        {
            var events = _mapper.Map<TblEvent>(newEvent);
            try
            {
                await _context.TblEvent.AddAsync(events);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return _mapper.Map<GetEventDto>(await _context.TblEvent.Include(x => x.Category).Include(x => x.Country).ThenInclude(x => x.Continent).FirstOrDefaultAsync(x => x.EventId == events.EventId));
        }

        public async Task DeleteEvent(int id)
        {
            var events = await _context.TblEvent.FirstOrDefaultAsync(x => x.EventId == id);
            _context.TblEvent.Remove(events);
            await _context.SaveChangesAsync();
        }

        public async Task<GetEventDto> GetEventById(int id)
        {
            var events = await _context.TblEvent
                .Include(c => c.Country)
                .ThenInclude(c => c.Continent)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(e => e.EventId == id);
            return _mapper.Map<GetEventDto>(events);
        }

        public async Task<PagedList<ExpandoObject>> GetEvents(eventParametres eventParametres)
        {
            

            var events = await _context.TblEvent
                .Include(cat => cat.Category)
                .Include(con => con.Country)
                .ThenInclude(con => con.Continent)
                .Where(date => date.EventDate < eventParametres.eventYearMax && date.EventDate > eventParametres.eventYearMin).AsQueryable()
                .Skip(eventParametres.PageSize * (eventParametres.PageNumber - 1))
                .Take(eventParametres.PageSize)
                .ApplySort(eventParametres.orderBy)
                .Select(e => _mapper.Map<GetEventDto>(e))
                .ToListAsync();

            var count = await _context.TblEvent.CountAsync();

            var shapedEvents = _dataShaper.ShapeData(events, eventParametres.Fields).ToList();

            //return PagedList<ExpandoObject>.ToPagedList(shapedEvents,count, eventParametres.PageNumber, eventParametres.PageSize);
            return new PagedList<ExpandoObject>(shapedEvents, count, eventParametres.PageNumber, eventParametres.PageSize);

        }

        public async Task UpdateEvent(UpdateEventDto updateEvent)
        {
            var events = await _context.TblEvent.FirstOrDefaultAsync(x => x.EventId == updateEvent.EventId);
            events.EventName = updateEvent.EventName;
            events.EventDetails = updateEvent.EventDetails;
            events.EventDate = updateEvent.EventDate;
            events.CategoryId = updateEvent.CategoryId;
            events.CountryId = updateEvent.CountryId;

            _context.TblEvent.Update(events);
            await _context.SaveChangesAsync();
        }
    }
}
