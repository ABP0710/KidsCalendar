using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Calendar.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CalendarController : ControllerBase
    {
        private List<CalendarEvent> _events = new List<CalendarEvent>
    {
        new CalendarEvent { Id = 1, Title = "Meeting", Description = "Team meeting", Icon = "meeting", StartTime = new DateTime(2024, 4, 4, 10, 0, 0), EndTime = new DateTime(2024, 4, 4, 11, 0, 0), UserId = "user123" },
        new CalendarEvent { Id = 2, Title = "Lunch", Description = "Lunch break", Icon = "lunch", StartTime = new DateTime(2024, 4, 5, 12, 0, 0), EndTime = new DateTime(2024, 4, 5, 13, 0, 0), UserId = "user456" }
    };
        [HttpPost("login")]
        public IActionResult Login(LoginModel login)
        {
            // Perform authentication (validate user credentials)
            // If authentication succeeds, create claims for the user
            var claims = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, login.Username),
            });

            // Generate JWT token
            var token = JwtService.GenerateToken("SecretKey", "Issuer", "Audience", 60, claims);

            // Return token to the client
            return Ok(new { Token = token });
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_events);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var calendarEvent = _events.FirstOrDefault(e => e.Id == id);
            if (calendarEvent == null)
            {
                return NotFound();
            }
            return Ok(calendarEvent);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CalendarEvent calendarEvent)
        {
            calendarEvent.Id = _events.Max(e => e.Id) + 1;
            _events.Add(calendarEvent);
            return CreatedAtAction(nameof(Get), new { id = calendarEvent.Id }, calendarEvent);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CalendarEvent calendarEvent)
        {
            var existingEvent = _events.FirstOrDefault(e => e.Id == id);
            if (existingEvent == null)
            {
                return NotFound();
            }
            existingEvent.Title = calendarEvent.Title;
            existingEvent.Description = calendarEvent.Description;
            existingEvent.Icon = calendarEvent.Icon;
            existingEvent.StartTime = calendarEvent.StartTime;
            existingEvent.EndTime = calendarEvent.EndTime;
            existingEvent.UserId = calendarEvent.UserId;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var calendarEvent = _events.FirstOrDefault(e => e.Id == id);
            if (calendarEvent == null)
            {
                return NotFound();
            }
            _events.Remove(calendarEvent);
            return NoContent();
        }
    }
}
