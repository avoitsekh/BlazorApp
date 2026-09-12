using System.Text.Json.Serialization;

namespace BlazorApp.Components.Pages.MortgageCalculator;

public sealed class Interest
{
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public DateTime? EffectiveDate;

	public double Rate;

	[JsonIgnore]
	public bool IsInitial => !EffectiveDate.HasValue;
}
