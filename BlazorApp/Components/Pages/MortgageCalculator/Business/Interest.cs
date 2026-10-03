using System.Text.Json.Serialization;

namespace BlazorApp.Components.Pages.MortgageCalculator;

public sealed class Interest : IRoundable
{
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public DateTime? EffectiveDate { get; set; }

	public double Rate { get; set => field = this.Round(value); }

	[JsonIgnore]
	public bool IsInitial => !EffectiveDate.HasValue;
}
