using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Services
{
    public class BookingService
    {
        private List<Booking> bookings = new List<Booking>();
        private int nextId = 1;
        private ClientService clientService;
        private DeskService deskService;

        public BookingService(ClientService clientService, DeskService deskService)
        {
            this.clientService = clientService;
            this.deskService = deskService;

            // Добавим тестовые бронирования
            var testBooking = new Booking
            {
                Id = nextId++,
                ClientId = 1,
                DeskId = 1,
                Date = DateTime.Today,
                StartTime = new TimeSpan(10, 0, 0),
                EndTime = new TimeSpan(12, 0, 0),
                IsPaid = true,
                TotalPrice = 400
            };
            bookings.Add(testBooking);
        }

        public List<Booking> GetAllBookings()
        {
            return bookings;
        }

        public List<Booking> GetBookingsByDate(DateTime date)
        {
            return bookings.Where(b => b.Date.Date == date.Date).ToList();
        }

        public List<Booking> GetBookingsByClient(int clientId)
        {
            return bookings.Where(b => b.ClientId == clientId).ToList();
        }

        public string CreateBooking(int clientId, int deskId, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            // Проверка существования клиента
            var client = clientService.GetClientById(clientId);
            if (client == null)
                return "Ошибка: Клиент не найден";

            // Проверка существования стола
            var desk = deskService.GetDeskById(deskId);
            if (desk == null)
                return "Ошибка: Стол не найден";

            if (!desk.IsActive)
                return "Ошибка: Стол недоступен для бронирования";

            // Проверка на пересечение бронирований
            bool isBooked = bookings.Any(b =>
                b.DeskId == deskId &&
                b.Date.Date == date.Date &&
                ((startTime >= b.StartTime && startTime < b.EndTime) ||
                 (endTime > b.StartTime && endTime <= b.EndTime) ||
                 (startTime <= b.StartTime && endTime >= b.EndTime)));

            if (isBooked)
                return "Ошибка: Стол уже забронирован на это время";

            // Расчет стоимости
            decimal hours = (decimal)(endTime - startTime).TotalHours;
            decimal totalPrice = desk.PricePerHour * hours;

            var booking = new Booking
            {
                Id = nextId++,
                ClientId = clientId,
                DeskId = deskId,
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                IsPaid = false,
                TotalPrice = totalPrice
            };

            bookings.Add(booking);
            return $"✓ Бронь №{booking.Id} успешно создана! Сумма: {totalPrice} руб";
        }

        public string CancelBooking(int bookingId)
        {
            var booking = bookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking == null)
                return "Ошибка: Бронь не найдена";

            bookings.Remove(booking);
            return $"✓ Бронь №{bookingId} отменена";
        }

        public string MarkAsPaid(int bookingId)
        {
            var booking = bookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking == null)
                return "Ошибка: Бронь не найдена";

            if (booking.IsPaid)
                return "Бронь уже оплачена";

            booking.IsPaid = true;
            return $"✓ Бронь №{bookingId} отмечена как оплаченная";
        }

        public decimal GetDailyRevenue(DateTime date)
        {
            return bookings.Where(b => b.Date.Date == date.Date && b.IsPaid)
                           .Sum(b => b.TotalPrice);
        }

        public string GetBookingDetails(int bookingId)
        {
            var booking = bookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking == null)
                return "Бронь не найдена";

            var client = clientService.GetClientById(booking.ClientId);
            var desk = deskService.GetDeskById(booking.DeskId);

            return $@"
=== Детали брони №{booking.Id} ===
Клиент: {client?.Name ?? "Неизвестно"} (ID: {booking.ClientId})
Стол: №{desk?.Number ?? 0} ({desk?.Type ?? "Неизвестно"})
Дата: {booking.Date:dd.MM.yyyy}
Время: {booking.StartTime:hh\\:mm} - {booking.EndTime:hh\\:mm}
Стоимость: {booking.TotalPrice} руб
Статус оплаты: {(booking.IsPaid ? "ОПЛАЧЕНО" : "НЕ ОПЛАЧЕНО")}
";
        }
    }
}
