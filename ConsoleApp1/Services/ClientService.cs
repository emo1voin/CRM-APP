using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Services
{
    public class ClientService
    {
        private List<Client> clients = new List<Client>();
        private int nextId = 1;

        public ClientService()
        {
            clients.Add(new Client { Id = nextId++, Name = "Иван Петров", Phone = "+7(999)123-45-67", Email = "ivan@mail.ru" });
            clients.Add(new Client { Id = nextId++, Name = "Мария Сидорова", Phone = "+7(999)765-43-21", Email = "maria@mail.ru" });
        }

        public List<Client> GetAllClients()
        {
            return clients;
        }

        public Client GetClientById(int id)
        {
            return clients.FirstOrDefault(c => c.Id == id);
        }

        public List<Client> SearchClients(string searchTerm)
        {
            return clients.Where(c =>
                c.Name.ToLower().Contains(searchTerm.ToLower()) ||
                c.Phone.Contains(searchTerm) ||
                c.Email.ToLower().Contains(searchTerm.ToLower())
            ).ToList();
        }

        public void AddClient(Client client)
        {
            client.Id = nextId++;
            clients.Add(client);
            Console.WriteLine($"\n✓ Клиент {client.Name} успешно добавлен с ID {client.Id}");
        }

        public bool UpdateClient(int id, string name, string phone, string email)
        {
            var client = GetClientById(id);
            if (client == null)
                return false;

            client.Name = name;
            client.Phone = phone;
            client.Email = email;
            return true;
        }

        public bool DeleteClient(int id)
        {
            var client = GetClientById(id);
            if (client == null)
                return false;

            return clients.Remove(client);
        }
    }
}
