using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorApp.Components.Pages.ChatRoom;

public sealed class ChatClient : IDisposable
{
	public readonly string HubUrl;
	public Action<ChatMessage>? MessageReceived;
	public Action<string>? Error;

	HubConnection? hubConnection;

	public bool IsConnected => hubConnection?.State == HubConnectionState.Connected;

	public string? Username { get; private set; }

	public ChatClient(string hubUrl)
	{
		HubUrl = hubUrl;
	}

	public async Task Join(string username)
	{
		string? errorMessage = null;

		try
		{
			if (string.IsNullOrWhiteSpace(username))
			{
				errorMessage = "Invalid username.";
				return;
			}

			hubConnection = new HubConnectionBuilder()
				.WithUrl(HubUrl)
				.Build();

			hubConnection.On<ChatMessage>(nameof(ChatHub.Broadcast), OnReceiveMessage);
			await hubConnection.StartAsync();

			Username = username.Trim();
			if (!await hubConnection.InvokeAsync<bool>(nameof(ChatHub.Register), Username))
			{
				errorMessage = $"{Username} has already joined this chat room. Please use another name.";
				return;
			}

			await SendMessage(new($"{Username} joined chat room.") { IsNotification = true });
		}
		catch (Exception e)
		{
			errorMessage = $"Unable to join the chat. Error: {e.Message}";
		}
		finally
		{
			if (errorMessage != null)
			{
				OnError(errorMessage);
				await Exit(notify: false);
			}
		}
	}

	public async Task Exit(bool notify = true)
	{
		if (IsConnected && notify)
		{
			await SendMessage(new($"{Username} left chat room.") { IsNotification = true });
			Username = null;
		}

		if (hubConnection != null)
		{
			await hubConnection.DisposeAsync();
			hubConnection = null;
		}
	}

	public async Task SendMessage(ChatMessage message)
	{
		if (IsConnected && !message.IsEmpty)
		{
			message.Username = Username!;
			await hubConnection!.SendAsync(nameof(ChatHub.Broadcast), message);
		}
	}

	void OnReceiveMessage(ChatMessage message)
	{
		message.IsMine = message.Username?.Equals(Username, StringComparison.OrdinalIgnoreCase) ?? false;
		MessageReceived?.Invoke(message);
	}

	void OnError(string errorMessage)
	{
		Error?.Invoke(errorMessage);
	}

	public void Dispose()
	{
		_ = Exit();
	}
}
