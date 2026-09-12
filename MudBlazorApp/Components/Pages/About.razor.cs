namespace MudBlazorApp.Components.Pages;

public partial class About
{
	string greetings = "Hello...";

	async Task OnTimeZoneChanged(TimeZoneInfo timeZone)
	{
		var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
		var timeOfDay = GetTimeOfDayAsString(localTime);
		greetings = "Hello..." + (timeOfDay != "night" ? $" and good {timeOfDay}!" : string.Empty);
	}

	string GetTimeOfDayAsString(DateTime time)
	{
		var now = time.TimeOfDay;
		var _5am = new TimeSpan(5, 0, 0);
		var _7am = new TimeSpan(7, 0, 0);
		var _noon = new TimeSpan(12, 0, 0);
		var _5pm = new TimeSpan(17, 0, 0);
		var _11pm = new TimeSpan(23, 30, 0);

		if (now.IsBetween(_5am, _7am))
		{
			return "early morning";
		}
		else if (now.IsBetween(_7am, _noon))
		{
			return "morning";
		}
		else if (now.IsBetween(_noon, _5pm))
		{
			return "afternoon";
		}
		else if (now.IsBetween(_5pm, _11pm))
		{
			return "evening";
		}

		return "night";
	}
}

static class ExtensionMethods
{
	public static bool IsBetween(this TimeSpan value, TimeSpan from, TimeSpan to)
	{
		return value > from && value < to;
	}
}

