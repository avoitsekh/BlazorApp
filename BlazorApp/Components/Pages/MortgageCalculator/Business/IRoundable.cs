namespace BlazorApp.Components.Pages.MortgageCalculator;

public interface IRoundable
{
}

public static class IRoundableExtensionMethods
{
	public static double Round(this IRoundable roundable, double value, int decimals = 2)
	{
		return Math.Round(value, decimals);
	}
}
