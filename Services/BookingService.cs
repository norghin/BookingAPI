using YachtCareAPI.Models;
using System.Collections.Concurrent;

namespace YachtCareAPI.Services
{
    public class BookingService : IBookingService
    {
        private readonly ConcurrentDictionary<int, Booking> _bookings = new();
        private int _seq = 0;

        // Допустимые статусы
        private static readonly HashSet<string> AllowedStatuses = new()
        {
            "Pending",
            "Confirmed",
            "InProgress",
            "Completed",
            "Cancelled"
        };

        public IEnumerable<Booking> GetAll()
        {
            return _bookings.Values.OrderBy(b => b.Id);
        }

        public Booking Add(BookingRequest request)
        {
            var id = Interlocked.Increment(ref _seq);

            var booking = new Booking
            {
                Id = id,
                ClientName = request.ClientName,
                ServiceType = request.ServiceType,
                PreferredDate = request.PreferredDate,
                Status = "Pending",
                PriceList = new List<PriceItem>()
            };

            _bookings[id] = booking;
            return booking;
        }

        public bool IsDateTaken(DateTime date)
        {
            return _bookings.Values.Any(b => b.PreferredDate.Date == date.Date);
        }

        public Booking? GetById(int id)
        {
            _bookings.TryGetValue(id, out var booking);
            return booking;
        }

        // ===== ДОБАВЛЕНИЕ ОДНОГО МАТЕРИАЛА / РАБОТЫ =====
        public Booking? AddPriceItem(int bookingId, PriceItem item)
        {
            if (!_bookings.TryGetValue(bookingId, out var booking))
                return null;

            booking.PriceList.Add(item);
            return booking;
        }

        // ===== ОБНОВЛЕНИЕ ЗАЯВКИ ЦЕЛИКОМ (PUT) =====
        public Booking? Update(int id, BookingRequest request)
        {
            if (!_bookings.TryGetValue(id, out var booking))
                return null;

            booking.ClientName = request.ClientName;
            booking.ServiceType = request.ServiceType;
            booking.PreferredDate = request.PreferredDate;

            if (!string.IsNullOrWhiteSpace(request.Status) &&
                AllowedStatuses.Contains(request.Status))
            {
                booking.Status = request.Status;
            }

            if (request.PriceList != null)
            {
                booking.PriceList = request.PriceList;
            }

            return booking;
        }

        public bool Remove(int id)
        {
            return _bookings.TryRemove(id, out _);
        }

        public bool RemovePriceItem(int bookingId, int itemIndex)
        {
            if (!_bookings.TryGetValue(bookingId, out var booking))
                return false;

            if (itemIndex < 0 || itemIndex >= booking.PriceList.Count)
                return false;

            booking.PriceList.RemoveAt(itemIndex);
            return true;
        }

    }
}
