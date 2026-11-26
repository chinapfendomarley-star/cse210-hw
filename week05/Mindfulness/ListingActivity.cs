using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

sealed class ListingActivity : Activity
{
    // Keeping a set of prompts that is returned in a randomized cycle.
    private readonly RandomizedCycle<string> _prompts = new RandomizedCycle<string>(new[]
    {
        "Who are people that you appreciate?",
        "What are personal strengths of yours?",
        "Who are people that you have helped this week?",
        "When have you felt the Holy Ghost this month?",
        "Who are some of your personal heroes?"
    });

    // Configuring this activity with its name and description.
    public ListingActivity()
        : base(
            "Listing Activity",
            "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.")
    {
    }

    // Showing a prompt, accept entries for the selected duration, and report the count.
    protected override void RunActivity()
    {
        Console.WriteLine("\nList responses to this prompt:");
        Console.WriteLine($"--- {_prompts.Next()} ---");
        PauseWithCountdown(5, "You may begin listing in");
        Console.WriteLine("Enter one item per line. Your time starts now.");

        // Using line reading or interactive key reading based on how input is provided.
        Stopwatch timer = Stopwatch.StartNew();
        List<string> items = Console.IsInputRedirected
            ? ReadRedirectedEntries(timer)
            : ReadInteractiveEntries(timer);

        Console.WriteLine($"\nYou listed {items.Count} item(s).");
    }

    // Reading complete lines when input is redirected, such as from a test or file.
    private List<string> ReadRedirectedEntries(Stopwatch timer)
    {
        List<string> items = new List<string>();
        while (GetRemainingSeconds(timer) > 0)
        {
            string item = Console.ReadLine();
            if (item == null)
            {
                break;
            }

            // Only count nonblank entries received before time expires.
            if (GetRemainingSeconds(timer) > 0 && !string.IsNullOrWhiteSpace(item))
            {
                items.Add(item.Trim());
            }
        }

        return items;
    }

    // Reading individual keyboard input while also displaying the remaining time in the title.
    private List<string> ReadInteractiveEntries(Stopwatch timer)
    {
        List<string> items = new List<string>();
        string currentEntry = "";
        int lastDisplayedSecond = -1;

        Console.Write("> ");
        // Continuing collecting entries until the activity duration expires.
        while (GetRemainingSeconds(timer) > 0)
        {
            // Process a key as soon as it is available.
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    // Add the completed line only if it contains text.
                    if (!string.IsNullOrWhiteSpace(currentEntry))
                    {
                        items.Add(currentEntry.Trim());
                    }

                    currentEntry = "";
                    Console.WriteLine();
                    if (GetRemainingSeconds(timer) > 0)
                    {
                        Console.Write("> ");
                    }
                }
                else if (key.Key == ConsoleKey.Backspace && currentEntry.Length > 0)
                {
                    // Removing the most recently typed character.
                    currentEntry = currentEntry.Substring(0, currentEntry.Length - 1);
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    // Adding printable characters to the current entry.
                    currentEntry += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }

            // Updating the window title only when the remaining whole-second value changes.
            int remaining = GetRemainingSeconds(timer);
            if (remaining != lastDisplayedSecond)
            {
                Console.Title = $"Listing Activity - {remaining} seconds remaining";
                lastDisplayedSecond = remaining;
            }

            Thread.Sleep(40);
        }

        Console.Title = "Mindfulness Activities";
        Console.WriteLine("\nTime is up.");
        return items;
    }
}
