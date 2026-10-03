
namespace BlazorApp.Components.Pages.MortgageCalculator;

public sealed class Payment : IRoundable
{
	public int Number { get; set; }
	
	public DateTime PaymentDate { get; set; }

	public int? Year { get; set; }
	
	public string? Interest { get; set; }
	
	public string? InterestAccrualPeriod { get; set; }
	
	public double InterestAmount { get; set => field = this.Round(value); }
	
	public double PrincipalAmount { get; set => field = this.Round(value); }
	
	public double ExtraAmount { get; set => field = this.Round(value); }
	
	public double PaymentAmount { get; set => field = this.Round(value); }
	
	public double Balance { get; set => field = this.Round(value); }

}
