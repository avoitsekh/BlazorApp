namespace MudBlazorApp.Components.Pages.MortgageCalculator;

public static class Constants
{
	#region Dropdowns

	public static Dictionary<string, MortgageDetails.InterestRateTypes> InterestRateType = new()
	{
		{ "Fixed", MortgageDetails.InterestRateTypes.Fixed },
		{ "ARM", MortgageDetails.InterestRateTypes.ARM },
	};

	public static Dictionary<string, MortgageDetails.InterestAccrualMethods> InterestRateAccrualMethod = new()
	{
		{ "Per Day", MortgageDetails.InterestAccrualMethods.PerDay },
		{ "Per Payment", MortgageDetails.InterestAccrualMethods.PerPayment },
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

	#endregion

	#region Labels
	#endregion

	#region Grid Headers

	public const string No = "No.";
	public const string PaymentDate = "Payment Date";
	public const string Year = "Year";
	public const string Interest = "Interest";
	public const string InterestAccrualPeriod = "Interest Accrual Period";
	public const string InterestAmount = "Interest Amount";
	public const string PrincipalAmount = "Principal Amount";
	public const string ExtraAmount = "Extra Amount";
	public const string PaymentAmount = "Payment Amount";
	public const string RemainingBalance = "Remaining Balance";

	#endregion
}
