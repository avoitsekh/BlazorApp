namespace BlazorApp.Components.Pages.MortgageCalculator;

public sealed class PaymentStatistics()
{
	public PaymentCollection? Payments;
	public PaymentCollection? PaymentsNoExtras;

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

	MortgageDetails Details => Payments.Details;

	public void CalculateSummary()
	{
		if (Payments.HasData())
		{
			EffectiveAnnualRate = Payments.GetEffectiveAnnualRate(Details.InterestRates.InitialRate) * 100;
			//EffectiveAnnualRate = GetEffectiveAnnualRate(Details.InterestRates.InitialRate);
			PaymentAmount = Payments.First().PaymentAmount;
			InterestAmount = Payments.First().InterestAmount;
			PrincipalAmount = Payments.First().PrincipalAmount;

			if (PaymentsNoExtras.HasData())
			{
				var totalDays = (Payments.Last().PaymentDate - Details.AdvanceDate).TotalDays;
				YearsToPayOff = Math.Round(totalDays / 365.2425, 1);
				AmortizationOffset = (((double)Payments.Count / PaymentsNoExtras.Count) - 1) * 100;
			}
			else
			{
				YearsToPayOff = Details.AmortizationPeriodInYears;
				AmortizationOffset = default;
			}
		}
	}

	public void CalculateForYear(int year)
	{
		if (year > 0)
		{
			if (Payments.HasData())
			{
				var lastPayment = Payments.LastPaymentForYear(year);
				if (lastPayment != null)
				{
					int lastPaymentNumber = lastPayment.Number;
					LastPaymentDate = lastPayment.PaymentDate;
					RemainingBalance = lastPayment.Balance;

					var paymentsForPeriod = Payments.GetPaymentsForPeriod(1, lastPaymentNumber);
					InterestPaid = paymentsForPeriod.SumAndRoundToCents(x => x.InterestAmount);
					PrincipalPaid = paymentsForPeriod.SumAndRoundToCents(x => x.PrincipalAmount);
					ExtraPaid = paymentsForPeriod.SumAndRoundToCents(x => x.ExtraAmount);
					TotalPaid = paymentsForPeriod.SumAndRoundToCents(x => x.PaymentAmount);
					NumberOfPayments = paymentsForPeriod.Count;

					if (PaymentsNoExtras.HasData())
					{
						var lastPaymentNoExtra = PaymentsNoExtras.LastPaymentForYear(year);
						var paymentsForPeriodNoExtra = PaymentsNoExtras.GetPaymentsForPeriod(1, lastPaymentNumber);
						var interestPaidNoExtra = paymentsForPeriodNoExtra.SumAndRoundToCents(x => x.InterestAmount);
						InterestSavings = interestPaidNoExtra - InterestPaid;
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
