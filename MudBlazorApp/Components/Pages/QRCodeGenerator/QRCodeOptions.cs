using Microsoft.Extensions.Options;
using SkiaSharp.QrCode;
using SkiaSharp.QrCode.Image;

namespace MudBlazorApp.Components.Pages.QRCodeGenerator;

/// <summary>
/// QR generation options bound to the page controls. Mirrors the option set of the
/// SkiaSharp.QrCode.Playground <c>QrRequest</c>, using library enums directly since
/// no JS interop boundary is involved.
/// </summary>
public sealed class QRCodeOptions
{
	public string Content { get; set; } = "Hello World!";

	/// <summary>Symbology: Standard QR (versions 1-40) or Micro QR (M1-M4).</summary>
	public SymbologyKind Symbology { get; set; } = SymbologyKind.QrCode;

	/// <summary>Error correction level. H is recommended when a logo overlays the code.</summary>
	public ECCLevel Ecc { get; set; } = ECCLevel.H;

	/// <summary>Micro QR error correction level (M1 supports error detection only).</summary>
	public MicroQREccLevel MicroEcc { get; set; } = MicroQREccLevel.M;

	/// <summary>Exported image size in pixels (square). The on-screen preview scales to fit.</summary>
	public int Size { get; set; } = 512;

	/// <summary>Quiet zone in modules (0-10). The specification default is 4 (Micro QR: 2).</summary>
	public int QuietZone { get; set; } = 4;

	/// <summary>QR version 1-40 (Micro QR: 1-4 for M1-M4), or -1 for automatic selection.</summary>
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

	/// <summary>Module color as #RRGGBB. Ignored while the gradient is enabled.</summary>
	public string ModuleColor1 { get; set; } = "#000000";

	public string ModuleColor2 { get; set; } = "#000000";

	/// <summary>Background color as #RRGGBB. Ignored while <see cref="TransparentBackground"/> is set.</summary>
	public string Background { get; set; } = "#ffffff";
	public bool TransparentBackground { get; set; }
	public bool GradientEnabled { get; set; } = true;
	public string GradientStart { get; set; } = "#fa7e1e";
	public string GradientEnd { get; set; } = "#962fbf";
	
	
	public GradientDirection GradientDirection { get; set; } = GradientDirection.None;

	public static Dictionary<string, GradientDirection> GradientOptions = new()
	{
		{ "None", GradientDirection.None },
		{ "Gradient (Left → Right)", GradientDirection.LeftToRight },
		{ "Gradient (Top → Bottom)", GradientDirection.TopToBottom },
		{ "Gradient (Top-left → Bottom-right)", GradientDirection.TopLeftToBottomRight },
		{ "Gradient (Top-right → Bottom-left)", GradientDirection.TopRightToBottomLeft },
	};


	public LogoMode LogoMode { get; set; } = LogoMode.BuiltIn;

	/// <summary>Logo size as a percentage of the QR side length (1-40).</summary>
	public int LogoSizePercent { get; set; } = 18;

	/// <summary>Border padding around the logo in pixels (0-24).</summary>
	public int LogoBorderWidth { get; set; } = 6;
}

/// <summary>Symbology choices exposed by the page.</summary>
public enum SymbologyKind
{
    QrCode,
    MicroQR,
}

/// <summary>Module shape choices exposed by the page.</summary>
public enum ModuleShapeKind
{
    Rectangle,
    //Circle,
    Rounded,
}

/// <summary>Finder pattern choices exposed by the page. Auto follows the module shape.</summary>
public enum FinderShapeKind
{
    Auto,
    Rectangle,
    Circle,
    Rounded,
    RoundedCircle,
}

public enum LogoMode
{
    None,
    BuiltIn,
    Custom,
}
