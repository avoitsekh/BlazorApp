using static MudBlazorApp.Components.Pages.MortgageCalculator.MortgageDetails;

namespace MudBlazorApp.Components.Pages.MortgageCalculator;

public sealed class PaymentCollection : List<Payment>
{
	public readonly MortgageDetails Details;

	public bool HasExtraPayments => this.Any(x => x.ExtraAmount > 0D);

	PaymentCollection(MortgageDetails settings)
		: base(settings.PaymentFrequency * settings.AmortizationPeriodInYears)
	{
		Details = settings;
	}

	public static PaymentCollection Create(MortgageDetails settings, bool countExtraPayments = true)
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
		DateTime paymentDate = Details.AdvanceDate;
		bool isLastPayment = false;
		int year = 1;

		for (int i = 1; i <= paymentCount; i++)
		{
			DateTime previousPaymentDate = paymentDate;
			paymentDate = GetNextPaymentDate(paymentDate, i);

			double interestAmount = CalculateInterestAmount(previousPaymentDate, paymentDate, balance);
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
			else if (i == paymentCount && balance > 0)
			{
				principalAmount += balance;
				paymentAmount = interestAmount + principalAmount;
				balance = 0;
			}

			double[] ratesForPeriod = Details.InterestRates.GetRatesForPeriod(previousPaymentDate, paymentDate);
			string ratesForPeriodFormatted = string.Join(" → ", ratesForPeriod.Select(x => string.Format("{0}%", x)));

			string accrualPeriod = Details.InterestAccrualMethod == InterestAccrualMethods.PerDay
				? string.Format("{0:yyyy-MM-dd} → {1:yyyy-MM-dd}", previousPaymentDate, paymentDate.AddDays(-1))
				: $"{ratesForPeriodFormatted} ÷ {Details.PaymentFrequency}";


			Add(new()
			{
				Number = i,
				PaymentDate = paymentDate,
				Year = i % Details.PaymentFrequency == 0 || isLastPayment ? year++ : null,
				IterestRate = ratesForPeriodFormatted,
				InterestAccrualPeriod = accrualPeriod,
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

	public double GetEffectiveAnnualRate(double contractRate)
	{
		return Compound(contractRate, Details.CompoundPeriod, 1);
	}

	double GetEffectiveAnnualRatePerPayment(double contractRate)
	{
		return Compound(contractRate, Details.CompoundPeriod, Details.PaymentFrequency);
	}

	double GetEffectiveAnnualRatePerDay(double contractRate, DateTime date)
	{
		var daysInYear = 365;

		if (Details.FinancialYear == FinancialYears._365or366)
		{
			var isLeapYear = DateTime.IsLeapYear(date.Year);
			daysInYear = isLeapYear ? 366 : 365;
		}
		else if (Details.FinancialYear == FinancialYears._360)
		{
			daysInYear = 360;
		}

		return Compound(contractRate, Details.CompoundPeriod, daysInYear);
	}

	double Compound(double contractRate, double compoundPeriod, int denominator)
	{
		return compoundPeriod == 0D
			? contractRate / 100 / denominator
			: Math.Pow(1D + contractRate / 100 / compoundPeriod, compoundPeriod / denominator) - 1;
	}

	double CalculatePaymentAmount(double interestRate, int paymentCount, double balance)
	{
		double ratePerPayment = GetEffectiveAnnualRatePerPayment(interestRate);
		return PMT(ratePerPayment, paymentCount, balance);
	}

	double CalculateInterestAmount(DateTime from, DateTime to, double balance)
	{
		var result = Details.InterestAccrualMethod == InterestAccrualMethods.PerDay
				? CalculateInterestAmountForPeriod(from, to, balance)
				: CalculateInterestAmountForPayment(from, to, balance);
		return RoundToCents(result);
	}

	double CalculateInterestAmountForPayment(DateTime from, DateTime to, double balance)
	{
		double result = 0D;
		double[] ratesForPeriod = Details.InterestRates.GetRatesForPeriod(from, to);

		if (ratesForPeriod.Length == 1)
		{
			double ratePerPayment = GetEffectiveAnnualRatePerPayment(ratesForPeriod[0]);
			result = balance * ratePerPayment;
		}
		else
		{
			result = CalculateInterestAmountForPeriod(from, to, balance);
		}

		return result;
	}

	double CalculateInterestAmountForPeriod(DateTime from, DateTime to, double balance)
	{
		var result = 0D;

		for (var day = from.Date; day.Date < to.Date; day = day.AddDays(1))
		{
			var interestRateForDay = Details.InterestRates.GetRate(day);
			result += balance * GetEffectiveAnnualRatePerDay(interestRateForDay, day);
		}

		return result;
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
		var fpdate = Details.AdvanceDate;
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

	public Payment? LastPaymentForYear(int year)
	{
		return this.FirstOrDefault(x => x.Year.HasValue && x.Year.Value == year);
	}

	public List<Payment> GetPaymentsForPeriod(int fromPayment, int toPayment)
	{
		return this.Where(x => x.Number >= fromPayment && x.Number <= toPayment).ToList();
	}

}
