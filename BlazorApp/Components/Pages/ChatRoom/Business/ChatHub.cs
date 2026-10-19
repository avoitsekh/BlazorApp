using Microsoft.AspNetCore.SignalR;

namespace BlazorApp.Components.Pages.ChatRoom;

class ChatHub : Hub
{
	public const string Path = "/chathub";

	static readonly Members members = new();

	public async Task Broadcast(Message message)
	{
		if (!message.IsEmpty)
		{
			await Clients.All.SendAsync(nameof(Broadcast), message);
		}
	}

	public async Task<bool> Register(string username)
	{
		var result = members.Add(Context.ConnectionId, username);
		if (result)
		{
			await Broadcast(new()
			{
				Body = $"{username} joined chat room",
				IsNotification = true
			});
		}
		return result;
	}

	async Task<bool> UnRegister(string connectionId)
	{
		var username = members.GetMember(connectionId);
		var result = members.Remove(Context.ConnectionId);
		if (result)
		{
			await Broadcast(new()
			{
				Body = $"{username} left chat room",
				IsNotification = true
			});
		}
		return result;
	}

	public override async Task OnConnectedAsync()
	{
		Console.WriteLine($"Connected: {Context.ConnectionId}");
		await base.OnConnectedAsync();
	}

	public override async Task OnDisconnectedAsync(Exception? e)
	{
		await UnRegister(Context.ConnectionId);

		Console.WriteLine($"Disconnected: {Context.ConnectionId} {e?.Message}");
		await base.OnDisconnectedAsync(e);
	}
}
