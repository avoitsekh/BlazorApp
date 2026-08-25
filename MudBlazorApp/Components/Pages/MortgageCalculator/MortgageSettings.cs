namespace MudBlazorApp.Components.Pages.MortgageCalculator;

public partial class MortgageSettings
{
	public double LoanAmount = 320000D;
	//public double AnnualRate = 2.49D;
	public int AmortizationPeriodInYears = 25;							// term
	public DateTime? AdvanceDate = new DateTime(2025, 01, 01);          // mortgage start date
	public int CompoundPeriod = 2;										// cp - interest compound period
	public int PaymentFrequency = 12;                                   // ppy - payments per year
	// TODO: add: public enum InterestAccrualMethod = per day or per payment
	public Interests InterestRates;
	public InterestRateTypes InterestRateType = InterestRateTypes.ARM;

	public enum InterestRateTypes { Fixed, ARM }


	public MortgageSettings()
	{
		InterestRates = new Interests(2.49D);

		//InterestRates = new Interests(AnnualRate)
		//{
		//	{ new DateTime(2025, 04, 19), 8.5D },
		//	{ new DateTime(2026, 10, 08), 6.35D },
		//	{ new DateTime(2028, 10, 08), 7D }
		//};
	}


}
