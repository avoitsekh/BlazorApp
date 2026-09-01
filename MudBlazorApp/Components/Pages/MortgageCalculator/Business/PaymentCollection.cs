namespace MudBlazorApp.Components.Pages.MortgageCalculator;

public sealed class PaymentCollection : List<Payment>
{
	public readonly MortgageDetails Details;

	public PaymentCollection()
	{
	}

	PaymentCollection(MortgageDetails settings)
		: base(settings.PaymentFrequency * settings.AmortizationPeriodInYears)
	{
		Details = settings;
	}

	public static PaymentCollection Generate(MortgageDetails settings, bool countExtraPayments = true)
	{
		var result = new PaymentCollection(settings);
		result.CalculateAmortizationSchedule(countExtraPayments);
		return result;
	}

	void CalculateAmortizationSchedule(bool countExtraPayments)
	{
		int paymentCount = Details.PaymentFrequency * Details.AmortizationPeriodInYears;    // nper
		double currentRate = Details.InterestRates.InitialRate;
		double balance = Details.LoanAmount;
		double paymentAmount = CalculatePaymentAmount(currentRate, paymentCount, balance);
		DateTime paymentDate = Details.AdvanceDate ?? DateTime.MinValue;
		bool isLastPayment = false;
		int year = 1;

		for (int i = 1; i <= paymentCount; i++)
		{
			DateTime previousPaymentDate = paymentDate;
			paymentDate = GetNextPaymentDate(paymentDate, i);

			double[] ratesForPeriod = Details.InterestRates.GetRatesForPeriod(previousPaymentDate, paymentDate);

			double interestAmount = CalculateInterest(previousPaymentDate, paymentDate, balance);
			double principalAmount = RoundToCents(paymentAmount - interestAmount);
			double extraAmount = countExtraPayments ? Details.ExtraPayments.GetExtraPaymentAmounts(i) : 0D;
			balance = RoundToCents(balance - principalAmount - extraAmount);

			if (balance <= 0)
			{
				extraAmount += balance;
				if (extraAmount < 0)
				{
					principalAmount += extraAmount;
					extraAmount = 0;
				}
				paymentAmount = interestAmount + principalAmount;
				balance = 0;
				isLastPayment = true;
			}

			Add(new()
			{
				Number = i,
				PaymentDate = paymentDate,
				Year = i % Details.PaymentFrequency == 0 || isLastPayment ? year++ : null,
				IterestRate = string.Join(" → ", ratesForPeriod.Select(x => string.Format("{0}%", x))),
				InterestAccrualPeriod = string.Format("{0:yyyy-MM-dd} → {1:yyyy-MM-dd}", previousPaymentDate, paymentDate.AddDays(-1)),
				InterestAmount = interestAmount,
				PrincipalAmount = principalAmount,
				ExtraAmount = extraAmount,
				PaymentAmount = paymentAmount + extraAmount,
				Balance = balance,
			});

			if (isLastPayment)
			{
				break;
			}

			double previousRate = currentRate;
			currentRate = Details.InterestRates.GetRate(paymentDate);
			if (currentRate != previousRate)
			{
				paymentAmount = CalculatePaymentAmount(currentRate, paymentCount - i, balance);
			}
		}
	}

	double GetEffectiveAnnualRate(double contractRate)
	{
		return Math.Pow(1D + contractRate / Details.CompoundPeriod, (double)Details.CompoundPeriod) - 1;
	}

	double GetEffectiveAnnualRatePerPayment(double contractRate)
	{
		return Math.Pow(1D + contractRate / Details.CompoundPeriod, (double)Details.CompoundPeriod / Details.PaymentFrequency) - 1;
	}

	double CalculatePaymentAmount(double interestRate, int paymentCount, double balance)
	{
		double ratePerPayment = GetEffectiveAnnualRatePerPayment(interestRate / 100);
		return PMT(ratePerPayment, paymentCount, balance);
	}

	//double CalculateInterestPerPeriod(DateTime periodStart, DateTime periodEnd, double balance, double annualRate)
	//{
	//	var daysPerPeriod = (periodEnd - periodStart).TotalDays;
	//	var isLeapYear = DateTime.IsLeapYear(periodEnd.Year);
	//	//return balance * (annualRate / 100) * (daysPerPeriod / (isLeapYear ? 366 : 365));

	//	var comp = (double)Settings.CompoundPeriod;
	//	return balance * (Math.Pow(1D + Settings.AnnualRate / 100 / comp, comp / (isLeapYear ? 366 : 365)) - 1) * daysPerPeriod;
	//}

	double CalculateInterest(DateTime from, DateTime to, double balance)
	{
		return Details.InterestAccrualMethod == MortgageDetails.InterestAccrualMethods.PerDay
				? CalculateInterestForPeriod(from, to, balance)
				: CalculateInterestForPayment(from, to, balance);
	}

	double CalculateInterestForPayment(DateTime from, DateTime to, double balance)
	{
		double result = 0D;
		double[] ratesForPeriod = Details.InterestRates.GetRatesForPeriod(from, to);

		if (ratesForPeriod.Length == 1)
		{
			double ratePerPayment = GetEffectiveAnnualRatePerPayment(ratesForPeriod[0] / 100);
			result = balance * ratePerPayment;
		}
		else
		{
			result = CalculateInterestForPeriod(from, to, balance);
		}

		return RoundToCents(result);
	}

	double CalculateInterestForPeriod(DateTime from, DateTime to, double balance)
	{
		var result = 0D;

		foreach (var day in EachDay(from, to))
		{
			var interestRateForDay = Details.InterestRates.GetRate(day);
			result += CalculateInterestForDay(day, balance, interestRateForDay);
		}

		return result;
	}

	double CalculateInterestForDay(DateTime date, double balance, double rate)
	{
		var isLeapYear = DateTime.IsLeapYear(date.Year);
		//return balance * (rate / 100) / (isLeapYear ? 366 : 365);	// Scotiabank formula (bad) - no compounding
		return balance * (Math.Pow(1D + rate / 100 / Details.CompoundPeriod, (double)Details.CompoundPeriod / (isLeapYear ? 366 : 365)) - 1);
	}

	IEnumerable<DateTime> EachDay(DateTime from, DateTime to)
	{
		for (var day = from.Date; day.Date < to.Date; day = day.AddDays(1))
		{
			yield return day;
		}
	}

	double PMT(double rate, int numberOfPayments, double loanAmount)
	{
		var denominator = Math.Pow(1 + rate, numberOfPayments) - 1;
		return RoundToCents((rate + (rate / denominator)) * loanAmount);
	}

	public static double RoundToCents(double amount)
	{
		return Math.Round(amount, 2);
	}

	DateTime GetNextPaymentDate(DateTime currentDate, int currentPaymentNumber)
	{
		var result = DateTime.MinValue;
		var fpdate = Details.AdvanceDate!.Value;
		var anchor = fpdate.Day;

		switch (Details.PaymentFrequency)
		{
			case 12:
				result = GetAnchorDate(currentDate.AddMonths(1), anchor);
				break;

			case 24:
				if (++currentPaymentNumber % 2 == 0)
				{
					result = anchor >= 15 ? GetAnchorDate(currentDate.AddMonths(1), fpdate.AddDays(-14).Day) : currentDate.AddDays(14);
				}
				else
				{
					result = GetAnchorDate(anchor >= 15 ? currentDate : currentDate.AddMonths(1), anchor);
				}
				break;

			case 52:
				result = currentDate.AddDays(7);
				break;

			case 26:
				result = currentDate.AddDays(14);
				break;
		}

		return result;
	}

	DateTime GetAnchorDate(DateTime date, int day)
	{
		return new DateTime(date.Year, date.Month, Math.Min(day, DateTime.DaysInMonth(date.Year, date.Month)));
	}

	public (double InterestAmount, double PrincipalAmount, double ExtraAmount, double PaymentAmount, double RemainingBalance, DateTime? LastPaymentDate, int Payments) GetStatsForYear(int year)
	{
		(double InterestAmount, double PrincipalAmount, double ExtraAmount, double PaymentAmount, double RemainingBalance, DateTime? LastPaymentDate, int Payments) result = new();

		if (year > 0)
		{
			var lastPayment = this.FirstOrDefault(x => x.Year == year);
			if (lastPayment != null)
			{
				int lastPaymentNumber = lastPayment.Number;
				result.LastPaymentDate = lastPayment.PaymentDate;
				result.RemainingBalance = lastPayment.Balance;

				var paymentsForPeriod = this.Where(x => x.Number <= lastPaymentNumber).ToList();
				result.InterestAmount = RoundToCents(paymentsForPeriod.Sum(x => x.InterestAmount));
				result.PrincipalAmount = RoundToCents(paymentsForPeriod.Sum(x => x.PrincipalAmount));
				result.ExtraAmount = RoundToCents(paymentsForPeriod.Sum(x => x.ExtraAmount));
				result.PaymentAmount = RoundToCents(paymentsForPeriod.Sum(x => x.PaymentAmount));
				result.Payments = paymentsForPeriod.Count;
			}
		}

		return result;
	}
}
