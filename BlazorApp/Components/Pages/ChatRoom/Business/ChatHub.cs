using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;

namespace BlazorApp.Components.Pages.ChatRoom;

class ChatHub : Hub
{
	public const string Path = "/chathub";

	static readonly Members members = new();

	public async Task Broadcast(Message message)
	{
		if (!message.IsEmpty && (members.HasMember(message.Owner) || message.IsNotification))
		{
			await Clients.All.SendAsync(nameof(Broadcast), message);
		}
	}

	public async Task MembersChange(List<string> members)
	{
		await Clients.All.SendAsync(nameof(MembersChange), members.Order());
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
		await MembersChange(members.ToList());
		return result;
	}

	async Task<bool> UnRegister(string connectionId, Exception? e = default)
	{
		var username = members.GetMember(connectionId);
		var result = members.Remove(Context.ConnectionId);
		if (result)
		{
			await Broadcast(new()
			{
				Body = $"{username} left chat room",// + (e != null ? $" -- {e.GetBaseException().Message}" : ""),
				IsNotification = true
			});
		}
		await MembersChange(members.ToList());
		return result;
	}

	public override async Task OnConnectedAsync()
	{
		var transportType = Context.Features.Get<IHttpTransportFeature>()?.TransportType.ToString() ?? "(unknown)";
		var ipAddress = Context.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress?.MapToIPv4().ToString() ?? "(unknown)";
		Console.WriteLine($"Connected: {Context.ConnectionId}, {transportType}, {ipAddress}");

		await base.OnConnectedAsync();
	}

	public override async Task OnDisconnectedAsync(Exception? e)
	{
		await UnRegister(Context.ConnectionId, e);
		Console.WriteLine($"Disconnected: {Context.ConnectionId} {e?.Message}");

		await base.OnDisconnectedAsync(e);
	}
}
