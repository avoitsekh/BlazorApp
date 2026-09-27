namespace BlazorApp.Components.Pages.MortgageCalculator;

public sealed class PaymentStatistics(PaymentCollection? payments, PaymentCollection? paymentsNoExtras)
{
	public PaymentStatistics()
		: this(null, null)
	{
	}

	public double InterestPaid;
	public double PrincipalPaid;
	public double ExtraPaid;
	public double TotalPaid;
	public double RemainingBalance;
	public DateTime LastPaymentDate;
	public int NumberOfPayments;
	public double InterestSavings;

	public double PaymentAmount;
	public double InterestAmount;
	public double PrincipalAmount;
	public double EffectiveAnnualRate;
	public double YearsToPayOff;
	public double AmortizationOffset;

	const double yearAverageDays = 365.2425D;

	public void CalculateSummary()
	{
		if (payments.HasData())
		{
			var details = payments!.Details;

			EffectiveAnnualRate = payments.GetEffectiveAnnualRate(details.InterestRates.InitialRate) * 100;
			PaymentAmount = payments.First().PaymentAmount;
			InterestAmount = payments.First().InterestAmount;
			PrincipalAmount = payments.First().PrincipalAmount;

			var totalDays = (payments.Last().PaymentDate - details.AdvanceDate).TotalDays;
			YearsToPayOff = Math.Round(totalDays / yearAverageDays, 1);
			AmortizationOffset = (((double)payments.Count / (details.PaymentFrequency * details.AmortizationPeriodInYears)) - 1) * 100;
		}
	}

	public void CalculateForYear(int year)
	{
		if (year > 0)
		{
			if (payments.HasData())
			{
				var lastPayment = payments!.GetLastPaymentForYear(year);
				if (lastPayment != null)
				{
					int lastPaymentNumber = lastPayment.Number;
					LastPaymentDate = lastPayment.PaymentDate;
					RemainingBalance = lastPayment.Balance;

					var paymentsForPeriod = payments.GetAllPaymentsForPeriod(1, lastPaymentNumber);
					InterestPaid = PaymentCollection.RoundToCents(paymentsForPeriod.Sum(x => x.InterestAmount));
					PrincipalPaid = PaymentCollection.RoundToCents(paymentsForPeriod.Sum(x => x.PrincipalAmount));
					ExtraPaid = PaymentCollection.RoundToCents(paymentsForPeriod.Sum(x => x.ExtraAmount));
					TotalPaid = PaymentCollection.RoundToCents(paymentsForPeriod.Sum(x => x.PaymentAmount));
					NumberOfPayments = paymentsForPeriod.Count;

					if (paymentsNoExtras.HasData())
					{
						var lastPaymentNoExtras = paymentsNoExtras!.GetLastPaymentForYear(year);
						var paymentsForPeriodNoExtras = paymentsNoExtras.GetAllPaymentsForPeriod(1, lastPaymentNumber);
						var interestPaidNoExtras = PaymentCollection.RoundToCents(paymentsForPeriodNoExtras.Sum(x => x.InterestAmount));
						InterestSavings = interestPaidNoExtras - InterestPaid;
					}
					else
					{
						InterestSavings = default;
					}
				}
			}
			else
			{
				InterestPaid = default;
				PrincipalPaid = default;
				ExtraPaid = default;
				TotalPaid = default;
				RemainingBalance = default;
				LastPaymentDate = default;
				NumberOfPayments = default;
				InterestSavings = default;
			}
		}

	}
}
