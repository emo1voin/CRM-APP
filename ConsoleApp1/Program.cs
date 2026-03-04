using ConsoleApp1.Models;
using ConsoleApp1.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoworkingCRM
{
    class Program
    {
        private static ClientService clientService = new ClientService();
        private static DeskService deskService = new DeskService();
        private static BookingService bookingService = new BookingService(clientService, deskService);

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "CRM Коворкинг Центр";

            while (true)
            {
                Console.Clear();
                DrawHeader();
                Console.WriteLine("\n┌─────────────────────────────────────┐");
                Console.WriteLine("│         ГЛАВНОЕ МЕНЮ                │");
                Console.WriteLine("├─────────────────────────────────────┤");
                Console.WriteLine("│ 1. 👥 Управление клиентами          │");
                Console.WriteLine("│ 2. 🪑 Управление столами             │");
                Console.WriteLine("│ 3. 📅 Управление бронированиями      │");
                Console.WriteLine("│ 4. 💰 Отчеты и статистика           │");
                Console.WriteLine("│ 0. ❌ Выход                          │");
                Console.WriteLine("└─────────────────────────────────────┘");
                Console.Write("\nВыберите пункт: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageClients();
                        break;
                    case "2":
                        ManageDesks();
                        break;
                    case "3":
                        ManageBookings();
                        break;
                    case "4":
                        ShowReports();
                        break;
                    case "0":
                        Console.WriteLine("\n👋 До свидания!");
                        return;
                    default:
                        Console.WriteLine("\n❌ Неверный выбор. Нажмите любую клавишу...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void DrawHeader()
        {
            Console.WriteLine(@"
╔══════════════════════════════════════════════════════════╗
║                КОВОРКИНГ CRM СИСТЕМА                     ║
║                   Версия 1.0 (MVP)                       ║
╚══════════════════════════════════════════════════════════╝");
        }

        static void ManageClients()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n┌─────────────────────────────────────┐");
                Console.WriteLine("│         УПРАВЛЕНИЕ КЛИЕНТАМИ        │");
                Console.WriteLine("├─────────────────────────────────────┤");
                Console.WriteLine("│ 1. 📋 Просмотр всех клиентов        │");
                Console.WriteLine("│ 2. 🔍 Поиск клиента                 │");
                Console.WriteLine("│ 3. ➕ Добавить клиента               │");
                Console.WriteLine("│ 4. ✏️ Редактировать клиента          │");
                Console.WriteLine("│ 5. 🗑️ Удалить клиента                │");
                Console.WriteLine("│ 0. 🔙 Назад                          │");
                Console.WriteLine("└─────────────────────────────────────┘");
                Console.Write("\nВыберите пункт: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllClients();
                        break;
                    case "2":
                        SearchClients();
                        break;
                    case "3":
                        AddClient();
                        break;
                    case "4":
                        EditClient();
                        break;
                    case "5":
                        DeleteClient();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("\n❌ Неверный выбор. Нажмите любую клавишу...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void ShowAllClients()
        {
            Console.Clear();
            Console.WriteLine("\n=== СПИСОК КЛИЕНТОВ ===\n");

            var clients = clientService.GetAllClients();
            if (clients.Count == 0)
            {
                Console.WriteLine("Клиентов нет");
            }
            else
            {
                foreach (var client in clients)
                {
                    Console.WriteLine(client);
                }
            }

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void SearchClients()
        {
            Console.Clear();
            Console.WriteLine("\n=== ПОИСК КЛИЕНТА ===\n");
            Console.Write("Введите имя, телефон или email: ");
            var searchTerm = Console.ReadLine();

            var results = clientService.SearchClients(searchTerm);

            Console.WriteLine($"\nНайдено клиентов: {results.Count}\n");
            foreach (var client in results)
            {
                Console.WriteLine(client);
            }

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void AddClient()
        {
            Console.Clear();
            Console.WriteLine("\n=== ДОБАВЛЕНИЕ НОВОГО КЛИЕНТА ===\n");

            var client = new Client();

            Console.Write("Введите ФИО: ");
            client.Name = Console.ReadLine();

            Console.Write("Введите телефон: ");
            client.Phone = Console.ReadLine();

            Console.Write("Введите email: ");
            client.Email = Console.ReadLine();

            clientService.AddClient(client);

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void EditClient()
        {
            Console.Clear();
            Console.WriteLine("\n=== РЕДАКТИРОВАНИЕ КЛИЕНТА ===\n");

            Console.Write("Введите ID клиента: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("❌ Неверный ID");
                Console.ReadKey();
                return;
            }

            var client = clientService.GetClientById(id);
            if (client == null)
            {
                Console.WriteLine("❌ Клиент не найден");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nТекущие данные: {client}");
            Console.WriteLine("\nВведите новые данные (или оставьте пустым для сохранения текущего значения):");

            Console.Write($"ФИО [{client.Name}]: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) name = client.Name;

            Console.Write($"Телефон [{client.Phone}]: ");
            string phone = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(phone)) phone = client.Phone;

            Console.Write($"Email [{client.Email}]: ");
            string email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(email)) email = client.Email;

            if (clientService.UpdateClient(id, name, phone, email))
            {
                Console.WriteLine("\n✓ Данные клиента обновлены");
            }
            else
            {
                Console.WriteLine("\n❌ Ошибка при обновлении");
            }

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void DeleteClient()
        {
            Console.Clear();
            Console.WriteLine("\n=== УДАЛЕНИЕ КЛИЕНТА ===\n");

            Console.Write("Введите ID клиента: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("❌ Неверный ID");
                Console.ReadKey();
                return;
            }

            var client = clientService.GetClientById(id);
            if (client == null)
            {
                Console.WriteLine("❌ Клиент не найден");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nКлиент для удаления: {client}");
            Console.Write("\nВы уверены? (д/н): ");
            var confirm = Console.ReadLine();

            if (confirm.ToLower() == "д" || confirm.ToLower() == "yes" || confirm.ToLower() == "y")
            {
                if (clientService.DeleteClient(id))
                {
                    Console.WriteLine("\n✓ Клиент удален");
                }
                else
                {
                    Console.WriteLine("\n❌ Ошибка при удалении");
                }
            }

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void ManageDesks()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n┌─────────────────────────────────────┐");
                Console.WriteLine("│         УПРАВЛЕНИЕ СТОЛАМИ          │");
                Console.WriteLine("├─────────────────────────────────────┤");
                Console.WriteLine("│ 1. 📋 Просмотр всех столов          │");
                Console.WriteLine("│ 2. ✅ Доступные столы               │");
                Console.WriteLine("│ 3. ➕ Добавить стол                  │");
                Console.WriteLine("│ 4. ✏️ Редактировать стол             │");
                Console.WriteLine("│ 5. 🔄 Изменить статус стола         │");
                Console.WriteLine("│ 0. 🔙 Назад                          │");
                Console.WriteLine("└─────────────────────────────────────┘");
                Console.Write("\nВыберите пункт: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllDesks();
                        break;
                    case "2":
                        ShowAvailableDesks();
                        break;
                    case "3":
                        AddDesk();
                        break;
                    case "4":
                        EditDesk();
                        break;
                    case "5":
                        ToggleDeskStatus();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("\n❌ Неверный выбор. Нажмите любую клавишу...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void ShowAllDesks()
        {
            Console.Clear();
            Console.WriteLine("\n=== ВСЕ СТОЛЫ ===\n");

            var desks = deskService.GetAllDesks();
            foreach (var desk in desks)
            {
                Console.WriteLine(desk);
            }

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void ShowAvailableDesks()
        {
            Console.Clear();
            Console.WriteLine("\n=== ДОСТУПНЫЕ СТОЛЫ ===\n");

            var desks = deskService.GetAvailableDesks();
            foreach (var desk in desks)
            {
                Console.WriteLine(desk);
            }

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void AddDesk()
        {
            Console.Clear();
            Console.WriteLine("\n=== ДОБАВЛЕНИЕ НОВОГО СТОЛА ===\n");

            var desk = new Desk();

            Console.Write("Введите номер стола: ");
            desk.Number = int.Parse(Console.ReadLine());

            Console.Write("Введите тип (Обычное/VIP/Переговорка): ");
            desk.Type = Console.ReadLine();

            Console.Write("Введите цену за час (руб): ");
            desk.PricePerHour = decimal.Parse(Console.ReadLine());

            desk.IsActive = true;

            deskService.AddDesk(desk);

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void EditDesk()
        {
            Console.Clear();
            Console.WriteLine("\n=== РЕДАКТИРОВАНИЕ СТОЛА ===\n");

            Console.Write("Введите ID стола: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("❌ Неверный ID");
                Console.ReadKey();
                return;
            }

            var desk = deskService.GetDeskById(id);
            if (desk == null)
            {
                Console.WriteLine("❌ Стол не найден");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nТекущие данные: {desk}");
            Console.WriteLine("\nВведите новые данные (или оставьте пустым для сохранения текущего значения):");

            Console.Write($"Номер стола [{desk.Number}]: ");
            string numberStr = Console.ReadLine();
            int number = desk.Number;
            if (!string.IsNullOrWhiteSpace(numberStr)) int.TryParse(numberStr, out number);

            Console.Write($"Тип [{desk.Type}]: ");
            string type = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(type)) type = desk.Type;

            Console.Write($"Цена за час [{desk.PricePerHour}]: ");
            string priceStr = Console.ReadLine();
            decimal price = desk.PricePerHour;
            if (!string.IsNullOrWhiteSpace(priceStr)) decimal.TryParse(priceStr, out price);

            Console.Write($"Доступен (true/false) [{desk.IsActive}]: ");
            string activeStr = Console.ReadLine();
            bool isActive = desk.IsActive;
            if (!string.IsNullOrWhiteSpace(activeStr)) bool.TryParse(activeStr, out isActive);

            if (deskService.UpdateDesk(id, number, type, price, isActive))
            {
                Console.WriteLine("\n✓ Данные стола обновлены");
            }
            else
            {
                Console.WriteLine("\n❌ Ошибка при обновлении");
            }

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void ToggleDeskStatus()
        {
            Console.Clear();
            Console.WriteLine("\n=== ИЗМЕНЕНИЕ СТАТУСА СТОЛА ===\n");

            Console.Write("Введите ID стола: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("❌ Неверный ID");
                Console.ReadKey();
                return;
            }

            var desk = deskService.GetDeskById(id);
            if (desk == null)
            {
                Console.WriteLine("❌ Стол не найден");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nТекущий статус: {(desk.IsActive ? "Доступен" : "Недоступен")}");

            if (deskService.ToggleDeskStatus(id))
            {
                Console.WriteLine($"\n✓ Статус изменен на: {(desk.IsActive ? "Доступен" : "Недоступен")}");
            }
            else
            {
                Console.WriteLine("\n❌ Ошибка при изменении статуса");
            }

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void ManageBookings()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n┌─────────────────────────────────────┐");
                Console.WriteLine("│      УПРАВЛЕНИЕ БРОНИРОВАНИЯМИ      │");
                Console.WriteLine("├─────────────────────────────────────┤");
                Console.WriteLine("│ 1. 📋 Все бронирования              │");
                Console.WriteLine("│ 2. 📅 Бронирования на дату          │");
                Console.WriteLine("│ 3. 👤 Бронирования клиента          │");
                Console.WriteLine("│ 4. ➕ Создать бронь                  │");
                Console.WriteLine("│ 5. ❌ Отменить бронь                 │");
                Console.WriteLine("│ 6. 💳 Отметить оплату                │");
                Console.WriteLine("│ 7. ℹ️ Детали брони                    │");
                Console.WriteLine("│ 0. 🔙 Назад                          │");
                Console.WriteLine("└─────────────────────────────────────┘");
                Console.Write("\nВыберите пункт: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllBookings();
                        break;
                    case "2":
                        ShowBookingsByDate();
                        break;
                    case "3":
                        ShowBookingsByClient();
                        break;
                    case "4":
                        CreateBooking();
                        break;
                    case "5":
                        CancelBooking();
                        break;
                    case "6":
                        MarkBookingAsPaid();
                        break;
                    case "7":
                        ShowBookingDetails();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("\n❌ Неверный выбор. Нажмите любую клавишу...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void ShowAllBookings()
        {
            Console.Clear();
            Console.WriteLine("\n=== ВСЕ БРОНИРОВАНИЯ ===\n");

            var bookings = bookingService.GetAllBookings();
            if (bookings.Count == 0)
            {
                Console.WriteLine("Бронирований нет");
            }
            else
            {
                foreach (var booking in bookings)
                {
                    Console.WriteLine(booking);
                }
            }

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void ShowBookingsByDate()
        {
            Console.Clear();
            Console.WriteLine("\n=== БРОНИРОВАНИЯ НА ДАТУ ===\n");

            Console.Write("Введите дату (ДД.ММ.ГГГГ): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
            {
                Console.WriteLine("❌ Неверный формат даты");
                Console.ReadKey();
                return;
            }

            var bookings = bookingService.GetBookingsByDate(date);
            Console.WriteLine($"\nБронирований на {date:dd.MM.yyyy}: {bookings.Count}\n");

            foreach (var booking in bookings)
            {
                Console.WriteLine(booking);
            }

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void ShowBookingsByClient()
        {
            Console.Clear();
            Console.WriteLine("\n=== БРОНИРОВАНИЯ КЛИЕНТА ===\n");

            Console.Write("Введите ID клиента: ");
            if (!int.TryParse(Console.ReadLine(), out int clientId))
            {
                Console.WriteLine("❌ Неверный ID");
                Console.ReadKey();
                return;
            }

            var bookings = bookingService.GetBookingsByClient(clientId);
            var client = clientService.GetClientById(clientId);

            Console.WriteLine($"\nБронирований клиента {client?.Name ?? "Неизвестно"}: {bookings.Count}\n");

            foreach (var booking in bookings)
            {
                Console.WriteLine(booking);
            }

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void CreateBooking()
        {
            Console.Clear();
            Console.WriteLine("\n=== СОЗДАНИЕ НОВОЙ БРОНИ ===\n");

            // Показываем доступные столы
            Console.WriteLine("Доступные столы:");
            foreach (var desk in deskService.GetAvailableDesks())
            {
                Console.WriteLine($"  {desk}");
            }

            Console.WriteLine("\n--- Данные бронирования ---");

            Console.Write("ID клиента: ");
            if (!int.TryParse(Console.ReadLine(), out int clientId))
            {
                Console.WriteLine("❌ Неверный ID клиента");
                Console.ReadKey();
                return;
            }

            Console.Write("ID стола: ");
            if (!int.TryParse(Console.ReadLine(), out int deskId))
            {
                Console.WriteLine("❌ Неверный ID стола");
                Console.ReadKey();
                return;
            }

            Console.Write("Дата (ДД.ММ.ГГГГ): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
            {
                Console.WriteLine("❌ Неверный формат даты");
                Console.ReadKey();
                return;
            }

            Console.Write("Время начала (ЧЧ:ММ): ");
            if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan startTime))
            {
                Console.WriteLine("❌ Неверный формат времени");
                Console.ReadKey();
                return;
            }

            Console.Write("Время окончания (ЧЧ:ММ): ");
            if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan endTime))
            {
                Console.WriteLine("❌ Неверный формат времени");
                Console.ReadKey();
                return;
            }

            if (endTime <= startTime)
            {
                Console.WriteLine("❌ Время окончания должно быть позже времени начала");
                Console.ReadKey();
                return;
            }

            var result = bookingService.CreateBooking(clientId, deskId, date, startTime, endTime);
            Console.WriteLine($"\n{result}");

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void CancelBooking()
        {
            Console.Clear();
            Console.WriteLine("\n=== ОТМЕНА БРОНИ ===\n");

            Console.Write("Введите ID брони: ");
            if (!int.TryParse(Console.ReadLine(), out int bookingId))
            {
                Console.WriteLine("❌ Неверный ID");
                Console.ReadKey();
                return;
            }

            var result = bookingService.CancelBooking(bookingId);
            Console.WriteLine($"\n{result}");

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void MarkBookingAsPaid()
        {
            Console.Clear();
            Console.WriteLine("\n=== ОТМЕТКА ОБ ОПЛАТЕ ===\n");

            Console.Write("Введите ID брони: ");
            if (!int.TryParse(Console.ReadLine(), out int bookingId))
            {
                Console.WriteLine("❌ Неверный ID");
                Console.ReadKey();
                return;
            }

            var result = bookingService.MarkAsPaid(bookingId);
            Console.WriteLine($"\n{result}");

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void ShowBookingDetails()
        {
            Console.Clear();
            Console.WriteLine("\n=== ДЕТАЛИ БРОНИ ===\n");

            Console.Write("Введите ID брони: ");
            if (!int.TryParse(Console.ReadLine(), out int bookingId))
            {
                Console.WriteLine("❌ Неверный ID");
                Console.ReadKey();
                return;
            }

            var details = bookingService.GetBookingDetails(bookingId);
            Console.WriteLine(details);

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void ShowReports()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n┌─────────────────────────────────────┐");
                Console.WriteLine("│         ОТЧЕТЫ И СТАТИСТИКА         │");
                Console.WriteLine("├─────────────────────────────────────┤");
                Console.WriteLine("│ 1. 💰 Выручка за сегодня            │");
                Console.WriteLine("│ 2. 📊 Выручка за дату               │");
                Console.WriteLine("│ 0. 🔙 Назад                          │");
                Console.WriteLine("└─────────────────────────────────────┘");
                Console.Write("\nВыберите пункт: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowTodayRevenue();
                        break;
                    case "2":
                        ShowRevenueByDate();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("\n❌ Неверный выбор. Нажмите любую клавишу...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void ShowTodayRevenue()
        {
            Console.Clear();
            var revenue = bookingService.GetDailyRevenue(DateTime.Today);
            Console.WriteLine($"\n💰 Выручка за сегодня ({DateTime.Today:dd.MM.yyyy}): {revenue} руб");
            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void ShowRevenueByDate()
        {
            Console.Clear();
            Console.WriteLine("\n=== ВЫРУЧКА ЗА ДАТУ ===\n");

            Console.Write("Введите дату (ДД.ММ.ГГГГ): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
            {
                Console.WriteLine("❌ Неверный формат даты");
                Console.ReadKey();
                return;
            }

            var revenue = bookingService.GetDailyRevenue(date);
            Console.WriteLine($"\n💰 Выручка за {date:dd.MM.yyyy}: {revenue} руб");

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}