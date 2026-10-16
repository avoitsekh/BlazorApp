using SkiaSharp;
using System.Collections.Concurrent;

namespace BlazorApp.Components.Pages.ChatRoom;

public class Members
{
	readonly ConcurrentDictionary<string, string> members = new();

	public bool Add(string connectionId, string username)
	{
		username = username.Trim();
		if (HasMember(username))
		{
			return false;
		}

		members[connectionId] = username;
		return true;
	}

	public bool Remove(string connectionId)
	{
		return members.Remove(connectionId, out _);
	}

	public bool HasMember(string username)
	{
		return members.Any(x => x.Value.Equals(username, StringComparison.OrdinalIgnoreCase));
	}

	public string GetMember(string connectionId)
	{
		return members[connectionId];
	}

	public bool RemoveMember(string username)
	{
		username = username.Trim();
		return members.TryRemove(members.First(x => x.Value.Equals(username, StringComparison.OrdinalIgnoreCase)));
	}

}
