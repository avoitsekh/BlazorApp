namespace BlazorApp.Services;

class AppBarOptions
{
	public event EventHandler? ValueChanged;

	public string? Title
	{
		get;
		set
		{
			if (field != value)
			{
				field = value;
				OnValueChanged();
			}
		}
	}

	void OnValueChanged()
	{
		ValueChanged?.Invoke(this, EventArgs.Empty);
	}
}
