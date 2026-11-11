using System;
using System.Collections.Generic;
using System.Linq;

// Storing a reference and its words, and manages hiding and displaying the scripture.
public class Scripture
{
    private readonly Reference _reference;
    private readonly List<Word> _words;
    private readonly Random _random;

    // Splitting the verse into individual Word objects so each word can be hidden separately.
    public Scripture(Reference reference, string text)
    {
        _reference = reference ?? throw new ArgumentNullException(nameof(reference));
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Scripture text is required.", nameof(text));
        }

        _words = text.Split((char[])null, StringSplitOptions.RemoveEmptyEntries)
            .Select(word => new Word(word))
            .ToList();
        _random = new Random();
    }

    // Returning the reference followed by the current visible and hidden word forms.
    public string GetDisplayText()
    {
        string text = string.Join(" ", _words.Select(word => word.GetDisplayText()));
        return $"{_reference} {text}";
    }

    // Returning a clue version of the verse without changing which words are hidden.
    public string GetHintText()
    {
        string text = string.Join(" ", _words.Select(word => word.GetHintText()));
        return $"{_reference} {text}";
    }

    // Tracking progress so the learner can see how much of the verse remains visible.
    public int HiddenWordCount => _words.Count(word => word.IsHidden);
    public int TotalWordCount => _words.Count;

    // Hide only visible words, avoiding repeated random selections in the same round.
    public void HideRandomWords(int numberToHide)
    {
        List<Word> visibleWords = _words.Where(word => !word.IsHidden).ToList();
        int wordsToHide = Math.Min(Math.Max(numberToHide, 0), visibleWords.Count);

        for (int i = 0; i < wordsToHide; i++)
        {
            int selectedIndex = _random.Next(visibleWords.Count);
            visibleWords[selectedIndex].Hide();
            visibleWords.RemoveAt(selectedIndex);
        }
    }

    // A scripture is complete when every word has been hidden.
    public bool IsCompletelyHidden()
    {
        return _words.All(word => word.IsHidden);
    }

    // Reusing a scripture from the library starts a fresh memorization session.
    public void Reset()
    {
        foreach (Word word in _words)
        {
            word.Reset();
        }
    }
}
