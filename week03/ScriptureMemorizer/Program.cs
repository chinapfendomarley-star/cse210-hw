using System;
using System.IO;
using System.Text.Json;

//Memorization session using a randomly selected scripture from the library.
class Program
{
    static void Main(string[] args)
    {
        // The editable library beside the executable that can be changed without rebuilding.
        string libraryPath = Path.Combine(AppContext.BaseDirectory, "scripture-library.json");
        ScriptureLibrary library;

        // Reporting expected file and data problems clearly instead of starting with an empty library.
        try
        {
            library = ScriptureLibrary.LoadFromFile(libraryPath);
        }
        catch (Exception exception) when (
            exception is IOException ||
            exception is JsonException ||
            exception is InvalidDataException ||
            exception is UnauthorizedAccessException)
        {
            Console.Error.WriteLine($"Unable to load the scripture library: {exception.Message}");
            return;
        }

        // Starting at normal difficulty: hide three new words after each Enter key press.
        Scripture scripture = library.GetRandomScripture();
        int wordsPerRound = 3;

        while (true)
        {
            Console.Clear();
            Console.WriteLine(scripture.GetDisplayText());
            Console.WriteLine($"Hidden words: {scripture.HiddenWordCount}/{scripture.TotalWordCount}");

            // Check after displaying so the fully hidden scripture is the final display.
            if (scripture.IsCompletelyHidden())
            {
                Console.WriteLine();
                Console.WriteLine("You memorized this scripture!");
                Console.WriteLine("1. Memorize another scripture");
                Console.WriteLine("2. Quit");
                Console.Write("Choose an option: ");
                string completionResponse = Console.ReadLine();

                if (completionResponse == "1")
                {
                    scripture = library.GetRandomScripture(scripture);
                    continue;
                }

                if (completionResponse == "2" || completionResponse == null)
                {
                    break;
                }

                Console.WriteLine("Invalid option. Choose 1 or 2.");
                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();
                continue;
            }

            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("1. Hide words");
            Console.WriteLine("2. Show first-letter hints");
            Console.WriteLine("3. Set difficulty to easy");
            Console.WriteLine("4. Set difficulty to normal");
            Console.WriteLine("5. Set difficulty to hard");
            Console.WriteLine("6. Quit");
            Console.Write("Choose an option: ");
            string response = Console.ReadLine();

            if (response == null || response == "6")
            {
                break;
            }

            if (response == "1")
            {
                scripture.HideRandomWords(wordsPerRound);
            }
            else if (response == "2")
            {
                // Show a clue temporarily; the scripture's hidden-word state is not changed.
                Console.Clear();
                Console.WriteLine(scripture.GetDisplayText());
                Console.WriteLine();
                Console.WriteLine($"First-letter hint: {scripture.GetHintText()}");
                Console.WriteLine();
                Console.Write("Press enter to continue: ");
                Console.ReadLine();
            }
            else if (response == "3")
            {
                wordsPerRound = 1;
            }
            else if (response == "4")
            {
                wordsPerRound = 3;
            }
            else if (response == "5")
            {
                wordsPerRound = 5;
            }
            else
            {
                Console.WriteLine("Invalid option. Choose a number from 1 to 6.");
                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();
            }
        }
    }
}
