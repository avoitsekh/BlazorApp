using MudBlazorApp.Components.Pages.CurrencyConverter.DTOs;

namespace MudBlazorApp.Components.Pages.CurrencyConverter;

public class CurrencyConverterDataService(HttpClient httpClient, IConfiguration config)
{
	readonly string apiUrl = config["ExchangeRateApi:BaseUrl"]!;
	readonly Dictionary<(CurrencyType?, CurrencyType?, DateTime?), decimal?> exchangeRateCache = new();

	public async Task<CurrencyType[]> GetSupportedCurrenciesAsync()
	{
		return await httpClient.GetFromJsonAsync<CurrencyType[]>($"{apiUrl}/currencies");
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
					var rates = await httpClient.GetFromJsonAsync<ExchangeRate[]>($"{apiUrl}/rates?base={fromCurrency}&quotes={toCurrency}&date={exchangeRateDate:yyyy-MM-dd}");
					result = rates?.FirstOrDefault()?.rate;
					exchangeRateCache[key] = result;
				}
			}
		}

		return result;
	}
}
