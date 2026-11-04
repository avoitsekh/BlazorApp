using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorApp.Components.Pages.ChatRoom;

public sealed class ChatClient : IDisposable
{
	public event EventHandler? Joining;
	public event EventHandler? Joined;
	public event EventHandler<Message>? MessageReceived;
	public event EventHandler<List<string>>? MembersChanged;
	public event EventHandler<string>? Error;

	HubConnection? _hubConnection;
	TimeZoneInfo? _timeZone;

	public string? Username { get; private set; }

	public bool IsJoined { get; private set; }

	public bool IsJoining { get; private set; }

	public IReadOnlyList<string> Members => _members;
	List<string> _members = new List<string>(10);

	public IReadOnlyList<Message> Messages => _messages;
	List<Message> _messages = new List<Message>(40);

	public async Task Join(string username, string hubUrl, TimeZoneInfo? timeZone = default)
	{
		_timeZone = timeZone;
		string? errorMessage = null;

		try
		{
			Username = username?.Trim();
			if ((errorMessage = ValidateUsername(Username!)) != null)
			{
				return;
			}

			OnJoining();

			await Task.Delay(60);

			_hubConnection = new HubConnectionBuilder()
				.WithUrl(hubUrl)
				//.WithKeepAliveInterval(TimeSpan.FromSeconds(1000))
				.WithAutomaticReconnect([TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(30)])
				.Build();

			_hubConnection.On<Message>(nameof(ChatHub.Broadcast), OnReceiveMessage);
			_hubConnection.On<List<string>>(nameof(ChatHub.MembersChange), OnMembersChanged);

			_hubConnection.Reconnected += s => RegisterNewMember(Username!);
			//hubConnection.Closed += e => OnError(e?.Message);

			await _hubConnection.StartAsync();

			if (!await RegisterNewMember(Username!))
			{
				errorMessage = $"{Username} has already joined this chat room. Please use another name.";
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
		IsJoining = IsJoined = false;
		Username = null;
		_members.Clear();
		_messages.Clear();

		if (_hubConnection != null)
		{
			await _hubConnection.DisposeAsync();
			_hubConnection = null;
		}
	}

	public async Task SendMessage(Message message)
	{
		if (_hubConnection?.State == HubConnectionState.Connected)
		{
			message.Owner = Username;
			await _hubConnection.SendAsync(nameof(ChatHub.Broadcast), message);
		}
	}

	async Task<bool> RegisterNewMember(string username)
	{
		return await _hubConnection!.InvokeAsync<bool>(nameof(ChatHub.Register), Username);
	}

	void OnReceiveMessage(Message message)
	{
		message.IsMine = message.Owner?.Equals(Username, StringComparison.OrdinalIgnoreCase) ?? false;
		message.Received = ConvertToLocalDateTime(DateTime.UtcNow);

		_messages.Add(message);
		MessageReceived?.Invoke(this, message);
	}

	void OnMembersChanged(List<string> list)
	{
		_members.Clear();
		_members.AddRange(list);

		MembersChanged?.Invoke(this, list);
	}

	void OnJoining()
	{
		_members.Clear();
		_messages.Clear();

		IsJoining = true;
		IsJoined = false;

		Joining?.Invoke(this, EventArgs.Empty);
	}

	void OnJoined()
	{
		IsJoining = false;
		IsJoined = true;

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

	DateTime ConvertToLocalDateTime(DateTime utcDate)
	{
		return _timeZone != null ? TimeZoneInfo.ConvertTimeFromUtc(utcDate, _timeZone) : utcDate;
	}

	string? ValidateUsername(string username)
	{
		string? errorMessage = null;

		if (string.IsNullOrWhiteSpace(username))
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
			errorMessage = "Invalid username. Consider having username with only 'Firstname', 'Firstname Lastname', or 'Firstname Middlename Lastname'.";
		}

		return errorMessage;
	}
}
