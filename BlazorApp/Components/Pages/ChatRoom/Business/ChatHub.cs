using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace BlazorApp.Components.Pages.ChatRoom;

class ChatHub : Hub
{
	public const string Path = "/chathub";

	static readonly ConcurrentDictionary<string, string> chatMembers = new();

	public async Task Broadcast(ChatMessage message)
	{
		if (!message.IsEmpty)
		{
			await Clients.All.SendAsync(nameof(Broadcast), message);
		}
	}

	public bool Register(string username)
	{
		username = username.Trim();
		if (chatMembers.Any(x => x.Value.Equals(username, StringComparison.OrdinalIgnoreCase)))
		{
			return false;
		}

		chatMembers[Context.ConnectionId] = username;
		return true;
	}

	//public void Remove(string username)
	//{
	//	username = username.Trim();
	//	chatMembers.TryRemove(chatMembers.First(x => x.Value.Equals(username, StringComparison.OrdinalIgnoreCase)));
	//}

	public override async Task OnConnectedAsync()
	{
		Console.WriteLine($"Connected: {Context.ConnectionId}");
		await base.OnConnectedAsync();
	}

	public override async Task OnDisconnectedAsync(Exception? e)
	{
		chatMembers.Remove(Context.ConnectionId, out _);

		Console.WriteLine($"Disconnected: {Context.ConnectionId} {e?.Message}");
		await base.OnDisconnectedAsync(e);
	}
}
