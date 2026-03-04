using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Models
{
    public class Desk
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Type { get; set; } // "Обычное место", "VIP", "Переговорка"
        public decimal PricePerHour { get; set; }
        public bool IsActive { get; set; }

        public override string ToString()
        {
            return $"ID: {Id} | Стол №{Number} | {Type} | {PricePerHour} руб/час | {(IsActive ? "Доступен" : "Недоступен")}";
        }
    }
}
