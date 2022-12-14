using System;
using System.Collections.Generic;
using System.Linq;
using Roommates.Models;
using Roommates.Repositories;


namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true;TrustServerCertificate=true;";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Update a room"):
                        List<Room> roomOptions = roomRepo.GetAll();
                        foreach (Room r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to update? ");
                        int selectedRoomId = int.Parse(Console.ReadLine());
                        Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

                        Console.Write("New Name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.Write("New Max Occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

                        roomRepo.Update(selectedRoom);

                        Console.WriteLine("Room has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Delete a room"):
                        List<Room> roomsToDelete = roomRepo.GetAll();
                        foreach (Room r in roomsToDelete)
                        {
                            Console.WriteLine($"{r.Id}. {r.Name} Max occupancy {r.MaxOccupancy}");
                        }
                        Console.WriteLine();
                        Console.Write("Select a room to delete > ");
                        int roomSelection = int.Parse(Console.ReadLine());
                        roomRepo.Delete(roomSelection);
                        Console.WriteLine("Room deleted!");
                        Console.ReadKey();
                        break;
                    case ("Show all chores"):
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"{c.Name}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show unassigned chores"):
                        List<Chore> unassignedChores = choreRepo.GetUnassignedChores();
                        foreach (Chore c in unassignedChores)
                        {
                            Console.WriteLine($"{c.Name}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Assign chore to roommate"):
                        List<Chore> choresToAssign = choreRepo.GetAll();
                        foreach (Chore c in choresToAssign)
                        {
                            Console.WriteLine($"{c.Id}. {c.Name}");
                        }
                        Console.WriteLine();
                        Console.Write("Select a chore >");
                        int choreId = int.Parse(Console.ReadLine());
                        List<Roommate> roommatesToAssign = roommateRepo.GetAll();
                        foreach (Roommate r in roommatesToAssign)
                        {
                            Console.WriteLine($"{r.Id}. {r.FirstName} {r.LastName}");
                        }
                        Console.WriteLine();
                        Console.Write("Select a roommate >");
                        int roomieId = int.Parse(Console.ReadLine());
                        bool success = choreRepo.AssignChore(roomieId, choreId);
                        Console.WriteLine(success ? "Chore assigned!" : "Somethig went wrong");
                        Console.ReadKey();
                        break;
                    case ("Add a chore"):
                        Chore newChore = new Chore();
                        Console.WriteLine();
                        Console.Write("Enter chore name > ");
                        newChore.Name = Console.ReadLine();
                        choreRepo.Insert(newChore);
                        break;
                    case ("Update a chore"):
                        List<Chore> choresToUpdate = choreRepo.GetAll();
                        foreach (Chore c in choresToUpdate)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }

                        Console.Write("Which chore would you like to update? ");
                        int selectedChoreId = int.Parse(Console.ReadLine());
                        Chore selectedChore = choresToUpdate.FirstOrDefault(c => c.Id == selectedChoreId);

                        Console.Write("New Name: ");
                        selectedChore.Name = Console.ReadLine();

                        choreRepo.Update(selectedChore);

                        Console.WriteLine("Chore has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Delete a chore"):
                        List<Chore> choresToDelete = choreRepo.GetAll();
                        foreach (Chore c in choresToDelete)
                        {
                            Console.WriteLine($"{c.Id}. {c.Name}");
                        }
                        Console.WriteLine();
                        Console.Write("Select a chore to delete > ");
                        int choreSelection = int.Parse(Console.ReadLine());
                        choreRepo.Delete(choreSelection);
                        Console.WriteLine("Chore deleted!");
                        Console.ReadKey();
                        break;
                    case ("Search for roommate"):
                        Console.Write("Roommate Id: ");
                        int roommateId = int.Parse(Console.ReadLine());

                        Roommate roommate = roommateRepo.GetById(roommateId);

                        Console.WriteLine($"{roommate.FirstName} stays in {roommate.Room.Name} with a rent portion of {roommate.RentPortion}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Add a room",
                "Update a room",
                "Delete a room",
                "Show all chores",
                "Show unassigned chores",
                "Assign chore to roommate",
                "Add a chore",
                "Update a chore",
                "Delete a chore",
                "Search for roommate",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}