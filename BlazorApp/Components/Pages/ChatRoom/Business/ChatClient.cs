using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorApp.Components.Pages.ChatRoom;

public sealed class ChatClient : IDisposable
{
	public readonly string HubUrl;
	public Action<Message>? MessageReceived;
	public Action<string>? Error;
	public Action<List<string>>? MembersChanged;

	HubConnection? hubConnection;

	public bool IsConnected => hubConnection?.State == HubConnectionState.Connected;

	public string? CurrentUsername { get; private set; }


	public ChatClient(string hubUrl)
	{
		HubUrl = hubUrl;
	}

	public async Task Join(string username)
	{
		string? errorMessage = null;

		try
		{
			CurrentUsername = username?.Trim();
			if ((errorMessage = ValidateUsername(CurrentUsername)) != null)
			{
				return;
			}

			hubConnection = new HubConnectionBuilder()
				.WithUrl(HubUrl)
				.Build();

			hubConnection.On<Message>(nameof(ChatHub.Broadcast), OnReceiveMessage);
			hubConnection.On<List<string>>(nameof(ChatHub.MembersChange), OnMembersChanged);
			await hubConnection.StartAsync();

			if (!await hubConnection.InvokeAsync<bool>(nameof(ChatHub.Register), CurrentUsername))
			{
				errorMessage = $"{CurrentUsername} has already joined this chat room. Please use another name.";
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
		MessageReceived?.Invoke(message);
	}

	private void OnMembersChanged(List<string> list)
	{
		MembersChanged?.Invoke(list);
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
