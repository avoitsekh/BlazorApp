using static MudBlazorApp.Components.Pages.MortgageCalculator.MortgageDetails;

namespace MudBlazorApp.Components.Pages.MortgageCalculator;

public sealed class PresetCollection : Dictionary<string, Func<MortgageDetails>>
{
	public PresetCollection()
	{
		Add("Fixed Rate with Extra Payments", () => new()
		{
			LoanAmount = 300000D,
			InterestRates = new InterestCollection(8D),
			AmortizationPeriodInYears = 30,
			AdvanceDate = new DateTime(2023, 10, 03),
			CompoundPeriod = 2,
			PaymentFrequency = 24,
			InterestRateType = InterestRateTypes.Fixed,
			InterestAccrualMethod = InterestAccrualMethods.PerPayment,
			ExtraPayments = new ExtraPaymentCollection()
			{
				{ 59, 59, 64000D }
			},
		});

		Add("Variable Rate with Extra Payments", () => new()
		{
			LoanAmount = 700000D,
			InterestRates = new InterestCollection(4.5D)
			{
				{ new DateTime(2020, 10, 20), 5.5D },
				{ new DateTime(2021, 05, 07), 2.8D },
				{ new DateTime(2025, 09, 18), 6.8D },
			},
			AmortizationPeriodInYears = 25,
			AdvanceDate = new DateTime(2020, 06, 03),
			CompoundPeriod = 12,
			PaymentFrequency = 12,
			InterestRateType = InterestRateTypes.ARM,
			InterestAccrualMethod = InterestAccrualMethods.PerDay,
			ExtraPayments = new ExtraPaymentCollection()
			{
				{ 3, null, 300D },
				{ 201, 201, 100000D },
			},
		});
	}
}
