namespace BlazorApp.Components.Pages.MortgageCalculator;

public static class MortgageDetailsValidation
{
	public static Func<double, string> ValidateLoanAmount => amount => amount switch
	{
		< 1000D => "Value cannot be less than 1,000",
		_ => string.Empty
	};

	public static Func<double, string> ValidateInterestRate => rate => rate switch
	{
		0D => "Value cannot be 0",
		_ => string.Empty
	};

	public static Func<int, string> ValidateAmortizationPeriodInYears => years => years switch
	{
		0 => "Value cannot be 0",
		_ => string.Empty
	};

	public static Func<DateTime?, string> ValidateAdvanceDate => date =>
		!date.HasValue || date == DateTime.MinValue ? "Value cannot be empty" : string.Empty;
}
