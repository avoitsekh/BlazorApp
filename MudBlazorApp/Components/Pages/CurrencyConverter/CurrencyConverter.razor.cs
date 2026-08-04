using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using MudBlazorApp.Components.Pages.CurrencyConverter.DTOs;
using MudBlazorApp.Services;

namespace MudBlazorApp.Components.Pages.CurrencyConverter
{
	public partial class CurrencyConverter
	{
		[Inject]
		ICurrencyConverterDataService DataService { get; set; } = default!;

		[Inject]
		CurrencyConverterState State { get; set; } = default!;

		[Inject]
		ClipboardService clipboardService { get; set; } = default!;

		bool isProcessing = false;

		async void OnValidSubmit(EditContext context)
		{
			try
			{
				isProcessing = true;
				await Task.Delay(150);
				var exchangeRate = await DataService.GetExchangeRate(State.FromCurrency, State.ToCurrency, State.ExchangeRateDate);
				State.Calculate(exchangeRate);
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
			var currencies = DataService.SupportedCurrencies;
			return string.IsNullOrWhiteSpace(text) ? currencies : currencies.Where(x => x.iso_code.StartsWith(text, StringComparison.InvariantCultureIgnoreCase) || x.name.Contains(text, StringComparison.InvariantCultureIgnoreCase));
		}

		async Task OnFromAmountInput(ChangeEventArgs args)
		{
			State.ResetToAmount();
		}

		void CopyToClipboard(decimal? amount)
		{
			clipboardService.Write(amount, "Amount copied to clipboard");
		}

		string Pluralize(string currencyName, decimal? amount)
		{
			if (amount > 1m && !string.IsNullOrWhiteSpace(currencyName))
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
	}
}
