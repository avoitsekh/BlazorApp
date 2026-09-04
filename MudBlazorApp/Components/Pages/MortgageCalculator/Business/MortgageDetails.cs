using System.Text.Json;

namespace MudBlazorApp.Components.Pages.MortgageCalculator;

public class MortgageDetails
{
	public double LoanAmount = 320000D;
	public InterestCollection InterestRates = new(2.49D);
	public int AmortizationPeriodInYears = 25;							// term
	public DateTime? AdvanceDate = new DateTime(2025, 01, 01);          // mortgage start date
	public int CompoundPeriod = 2;										// cp - interest compound period
	public int PaymentFrequency = 12;                                   // ppy - payments per year
	public InterestRateTypes InterestRateType = InterestRateTypes.ARM;
	public InterestAccrualMethods InterestAccrualMethod = InterestAccrualMethods.PerDay;
	public ExtraPaymentCollection ExtraPayments = new();

	public enum InterestRateTypes { Fixed, ARM }
	public enum InterestAccrualMethods { PerDay, PerPayment }

	#region JSON Import/Export

	public static string ToJsonString(MortgageDetails details)
	{
		return JsonSerializer.Serialize(details, new JsonSerializerOptions { IncludeFields = true, WriteIndented = true });
	}

	public static MortgageDetails FromJsonString(string jsonString)
	{
		try
		{
			return JsonSerializer.Deserialize<MortgageDetails>(jsonString, new JsonSerializerOptions { IncludeFields = true });
		}
		catch
		{
			throw new Exception("File appears to be corrupted or invalid.");
		}
	}

	#endregion

}
