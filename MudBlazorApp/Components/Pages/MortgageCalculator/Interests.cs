using System.Collections;

namespace MudBlazorApp.Components.Pages.MortgageCalculator;

public class Interests : IEnumerable<Interest>
{
	List<Interest> _interests = new();

	public double InitialRate;

	public Interests(double initialRate)
	{
		InitialRate = initialRate;
	}

	public void Add(DateTime? effectiveDate, double rate)
	{
		Add(new Interest { EffectiveDate = effectiveDate, Rate = rate });
	}

	public void Add(Interest interest)
	{
		_interests.Add(interest);
		Sort();
	}

	public void RemoveAt(DateTime? effectiveDate)
	{
		var interest = _interests.FirstOrDefault(x => x.EffectiveDate == effectiveDate);
		if (interest != null)
		{
			Remove(interest);
		}
	}

	public void Remove(Interest interest)
	{
		_interests.Remove(interest);
		Sort();
	}

	void Sort()
	{
		_interests = _interests.OrderBy(x => x.EffectiveDate).ToList();
	}

	public double GetRate(DateTime effectiveDate)
	{
		return _interests.LastOrDefault(x => x.EffectiveDate <= effectiveDate)?.Rate ?? InitialRate;
	}

	public double[] GetRatesForPeriod(DateTime from, DateTime to)
	{
		var result = new List<double>
		{
			GetRate(from)
		};
		result.AddRange(_interests.Where(x => x.EffectiveDate > from && x.EffectiveDate < to).Select(x => x.Rate));
		return result.ToArray();
	}

	public void Clear()
	{
		_interests.Clear();
	}

	public int Count => _interests.Count;

	public IEnumerator<Interest> GetEnumerator()
	{
		return _interests.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
