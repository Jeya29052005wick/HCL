
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Services;

public class TrainingService
{
    private readonly Course[] courses;

    public TrainingService(Course[] courses)
    {
        this.courses = courses;
    }

    public void DisplayAllCourses()
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 85));

        Console.WriteLine(
            "{0,-10} {1,-28} {2,12} {3,10} {4,12}",
            "Code", "Course Title", "Fee (Rs.)", "Seats", "Available");

        Console.WriteLine(new string('-', 85));

        foreach (Course course in courses)
        {
            Console.WriteLine(
                "{0,-10} {1,-28} {2,12:F2} {3,10} {4,12}",
                course.CourseCode,
                course.Title,
                course.Fee,
                course.TotalSeats,
                course.AvailableSeats);
        }

        Console.WriteLine(new string('=', 85));
    }

    public void SearchCourse()
    {
        Console.Write("Enter course code or title: ");
        string searchTerm = Console.ReadLine()?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            Console.WriteLine("Search term cannot be empty.");
            return;
        }

        bool found = false;

        foreach (Course course in courses)
        {
            if (course.CourseCode.Equals(
                    searchTerm, StringComparison.OrdinalIgnoreCase) ||
                course.Title.Contains(
                    searchTerm, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine(
                    $"{course.CourseCode} | {course.Title} | " +
                    $"Rs. {course.Fee:F2} | Available: {course.AvailableSeats}");

                found = true;
            }
        }

        if (!found)
            Console.WriteLine("No matching course found.");
    }

    public void SortCoursesByFee()
    {
        Console.WriteLine("1. Low to High");
        Console.WriteLine("2. High to Low");
        Console.Write("Choose sorting order: ");

        string choice = Console.ReadLine() ?? "";

        if (choice != "1" && choice != "2")
        {
            Console.WriteLine("Invalid sorting option.");
            return;
        }

        bool ascending = choice == "1";
        Course[] sortedCourses = (Course[])courses.Clone();

        for (int i = 0; i < sortedCourses.Length - 1; i++)
        {
            for (int j = 0; j < sortedCourses.Length - 1 - i; j++)
            {
                bool swap = ascending
                    ? sortedCourses[j].Fee > sortedCourses[j + 1].Fee
                    : sortedCourses[j].Fee < sortedCourses[j + 1].Fee;

                if (swap)
                {
                    Course temp = sortedCourses[j];
                    sortedCourses[j] = sortedCourses[j + 1];
                    sortedCourses[j + 1] = temp;
                }
            }
        }

        foreach (Course course in sortedCourses)
        {
            Console.WriteLine(
                $"{course.CourseCode,-10} {course.Title,-28} " +
                $"Rs. {course.Fee,10:F2}");
        }
    }
}
