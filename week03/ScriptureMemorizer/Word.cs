using System;
using System.Text;

// Representing one scripture word and controls its normal, hidden, and hint displays.
public class Word
{
    private readonly string _text;
    private bool _isHidden;

    // Keeping the original text so it can be displayed or masked as needed.
    public Word(string text)
    {
        _text = text ?? throw new ArgumentNullException(nameof(text));
        _isHidden = false;
    }

    public bool IsHidden => _isHidden;

    // Marking this word as hidden for subsequent displays.
    public void Hide()
    {
        _isHidden = true;
    }

    public void Reset()
    {
        _isHidden = false;
    }

    // Replacing letters with underscores while leaving punctuation intact.
    public string GetDisplayText()
    {
        if (!_isHidden)
        {
            return _text;
        }

        StringBuilder hiddenText = new StringBuilder(_text.Length);
        foreach (char character in _text)
        {
            hiddenText.Append(char.IsLetter(character) ? '_' : character);
        }

        return hiddenText.ToString();
    }

    // Giving a recall clue by showing the first letter of a hidden word.
    public string GetHintText()
    {
        if (!_isHidden)
        {
            return _text;
        }

        StringBuilder hintText = new StringBuilder(_text.Length);
        bool initialShown = false;

        foreach (char character in _text)
        {
            // Preserving punctuation and spacing-related characters in the clue.
            if (!char.IsLetter(character))
            {
                hintText.Append(character);
            }
            else if (!initialShown)
            {
                hintText.Append(character);
                initialShown = true;
            }
            else
            {
                hintText.Append('_');
            }
        }

        return hintText.ToString();
    }
}
