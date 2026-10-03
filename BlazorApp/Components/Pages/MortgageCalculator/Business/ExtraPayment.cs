using System.Text.Json.Serialization;

namespace BlazorApp.Components.Pages.MortgageCalculator;

public sealed class ExtraPayment : IRoundable
{
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public int? FromPayment { get; set; }

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public int? ToPayment { get; set; }

	public double Amount { get; set => field = this.Round(value); }

	[JsonIgnore]
	public bool IsLumpSum => FromPayment.HasValue && ToPayment.HasValue && FromPayment == ToPayment;

	[JsonIgnore]
	public bool IsRecurring => FromPayment.HasValue && (!ToPayment.HasValue || FromPayment < ToPayment);

}
