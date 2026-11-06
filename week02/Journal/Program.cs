using System;

class Program
{
    
    // prompts to guide users in their journaling experience, encouraging reflection on daily experiences and personal growth. 
private static readonly string[] _prompts =
    {
        "What small moment made you smile today?",
        "Who helped you learn something new today?",
        "What is one thing you would like to do differently tomorrow?",
        "When did you feel most focused or at peace today?",
        "What challenge did you handle better than you expected?",
        "What is something you are grateful for that you often take for granted?"
    };

    static void Main(string[] args)
    {
        // Exceeding the basic requirements by using JSON storage, which safely preserves commas and quotation marks in journal responses, and displaying the current date in the welcome menu.
        Journal journal = new Journal();
        bool running = true;

         // The main loop of the program, which continues to run until the user chooses to quit.
        while (running)
        {
            Console.WriteLine("\nWelcome to My Journal App!");
            Console.WriteLine($"Today's date: {DateTime.Now:MMMM d, yyyy}");
            Console.WriteLine("Where you are both the defendant and the judge—be kind to yourself.");
            Console.WriteLine("\n1. Write a new entry");
            Console.WriteLine("2. Display the journal");
            Console.WriteLine("3. Save the journal");
            Console.WriteLine("4. Load the journal");
            Console.WriteLine("5. Quit");
            Console.Write("\nChoose an option: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    WriteEntry(journal);
                    break;
                case "2":
                    journal.DisplayEntries();
                    break;
                case "3":
                    SaveJournal(journal);
                    break;
                case "4":
                    LoadJournal(journal);
                    break;
                case "5":
                    running = false;
                    break;
                case null:
                    running = false;
                    break;
                default:
                    Console.WriteLine("Please choose a number from 1 to 5.");
                    break;
            }
        }
    }
// The prompts that asks the user a random question from the _prompts array, collects their response, and adds it to the journal.
    private static void WriteEntry(Journal journal)
    {
        string prompt = _prompts[Random.Shared.Next(_prompts.Length)];
        Console.WriteLine($"\n{prompt}");
        Console.Write("> ");
        string response = Console.ReadLine() ?? string.Empty;
        journal.AddEntry(prompt, response);
        Console.WriteLine("Your entry has been added.");
    }
// The prompts that asks the user for a filename and attempts to save the journal to that file, handling any exceptions that may occur during the process.
    private static void SaveJournal(Journal journal)
    {
        Console.Write("Enter a filename to save to: ");
        string filename = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(filename))
        {
            Console.WriteLine("A filename is required.");
            return;
        }

        try
        {
            journal.SaveToFile(filename);
            Console.WriteLine($"Journal saved to {filename}.");
        }
        catch (Exception exception) when (
            exception is IOException ||
            exception is UnauthorizedAccessException ||
            exception is ArgumentException)
        {
            Console.WriteLine($"Could not save the journal: {exception.Message}");
        }
    }
// The prompts that asks the user for a filename and attempts to load the journal from that file, handling any exceptions that may occur during the process.
    private static void LoadJournal(Journal journal)
    {
        Console.Write("Enter a filename to load from: ");
        string filename = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(filename))
        {
            Console.WriteLine("A filename is required.");
            return;
        }

        try
        {
            journal.LoadFromFile(filename);
            Console.WriteLine($"Journal loaded from {filename}.");
        }
        catch (Exception exception) when (
            exception is IOException ||
            exception is UnauthorizedAccessException ||
            exception is ArgumentException ||
            exception is System.Text.Json.JsonException)
        {
            Console.WriteLine($"Could not load the journal: {exception.Message}");
        }
    }
}
