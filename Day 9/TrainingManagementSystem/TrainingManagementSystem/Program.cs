
using TrainingManagementSystem.Models;
using TrainingManagementSystem.Services;

Course[] courses =
{
    new Course("CS101", "C# Programming", 5000m, 30, 40),
    new Course("CS102", "Java Programming", 6000m, 35, 35),
    new Course("WD101", "Web Development", 4500m, 25, 30),
    new Course("PY101", "Python Programming", 5500m, 30, 25),
    new Course("DS101", "Data Science", 8000m, 45, 20)
};

TrainingService trainingService = new TrainingService(courses);
LearnerService learnerService = new LearnerService();

EnrollmentService enrollmentService =
    new EnrollmentService(courses, learnerService);

while (true)
{
    Console.WriteLine("\n========================================");
    Console.WriteLine(" TRAINING MANAGEMENT SYSTEM");
    Console.WriteLine("========================================");
    Console.WriteLine("1. Display all courses");
    Console.WriteLine("2. Search courses");
    Console.WriteLine("3. Sort courses by fee");
    Console.WriteLine("4. Register learner");
    Console.WriteLine("5. Display all learners");
    Console.WriteLine("6. Search learner");
    Console.WriteLine("7. Enroll learner in course");
    Console.WriteLine("8. View enrollment records");
    Console.WriteLine("9. Cancel enrollment");
    Console.WriteLine("0. Exit");
    Console.Write("Enter your choice: ");

    string choice = Console.ReadLine() ?? "";

    switch (choice)
    {
        case "1":
            trainingService.DisplayAllCourses();
            break;

        case "2":
            trainingService.SearchCourse();
            break;

        case "3":
            trainingService.SortCoursesByFee();
            break;

        case "4":
            Console.Write("Enter learner ID: ");
            string id = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Enter learner name: ");
            string name = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Enter contact number: ");
            string contact = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Enter email: ");
            string email = Console.ReadLine()?.Trim() ?? "";

            Learner learner = new Learner(id, name, contact, email);
            learnerService.RegisterLearner(learner);
            break;

        case "5":
            learnerService.DisplayAllLearners();
            break;

        case "6":
            learnerService.SearchLearner();
            break;

        case "7":
            enrollmentService.EnrollLearner();
            break;

        case "8":
            enrollmentService.DisplayEnrollments();
            break;

        case "9":
            enrollmentService.CancelEnrollment();
            break;

        case "0":
            Console.WriteLine("Thank you for using the application!");
            return;

        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }
}
