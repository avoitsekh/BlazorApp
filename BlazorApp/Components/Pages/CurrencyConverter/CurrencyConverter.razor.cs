using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using BlazorApp.Components.Pages.CurrencyConverter.DTOs;
using BlazorApp.Services;
using System.Globalization;

namespace BlazorApp.Components.Pages.CurrencyConverter;

public partial class CurrencyConverter
{
	[Inject]
	CurrencyConverterDataService DataService { get; set; } = default!;

	[Inject]
	CurrencyConverterState State { get; set; } = default!;

	[Inject]
	JSExtensionsService JS { get; set; } = default!;

	[Inject]
	NavigationManager NavigationManager { get; set; } = default!;

	[Inject]
	ISnackbar Snackbar { get; set; } = default!;


	[SupplyParameterFromQuery(Name = "from")]
	public string? FromCurrency { get; set; }

	[SupplyParameterFromQuery(Name = "to")]
	public string? ToCurrency { get; set; }

	[SupplyParameterFromQuery()]
	public string? Date { get; set; }

	[SupplyParameterFromQuery()]
	public string? Amount { get; set; }


	MudMessageBox messageBox = default!;
	bool isProcessing = false;
	string? exampleUri;

	protected async override Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			var uriQueryString = new Dictionary<string, object?>
			{
				{ "from", "CAD" },
				{ "to", "USD" },
				{ "date", DateTime.Today.ToString("yyyy-MM-dd") },
				{ "amount", 123.45 },
			};
			exampleUri = NavigationManager.GetUriWithQueryParameters(uriQueryString);

			var hasQueryString = !string.IsNullOrWhiteSpace(FromCurrency) ||
				!string.IsNullOrWhiteSpace(ToCurrency) ||
				!string.IsNullOrWhiteSpace(Date) ||
				!string.IsNullOrWhiteSpace(Amount);

			if (!hasQueryString)
			{
				// Prefetch supported currencies
				await GetSupportedCurrenciesAsync();
			}
			else
			{
				if (!string.IsNullOrWhiteSpace(FromCurrency))
				{
					await GetSupportedCurrenciesAsync();
					State.FromCurrency = State.SupportedCurrencies.FirstOrDefault(x => x.iso_code == FromCurrency);
				}
				if (!string.IsNullOrWhiteSpace(ToCurrency))
				{
					await GetSupportedCurrenciesAsync();
					State.ToCurrency = State.SupportedCurrencies.FirstOrDefault(x => x.iso_code == ToCurrency);
				}
				if (!string.IsNullOrWhiteSpace(Date) && DateTime.TryParseExact(Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
				{
					State.ExchangeRateDate = date;
				}
				if (!string.IsNullOrWhiteSpace(Amount) && decimal.TryParse(Amount, out var amount))
				{
					State.FromAmount = amount;
				}

				StateHasChanged();
			}
		}
	}

	async void OnValidSubmit(EditContext context)
	{
		try
		{
			isProcessing = true;
			await Task.Delay(150);
			var exchangeRate = await DataService.GetExchangeRate(State.FromCurrency, State.ToCurrency, State.ExchangeRateDate);
			State.Calculate(exchangeRate);
		}
		catch
		{
			Snackbar?.Add("Error fetching exchange rate from public API", Severity.Error, config => config.VisibleStateDuration = int.MaxValue);
		}
		finally
		{
			isProcessing = false;
			StateHasChanged();
		}
	}

	async Task<IEnumerable<CurrencyType>> SearchCurrencyDropDown(string text, CancellationToken token)
	{
		await Task.Delay(50, token);
		return string.IsNullOrWhiteSpace(text) ? State.SupportedCurrencies : State.SupportedCurrencies.Where(x => x.iso_code.StartsWith(text, StringComparison.InvariantCultureIgnoreCase) || x.name.Contains(text, StringComparison.InvariantCultureIgnoreCase));
	}

	async Task OnFromAmountInput(ChangeEventArgs args)
	{
		State.ResetToAmount();
	}

	async Task CopyToClipboard(decimal? amount)
	{
		await JS.CopyToClipboardAsync(amount.ToString());
		Snackbar?.Add("Amount copied to clipboard", Severity.Info);
	}

	string Pluralize(string currencyName, decimal? amount)
	{
		if (!string.IsNullOrWhiteSpace(currencyName) && amount > 1m)
		{
			if (currencyName.EndsWith("ch") ||
				currencyName.EndsWith("s") ||
				currencyName.EndsWith("sh") ||
				currencyName.EndsWith("ss") ||
				currencyName.EndsWith("x") ||
				currencyName.EndsWith("z"))
			{
				return currencyName + "es";
			}
			return currencyName + "s";
		}
		return currencyName;
	}

	async Task OnInformationClicked()
	{
		await messageBox.ShowAsync();
	}

	async Task GetSupportedCurrenciesAsync()
	{
		try
		{
			if (State.SupportedCurrencies == null)
			{
				State.SupportedCurrencies = await DataService.GetSupportedCurrenciesAsync();
			}
		}
		catch
		{
			Snackbar.Add("Error fetching currency list from public API", Severity.Error, config => config.VisibleStateDuration = int.MaxValue);
			State.SupportedCurrencies = [];
		}
	}

	string? GetHelperText(CurrencyType? currency)
	{
		return currency != null ? $"ISO code: {currency.iso_code}, symbol: {currency.symbol}" : null;
	}
}
