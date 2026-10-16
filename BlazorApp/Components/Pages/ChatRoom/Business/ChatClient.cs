using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorApp.Components.Pages.ChatRoom;

public sealed class ChatClient : IDisposable
{
	public readonly string HubUrl;
	public Action<Message>? MessageReceived;
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
			Username = username?.Trim();
			if ((errorMessage = ValidateUsername(Username)) != null)
			{
				return;
			}

			hubConnection = new HubConnectionBuilder()
				.WithUrl(HubUrl)
				.Build();

			hubConnection.On<Message>(nameof(ChatHub.Broadcast), OnReceiveMessage);
			await hubConnection.StartAsync();

			if (!await hubConnection.InvokeAsync<bool>(nameof(ChatHub.Register), Username))
			{
				errorMessage = $"{Username} has already joined this chat room. Please use another name.";
			}
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
				await Exit();
			}
		}
	}

	public async Task Exit()
	{
		Username = null;
		if (hubConnection != null)
		{
			await hubConnection.DisposeAsync();
			hubConnection = null;
		}
	}

	public async Task SendMessage(Message message)
	{
		if (IsConnected && !message.IsEmpty)
		{
			message.Owner = Username!;
			await hubConnection!.SendAsync(nameof(ChatHub.Broadcast), message);
		}
	}

	void OnReceiveMessage(Message message)
	{
		message.IsMine = message.Owner?.Equals(Username, StringComparison.OrdinalIgnoreCase) ?? false;
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

	string? ValidateUsername(string username)
	{
		string? errorMessage = null;

		if (string.IsNullOrEmpty(username))
		{
			errorMessage = "Username cannot be empty.";
		}
		else if (username.Length < 2)
		{
			errorMessage = "Username is too short. Consider having username with at least 2 characters.";
		}
		else if (username.Length > 50)
		{
			errorMessage = "Username is too long. Consider having username with 50 characters or less.";
		}
		else if (username.Split(' ').Length > 3)
		{
			errorMessage = @"Invalid username. Consider having username with only firstname, firstname + lastname, or firstname + middlename + lastname.";
		}

		return errorMessage;
	}
}
