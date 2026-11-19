using System;
using System.Collections.Generic;
using System.Linq;

// Represents a viewer's comment and formats it for display.
class Comment
{
    public string CommenterName { get; set; }
    public string TextContent { get; set; }
    public int LikesCount { get; private set; }
    public int DislikesCount { get; private set; }
    private readonly List<Comment> _replies = new List<Comment>();
    public IReadOnlyList<Comment> Replies => _replies;

    public Comment(string commenterName, string textContent)
    {
        CommenterName = commenterName;
        TextContent = textContent;
    }

    public string DisplayComment()
    {
        return $"{CommenterName}: \"{TextContent}\" (Likes: {LikesCount}, Dislikes: {DislikesCount})";
    }

    public void AddReply(Comment reply)
    {
        _replies.Add(reply);
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

    public int GetTotalReplyCount()
    {
        return _replies.Sum(reply => 1 + reply.GetTotalReplyCount());
    }

    public int GetEngagementDepth()
    {
        return _replies.Count == 0
            ? 0
            : 1 + _replies.Max(reply => reply.GetEngagementDepth());
    }

    public void DisplayThread(int indentation = 2)
    {
        Console.WriteLine($"{new string(' ', indentation)}- {DisplayComment()}");
        foreach (Comment reply in _replies)
        {
            reply.DisplayThread(indentation + 2);
        }
    }
}