﻿using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    public class TaskService
    {
        public List<ToDoTask> Tasks { get; set; }
        public string Path { get; set; }

        public TaskService()
        {
            Tasks = new List<ToDoTask>();
            Path = @"CSV\";
        }

        public void Add(string[] array)
        {
            if (array.Length == 2 && !string.IsNullOrWhiteSpace(array[1]))
            {
                var task = new ToDoTask
                {
                    Name = array[1],
                    DateAdded = DateTime.Now,
                    Status = "New",
                    Id = Tasks.Count + 1
                };

                Tasks.Add(task);
                Console.WriteLine("Task with id {0} added successfully.", task.Id);
            }

            else
            {
                Console.WriteLine("Task name cannot be empty or contain spaces.");
            }
        }

        public void Show()
        {
            if (Tasks.Count == 0)
            {
                Console.WriteLine("List is currently empty.");
            }

            else
            {
                foreach (var i in Tasks)
                {
                    if (i.Status == "Completed")
                    {
                        Console.WriteLine("Id: {0}, Name: {1} Added: {2}, Status: {3}, Finished: {4}", i.Id, i.Name, i.DateAdded, i.Status, i.DateFinished);
                    }

                    else
                    {
                        Console.WriteLine("Id: {0}, Name: {1}, Added: {2}, Status: {3}", i.Id, i.Name, i.DateAdded, i.Status);
                    }
                }
            }
        }

        public void Start(string[] array)
        {
            var isNumeric = int.TryParse(array[1], out var id);

            if (isNumeric && id > 0 && id <= Tasks.Count)
            {
                var task = Tasks.First(t => t.Id == id);

                if (task.Status == "In progress")
                {
                    Console.WriteLine("Task is already in progress.");
                }

                else
                {
                    task.Status = "In progress";
                    Console.WriteLine("Task with id {0} in progress.", task.Id);
                }
            }

            else
            {
                Console.WriteLine("Invalid Id.");
            }
        }

        public void Complete(string[] array)
        {
            var isNumeric = int.TryParse(array[1], out var id);

            if (isNumeric && id > 0 && id <= Tasks.Count)
            {
                var task = Tasks.First(t => t.Id == id);

                if (task.Status == "Completed")
                {
                    Console.WriteLine("Task has already been completed.");
                }

                else
                {
                    task.Status = "Completed";
                    task.DateFinished = DateTime.Now;
                    Console.WriteLine("Task with id {0} completed.", task.Id);
                }
            }

            else
            {
                Console.WriteLine("Invalid Id.");
            }
        }

        public void Export(string[] array)
        {
            if (Tasks.Count == 0)
            {
                Console.WriteLine("List is currently empty.");
            }

            else if (array.Length > 2)
            {
                Console.WriteLine("File name cannot contain spaces.");
            }

            else
            {
                var fileName = array[1];

                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }

                using (var writer = new StreamWriter(Path + fileName + ".csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(Tasks);
                }

                Console.WriteLine("File saved.");
            }
        }

        public void Import(string[] array)
        {
            var fileName = array[1];
            var fullPath = Path + fileName + ".csv";

            if (!File.Exists(fullPath))
            {
                Console.WriteLine("File named '{0}' does not exist.", fileName);
            }

            else
            {
                using (var reader = new StreamReader(fullPath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<ToDoTask>();
                    Tasks = records.ToList();
                }

                Console.WriteLine("File imported.");
            }
        }

        public void Invalid()
        {
            Console.WriteLine("Invalid command.");
        }
    }
}
