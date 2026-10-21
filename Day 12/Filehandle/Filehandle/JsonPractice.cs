
using System;
using System.Text.Json;

namespace Filehandle
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }

        

        public Student(int studentId, string name)
        {
            StudentId = studentId;
            Name = name;
        }
    }

    internal class JsonPractice
    {
        public void Run()
        {
            Student s1 = new Student(1, "Jeyachandran");

            string json1 = JsonSerializer.Serialize(s1);

            Console.WriteLine(json1);

            //De-serialization

            Student json2 = JsonSerializer.Deserialize<Student>(json1);
            Console.WriteLine(json2.StudentId);
            Console.WriteLine(json2.Name);
        }
    }
}
