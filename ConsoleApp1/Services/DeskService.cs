using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Services
{
    public class DeskService
    {
        private List<Desk> desks = new List<Desk>();
        private int nextId = 1;

        public DeskService()
        {
            desks.Add(new Desk { Id = nextId++, Number = 1, Type = "Обычное место", PricePerHour = 200, IsActive = true });
            desks.Add(new Desk { Id = nextId++, Number = 2, Type = "Обычное место", PricePerHour = 200, IsActive = true });
            desks.Add(new Desk { Id = nextId++, Number = 3, Type = "VIP место", PricePerHour = 500, IsActive = true });
            desks.Add(new Desk { Id = nextId++, Number = 4, Type = "Переговорка", PricePerHour = 1000, IsActive = true });
            desks.Add(new Desk { Id = nextId++, Number = 5, Type = "Обычное место", PricePerHour = 200, IsActive = false }); // Недоступен
        }

        public List<Desk> GetAllDesks()
        {
            return desks;
        }

        public List<Desk> GetAvailableDesks()
        {
            return desks.Where(d => d.IsActive).ToList();
        }

        public Desk GetDeskById(int id)
        {
            return desks.FirstOrDefault(d => d.Id == id);
        }

        public void AddDesk(Desk desk)
        {
            desk.Id = nextId++;
            desks.Add(desk);
            Console.WriteLine($"\n✓ Стол №{desk.Number} успешно добавлен с ID {desk.Id}");
        }

        public bool UpdateDesk(int id, int number, string type, decimal price, bool isActive)
        {
            var desk = GetDeskById(id);
            if (desk == null)
                return false;

            desk.Number = number;
            desk.Type = type;
            desk.PricePerHour = price;
            desk.IsActive = isActive;
            return true;
        }

        public bool ToggleDeskStatus(int id)
        {
            var desk = GetDeskById(id);
            if (desk == null)
                return false;

            desk.IsActive = !desk.IsActive;
            return true;
        }
    }
}
