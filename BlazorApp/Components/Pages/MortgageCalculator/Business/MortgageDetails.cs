using System.Text.Json;
using System.Text.Json.Serialization;

namespace MudBlazorApp.Components.Pages.MortgageCalculator;

public sealed class MortgageDetails
{
	public double LoanAmount = 300000D;
	public InterestCollection InterestRates = new(5.49D);
	public int AmortizationPeriodInYears = 30;                          // term
	public DateTime AdvanceDate = DateTime.Today;						// mortgage start date
	public int CompoundPeriod = 2;										// cp - interest compound period
	public int PaymentFrequency = 12;									// ppy - payments per year
	public InterestAccrualMethods InterestAccrualMethod = InterestAccrualMethods.PerDay;
	public FinancialYears FinancialYear = FinancialYears._365or366;
	public InterestRateTypes InterestRateType = InterestRateTypes.ARM;
	public ExtraPaymentCollection ExtraPayments = new();

	public enum InterestRateTypes { Fixed, ARM }
	public enum InterestAccrualMethods { PerDay, PerPayment }
	public enum FinancialYears { _365or366, _365, _360 }

	[JsonIgnore]
	public bool IsVariableRateMortgage => InterestRateType != InterestRateTypes.Fixed;

	public string ValidateLoanAmount(double amount) => amount switch
	{
		< 1000D => "Value cannot be less than 1,000",
		_ => string.Empty
	};

	public string ValidateInterestRate(double rate) => rate switch
	{
		0D => "Value cannot be 0",
		_ => string.Empty
	};

	public string ValidateAmortizationPeriodInYears(int years) => years switch
	{
		0 => "Value cannot be 0",
		_ => string.Empty
	};

	public string ValidateAdvanceDate(DateTime? date)
	{
		if (!date.HasValue || date == DateTime.MinValue)
		{
			return "Value cannot be empty";
		}
		return string.Empty;
	}


	#region JSON Import/Export

	static JsonSerializerOptions options => field ??= new JsonSerializerOptions { IncludeFields = true, WriteIndented = true };

	public static string ToJsonString(MortgageDetails details)
	{
		return JsonSerializer.Serialize(details, options);
	}

	public static MortgageDetails FromJsonString(string jsonString)
	{
		try
		{
			return JsonSerializer.Deserialize<MortgageDetails>(jsonString, options);
		}
		catch
		{
			throw new Exception("File appears to be corrupted or invalid.");
		}
	}

	#endregion

}
