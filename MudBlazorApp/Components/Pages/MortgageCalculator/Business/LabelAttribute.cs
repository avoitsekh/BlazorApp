namespace MudBlazorApp.Components.Pages.MortgageCalculator;

[AttributeUsage(AttributeTargets.Field)]
public sealed class LabelAttribute : Attribute
{
	public readonly string Name;
	public readonly string Description;

	public LabelAttribute(string name)
	{
		Name = name;
		Description = ""; //todo: add for info containers
	}
}
