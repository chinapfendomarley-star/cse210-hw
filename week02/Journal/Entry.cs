using System;
// The prompts that defines a journal entry with a prompt, response, and date, and provides methods to initialize a new entry and set the date to the current date and time.
public class Entry
{
    public string Prompt { get; set; }
    public string Response { get; set; }
    public string Date { get; set; }
    public Entry()
    {
        Prompt = string.Empty;
        Response = string.Empty;
        Date = string.Empty;
    }
// The prompts that initializes a new journal entry with the provided prompt and response, and sets the date to the current date and time.
    public Entry(string prompt, string response)
    {
        Prompt = prompt;
        Response = response;
        Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
