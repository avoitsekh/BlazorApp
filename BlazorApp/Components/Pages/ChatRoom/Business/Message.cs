namespace BlazorApp.Components.Pages.ChatRoom;

public sealed class Message
{
	public string? Owner;
	public string? Body;
	public bool IsMine;
	public bool IsNotification;

	public bool IsEmpty => string.IsNullOrWhiteSpace(Body);

	public void Clear()
	{
		Body = null;
	}
}
