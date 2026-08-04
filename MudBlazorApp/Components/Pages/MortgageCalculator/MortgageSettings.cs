namespace MudBlazorApp.Components.Pages.MortgageCalculator
{
	public partial class MortgageSettings
	{
		public double LoanAmount = 320000;
		public double AnnualRate = 2.49;
		public int AmortizationPeriodInYears = 25;							// term
		public DateTime? FirstPaymentDate = new DateTime(2020, 01, 31);     // fpdate - first payment date
		public int CompoundPeriod = 2;										// cp - interest compound period
		public int PaymentFrequency = 24;                                   // ppy - payments per year

		public async Task<List<Payment>> CalculateAmortizationScheduleAsync()
		{
			return CalculateAmortizationSchedule();
		}

		public List<Payment> CalculateAmortizationSchedule()
		{
			int totalNumberOfPayments = PaymentFrequency * AmortizationPeriodInYears;    // nper
			List<Payment> result = new List<Payment>(totalNumberOfPayments);

			double ratePerPayment = Math.Pow(1D + AnnualRate / 100 / CompoundPeriod, (double)CompoundPeriod / PaymentFrequency) - 1;
			double paymentAmount = PMT(ratePerPayment, totalNumberOfPayments, LoanAmount);

			double balance = LoanAmount;
			DateTime paymentDate = FirstPaymentDate ?? DateTime.MinValue;

			for (int i = 1; i <= totalNumberOfPayments; i++)
			{
				double interestPaid = RoundToCents(balance * ratePerPayment);

				if (i == totalNumberOfPayments)
				{
					paymentAmount = RoundToCents(balance + interestPaid);
				}

				double principalPaid = RoundToCents(paymentAmount - interestPaid);
				balance = RoundToCents(balance - principalPaid);

				result.Add(new()
				{
					Number = i,
					PaymentDate = paymentDate,
					AnnualInterestRate = AnnualRate,
					InterestPaid = interestPaid,
					PaymentAmount = paymentAmount,
					PrincipalPaid = principalPaid,
					Balance = balance,
					Year = i % PaymentFrequency == 0 ? i / PaymentFrequency : null
				});

				paymentDate = GetNextPaymentDate(paymentDate, i);
			}

			return result;
		}

		double PMT(double rate, int totalNumberOfMonths, double loanAmount)
		{
			var denominator = Math.Pow(1 + rate, totalNumberOfMonths) - 1;
			return RoundToCents((rate + (rate / denominator)) * loanAmount);
		}

		public static double RoundToCents(double amount)
		{
			return Math.Round(amount, 2);
		}

		DateTime GetNextPaymentDate(DateTime currentDate, int currentPaymentNumber)
		{
			var result = DateTime.MinValue;
			var fpdate = FirstPaymentDate!.Value;
			var anchor = fpdate.Day;

			switch (PaymentFrequency)
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
}
