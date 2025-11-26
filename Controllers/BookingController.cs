using Microsoft.AspNetCore.Mvc;
using YachtCareAPI.Models;
using YachtCareAPI.Services;

namespace YachtCareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _service;

        public BookingController(IBookingService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Booking>> GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpPost]
        public ActionResult<Booking> Create(BookingRequest request)
        {
            if (request.PreferredDate < DateTime.UtcNow.Date)
                return BadRequest("Дата услуги не может быть в прошлом.");

            if (request.PreferredDate > DateTime.UtcNow.AddYears(1))
                return BadRequest("Дата услуги не может быть позже чем через 1 год.");

            if (_service.IsDateTaken(request.PreferredDate))
                return Conflict("На выбранную дату уже есть бронирование.");

            var booking = _service.Add(request);
            return Ok(booking);
        }

        [HttpPut("{id}")]
        public ActionResult<Booking> Update(int id, BookingRequest request)
        {
            if (request.PreferredDate < DateTime.UtcNow.Date)
                return BadRequest("Дата услуги не может быть в прошлом.");
            if (request.PreferredDate > DateTime.UtcNow.AddYears(1))
                return BadRequest("Дата услуги не может быть позже чем через 1 год.");
            if (_service.IsDateTaken(request.PreferredDate) && _service.GetById(id)?.PreferredDate.Date != request.PreferredDate.Date)
                return Conflict("На выбранную дату уже есть другое бронирование.");

            var updated = _service.Update(id, request);
            if (updated == null)
                return NotFound($"Бронирование с ID {id} не найдено.");

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var removed = _service.Remove(id);
            if (!removed)
                return NotFound($"Бронирование с ID {id} не найдено.");

            return NoContent();
        }
    }
}
