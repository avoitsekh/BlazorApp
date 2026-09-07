namespace MudBlazorApp.Components.Pages.MortgageCalculator;

public sealed class Payment
{
	[Label(Constants.No)]
	public int Number;

	[Label(Constants.PaymentDate)]
	public DateTime PaymentDate;

	[Label(Constants.Year)]
	public int? Year;

	[Label(Constants.Interest)]
	public string IterestRate;

	[Label(Constants.InterestAccrualPeriod)]
	public string InterestAccrualPeriod;

	[Label(Constants.InterestAmount)]
	public double InterestAmount;

	[Label(Constants.PrincipalAmount)]
	public double PrincipalAmount;

	[Label(Constants.ExtraAmount)]
	public double ExtraAmount;

	[Label(Constants.PaymentAmount)]
	public double PaymentAmount;

	[Label(Constants.RemainingBalance)]
	public double Balance;
}
