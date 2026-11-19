using System;
using System.Collections.Generic;
using System.Linq;

// Represents video details, tags, and associated comments.
class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int LengthInSeconds { get; set; }
    public int LikesCount { get; private set; }
    public int DislikesCount { get; private set; }
    private readonly List<Comment> _comments;
    private readonly List<Tag> _tags = new List<Tag>();
    public IReadOnlyList<Tag> Tags => _tags;

    public Video(string title, string author, int lengthInSeconds)
    {
        Title = title;
        Author = author;
        LengthInSeconds = lengthInSeconds;
        _comments = new List<Comment>();
    }

    public void AddComment(Comment comment)
    {
        _comments.Add(comment);
    }

    public int GetTotalComments()
    {
        return _comments.Sum(comment => 1 + comment.GetTotalReplyCount());
    }

    public void AddLike()
    {
        LikesCount++;
    }

    public void RemoveLike()
    {
        if (LikesCount > 0)
        {
            LikesCount--;
        }
    }

    public void AddDislike()
    {
        DislikesCount++;
    }

    public void RemoveDislike()
    {
        if (DislikesCount > 0)
        {
            DislikesCount--;
        }
    }

    public double GetEngagementRatio()
    {
        int totalReactions = LikesCount + DislikesCount;
        return totalReactions == 0 ? 0 : (double)LikesCount / totalReactions;
    }

    public void AddTag(Tag tag)
    {
        if (!_tags.Any(existingTag => string.Equals(existingTag.Name, tag.Name, StringComparison.OrdinalIgnoreCase)))
        {
            _tags.Add(tag);
        }
    }

    public bool HasTag(string tagName)
    {
        return _tags.Any(tag => string.Equals(tag.Name, tagName, StringComparison.OrdinalIgnoreCase));
    }

    public static IEnumerable<Video> FindByTag(IEnumerable<Video> videos, string tagName)
    {
        return videos.Where(video => video.HasTag(tagName));
    }

    public void DisplayDetails()
    {
        Console.WriteLine($"Title: {Title}");
        Console.WriteLine($"Author: {Author}");
        Console.WriteLine($"Length: {LengthInSeconds} seconds");
        Console.WriteLine($"Total Comments: {GetTotalComments()}");
        Console.WriteLine($"Likes: {LikesCount}, Dislikes: {DislikesCount}, Engagement Ratio: {GetEngagementRatio():P0}");
        Console.WriteLine($"Tags: {string.Join(", ", _tags.Select(tag => tag.Name))}");
        Console.WriteLine();
        Console.WriteLine("Comments:");
        foreach (Comment comment in _comments)
        {
            comment.DisplayThread();
        }
        Console.WriteLine(new string('-', 40));
        Console.WriteLine();
    }
}