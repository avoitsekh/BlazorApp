using System.Reflection.Metadata.Ecma335;

namespace MudBlazorApp.Components.Pages.MortgageCalculator;

public partial class MortgageSettings
{
	private string ValidateLoanAmountCore(double arg)
	{
		return arg switch
		{
			0D => "Cannot be 0",
			< 1000D => "Cannot be less than 1,000",
			_ => string.Empty
		};

		//return "Cannot be less than 1,000";
		// var result = Validate(arg);
		// if (result.IsValid)
		// 	return new string[0];
		// return result.Errors.Select(e => e.ErrorMessage);
	}

	//public Func<double, string> ValidateLoanAmount => ValidateLoanAmountCore;
	public Func<double, string> ValidateLoanAmount => x => x switch
	{
		0D => "Value cannot be 0",
		< 1000D => "Value cannot be less than 1,000",
		_ => string.Empty
	};

	public Func<double, string> ValidateInterestRate => x => x switch
	{
		0D => "Value cannot be 0",
		< 0D => "Value cannot be less than 0",
		_ => string.Empty
	};
	public Func<DateTime, string> ValidateAdvanceDate => x =>
	{
		return "Test Error 123";
	};
}
