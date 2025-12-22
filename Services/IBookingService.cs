using YachtCareAPI.Models;

namespace YachtCareAPI.Services
{
    public interface IBookingService
    {
        IEnumerable<Booking> GetAll();
        Booking Add(BookingRequest request);
        bool IsDateTaken(DateTime date);

        // Добавляем новые методы
        Booking? GetById(int id);
        Booking? Update(int id, BookingRequest request);
        Booking? AddPriceItem(int bookingId, PriceItem item);
        bool RemovePriceItem(int bookingId, int itemIndex);
        bool Remove(int id);
    }
}
