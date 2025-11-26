using YachtCareAPI.Models;
using System.Collections.Concurrent;

namespace YachtCareAPI.Services
{
    public class BookingService : IBookingService
    {
        private readonly ConcurrentDictionary<int, Booking> _bookings = new();
        private int _seq = 0;

        public IEnumerable<Booking> GetAll() => _bookings.Values.OrderBy(b => b.Id);

        public Booking Add(BookingRequest request)
        {
            var id = Interlocked.Increment(ref _seq);
            var booking = new Booking
            {
                Id = id,
                ClientName = request.ClientName,
                ServiceType = request.ServiceType,
                PreferredDate = request.PreferredDate,
                Status = "Pending"
            };
            _bookings[id] = booking;
            return booking;
        }

        public bool IsDateTaken(DateTime date)
        {
            return _bookings.Values.Any(b => b.PreferredDate.Date == date.Date);
        }

        // Новый метод: получить бронирование по ID
        public Booking? GetById(int id)
        {
            _bookings.TryGetValue(id, out var booking);
            return booking;
        }

        // Новый метод: обновить бронирование
        public Booking? Update(int id, BookingRequest request)
        {
            if (!_bookings.ContainsKey(id))
                return null;

            var booking = _bookings[id];
            booking.ClientName = request.ClientName;
            booking.ServiceType = request.ServiceType;
            booking.PreferredDate = request.PreferredDate;
            return booking;
        }

        // Новый метод: удалить бронирование
        public bool Remove(int id)
        {
            return _bookings.TryRemove(id, out _);
        }
    }
}
