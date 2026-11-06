using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class Journal
{
    private List<Entry> _entries = new List<Entry>();

    public void AddEntry(string prompt, string response)
    {
        _entries.Add(new Entry(prompt, response));
    }

// The prompts that displays all the entries in the journal, showing the date, prompt, and response for each entry.
    public void DisplayEntries()
    {
        if (_entries.Count == 0)
        {
            Console.WriteLine("Your journal is empty.");
            return;
        }

        foreach (Entry entry in _entries)
        {
            Console.WriteLine($"\nDate: {entry.Date}");
            Console.WriteLine($"Prompt: {entry.Prompt}");
            Console.WriteLine($"Response: {entry.Response}");
        }
    }
// The prompts that saves the journal entries to a specified file in JSON format, and loads the journal entries from a specified file, handling any exceptions that may occur during the process.
    public void SaveToFile(string filename)
    {
        string json = JsonSerializer.Serialize(_entries, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        File.WriteAllText(filename, json);
    }
// The prompts that loads the journal entries from a specified file, deserializing the JSON data into a list of Entry objects, and handling any exceptions that may occur during the process.
    public void LoadFromFile(string filename)
    {
        string json = File.ReadAllText(filename);
        List<Entry> loadedEntries = JsonSerializer.Deserialize<List<Entry>>(json);

        if (loadedEntries == null || loadedEntries.Any(entry => entry == null))
        {
            throw new InvalidDataException("The file does not contain a valid journal.");
        }

        _entries = loadedEntries;
    }
}
