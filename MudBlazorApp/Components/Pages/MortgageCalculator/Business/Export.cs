using OfficeOpenXml;

namespace MudBlazorApp.Components.Pages.MortgageCalculator;

public static class Export
{
	static Export()
	{
		ExcelPackage.License.SetNonCommercialPersonal("Blazor Portfolio Project - AV");
	}

	public static void ToExcel(PaymentCollection payments)
	{
		string filename = "C:\\Temp\\my-file.xlsx";


		using ExcelPackage ep = new();
		using ExcelWorksheet ws = ep.Workbook.Worksheets.Add("Amortization Schedule");

		ws.Cells.Style.Font.Name = "Arial";
		ws.Cells.Style.Font.Size = 9.0f;

		// ws.Cells.LoadFromCollection<Payment>(payments, PrintHeaders: true);

		var fields = typeof(Payment).GetFields();
		var attributes = fields.Select(x => x.GetCustomAttributes(typeof(LabelAttribute), true).First() as LabelAttribute).ToArray();
		
		var list = new List<object[]>(payments.Count + 1)
		{
			attributes.Select(x => x.Name).ToArray(),
		};

		foreach (var payment in payments)
		{
			list.Add(fields.Select(x => x.GetValue(payment)).ToArray());
		}

		ws.Cells.LoadFromArrays(list);

		for (int i = 0; i < fields.Length; i++)
		{
			var idx = i + 1;
			var dataType = fields[i].FieldType;

			if (dataType == typeof(DateTime))
			{
				ws.Column(idx).Style.Numberformat.Format = "yyyy-MM-dd";
				ws.Column(idx).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
				ws.Column(idx).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
			}
			else if (dataType == typeof(double))
			{
				ws.Column(idx).Style.Numberformat.Format = "#,##0.00";
				ws.Column(idx).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
				ws.Column(idx).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
			}
			else
			{
				ws.Column(idx).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
				ws.Column(idx).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
			}
		}

		ws.View.FreezePanes(2, 1);
		ws.Row(1).Style.Font.Bold = true;
		ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
		ws.Row(1).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
		ws.Cells[ws.Dimension.Address].AutoFilter = true;
		ws.Cells[ws.Dimension.Address].AutoFitColumns();

		ws.PrinterSettings.PaperSize = ePaperSize.Letter;
		ws.PrinterSettings.Orientation = eOrientation.Landscape;
		ws.PrinterSettings.TopMargin = 0.2D;
		ws.PrinterSettings.BottomMargin = 0.2D;
		ws.PrinterSettings.LeftMargin = 0.2D;
		ws.PrinterSettings.RightMargin = 0.2D;
		ws.PrinterSettings.HeaderMargin = 0.2D;
		ws.PrinterSettings.FooterMargin = 0.2D;
		ws.PrinterSettings.HorizontalCentered = true;

		ep.SaveAs(new FileInfo(filename));
	}
}
