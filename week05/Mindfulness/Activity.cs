using System;
using System.Diagnostics;
using System.Threading;

abstract class Activity
{
    // Storing the activity name and description shared by every activity.
    private readonly string _name;
    private readonly string _description;
    private int _durationSeconds;

    // Setting the name and description supplied by a derived activity.
    protected Activity(string name, string description)
    {
        _name = name;
        _description = description;
    }

    // Exposing the session duration to derived activity classes.
    protected int DurationSeconds
    {
        get { return _durationSeconds; }
    }

    // Running the complete activity flow from starting message through ending message.
    public void Run()
    {
        ShowStartingMessage();
        RunActivity();
        ShowEndingMessage();
    }

    // Require each derived activity to implement its own exercise behavior.
    protected abstract void RunActivity();

    // Pausing for a given time while showing a counting-down timer.
    protected void PauseWithCountdown(int seconds, string message)
    {
        DateTime endTime = DateTime.UtcNow.AddSeconds(seconds);
        int lastDisplayed = -1;

        // Update the countdown only when the displayed whole-second value changes.
        while (DateTime.UtcNow < endTime)
        {
            int remaining = (int)Math.Ceiling((endTime - DateTime.UtcNow).TotalSeconds);
            if (remaining != lastDisplayed)
            {
                Console.Write($"\r{message} {remaining}   ");
                lastDisplayed = remaining;
            }

            Thread.Sleep(100);
        }

        // Move to a new line after the countdown finishes.
        Console.WriteLine();
    }

    // Pausing for a given time while showing a rotating spinner.
    protected void PauseWithSpinner(int seconds, string message)
    {
        char[] frames = { '|', '/', '-', '\\' };
        Stopwatch timer = Stopwatch.StartNew();
        int frame = 0;

        // Cycle through the spinner frames until the pause duration passes.
        while (timer.Elapsed.TotalSeconds < seconds)
        {
            Console.Write($"\r{frames[frame]} {message}   ");
            frame = (frame + 1) % frames.Length;
            Thread.Sleep(150);
        }

        // Move to a new line after the spinner finishes.
        Console.WriteLine();
    }

    // Calculating how many seconds remain in the selected activity duration.
    protected int GetRemainingSeconds(Stopwatch timer)
    {
        return Math.Max(0, _durationSeconds - (int)Math.Floor(timer.Elapsed.TotalSeconds));
    }

    // Asking for the user duration and displaying the starting countdown.
    private void ShowStartingMessage()
    {
        Console.WriteLine($"\n{_name}");
        Console.WriteLine(_description);

        // Continuing prompting until the user enters a positive whole number.
        while (true)
        {
            Console.Write("How long, in seconds, would you like for your session? ");
            string input = Console.ReadLine();
            if (int.TryParse(input, out _durationSeconds) && _durationSeconds > 0)
            {
                break;
            }

            Console.WriteLine("Please enter a whole number greater than zero.");
        }

        Console.WriteLine("\nPrepare to begin...");
        PauseWithCountdown(3, "Starting in");
    }

    // Displaying the completion message and a short final pause.
    private void ShowEndingMessage()
    {
        Console.WriteLine("\nYou did a great job!");
        PauseWithSpinner(2, "Take a moment to notice how you feel.");
        Console.WriteLine($"You have completed the {_name} for {_durationSeconds} seconds.");
        PauseWithSpinner(3, "Well done");
    }
}

// Providing a list of items that is randomized and does not repeat until every item has appeared.
sealed class RandomizedCycle<T>
{
    // Keeping the original items and track items that have not yet been selected.
    private readonly T[] _items;
    private readonly Random _random = new Random();
    private readonly System.Collections.Generic.List<T> _remaining;

    // Make a copy so changes to the original array do not affect this cycle.
    public RandomizedCycle(T[] items)
    {
        _items = (T[])items.Clone();
        _remaining = new System.Collections.Generic.List<T>();
    }

    // Returning a random item and remove it until all items have been used.
    public T Next()
    {
        // Refill the remaining list after every item has been selected.
        if (_remaining.Count == 0)
        {
            _remaining.AddRange(_items);
        }

        int index = _random.Next(_remaining.Count);
        T item = _remaining[index];
        _remaining.RemoveAt(index);
        return item;
    }
}
