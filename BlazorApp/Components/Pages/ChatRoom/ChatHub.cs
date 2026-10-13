using Microsoft.AspNetCore.SignalR;

namespace BlazorApp.Components.Pages.ChatRoom;

class ChatHub : Hub
{
	public const string Path = "/chathub";

	public async Task Broadcast(string username, string message)
	{
		await Clients.All.SendAsync("Broadcast", username, message);
	}

	public override async Task OnConnectedAsync()
	{
		Console.WriteLine($"Connected: {Context.ConnectionId}");
		await base.OnConnectedAsync();
	}

	public override async Task OnDisconnectedAsync(Exception? e)
	{
		Console.WriteLine($"Disconnected: {Context.ConnectionId} {e?.Message}");
		await base.OnDisconnectedAsync(e);
	}
}
