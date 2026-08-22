using System.Collections;

namespace MudBlazorApp.Components.Pages.MortgageCalculator;

public class Interests : IEnumerable<Interest>
{
	List<Interest> _interests = new();

	public Interests(double initialRate)
	{
		Add(DateTime.MinValue, initialRate);
	}

	public void Add(DateTime effectiveDate, double rate)
	{
		Add(new Interest { EffectiveDate = effectiveDate, Rate = rate });
	}

	public void Add(Interest interest)
	{
		_interests.Add(interest);
		Sort();
	}

	public void Remove(Interest interest)
	{
		_interests.Add(interest);
		Sort();
	}

	void Sort()
	{
		_interests = _interests.OrderBy(x => x.EffectiveDate).ToList();
	}

	public double GetRate(DateTime effectiveDate)
	{
		return _interests.Last(x => x.EffectiveDate <= effectiveDate).Rate;
	}

	public double[] GetRatesForAccrualPeriod(DateTime from, DateTime to)
	{
		var result = new List<double>
		{
			GetRate(from)
		};
		result.AddRange(_interests.Where(x => x.EffectiveDate > from && x.EffectiveDate < to).Select(x => x.Rate));
		return result.ToArray();
	}

	public IEnumerator<Interest> GetEnumerator()
	{
		return _interests.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
