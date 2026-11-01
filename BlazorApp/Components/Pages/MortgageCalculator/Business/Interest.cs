using System.Text.Json.Serialization;

namespace BlazorApp.Components.Pages.MortgageCalculator;

public sealed class Interest
{
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public DateTime? EffectiveDate { get; set; }

	public double Rate { get; set => field = Math.Round(value, 2); }

	[JsonIgnore]
	public bool IsInitial => !EffectiveDate.HasValue;
}
