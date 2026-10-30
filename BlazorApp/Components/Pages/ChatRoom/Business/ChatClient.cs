using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorApp.Components.Pages.ChatRoom;

public sealed class ChatClient(string hubUrl) : IDisposable
{
	public readonly string HubUrl = hubUrl;

	public event EventHandler? Joining;
	public event EventHandler? Joined;
	public event EventHandler<Message>? MessageReceived;
	public event EventHandler<List<string>>? MembersChanged;
	public event EventHandler<string>? Error;

	HubConnection? hubConnection;

	public bool IsConnected => hubConnection?.State == HubConnectionState.Connected;

	public string? CurrentUsername { get; private set; }

	public async Task Join(string username)
	{
		string? errorMessage = null;

		try
		{
			CurrentUsername = username?.Trim();
			if ((errorMessage = ValidateUsername(CurrentUsername!)) != null)
			{
				return;
			}

			OnJoining();

			await Task.Delay(60);

			hubConnection = new HubConnectionBuilder()
				.WithUrl(HubUrl)
				.WithAutomaticReconnect()
				.Build();

			hubConnection.On<Message>(nameof(ChatHub.Broadcast), OnReceiveMessage);
			hubConnection.On<List<string>>(nameof(ChatHub.MembersChange), OnMembersChanged);
			await hubConnection.StartAsync();

			if (!await hubConnection.InvokeAsync<bool>(nameof(ChatHub.Register), CurrentUsername))
			{
				errorMessage = $"{CurrentUsername} has already joined this chat room. Please use another name.";
				return;
			}

			OnJoined();
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
		CurrentUsername = null;
		if (hubConnection != null)
		{
			await hubConnection.DisposeAsync();
			hubConnection = null;
		}
	}

	public async Task SendMessage(Message message)
	{
		if (IsConnected)
		{
			message.Owner = CurrentUsername!;
			await hubConnection!.SendAsync(nameof(ChatHub.Broadcast), message);
		}
	}

	void OnReceiveMessage(Message message)
	{
		message.IsMine = message.Owner?.Equals(CurrentUsername, StringComparison.OrdinalIgnoreCase) ?? false;
		MessageReceived?.Invoke(this, message);
	}

	private void OnMembersChanged(List<string> list)
	{
		MembersChanged?.Invoke(this, list);
	}

	void OnJoining()
	{
		Joining?.Invoke(this, EventArgs.Empty);
	}

	void OnJoined()
	{
		Joined?.Invoke(this, EventArgs.Empty);
	}

	void OnError(string errorMessage)
	{
		Error?.Invoke(this, errorMessage);
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
