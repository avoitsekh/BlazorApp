namespace BlazorApp.Components.Pages.MortgageCalculator;

public static class ExtensionMethods
{
	public static double SumAndRoundToCents<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
	{
		return PaymentCollection.RoundToCents(Enumerable.Sum<TSource>(source, selector));
	}

	public static bool HasData(this PaymentCollection? value)
	{
		return value != null && value.Count > 0;
	}

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
