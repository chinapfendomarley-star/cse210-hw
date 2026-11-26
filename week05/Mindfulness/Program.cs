using System;

class Program
{
    static void Main(string[] args)
    {
        // Instance of each activity so they can be selected by menu number.
        Activity[] activities =
        {
            new BreathingActivity(),
            new ReflectionActivity(),
            new ListingActivity()
        };

        //Loop repeating the menu until the user chooses to quit.
        while (true)
        {
            // Display the available mindfulness activities.
            Console.WriteLine("\nMindfulness Activities:");
            Console.WriteLine("  1. Breathing Activity");
            Console.WriteLine("  2. Reflection Activity");
            Console.WriteLine("  3. Listing Activity");
            Console.WriteLine("  4. Quit");
            Console.Write("Select an option: ");

            // Reading the user's menu choice.
            string choice = Console.ReadLine();

            // Exiting the program when the user selects Quit.
            if (choice == "4")
            {
                Console.WriteLine("Thank 🙏 you for taking time to be mindful!");
                break;
            }

            // Converting the menu number to an activity index and run the selected activity.
            if (int.TryParse(choice, out int index) && index >= 1 && index <= activities.Length)
            {
                activities[index - 1].Run();
            }
            else
            {
                // Showing an error when the choice is not a valid menu option.
                Console.WriteLine("Please choose 1, 2, 3, or 4.");
            }
        }
    }
}
