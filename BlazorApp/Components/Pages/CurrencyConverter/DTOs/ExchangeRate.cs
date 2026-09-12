namespace BlazorApp.Components.Pages.CurrencyConverter.DTOs;

public class ExchangeRate
{
	public string date { get; set; }
	public string @base { get; set; }
	public string quote { get; set; }
	public decimal rate { get; set; }
}
