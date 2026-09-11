namespace MudBlazorApp.Components.Pages.MortgageCalculator;

public sealed class Payment
{
	public int Number;
	public DateTime PaymentDate;
	public int? Year;
	public string? IterestRate;
	public string? InterestAccrualPeriod;
	public double InterestAmount;
	public double PrincipalAmount;
	public double ExtraAmount;
	public double PaymentAmount;
	public double Balance;
}
