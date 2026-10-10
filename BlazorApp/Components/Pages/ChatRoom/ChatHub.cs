using Microsoft.AspNetCore.SignalR;

namespace BlazorApp.Components.Pages.ChatRoom;

class ChatHub : Hub
{
	public const string Path = "/chathub";

	public async Task Broadcast(string username, string message)
	{
		await Clients.All.SendAsync("Broadcast", username, message);
	}

	public override Task OnConnectedAsync()
	{
		Console.WriteLine($"{Context.ConnectionId} connected");
		return base.OnConnectedAsync();
	}

	public override async Task OnDisconnectedAsync(Exception? e)
	{
		Console.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}");
		await base.OnDisconnectedAsync(e);
	}
}
