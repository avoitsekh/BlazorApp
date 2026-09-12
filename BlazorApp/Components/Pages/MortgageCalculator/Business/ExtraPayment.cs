using System.Text.Json.Serialization;

namespace MudBlazorApp.Components.Pages.MortgageCalculator;

public sealed class ExtraPayment
{
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public int? FromPayment;

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public int? ToPayment;

	public double Amount;

	[JsonIgnore]
	public bool IsLumpSum => FromPayment.HasValue && ToPayment.HasValue && FromPayment == ToPayment;

	[JsonIgnore]
	public bool IsRecurring => FromPayment.HasValue && (!ToPayment.HasValue || FromPayment < ToPayment);

}
