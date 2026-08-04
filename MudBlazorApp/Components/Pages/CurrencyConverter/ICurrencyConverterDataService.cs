using MudBlazorApp.Components.Pages.CurrencyConverter.DTOs;

namespace MudBlazorApp.Components.Pages.CurrencyConverter
{
	public interface ICurrencyConverterDataService
	{
		CurrencyType[] SupportedCurrencies { get; }

		Task<decimal?> GetExchangeRate(CurrencyType? fromCurrency, CurrencyType? toCurrency, DateTime? exchangeRateDate);
	}
}
