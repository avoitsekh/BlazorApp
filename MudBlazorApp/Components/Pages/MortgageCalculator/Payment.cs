namespace MudBlazorApp.Components.Pages.MortgageCalculator;

public class Payment
{
	public int Number { get; set; }
	public DateTime PaymentDate { get; set; }
	public int? Year { get; set; }
	public string PaymentPeriodInterestRate { get; set; }
	public string InterestAccrualPeriod { get; set; }
	public double InterestAmount { get; set; }
	public double PrincipalAmount { get; set; }
	public double PaymentAmount { get; set; }
	public double Balance { get; set; }
}
