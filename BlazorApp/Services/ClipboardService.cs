using Microsoft.JSInterop;
using MudBlazor;

namespace BlazorApp.Services;

public class ClipboardService(IJSRuntime jsRuntime, ISnackbar snackbar)
{
	public void Write<T>(T value, string message = "Copied to clipboard")
	{
		jsRuntime?.InvokeVoidAsync("mudWindow.copyToClipboard", value);
		snackbar?.Add(message, Severity.Info);
	}
}
