using System;
using System.Collections.Generic;
using System.IO;

namespace HotelManagementSystem
{
    class Room
    {
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public double Price { get; set; }
        public bool IsAllocated { get; set; } = false;
        public string GuestName { get; set; } = null;

        public Room(string number, string type, double price)
        {
            RoomNumber = number;
            RoomType = type;
            Price = price;
        }

        public string GetRoomData()
        {
            return $"Room {RoomNumber} | Type: {RoomType} | Price: {Price} | Guest: {GuestName} | Allocated: {IsAllocated}";
        }
    }

    class HotelManagement
    {
        private Dictionary<string, Room> rooms = new Dictionary<string, Room>();
        private string studentId = "12345";
        private string filePath => $"lhms_{studentId}.txt";
        private string backupPath => $"lhms_{studentId}_backup.txt";

        public void AddRoom()
        {
            try
            {
                Console.Write("Enter Room Number: ");
                string number = Console.ReadLine();

                if (rooms.ContainsKey(number))
                {
                    Console.WriteLine("Room already exists.");
                    return;
                }

                Console.Write("Enter Room Type: ");
                string type = Console.ReadLine();

                Console.Write("Enter Room Price: ");
                double price = Convert.ToDouble(Console.ReadLine()); // May cause FormatException

                rooms[number] = new Room(number, type, price);
                Console.WriteLine("Room added successfully.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input! Please enter a valid number for the price.");
            }
            finally
            {
                Console.WriteLine("Price input attempt finished.\n");
            }
        }

        public void DisplayRooms()
        {
            if (rooms.Count == 0)
            {
                Console.WriteLine("No rooms available.");
                return;
            }

            foreach (var room in rooms.Values)
            {
                string status = room.IsAllocated ? $"Allocated to {room.GuestName}" : "Available";
                Console.WriteLine($"Room {room.RoomNumber} - {room.RoomType} - ${room.Price} - {status}");
            }
        }

        public void AllocateRoom()
        {
            try
            {
                Console.Write("Enter Room Number to Allocate: ");
                string number = Console.ReadLine();

                if (!rooms.ContainsKey(number))
                    throw new InvalidOperationException("Room number does not exist.");

                if (rooms[number].IsAllocated)
                {
                    Console.WriteLine("Room is already allocated.");
                    return;
                }

                Console.Write("Enter Guest Name: ");
                string guest = Console.ReadLine();

                rooms[number].IsAllocated = true;
                rooms[number].GuestName = guest;

                Console.WriteLine($"Room {number} allocated to {guest}.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Room allocation attempt complete.\n");
            }
        }

        public void DeallocateRoom()
        {
            Console.Write("Enter Room Number to Deallocate: ");
            string number = Console.ReadLine();

            if (!rooms.ContainsKey(number))
            {
                Console.WriteLine("Room not found.");
                return;
            }

            if (!rooms[number].IsAllocated)
            {
                Console.WriteLine("Room is already free.");
                return;
            }

            string guest = rooms[number].GuestName;
            rooms[number].IsAllocated = false;
            rooms[number].GuestName = null;

            Console.WriteLine($"Room {number} de-allocated from {guest}.");
        }

        public void DisplayAllocations()
        {
            bool allocated = false;
            foreach (var room in rooms.Values)
            {
                if (room.IsAllocated)
                {
                    Console.WriteLine(room.GetRoomData());
                    allocated = true;
                }
            }

            if (!allocated)
                Console.WriteLine("No room allocations yet.");
        }

        public void Billing()
        {
            Console.WriteLine("Billing Feature is Under Construction and will be added soon…!!!");
        }

        public void SaveToFile()
        {
            try
            {
                if (!File.Exists(filePath))
                    File.Create(filePath).Close();

                // Simulate UnauthorizedAccessException
                // File.SetAttributes(filePath, FileAttributes.ReadOnly); // Uncomment to simulate

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"--- Room Allocation Snapshot @ {DateTime.Now} ---");
                    foreach (var room in rooms.Values)
                    {
                        if (room.IsAllocated)
                            writer.WriteLine(room.GetRoomData());
                    }
                }

                Console.WriteLine("Room allocation saved to file.");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Access Denied: {ex.Message}");
            }
            finally
            {
                // Restore write access if needed
                if (File.Exists(filePath))
                    File.SetAttributes(filePath, FileAttributes.Normal);

                Console.WriteLine("File save attempt completed.\n");
            }
        }

        public void ShowFromFile()
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"File {filePath} not found.");

                string content = File.ReadAllText(filePath);
                Console.WriteLine(content);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File Error: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("File read operation completed.\n");
            }
        }

        public void BackupAndClearFile()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("No file to backup.");
                    return;
                }

                string content = File.ReadAllText(filePath);

                using (StreamWriter writer = new StreamWriter(backupPath, true))
                {
                    writer.WriteLine($"--- Backup @ {DateTime.Now} ---");
                    writer.WriteLine(content);
                }

                File.WriteAllText(filePath, "");
                Console.WriteLine("Backup completed. Original file cleared.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Backup Error: {ex.Message}");
            }
        }

        public void Run()
        {
            string choice;
            do
            {
                Console.WriteLine("\n--- Hotel Management System Menu ---");
                Console.WriteLine("1. Add Rooms");
                Console.WriteLine("2. Display Rooms");
                Console.WriteLine("3. Allocate Rooms");
                Console.WriteLine("4. De-Allocate Rooms");
                Console.WriteLine("5. Display Room Allocation Details");
                Console.WriteLine("6. Billing");
                Console.WriteLine("7. Save Room Allocation to File");
                Console.WriteLine("8. Show Room Allocation from File");
                Console.WriteLine("10. Backup and Clear File");
                Console.WriteLine("9. Exit");
                Console.Write("Enter your choice: ");
                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddRoom(); break;
                    case "2": DisplayRooms(); break;
                    case "3": AllocateRoom(); break;
                    case "4": DeallocateRoom(); break;
                    case "5": DisplayAllocations(); break;
                    case "6": Billing(); break;
                    case "7": SaveToFile(); break;
                    case "8": ShowFromFile(); break;
                    case "10": BackupAndClearFile(); break;
                    case "9": Console.WriteLine("Exiting system..."); break;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            } while (choice != "9");
        }
    }

    class Program
    {
        static void Main()
        {
            HotelManagement system = new HotelManagement();
            system.Run();
        }
    }
}