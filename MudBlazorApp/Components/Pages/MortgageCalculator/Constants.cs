namespace MudBlazorApp.Components.Pages.MortgageCalculator;

public static class Constants
{
	public static Dictionary<string, MortgageDetails.InterestRateTypes> InterestRateType = new()
	{
		{ "Fixed", MortgageDetails.InterestRateTypes.Fixed },
		{ "ARM - Adjustable Rate Mortgage", MortgageDetails.InterestRateTypes.ARM },
	};

	public static Dictionary<string, MortgageDetails.InterestRateAccrualMethods> InterestRateAccrualMethod = new()
	{
		{ "Per Day", MortgageDetails.InterestRateAccrualMethods.PerDay },
		{ "Per Payment", MortgageDetails.InterestRateAccrualMethods.PerPayment },
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
