
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Services;

public class LearnerService
{
    private readonly Dictionary<string, Learner> learners = new();

    public bool RegisterLearner(Learner learner)
    {
        if (string.IsNullOrWhiteSpace(learner.LearnerId))
        {
            Console.WriteLine("Learner ID cannot be empty.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(learner.Name))
        {
            Console.WriteLine("Learner name cannot be empty.");
            return false;
        }

        if (!IsValidContact(learner.ContactNumber))
        {
            Console.WriteLine("Contact number must contain exactly 10 digits.");
            return false;
        }

        if (learners.ContainsKey(learner.LearnerId))
        {
            Console.WriteLine("Learner ID already exists.");
            return false;
        }

        learners.Add(learner.LearnerId, learner);
        Console.WriteLine("Learner registered successfully.");
        return true;
    }

    private bool IsValidContact(string contact)
    {
        if (contact.Length != 10)
            return false;

        foreach (char ch in contact)
        {
            if (!char.IsDigit(ch))
                return false;
        }

        return true;
    }

    public bool TryGetLearner(string learnerId, out Learner learner)
    {
        return learners.TryGetValue(learnerId, out learner!);
    }

    public void DisplayAllLearners()
    {
        if (learners.Count == 0)
        {
            Console.WriteLine("No learners registered.");
            return;
        }

        Console.WriteLine("\n--- REGISTERED LEARNERS ---");

        foreach (Learner learner in learners.Values)
        {
            Console.WriteLine(
                $"ID: {learner.LearnerId} | Name: {learner.Name} | " +
                $"Contact: {learner.ContactNumber} | Email: {learner.Email}");
        }
    }

    public void SearchLearner()
    {
        Console.Write("Enter learner ID: ");
        string id = Console.ReadLine()?.Trim() ?? "";

        if (TryGetLearner(id, out Learner learner))
        {
            Console.WriteLine($"ID: {learner.LearnerId}");
            Console.WriteLine($"Name: {learner.Name}");
            Console.WriteLine($"Contact: {learner.ContactNumber}");
            Console.WriteLine($"Email: {learner.Email}");
        }
        else
        {
            Console.WriteLine("Learner not found.");
        }
    }
}
