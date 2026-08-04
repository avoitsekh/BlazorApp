namespace MudBlazorApp.Components.Pages.MortgageCalculator
{
	public class Payment
	{
		public int Number { get; set; }
		public DateTime PaymentDate { get; set; }
		public int? Year { get; set; }
		public double AnnualInterestRate { get; set; }
		public double PaymentAmount { get; set; }
		// public double? ExtraPayment { get; set; }
		// public double? AdditionalPayment { get; set; }
		public double InterestPaid { get; set; }
		public double PrincipalPaid { get; set; }
		public double Balance { get; set; }
	}
}
