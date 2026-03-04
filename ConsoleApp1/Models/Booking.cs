using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int DeskId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsPaid { get; set; }
        public decimal TotalPrice { get; set; }

        public override string ToString()
        {
            string status = IsPaid ? "ОПЛАЧЕНО" : "НЕ ОПЛАЧЕНО";
            return $"ID: {Id} | Клиент: {ClientId} | Стол: {DeskId} | {Date:dd.MM.yyyy} | {StartTime:hh\\:mm}-{EndTime:hh\\:mm} | {status} | {TotalPrice} руб";
        }
    }
}
