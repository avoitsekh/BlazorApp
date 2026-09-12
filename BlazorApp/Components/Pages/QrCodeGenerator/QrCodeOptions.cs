using SkiaSharp.QrCode;
using SkiaSharp.QrCode.Image;

namespace BlazorApp.Components.Pages.QrCodeGenerator;

public sealed class QrCodeOptions
{
	public string Content { get; set; } = "Hello World!";

	public SymbologyKind Symbology { get; set; } = SymbologyKind.QrCode;

	public ECCLevel Ecc { get; set; } = ECCLevel.H;

	public MicroQREccLevel MicroEcc { get; set; } = MicroQREccLevel.M;

	public int Size { get; set; } = 512;

	public int QuietZone { get; set; } = 4;

	public int Version { get; set; } = -1;

	public ModuleShape? ModuleShape { get; set; } = null;

	public static Dictionary<string, ModuleShape?> ModuleShapes = new()
	{
		{ "Rectangle", null },
		{ "Circle", CircleModuleShape.Default },
		{ "Rounded Rectangle", RoundedRectangleModuleShape.Default },
	};

	public float ModuleShapeCornerRadius { get; set; } = 0f;

	public float ModuleSizePercent { get; set; } = 1.0f;

	public FinderPatternShape? FinderPatternShape { get; set; } = null;

	public static Dictionary<string, FinderPatternShape?> FinderPatternShapes = new()
	{
		{ "Auto (module shape)", null },
		{ "Rectangle", RectangleFinderPatternShape.Default },
		{ "Circle", CircleFinderPatternShape.Default },
		{ "Rounded", RoundedRectangleFinderPatternShape.Default },
		{ "Rounded + Circle", RoundedRectangleCircleFinderPatternShape.Default },
	};

	public string ModuleColor1 { get; set; } = "#000000";

	public string ModuleColor2 { get; set; } = "#000000";

	public bool TransparentBackground { get; set; }
	
	public GradientDirection GradientDirection { get; set; } = GradientDirection.None;

	public static Dictionary<string, GradientDirection> GradientOptions = new()
	{
		{ "None", GradientDirection.None },
		{ "Gradient (Left → Right)", GradientDirection.LeftToRight },
		{ "Gradient (Top → Bottom)", GradientDirection.TopToBottom },
		{ "Gradient (Top-left → Bottom-right)", GradientDirection.TopLeftToBottomRight },
		{ "Gradient (Top-right → Bottom-left)", GradientDirection.TopRightToBottomLeft },
	};

	public Preset Preset { get; set; } = Preset.ClassicBlackAndWhite;

	public static Dictionary<string, Preset> Presets = new()
	{
		{ "Custom", Preset.Custom },
		{ "Classic black & white", Preset.ClassicBlackAndWhite },
		{ "Ocean", Preset.Ocean },
		{ "Tech", Preset.Tech },
	};
}

public enum SymbologyKind
{
    QrCode,
    MicroQR,
}

public enum Preset
{
	Custom,
	ClassicBlackAndWhite,
	Ocean,
	Tech,
}
