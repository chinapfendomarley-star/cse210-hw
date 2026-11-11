using System;

// Stores the book, chapter, and one verse or a range of verses.
public class Reference
{
    private readonly string _book;
    private readonly int _chapter;
    private readonly int _startVerse;
    private readonly int _endVerse;

    // A single-verse reference that uses the same starting and ending verse.
    public Reference(string book, int chapter, int verse)
        : this(book, chapter, verse, verse)
    {
    }

    // Storing and validating a reference that may cover multiple verses.
    public Reference(string book, int chapter, int startVerse, int endVerse)
    {
        if (string.IsNullOrWhiteSpace(book))
        {
            throw new ArgumentException("A book name is required.", nameof(book));
        }

        if (chapter < 1 || startVerse < 1 || endVerse < startVerse)
        {
            throw new ArgumentOutOfRangeException(nameof(chapter), "Chapter and verse numbers must be positive, and the end verse cannot precede the start verse.");
        }

        _book = book;
        _chapter = chapter;
        _startVerse = startVerse;
        _endVerse = endVerse;
    }

    // Formatting the reference as Book chapter:verse or Book chapter:start-end.
    public override string ToString()
    {
        string verse = _startVerse == _endVerse
            ? _startVerse.ToString()
            : $"{_startVerse}-{_endVerse}";
        return $"{_book} {_chapter}:{verse}";
    }
}
