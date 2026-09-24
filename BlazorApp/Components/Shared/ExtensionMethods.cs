namespace BlazorApp.Components.Shared;

public static class ExtensionMethods
{
	public static Action Debounce(this Action action, int milliseconds)
	{
		CancellationTokenSource lastToken = null;

		return () =>
		{
			lastToken?.Cancel();

			try
			{
				lastToken?.Dispose();
			}
			catch
			{
			}

			var token = lastToken = new CancellationTokenSource();
			Task.Delay(milliseconds).ContinueWith(task => action(), token.Token);
		};
	}
}
