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

        [HttpGet("{id}")]
        public ActionResult<Booking> GetById(int id)
        {
            var booking = _service.GetById(id);

            if (booking == null)
                return NotFound();

            return Ok(booking);
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

        [HttpPost("{id}/price-item")]
        public ActionResult<Booking> AddPriceItem(int id, PriceItem item)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
                return BadRequest("Название не может быть пустым.");

            if (item.Price <= 0)
                return BadRequest("Цена должна быть больше нуля.");

            var updated = _service.AddPriceItem(id, item);
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

        [HttpDelete("{id}/price-item/{index}")]
        public ActionResult RemovePriceItem(int id, int index)
        {
            var removed = _service.RemovePriceItem(id, index);
            if (!removed)
                return NotFound("Материал или заявка не найдены");

            return NoContent();
        }

    }
}
