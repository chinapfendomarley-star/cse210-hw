using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

// Loading scripture entries from JSON and selects one at random for a practice session.
public class ScriptureLibrary
{
    private readonly List<Scripture> _scriptures;
    private readonly Random _random;

    // The library is constructed from validated scripture objects.
    private ScriptureLibrary(List<Scripture> scriptures)
    {
        _scriptures = scriptures;
        _random = new Random();
    }

    // Reading the JSON file, validating each entry, and converting it into a Scripture object.
    public static ScriptureLibrary LoadFromFile(string filePath)
    {
        string json = File.ReadAllText(filePath);
        List<ScriptureEntry> entries = JsonSerializer.Deserialize<List<ScriptureEntry>>(json);

        if (entries == null || entries.Count == 0)
        {
            throw new InvalidDataException("The file must contain at least one scripture.");
        }

        List<Scripture> scriptures = new List<Scripture>();

        for (int i = 0; i < entries.Count; i++)
        {
            ScriptureEntry entry = entries[i];

            if (entry == null ||
                string.IsNullOrWhiteSpace(entry.Book) ||
                string.IsNullOrWhiteSpace(entry.Text) ||
                entry.Chapter < 1 ||
                entry.StartVerse < 1 ||
                (entry.EndVerse.HasValue && entry.EndVerse.Value < entry.StartVerse))
            {
                throw new InvalidDataException($"Scripture entry {i + 1} has invalid or missing values.");
            }

            // Using the matching constructor to preserve single-verse and range references.
            Reference reference = entry.EndVerse.HasValue
                ? new Reference(entry.Book, entry.Chapter, entry.StartVerse, entry.EndVerse.Value)
                : new Reference(entry.Book, entry.Chapter, entry.StartVerse);

            scriptures.Add(new Scripture(reference, entry.Text));
        }

        return new ScriptureLibrary(scriptures);
    }

    // Picking an entry uniformly at random from the loaded library.
    public Scripture GetRandomScripture(Scripture previousScripture = null)
    {
        List<Scripture> candidates = _scriptures
            .Where(scripture => !ReferenceEquals(scripture, previousScripture))
            .ToList();

        if (candidates.Count == 0)
        {
            candidates = _scriptures;
        }

        int selectedIndex = _random.Next(candidates.Count);
        Scripture selectedScripture = candidates[selectedIndex];
        selectedScripture.Reset();
        return selectedScripture;
    }

    // Matching the properties in scripture-library.json during deserialization.
    private class ScriptureEntry
    {
        public ScriptureEntry()
        {
        }

        public string Book { get; set; }
        public int Chapter { get; set; }
        public int StartVerse { get; set; }
        public int? EndVerse { get; set; }
        public string Text { get; set; }
    }
}
