using MudBlazor;
using MudBlazorApp.Components.Pages.CurrencyConverter.DTOs;

namespace MudBlazorApp.Components.Pages.CurrencyConverter
{
	public class CurrencyConverterDataService(HttpClient httpClient, IConfiguration config, ISnackbar snackbar) : ICurrencyConverterDataService
	{
		readonly string apiUrl = config["ExchangeRateApi:BaseUrl"]!;

		public CurrencyType[]? SupportedCurrencies => supportedCurrencies ??= GetSupportedCurrencies();
		CurrencyType[]? supportedCurrencies = null;

		Dictionary<(CurrencyType?, CurrencyType?, DateTime?), decimal?> exchangeRateCache = new();

		CurrencyType[] GetSupportedCurrencies()
		{
			try
			{
				return httpClient.GetFromJsonAsync<CurrencyType[]>($"{apiUrl}/currencies").Result;
			}
			catch
			{
				snackbar?.Add("Error fetching currency list from public API", Severity.Error, config => config.VisibleStateDuration = int.MaxValue);
			}
			return [];
		}

		public async Task<decimal?> GetExchangeRate(CurrencyType? fromCurrency, CurrencyType? toCurrency, DateTime? exchangeRateDate)
		{
			decimal? result = null;

			if (fromCurrency != null && toCurrency != null)
			{
				if (fromCurrency == toCurrency)
				{
					result = 1m;
				}
				else if (exchangeRateDate != null)
				{
					var key = (fromCurrency, toCurrency, exchangeRateDate);
					if (exchangeRateCache.ContainsKey(key))
					{
						result = exchangeRateCache[key];
					}
					else
					{
						try
						{
							var rates = await httpClient.GetFromJsonAsync<ExchangeRate[]>($"{apiUrl}/rates?base={fromCurrency}&quotes={toCurrency}&date={exchangeRateDate:yyyy-MM-dd}");
							result = rates?.FirstOrDefault()?.rate;
							exchangeRateCache[key] = result;
						}
						catch
						{
							snackbar?.Add("Error fetching exchange rate from public API", Severity.Error, config => config.VisibleStateDuration = int.MaxValue);
						}
					}
				}
			}

			return result;
		}
	}
}
