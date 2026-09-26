using static BlazorApp.Components.Pages.MortgageCalculator.MortgageDetails;

namespace BlazorApp.Components.Pages.MortgageCalculator;

public static class Constants
{
	#region Filenames

	public const string MortgageDetailsSettingsFilename = "mortgage-details.json";
	public const string AmortizationScheduleExcelFilename = "amortization-schedule.xlsx";

	#endregion

	#region Dropdowns

	public static Dictionary<string, InterestRateTypes> InterestRateTypeValues = new()
	{
		{ "Fixed", InterestRateTypes.Fixed },
		{ "ARM", InterestRateTypes.ARM },
		{ "VRM", InterestRateTypes.VRM },
	};

	public static Dictionary<string, InterestAccrualMethods> InterestRateAccrualMethodValues = new()
	{
		{ "Per Day", InterestAccrualMethods.PerDay },
		{ "Per Payment", InterestAccrualMethods.PerPayment },
	};

	public static Dictionary<string, FinancialYears> FinancialYearValues = new()
	{
		{ "365 or 366 days", FinancialYears._365or366 },
		{ "365 days", FinancialYears._365 },
		{ "360 days", FinancialYears._360 },
	};

	public static Dictionary<string, int> CompoundPeriodValues = new()
	{
		{ "Semi-Annually", 2 },
		{ "Montly", 12 },
		{ "None", 0 },
	};

	public static Dictionary<string, int> PaymentFrequencyValues = new()
	{
		{ "Montly", 12 },
		{ "Semi-Monthly", 24 },
		{ "Weekly", 52 },
		{ "Bi-Weekly", 26 },
	};

	#endregion

	#region Labels

	public const string LoanAmount = "Loan Amount";
	public const string AnnualPercent = "Annual %";
	public const string AmortizationPeriod = "Amortization Period";
	public const string AdvanceDate = "Advance Date";
	public const string CompoundPeriod = "Compound Period";
	public const string PaymentFrequency = "Payment Frequency";
	public const string InterestAccrualMethod = "Interest Accrual Method";
	public const string FinancialYear = "Financial Year";
	public const string RateType = "Rate Type";
	public const string ExtraPayments = "Extra Payments";

	public const string EffectiveDate = "Effective Date";

	public const string Amount = "Amount";



	public const string BeginAtPaymentNo = "Begin at Payment No.";
	public const string EndAtPaymentNo = "End at Payment No.";

	//public const string XXXXXXXXX = "YYYYYYYYY";
	//public const string XXXXXXXXX = "YYYYYYYYY";
	//public const string XXXXXXXXX = "YYYYYYYYY";
	//public const string XXXXXXXXX = "YYYYYYYYY";
	//public const string XXXXXXXXX = "YYYYYYYYY";
	//public const string XXXXXXXXX = "YYYYYYYYY";
	//public const string XXXXXXXXX = "YYYYYYYYY";
	//public const string XXXXXXXXX = "YYYYYYYYY";
	//public const string XXXXXXXXX = "YYYYYYYYY";
	//public const string XXXXXXXXX = "YYYYYYYYY";

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
