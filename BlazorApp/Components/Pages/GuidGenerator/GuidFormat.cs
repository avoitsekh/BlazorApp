namespace BlazorApp.Components.Pages.GuidGenerator;

public class GuidFormat
{
	public bool Hyphens { get; set; } = true;

	public bool CurlyBraces { get; set; }

	public bool Uppercase { get; set; }

	public bool Quotes
	{
		get;
		set
		{
			if (DoubleQuotes && DoubleQuotes != !value)
			{
				DoubleQuotes = !value;
			}
			field = value;
		}
	}

	public bool DoubleQuotes
	{
		get;
		set
		{
			if (Quotes && Quotes != !value)
			{
				Quotes = !value;
			}
			field = value;
		}
	}

	public bool Commas { get; set; }

	public bool Base64
	{
		get;
		set
		{
			if (!value)
			{
				UrlEncode = false;
			}
			else
			{
				Hyphens = CurlyBraces = Uppercase = false;
			}
			field = value;
		}
	}

	public bool UrlEncode { get; set; }
}
