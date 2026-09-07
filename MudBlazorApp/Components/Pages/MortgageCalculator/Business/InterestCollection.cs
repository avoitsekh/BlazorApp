using System.Collections;

namespace MudBlazorApp.Components.Pages.MortgageCalculator;

public sealed class InterestCollection : IList<Interest>
{
	List<Interest> _interests = new();

	public bool HasAdditionalInterestRates => Count > 1;

	public double InitialRate
	{
		get => _interests[0].Rate;
		set => _interests[0].Rate = value;
	}

	public InterestCollection()
	{
	}

	public InterestCollection(double initialRate)
	{
		_interests.Add(new() { Rate = initialRate });
	}

	public void Add(DateTime? effectiveDate, double rate)
	{
		Add(new Interest { EffectiveDate = effectiveDate, Rate = rate });
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
		SortByEffectiveDate();
	}

	void SortByEffectiveDate()
	{
		_interests = _interests.OrderBy(x => x.EffectiveDate).ToList();
	}

	public double GetRate(DateTime effectiveDate)
	{
		return _interests.LastOrDefault(x => x.EffectiveDate <= effectiveDate)?.Rate ?? InitialRate;
	}

	public double[] GetRatesForPeriod(DateTime from, DateTime to)
	{
		List<double> result = new()
		{
			GetRate(from)
		};
		result.AddRange(_interests.Where(x => x.EffectiveDate > from && x.EffectiveDate < to).Select(x => x.Rate));
		return result.ToArray();
	}

	#region IList

	public int Count => _interests.Count;

	public bool IsReadOnly => false;

	public void Add(Interest interest)
	{
		_interests.Add(interest);
		SortByEffectiveDate();
	}

	public void Clear()
	{
		_interests.RemoveAll(x => x.EffectiveDate.HasValue);
	}

	public Interest this[int index]
	{
		get => _interests[index];
		set => _interests[index] = value;
	}

	public IEnumerator<Interest> GetEnumerator()
	{
		return _interests.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public int IndexOf(Interest item)
	{
		return _interests.IndexOf(item);
	}

	public void Insert(int index, Interest item)
	{
		_interests.Insert(index, item);
	}

	public void RemoveAt(int index)
	{
		_interests.RemoveAt(index);
	}

	public bool Contains(Interest item)
	{
		return _interests.Contains(item);
	}

	public void CopyTo(Interest[] array, int arrayIndex)
	{
		_interests.CopyTo(array, arrayIndex);
	}

	bool ICollection<Interest>.Remove(Interest item)
	{
		return _interests.Remove(item);
	}

	#endregion
}
