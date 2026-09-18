using Microsoft.JSInterop;
using System.Text;

namespace BlazorApp.Services;

public sealed class JSExtensionsService(IJSRuntime JS)
{
	public async Task DownloadFileAsync(string filename, string contentType, byte[] bytes)
	{
		await JS.InvokeVoidAsync("downloadFile", filename, contentType, bytes);
	}

	public async Task CopyToClipboardAsync(string text)
	{
		await CopyToClipboardAsync("text/plain", Encoding.UTF8.GetBytes(text));
	}

	public async Task CopyToClipboardAsync(string contentType, byte[] bytes)
	{
		await JS.InvokeVoidAsync("copyToClipboard", contentType, bytes);
	}

	public async Task<TimeZoneInfo?> GetBrowserTimeZoneAsync()
	{
		var timeZoneId = await JS.InvokeAsync<string>("getBrowserTimeZone");

		if (TimeZoneInfo.TryFindSystemTimeZoneById(timeZoneId, out var timeZoneInfo))
		{
			return timeZoneInfo;
		}

		return null;
	}
}
