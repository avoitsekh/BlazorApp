using System.Text.Json.Serialization;

namespace BlazorApp.Components.Pages.MortgageCalculator;

public sealed class MortgageDetails
{
	public double LoanAmount = 300000D;
	public InterestCollection InterestRates = new(5.49D);
	public int AmortizationPeriodInYears = 30;                          // term
	public DateTime AdvanceDate = DateTime.Today;						// mortgage start date
	public int CompoundPeriod = 2;										// cp - interest compound period
	public int PaymentFrequency = 12;									// ppy - payments per year
	public InterestAccrualMethods InterestAccrualMethod = InterestAccrualMethods.PerPayment;
	public FinancialYears FinancialYear = FinancialYears._365or366;
	public InterestRateTypes InterestRateType = InterestRateTypes.ARM;
	public ExtraPaymentCollection ExtraPayments = new();

	public enum InterestRateTypes { Fixed, ARM, VRM }
	public enum InterestAccrualMethods { PerDay, PerPayment }
	public enum FinancialYears { _365or366, _365, _360 }

	[JsonIgnore]
	public bool IsVariableRateMortgage => InterestRateType != InterestRateTypes.Fixed;

	[JsonIgnore]
	public int PaymentCount => PaymentFrequency * AmortizationPeriodInYears;    // nper

}
