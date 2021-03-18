using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlanJuden.MvcReportViewer
{
	public static class CoreHtmlHelpers
	{
		public static HtmlString RenderReportViewer(this IHtmlHelper helper, ReportViewerModel model, int? startPage = 1)
		{
			var sb = new StringBuilder();

			var reportServerDomainUri = new Uri(model.ServerUrl);
			var contentData = ReportServiceHelpers.ExportReportToFormat(model, ReportFormats.Html4_0, startPage, startPage);

			sb.AppendLine("<form class='form-horizontal' id='frmReportViewer'  name='frmReportViewer'>");
			sb.AppendLine("	<div class='ReportViewer row'>");
			sb.AppendLine("		<div class='ReportViewerHeader row'>");
            sb.AppendLine("			<div class='ParametersContainer col-12'>");
            sb.AppendLine("				<div class='Parameters col-12  p-3'>");

            sb.AppendLine(ParametersToHtmlString(contentData.Parameters, model));

            sb.AppendLine("				</div>");

            
            sb.AppendLine("			</div>");
            sb.AppendLine("				<div class='ReportViewerViewReport col-2 text-center'>");
            sb.AppendLine("					<button type='button' class='btn btn-primary ViewReport'>Ver Reporte</button>");
            sb.AppendLine("				</div>");

            sb.AppendLine("			<div class='ReportViewerToolbar row'>");
			sb.AppendLine("				<div class='ReportViewerPager col-12'>");
			sb.AppendLine("					<div class='btn-toolbar'>");

			if (model.EnablePaging)
			{ 
				sb.AppendLine("						<div class='btn-group'>");
				sb.AppendLine($"							<a href='#' title='Primera página' class='btn btn-default FirstPage'{(contentData.TotalPages == 1 ? " disabled='disabled'" : "")} style='color: steelblue;'><i class='fa fa-step-backward' aria-hidden='true'></i></a>");
				sb.AppendLine($"							<a href='#' title='Página Anterior' class='btn btn-default PreviousPage'{(contentData.TotalPages == 1 ? " disabled='disabled'" : "")} style='color: steelblue;'><i class='fa fa-chevron-left' aria-hidden='true'></i></span></a>");
                sb.AppendLine("						</div>");
                sb.AppendLine("						<div class='btn-group'>");
                sb.AppendLine($"							<span class='PagerNumbers'><input readonly='readonly' type='text' id='ReportViewerCurrentPage' name='ReportViewerCurrentPage' class='form-control' style='display: inline-block' value='{contentData.CurrentPage}' /><span> de</span> <span id='ReportViewerTotalPages'>{contentData.TotalPages}</span></span>");
                sb.AppendLine("						</div>");
                sb.AppendLine("						<div class='btn-group'>");
                sb.AppendLine($"							<a href='#' title='Siguiente página' class='btn btn-default NextPage'{(contentData.TotalPages == 1 ? " disabled='disabled'" : "")} style='color: steelblue;'><i class='fa fa-chevron-right' aria-hidden='true'></i></span></a>");
				sb.AppendLine($"							<a href='#' title='Última página' class='btn btn-default LastPage'{(contentData.TotalPages == 1 ? " disabled='disabled'" : "")} style='color: steelblue;'><i class='fa fa-step-forward' aria-hidden='true'></i></span></a>");
                sb.AppendLine("						</div>");
            }
            sb.AppendLine("						<div class='btn-group'>");
            sb.AppendLine("							<span class='SearchText' >");
            sb.AppendLine($"								<input type='text' id='ReportViewerSearchText' name='ReportViewerSearchText' class='form-control' value='' style='display: inline-block'/>");
            sb.AppendLine($"								<a href='#' title='Buscar' class='btn btn-info FindTextButton'><i class='fa fa-search' aria-hidden='true' style='padding-right: .5em;'></i></a>");
            sb.AppendLine("							</span>");
            sb.AppendLine("						</div>");
            sb.AppendLine("						<div class='btn-group'>");
            sb.AppendLine("							<a href='#' title='Exportar' class='dropdown-toggle btn btn-default' data-toggle='dropdown' role='button' aria-haspopup='true' area-expanded='false'>");
			sb.AppendLine("								<i class='fa fa-save' aria-hidden='true' style='color: steelblue;'></i>");
			sb.AppendLine("								<span class='caret'></span>");
			sb.AppendLine("							</a>");
			sb.AppendLine("							<ul class='dropdown-menu'>");
			sb.AppendLine("								<li class='dropdown-item'><a href='#' class='ExportCsv' style='color: black;'>CSV (comma delimited)</a></li>");
			sb.AppendLine("								<li class='dropdown-item'><a href='#' class='ExportExcelOpenXml' style='color: black;'>Excel</a></li>");
			//sb.AppendLine("								<li><a href='#' class='ExportMhtml'>MHTML (web archive)</a></li>");
			sb.AppendLine("								<li class='dropdown-item'><a href='#' class='ExportPdf' style='color: black;'>PDF</a></li>");
			//sb.AppendLine("								<li><a href='#' class='ExportTiff'>TIFF file</a></li>");
			sb.AppendLine("								<li class='dropdown-item'><a href='#' class='ExportWordOpenXml' style='color: black;'>Word</a></li>");
			sb.AppendLine("								<li class='dropdown-item'><a href='#' class='ExportXml' style='color: black;'>XML file with report data</a></li>");
			sb.AppendLine("							</ul>");
			//sb.AppendLine("						</div>");
			//sb.AppendLine("						<div class='btn-group'>");
			sb.AppendLine("							<a href='#' title='Refrescar' class='btn btn-default Refresh'><i class='fa fa-refresh' aria-hidden='true' style='color: green;'></i></a>");
			//sb.AppendLine("						</div>");
			//sb.AppendLine("						<div class='btn-group'>");
			sb.AppendLine("							<a href='#' title='Imprimir' class='btn btn-default Print'><i class='fa fa-print' aria-hidden='true' style='color: grey;'></i></a>");
			sb.AppendLine("						</div>");
			sb.AppendLine("					</div>");
			sb.AppendLine("				</div>");
			sb.AppendLine("			</div>");
			sb.AppendLine("		</div>");
			sb.AppendLine("		<div class='ReportViewerContentContainer'>");
			sb.AppendLine("			<div class='ReportViewerContent'>");

			if (model.IsMissingAnyRequiredParameterValues(contentData.Parameters))
			{
				sb.AppendLine("			<div class='ReportViewerInformation'>Complete los parámetros y ejecute el reporte...</div>");
			}
			else
			{
				if (model.AjaxLoadInitialReport)
				{
					sb.AppendLine("			<script type='text/javascript'>$(document).ready(function () { viewReportPage(1); });</script>");
				}
				else
				{
					if (contentData == null || contentData.ReportData == null || contentData.ReportData.Length == 0)
					{
						sb.AppendLine("");
					}
					else
					{
						var content = model.Encoding.GetString(contentData.ReportData);

						if (model.UseCustomReportImagePath && model.ReportImagePath.HasValue())
						{
							content = ReportServiceHelpers.ReplaceImageUrls(model, content);
						}

						sb.AppendLine($"			{content}");
					}
				}
			}

			sb.AppendLine("			</div>");
			sb.AppendLine("		</div>");
			sb.AppendLine("	</div>");
			sb.AppendLine("</form>");

			sb.AppendLine("<script type='text/javascript'>");
			sb.AppendLine("	function ReportViewer_Register_OnChanges() {");

			var dependencyFieldKeys = new List<string>();
			foreach (var parameter in contentData.Parameters.Where(x => x.Dependencies != null && x.Dependencies.Any()))
			{
				foreach (var key in parameter.Dependencies)
				{
					if (!dependencyFieldKeys.Contains(key))
					{
						dependencyFieldKeys.Add(key);
					}
				}
			}

			foreach (var queryParameter in contentData.Parameters.Where(x => dependencyFieldKeys.Contains(x.Name)))
			{
				sb.AppendLine("		$('#" + queryParameter.Name + "').change(function () {");
				sb.AppendLine("			reloadParameters();");
				sb.AppendLine("		});");
			}

			sb.AppendLine("	}");

			sb.AppendLine("</script>");

			return new HtmlString(sb.ToString());
		}

		public static string ParametersToHtmlString(System.Collections.Generic.List<ReportParameterInfo> parameters, ReportViewerModel model)
		{
			StringBuilder sb = new StringBuilder();

			if (parameters == null)
			{
				var contentData = new ReportExportResult();
				var definedParameters = ReportServiceHelpers.GetReportParameters(model, true);
				contentData.SetParameters(definedParameters, model.Parameters);
				parameters = contentData.Parameters;
			}
            int cantParametros = parameters.Count;
            int parametroActual = 0;
			//Parameters start
			foreach (var reportParameter in parameters)
			{
                
                
				
				if (reportParameter.PromptUser || model.ShowHiddenParameters)
				{
                    sb.AppendLine("					<div class='form-group'>");

                    sb.AppendLine($"						<div class='control-label col-6 col-md-6 text-left p-0'><label for='{reportParameter.Name}'>{reportParameter.Prompt.HtmlEncode()}</label></div>");

					sb.AppendLine("							<div class='col-md-12 col-12 col-lg-6 p-0'>");
					if (reportParameter.ValidValues != null && reportParameter.ValidValues.Any())
					{
						sb.AppendLine($"						<select id='{reportParameter.Name}' name='{reportParameter.Name}' class='form-control' {(reportParameter.MultiValue == true ? "multiple='multiple'" : "")}>");
						foreach (var value in reportParameter.ValidValues)
						{
							sb.AppendLine($"							<option value='{value.Value}' {(reportParameter.SelectedValues.Contains(value.Value) ? "selected='selected'" : "")}>{value.Label.HtmlEncode()}</option>");
						}
						sb.AppendLine($"						</select>");
					}
					else
					{
						var selectedValue = reportParameter.SelectedValues.FirstOrDefault();

						if (reportParameter.Type == ReportService.ParameterTypeEnum.Boolean)
						{
							sb.AppendLine($"						<input type='checkbox' id='{reportParameter.Name}' name='{reportParameter.Name}' class='form-check-input' {(selectedValue.ToBoolean() ? "checked='checked'" : "")} />");
						}
						else if (reportParameter.Type == ReportService.ParameterTypeEnum.DateTime)
						{
							sb.AppendLine($"						<input type='datetime' id='{reportParameter.Name}' name='{reportParameter.Name}' class='form-control form-control-sm' value='{selectedValue}' />");
						}
						else
						{
							sb.AppendLine($"						<input type='text' id='{reportParameter.Name}' name='{reportParameter.Name}' class='form-control form-control-sm' value='{selectedValue}' />");
						}
					}

					sb.AppendLine("							</div>");

                    //cierra row
                    sb.AppendLine("					</div>");
                    

                    parametroActual++;
                }
				else
				{
					if (reportParameter.SelectedValues != null && reportParameter.SelectedValues.Any())
					{
						var values = reportParameter.SelectedValues.Where(x => x != null).Select(x => x).ToArray();
						sb.AppendLine($"			<input type='hidden' id='{reportParameter.Name}' name='{reportParameter.Name}' value='{String.Join(",", values)}' />");
					}
				}

				sb.AppendLine($"			<input type='hidden' id='ReportViewerEnablePaging' name='ReportViewerEnablePaging' value='{model.EnablePaging}' />");

				//sb.AppendLine("					</div>");
                
                

            }

			return sb.ToString();
		}
	}
}
