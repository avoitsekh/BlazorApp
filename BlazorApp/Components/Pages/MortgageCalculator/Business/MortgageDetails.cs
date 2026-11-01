namespace BlazorApp.Components.Pages.MortgageCalculator;

public sealed class MortgageDetails
{
	public double LoanAmount { get; set => field = Math.Round(value, 2); } = 300000D;
	public InterestCollection InterestRates { get; set; } = new(5.49D);
	public int AmortizationPeriod { get; set; } = 30;								// term
	public DateTime AdvanceDate { get; set; } = DateTime.Today;						// mortgage start date
	public int CompoundPeriod { get; set; } = 2;									// cp - interest compound period
	public int PaymentFrequency { get; set; } = 12;									// ppy - payments per year
	public InterestAccrualMethods InterestAccrualMethod { get; set; } = InterestAccrualMethods.PerPayment;
	public FiscalYears FiscalYear { get; set; } = FiscalYears._365or366;
	public InterestRateTypes InterestRateType { get; set; } = InterestRateTypes.ARM;
	public ExtraPaymentCollection ExtraPayments { get; set; } = new();

	public bool IsFixed => InterestRateType == InterestRateTypes.Fixed;
	public bool IsARM => InterestRateType == InterestRateTypes.ARM;
	public bool IsVRM => InterestRateType == InterestRateTypes.VRM;
	public int PaymentCount => PaymentFrequency * AmortizationPeriod;				// nper

	public enum InterestRateTypes { Fixed, ARM, VRM }
	public enum InterestAccrualMethods { PerDay, PerPayment }
	public enum FiscalYears { _365or366, _365, _360 }

	public static string ConditionalFormat(double value)
	{
		return value % 1D == 0D ? "N0" : "N2";
	}

}
