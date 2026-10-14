namespace BlazorApp.Components.Pages.ChatRoom;

public sealed class ChatMessage
{
	public ChatMessage(string? body = default, string? username = default)
	{
		Body = body;
		Username = username;
	}

	public string? Username { get; set; }
	
	public string? Body { get; set; }

	public bool IsMine { get; set; }

	public bool IsNotification { get; set; }

	public bool IsEmpty => string.IsNullOrWhiteSpace(Body);

	public void Clear()
	{
		Body = null;
	}
}
