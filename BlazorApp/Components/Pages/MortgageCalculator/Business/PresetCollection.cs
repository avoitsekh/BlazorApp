using static BlazorApp.Components.Pages.MortgageCalculator.MortgageDetails;

namespace BlazorApp.Components.Pages.MortgageCalculator;

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

		Add("ARM with Extra Payments", () => new()
		{
			LoanAmount = 700000D,
			InterestRates = new InterestCollection(4.5D)
			{
				{ new DateTime(2020, 10, 20), 5.5D },
				{ new DateTime(2021, 05, 07), 4.9D },
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
				{ 115, 115, 50000D },
				{ 200, 200, 50000D },
			},
		});

		Add("VRM with Extra Payments", () => new()
		{
			LoanAmount = 500000D,
			InterestRates = new InterestCollection(3.35D)
			{
				{ new DateTime(2020, 10, 20), 3.6D },
				{ new DateTime(2021, 06, 15), 4.15D },
				{ new DateTime(2029, 10, 10), 3.75D },
			},
			AmortizationPeriodInYears = 25,
			AdvanceDate = new DateTime(2020, 02, 01),
			CompoundPeriod = 2,
			PaymentFrequency = 12,
			InterestRateType = InterestRateTypes.VRM,
			InterestAccrualMethod = InterestAccrualMethods.PerPayment,
			ExtraPayments = new ExtraPaymentCollection()
			{
				{ 100, 100, 100000D },
				{ 200, 200, 30000D },
			},
		});
	}
}
