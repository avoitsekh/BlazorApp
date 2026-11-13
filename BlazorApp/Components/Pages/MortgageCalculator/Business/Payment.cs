
namespace BlazorApp.Components.Pages.MortgageCalculator;

public sealed class Payment
{
	public int Number { get; set; }
	
	public DateTime PaymentDate { get; set; }

	public int? Year { get; set; }
	
	public string? Interest { get; set; }
	
	public string? PaymentPeriod { get; set; }
	
	public double InterestAmount { get; set => field = Math.Round(value, 2); }
	
	public double PrincipalAmount { get; set => field = Math.Round(value, 2); }
	
	public double ExtraAmount { get; set => field = Math.Round(value, 2); }
	
	public double PaymentAmount { get; set => field = Math.Round(value, 2); }
	
	public double Balance { get; set => field = Math.Round(value, 2); }

}
