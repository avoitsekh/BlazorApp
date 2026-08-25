namespace MudBlazorApp.Components.Pages.MortgageCalculator;

public static class Constants
{

	public static Dictionary<string, MortgageSettings.InterestRateTypes> InterestRateType = new()
	{
		{ "Fixed Rate", MortgageSettings.InterestRateTypes.Fixed },
		{ "Adjustable Rate Mortgage (ARM)", MortgageSettings.InterestRateTypes.ARM },
	};

	public static Dictionary<string, int> CompoundPeriod = new()
	{
		{ "Semi-Annually", 2 },
		{ "Montly", 12 },
	};

	public static Dictionary<string, int> PaymentFrequency = new()
	{
		{ "Montly", 12 },
		{ "Semi-Monthly", 24 },
		{ "Weekly", 52 },
		{ "Bi-Weekly", 26 },
	};
}
