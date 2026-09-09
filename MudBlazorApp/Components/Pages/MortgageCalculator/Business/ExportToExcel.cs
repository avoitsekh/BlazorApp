using OfficeOpenXml;
using System.Reflection;

namespace MudBlazorApp.Components.Pages.MortgageCalculator;

public static class ExportToExcel
{
	static ExportToExcel()
	{
		ExcelPackage.License.SetNonCommercialPersonal("Blazor Portfolio Project - AV");
	}

	public static byte[] GetByteArray(PaymentCollection payments)
	{
		var columnNames = new string[]
		{
			Constants.No,
			Constants.PaymentDate,
			Constants.Year,
			Constants.Interest,
			Constants.InterestAccrualPeriod,
			Constants.InterestAmount,
			Constants.PrincipalAmount,
			Constants.ExtraAmount,
			Constants.PaymentAmount,
			Constants.RemainingBalance,
		};

		var fields = new string[]
		{
			nameof(Payment.Number),
			nameof(Payment.PaymentDate),
			nameof(Payment.Year),
			nameof(Payment.IterestRate),
			nameof(Payment.InterestAccrualPeriod),
			nameof(Payment.InterestAmount),
			nameof(Payment.PrincipalAmount),
			nameof(Payment.ExtraAmount),
			nameof(Payment.PaymentAmount),
			nameof(Payment.Balance),
		};

		var table = new List<object[]>(payments.Count + 1);
		table.Add(columnNames);

		var objType = typeof(Payment);
		var fieldInfos = fields.Select(objType.GetField).ToList();

		foreach (var payment in payments)
		{
			var values = fieldInfos.Select(x => x.GetValue(payment)).ToArray();
			table.Add(values);
		}

		using ExcelPackage ep = new();
		using ExcelWorksheet ws = ep.Workbook.Worksheets.Add("Amortization Schedule");

		ws.Cells.Style.Font.Name = "Arial";
		ws.Cells.Style.Font.Size = 9.0f;
		ws.Cells.LoadFromArrays(table);

		for (int i = 0; i < fieldInfos.Count; i++)
		{
			var idx = i + 1;
			var dataType = fieldInfos[i].FieldType;

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

		return ep.GetAsByteArray();
	}
}
