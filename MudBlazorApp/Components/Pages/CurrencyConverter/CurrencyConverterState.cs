using MudBlazorApp.Components.Pages.CurrencyConverter.DTOs;
using System.ComponentModel.DataAnnotations;

namespace MudBlazorApp.Components.Pages.CurrencyConverter
{
	public class CurrencyConverterState
	{

		[Required(ErrorMessage = "'From Currency' cannot be empty")]
		public CurrencyType? FromCurrency
		{
			get; set { field = value; ResetToAmount(); }
		}

		[Required(ErrorMessage = "'To Currency' cannot be empty")]
		public CurrencyType? ToCurrency
		{
			get; set { field = value; ResetToAmount(); }
		}

		[ExchangeRateDate(ErrorMessage = "'Exchange Rate Date' is not valid")]
		public DateTime? ExchangeRateDate
		{
			get { return AreCurrenciesEqual && !field.HasValue ? DateTime.Today : field; }
			set { field = value; ResetToAmount(); }
		}

		[Required(ErrorMessage = "'From Amount' cannot be empty")]
		public decimal? FromAmount
		{
			get; set { field = value; ResetToAmount(); }
		}

		public decimal? ToAmount { get; private set; }

		public bool AreCurrenciesEqual => FromCurrency != null && ToCurrency != null && FromCurrency == ToCurrency;

		public void Calculate(decimal? exchangeRate)
		{
			ToAmount = FromAmount * exchangeRate;
			if (ToAmount.HasValue)
			{
				ToAmount = Math.Round(ToAmount.Value, 2);
			}
		}

		public void ResetToAmount()
		{
			if (ToAmount.HasValue)
			{
				ToAmount = null;
			}
		}
	}

	class ExchangeRateDateAttribute : ValidationAttribute
	{
		public override bool IsValid(object? value)
		{
			if (value != null)
			{
				var a = Convert.ToDateTime(value);
				return a <= DateTime.Today;
			}

			return false;
		}
	}
}
