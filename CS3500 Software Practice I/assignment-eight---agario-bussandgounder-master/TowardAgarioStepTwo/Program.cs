// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using TowardAgarioStepTwo;

Console.WriteLine("Hello, World!");

Student s = new Student("Jim", 4.0f);
//s.Name = "Jim";
//s.GPA = 4.0f;
//s.ID = 1234;

var options = new JsonSerializerOptions {IncludeFields = true};
string message = JsonSerializer.Serialize(s, options); 

Console.WriteLine($"For 1st student:{message}");

Student? b = JsonSerializer.Deserialize<Student>(message, options);

Console.WriteLine("Done");