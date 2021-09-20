using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WorldEvents.API.DTOs.Event;
using WorldEvents.API.Models;
using WorldEvents.API.Models.ModelsParametres;
using WorldEvents.API.Services.EventService;

namespace WorldEvents.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventsService;

        public EventsController(IEventService eventsService)
        {
            _eventsService = eventsService;
        }

        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEvents([FromQuery]eventParametres eventParametres)
        {
            if (!eventParametres.ValideYearRange)
            {
                return BadRequest("eventDateMax cannot be less than eventDateMin");
            }
            var events = await _eventsService.GetEvents(eventParametres);
            if (events == null)
            {
                return NoContent();
            }
            var metadata = new
            {
                events.TotalCount,
                events.PageSize,
                events.CurrentPage,
                events.HasNext,
                events.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(events);
        }
        
        
        
        [HttpGet("{id}",Name = "GetEventById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventById(int id)
        {
            var events = await _eventsService.GetEventById(id);
            if (events == null)
            {
                return NotFound();
            }
            return Ok(events);
        }
        
        
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCountry(AddEventDto newEvent)
        {
            if (newEvent == null)
            {
                return BadRequest();
            }
            var events = await _eventsService.AddEvent(newEvent);
            return CreatedAtRoute(nameof(GetEventById), new { Id = events.EventId }, events);
        }
        
        
        
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var even = await _eventsService.GetEventById(id);
            if (even == null)
            {
                return NotFound();
            }
            await _eventsService.DeleteEvent(id);
            return NoContent();
        }
        
        
        
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateEvent(UpdateEventDto updateEvent)
        {
            var events = await _eventsService.GetEventById(updateEvent.EventId);
            if (events == null)
            {
                return NotFound();
            }
            await _eventsService.UpdateEvent(updateEvent);
            return NoContent();
        }
    }
}
