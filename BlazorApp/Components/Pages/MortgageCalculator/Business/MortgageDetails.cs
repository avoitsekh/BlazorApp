using System.Text.Json.Serialization;

namespace BlazorApp.Components.Pages.MortgageCalculator;

public sealed class MortgageDetails : IRoundable
{
	public double LoanAmount { get; set => field = this.Round(value); } = 300000D;
	
	public InterestCollection InterestRates { get; set; } = new(5.49D);
	
	public int AmortizationPeriod { get; set; } = 30;						// term
	
	public DateTime AdvanceDate { get; set; } = DateTime.Today;						// mortgage start date
	
	public int CompoundPeriod { get; set; } = 2;									// cp - interest compound period
	
	public int PaymentFrequency { get; set; } = 12;									// ppy - payments per year
	
	public InterestAccrualMethods InterestAccrualMethod { get; set; } = InterestAccrualMethods.PerPayment;
	
	public FinancialYears FinancialYear { get; set; } = FinancialYears._365or366;
	
	public InterestRateTypes InterestRateType { get; set; } = InterestRateTypes.ARM;
	
	public ExtraPaymentCollection ExtraPayments { get; set; } = new();

	[JsonIgnore]
	public bool IsVariableRateMortgage => InterestRateType != InterestRateTypes.Fixed;

	[JsonIgnore]
	public int PaymentCount => PaymentFrequency * AmortizationPeriod;        // nper

	public enum InterestRateTypes { Fixed, ARM, VRM }
	public enum InterestAccrualMethods { PerDay, PerPayment }
	public enum FinancialYears { _365or366, _365, _360 }

	public static string ConditionalFormat(double value)
	{
		return value % 1D == 0D ? "N0" : "N2";
	}

}
