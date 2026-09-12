using System.Web;

namespace MudBlazorApp.Components.Pages.GuidGenerator;

public class GuidFactory
{
	public int GuidsToGenerate { get; set; } = 1;
	
	readonly GuidFormat format;

	public GuidFactory(GuidFormat format)
	{
		this.format = format;
	}

	public IEnumerable<string> Generate()
	{
		var result = new List<string>(GuidsToGenerate);

		for (int i = 0; i < GuidsToGenerate; i++)
		{
			result.Add(CreateGuidFormatted());
		}

		return result;
	}

	string CreateGuidFormatted()
	{
		string result;
		Guid newGuid = Guid.NewGuid();

		if (format.Base64)
		{
			result = Convert.ToBase64String(newGuid.ToByteArray());

			if (format.UrlEncode)
			{
				result = HttpUtility.UrlEncode(result);
			}
		}
		else
		{
			result = newGuid.ToString();

			if (!format.Hyphens)
			{
				result = result.Replace("-", string.Empty);
			}

			if (format.CurlyBraces)
			{
				result = "{" + result + "}";
			}

			if (format.Uppercase)
			{
				result = result.ToUpper();
			}
		}

		if (format.Quotes)
		{
			result = "'" + result + "'";
		}
		else if (format.DoubleQuotes)
		{
			result = "\"" + result + "\"";
		}

		if (format.Commas)
		{
			result = result + ", ";
		}

		return result;
	}
}
