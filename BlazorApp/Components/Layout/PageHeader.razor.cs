using Microsoft.AspNetCore.Components;

namespace MudBlazorApp.Components.Layout
{
	public partial class PageHeader
	{
		[Parameter]
		public string? Title { get; set; }
		[Parameter]
		public string? Description { get; set; }
	}
}
