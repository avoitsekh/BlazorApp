using System.Collections;

namespace MudBlazorApp.Components.Pages.MortgageCalculator;

public sealed class ExtraPaymentCollection : IList<ExtraPayment>
{
	List<ExtraPayment> _payments = new();

	public bool HasExtraPayments => Count > 0;

	public IEnumerable<ExtraPayment> LumpSums => _payments.Where(x => x.IsLumpSum);

	public IEnumerable<ExtraPayment> Recurring => _payments.Where(x => x.IsRecurring);

	public void Add(int? fromPayment, int? toPayment, double amount)
	{
		_payments.Add(new() { FromPayment = fromPayment, ToPayment = toPayment, Amount = amount });
	}

	public void Remove(ExtraPayment payment)
	{
		_payments.Remove(payment);
	}

	public double GetExtraPaymentAmounts(int paymentNumber)
	{
		return _payments.Where(x => x.FromPayment <= paymentNumber && (x.ToPayment == null || x.ToPayment >= paymentNumber)).Sum(x => x.Amount);
	}

	#region IList

	public int Count => _payments.Count;

	public bool IsReadOnly => false;

	public void Add(ExtraPayment ExtraPayment)
	{
		_payments.Add(ExtraPayment);
	}

	public void Clear()
	{
		_payments.Clear();
	}

	public ExtraPayment this[int index]
	{
		get => _payments[index];
		set => _payments[index] = value;
	}

	public IEnumerator<ExtraPayment> GetEnumerator()
	{
		return _payments.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public int IndexOf(ExtraPayment item)
	{
		return _payments.IndexOf(item);
	}

	public void Insert(int index, ExtraPayment item)
	{
		_payments.Insert(index, item);
	}

	public void RemoveAt(int index)
	{
		_payments.RemoveAt(index);
	}

	public bool Contains(ExtraPayment item)
	{
		return _payments.Contains(item);
	}

	public void CopyTo(ExtraPayment[] array, int arrayIndex)
	{
		_payments.CopyTo(array, arrayIndex);
	}

	bool ICollection<ExtraPayment>.Remove(ExtraPayment item)
	{
		return _payments.Remove(item);
	}

	#endregion
}
