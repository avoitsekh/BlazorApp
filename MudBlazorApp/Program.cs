using MudBlazor;
using MudBlazor.Services;
using MudBlazorApp.Components;
using MudBlazorApp.Components.Pages.CurrencyConverter;
using MudBlazorApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();
//builder.Services.AddMudServices(config =>
//{
//	config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
//	config.SnackbarConfiguration.ShowTransitionDuration = 200;
//	config.SnackbarConfiguration.HideTransitionDuration = 200;
//	config.SnackbarConfiguration.VisibleStateDuration = 4000;
//	config.SnackbarConfiguration.MaximumOpacity = 100;
//});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<CurrencyConverterDataService>();
builder.Services.AddScoped<CurrencyConverterState>();

builder.Services.AddScoped<ISnackbar, SnackbarService>();

builder.Services.AddMudBlazorSnackbar(options =>
{
	options.PositionClass = Defaults.Classes.Position.BottomRight;
	options.ShowTransitionDuration = 200;
	options.HideTransitionDuration = 200;
	options.VisibleStateDuration = 4000;
	options.MaximumOpacity = 100;
});

builder.Services.AddScoped<ClipboardService>();


builder.Services.AddCors(options => options.AddPolicy("Allow All CORS", p => p.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin()));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseHttpsRedirection();

app.UseCors("Allow All CORS");	// Allow connections from anywhere


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
