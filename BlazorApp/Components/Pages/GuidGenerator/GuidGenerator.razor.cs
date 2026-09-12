using Microsoft.AspNetCore.Components;
using BlazorApp.Services;

namespace BlazorApp.Components.Pages.GuidGenerator;

public partial class GuidGenerator
{
	readonly GuidFormat format;
	readonly GuidFactory factory;

	[Inject]
	ClipboardService clipboardService { get; set; }

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

	void CopyToClipboardButtonClick()
	{
		clipboardService.Write(GeneratedGuids);
	}
}
