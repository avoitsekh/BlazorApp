using System.Text.Json;

namespace MudBlazorApp.Components.Pages.MortgageCalculator;

public class MortgageDetails
{
	public double LoanAmount = 320000D;
	public Interests InterestRates = new Interests(2.49D);
	public int AmortizationPeriodInYears = 25;							// term
	public DateTime? AdvanceDate = new DateTime(2025, 01, 01);          // mortgage start date
	public int CompoundPeriod = 2;										// cp - interest compound period
	public int PaymentFrequency = 12;                                   // ppy - payments per year
	public InterestRateTypes InterestRateType = InterestRateTypes.ARM;
	public InterestRateAccrualMethods InterestRateAccrualMethod = InterestRateAccrualMethods.PerDay;

	public enum InterestRateTypes { Fixed, ARM }
	public enum InterestRateAccrualMethods { PerDay, PerPayment }

	public string ToJsonString()
	{
		return JsonSerializer.Serialize(this, new JsonSerializerOptions { IncludeFields = true, WriteIndented = true });
	}

	public void FromJsonString(string jsonString)
	{
		var details = JsonSerializer.Deserialize<MortgageDetails>(jsonString);
		if (details is not null)
		{
			LoanAmount = details.LoanAmount;
			// ...
		}
	}

}
