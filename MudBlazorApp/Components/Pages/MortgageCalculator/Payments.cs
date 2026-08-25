namespace MudBlazorApp.Components.Pages.MortgageCalculator;

public class Payments : List<Payment>
{
	public readonly MortgageSettings Settings;

	Payments(MortgageSettings settings)
		: base(settings.PaymentFrequency * settings.AmortizationPeriodInYears)
	{
		Settings = settings;
	}

	public static Payments Generate(MortgageSettings settings)
	{
		var result = new Payments(settings);
		result.CalculateAmortizationSchedule();
		return result;
	}

	void CalculateAmortizationSchedule()
	{
		int numberOfPayments = Settings.PaymentFrequency * Settings.AmortizationPeriodInYears;    // nper

		double currentRate = Settings.InterestRates.InitialRate;
		double balance = Settings.LoanAmount;
		double ratePerPayment = GetEffectiveAnnualRatePerPayment(currentRate / 100);
		double paymentAmount = PMT(ratePerPayment, numberOfPayments, balance);
		DateTime paymentDate = Settings.AdvanceDate ?? DateTime.MinValue;

		//Settings.InterestRates.Add(new DateTime(2025, 04, 22), 8.88D);

		for (int i = 1; i <= numberOfPayments; i++)
		{
			if (balance <= 0)
			{
				break;
			}

			var previousRate = currentRate;

			var previousPaymentDate = paymentDate;
			paymentDate = GetNextPaymentDate(paymentDate, i);


			//double interestPaid = RoundToCents(balance * ratePerPayment);
			//double interestPaid = CalculateInterestPerPeriod(previousPaymentDate, paymentDate, balance, AnnualRate);
			double interestAmount = CalculateInterestPerPeriod(previousPaymentDate, paymentDate, balance);

			double principalAmount = RoundToCents(paymentAmount - interestAmount);
			balance = RoundToCents(balance - principalAmount);

			int? year = i % Settings.PaymentFrequency == 0 ? i / Settings.PaymentFrequency : null;

			if (balance < 0)
			{
				paymentAmount += balance;
				principalAmount += balance;
				balance = 0;
				year = i / Settings.PaymentFrequency + 1;
			}


			var ratesForPeriod = Settings.InterestRates.GetRatesForPeriod(previousPaymentDate, paymentDate);

			Add(new()
			{
				Number = i,
				PaymentDate = paymentDate,
				PaymentPeriodInterestRate = string.Join(" → ", ratesForPeriod.Select(x => string.Format("{0}%", x))),
				InterestAccrualPeriod = string.Format("{0:yyyy-MM-dd} → {1:yyyy-MM-dd}", previousPaymentDate, paymentDate.AddDays(-1)),
				InterestAmount = interestAmount,
				PaymentAmount = paymentAmount,
				PrincipalAmount = principalAmount,
				Balance = balance,
				Year = year
			});

			currentRate = Settings.InterestRates.GetRate(paymentDate);

			if (currentRate != previousRate)    // interest rate has changed, re-calculate payment amount
			{
				ratePerPayment = GetEffectiveAnnualRatePerPayment(currentRate / 100);
				paymentAmount = PMT(ratePerPayment, numberOfPayments - i, balance);
			}
		}

		//var EffectiveAnnualRate = GetEffectiveAnnualRate(_settings.AnnualRate / 100) * 100;
	}

	//double GetEffectiveAnnualRate(double contractRate)
	//{
	//	return Math.Pow(1D + contractRate / _settings.CompoundPeriod, (double)_settings.CompoundPeriod) - 1;
	//}

	double GetEffectiveAnnualRatePerPayment(double contractRate)
	{
		return Math.Pow(1D + contractRate / Settings.CompoundPeriod, (double)Settings.CompoundPeriod / Settings.PaymentFrequency) - 1;
	}

	//double CalculateInterestPerPeriod(DateTime periodStart, DateTime periodEnd, double balance, double annualRate)
	//{
	//	var daysPerPeriod = (periodEnd - periodStart).TotalDays;
	//	var isLeapYear = DateTime.IsLeapYear(periodEnd.Year);
	//	//return balance * (annualRate / 100) * (daysPerPeriod / (isLeapYear ? 366 : 365));

	//	var comp = (double)Settings.CompoundPeriod;
	//	return balance * (Math.Pow(1D + Settings.AnnualRate / 100 / comp, comp / (isLeapYear ? 366 : 365)) - 1) * daysPerPeriod;
	//}

	//double CalculateInterestPerPeriod(DateTime from, DateTime to, double balance, double annualRate)
	//{
	//	return EachDay(from, to).Sum(day => CalculateInterestPerDay(day, balance, annualRate));
	//}

	double CalculateInterestPerPeriod(DateTime from, DateTime to, double balance)
	{
		var result = 0D;
		foreach (var day in EachDay(from, to))
		{
			var interestRateForDay = Settings.InterestRates.GetRate(day);
			result += CalculateInterestPerDay(day, balance, interestRateForDay);
		}
		return RoundToCents(result);
	}

	double CalculateInterestPerDay(DateTime date, double balance, double rate)
	{
		var isLeapYear = DateTime.IsLeapYear(date.Year);
		//return balance * (rate / 100) / (isLeapYear ? 366 : 365);	// Scotiabank formula (bad) - no compounding

		var comp = (double)Settings.CompoundPeriod;
		return balance * (Math.Pow(1D + rate / 100 / comp, comp / (isLeapYear ? 366 : 365)) - 1);
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
		var fpdate = Settings.AdvanceDate!.Value;
		var anchor = fpdate.Day;

		switch (Settings.PaymentFrequency)
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
}
