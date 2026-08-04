namespace MudBlazorApp.Components.Pages.MortgageCalculator
{
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
			0D => "Cannot be 0",
			< 1000D => "Cannot be less than 1,000",
			_ => string.Empty
		};
	}
}
