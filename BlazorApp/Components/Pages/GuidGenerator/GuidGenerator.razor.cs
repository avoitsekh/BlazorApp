using MudBlazor;

namespace BlazorApp.Components.Pages.GuidGenerator;

public partial class GuidGenerator
{
	readonly GuidFormat format;
	readonly GuidFactory factory;

	public string GeneratedGuids { get; set; } = string.Empty;
	public bool CopyToClipboardButtonEnabled => !string.IsNullOrWhiteSpace(GeneratedGuids);

	public GuidGenerator()
	{
		format = new GuidFormat();
		factory = new GuidFactory(format);
	}

	void GenerateButtonClick()
	{
		GeneratedGuids = string.Join(Environment.NewLine, factory.Generate());
	}

	async Task CopyToClipboardButtonClick()
	{
		await JS.CopyToClipboardAsync(GeneratedGuids);
		Snackbar?.Add("Copied to clipboard", Severity.Info);
	}
}
