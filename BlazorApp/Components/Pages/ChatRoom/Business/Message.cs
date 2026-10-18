namespace BlazorApp.Components.Pages.ChatRoom;

public sealed class Message
{
	public string? Owner { get; set; }
	public string? Body { get; set; }
	public bool IsMine { get; set; }
	public bool IsNotification { get; set; }

	public bool IsEmpty => string.IsNullOrWhiteSpace(Body);

	public void Clear()
	{
		Body = null;
	}
}
