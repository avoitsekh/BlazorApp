namespace MudBlazorApp.Components.Pages.CurrencyConverter.DTOs;

public class CurrencyType
{
	public string iso_code { get; set; }
	public string iso_numeric { get; set; }
	public string name { get; set; }
	public string symbol { get; set; }
	public string start_date { get; set; }
	public string end_date { get; set; }

	public override string ToString()
	{
		return iso_code;
	}

	public override bool Equals(object? obj)
	{
		return obj is CurrencyType && ToString().Equals(obj.ToString());
	}

	public static bool operator ==(CurrencyType? x, CurrencyType? y)
	{
		return Equals(x, y);
	}

	public static bool operator !=(CurrencyType? x, CurrencyType? y)
	{
		return !Equals(x, y);
	}
}
