using Joy_Template.UiComponents.Base;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Primitives;
using MVCTemplate.Sources.Repository;
using System.Text;

namespace Joy_Template.UiComponents.SystemUiComponents {
    public enum FieldType {
        Text,
        DateTime,
        Select
    }
    public record FilterData( string FieldName, FieldType Type);
    public class Table<TModel> : PairedHtmlTag where TModel : TbBase {
        private List<TModel> _source;
        private List<string> _headers;
        private List<Func<TModel, object>> _funcValues;
        private List<FilterData> _funcFilters;
        private bool _enableSearch;

        public Table(List<TModel> source, bool enableSearch = true) {
            _source = source;
            _headers = new List<string>();
            _funcValues = new List<Func<TModel, object>>();
            _funcFilters = new List<FilterData>();
            _enableSearch = enableSearch;
        }

        public Table<TModel> Filter(FilterData[] filters) {
            _funcFilters.AddRange(filters);
            return this;
        }

        public Table<TModel> Header(string[] headers) {
            _headers.AddRange(headers);
            return this;
        }
        public Table<TModel> Column(params Func<TModel, object>[] values) {
            _funcValues.AddRange(values);
            return this;
        }

        private string getPaginationHtml() {
            StringBuilder sb = new StringBuilder();
            
            sb.Append("<ul class=\"pagination \">");
            sb.Append("<li class=\"page-item\">\r\n      " +
                $"          <a class=\"page-link\" href=\"#\" aria-label=\"Previous\">\r\n        " +
                $"              <span aria-hidden=\"true\">&laquo;</span>\r\n      " +
                $"          </a>\r\n    " +
                $"      </li>\r\n    " +
                $"      <li class=\"page-item\"><a class=\"page-link\" href=\"#\">1</a></li>\r\n    " +
                $"      <li class=\"page-item\">\r\n      " +
                $"          <a class=\"page-link\" href=\"#\" aria-label=\"Next\">\r\n        " +
                $"              <span aria-hidden=\"true\">&raquo;</span>\r\n      " +
                $"          </a>\r\n    " +
                $"      </li>\r\n  ");
            sb.Append("</ul>");
            return sb.ToString();
        }

        private string getFilterPanelHtml() {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='container mb-2'>");
            if(_funcFilters.Count < 4) {
                sb.Append("<div class='row'>");
                _funcFilters.ForEach(filter => {
                    sb.Append($"<div class='col-3'>{getEditorHtml(filter)}</div>");
                });
                sb.Append("</div>");

            } else {
                var count = 0;
                _funcFilters.ForEach(filter => {
                    if(count == 0) {
                        sb.Append("<div class='row'>");
                    }
                    sb.Append("<div class='col-3'>");
                    sb.Append(getEditorHtml(filter));
                    sb.Append("</div>");
                    count++;
                    if(count == 4 || count == _funcFilters.Count) {
                        count = 0;
                        sb.Append("</div>");

                    }
                });
            }
            
            sb.Append("</div>");
            return sb.ToString();
        }

        private string getEditorHtml(FilterData filterData) {
            var editor = filterData.Type switch {
                FieldType.Text => "<div class=\"form-floating mb-3\">\r\n  " +
                                    $"<input type=\"text\"class=\"form-control\" id=\"floatingInput\" placeholder=\"{filterData.FieldName}\">\r\n  " +
                                    $"<label for=\"floatingInput\">{filterData.FieldName}</label>\r\n" +
                                  "</div>",
                FieldType.DateTime => "<div class=\"form-floating mb-3\">\r\n  " +
                                        $"<input type=\"datetime\" class=\"form-control\" id=\"floatingInput\" placeholder=\"{filterData.FieldName}\">\r\n  " +
                                        $"<label for=\"floatingInput\">{filterData.FieldName}</label>\r\n" +
                                      "</div>",
                FieldType.Select => "<div class=\"form-floating\">\r\n  " +
                                        "<select class=\"form-select\" id=\"floatingSelect\" aria-label=\"Floating label select example\">\r\n    " +
                                            "<option selected>Select</option>\r\n    " +
                                            "<option value=\"1\">One</option>\r\n    " +
                                            "<option value=\"2\">Two</option>\r\n    " +
                                            "<option value=\"3\">Three</option>\r\n  " +
                                        "</select>\r\n  " +
                                        $"<label for=\"floatingSelect\">{filterData.FieldName}</label>\r\n" +
                                    "</div>",
            };
            return editor;
        }

        public override HtmlString ToHtmlString(IHtmlHelper htmlHelper) {
            var sb = new StringBuilder();
            if(_enableSearch) {
                sb.Append(getFilterPanelHtml());
            }
            //sb.Append("<nav aria-label=\"Page navigation\">");
            sb.Append("<div class='container'>");
            sb.Append("<div class='row'>");
            if(_enableSearch) {
                sb.Append($"<div class='col col-3 float-start'><a class='btn btn-primary'>Search</a></div>");
            } else {
                sb.Append($"<div class='col-3 float-start'></div>");
            }
            sb.Append($"<div class='col col-6'></div>");
            sb.Append($"<div class='col col-3 position-relative'><div style='position:absolute; right:10px;'>{getPaginationHtml()}</div></div>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class='container'>");
            sb.Append("<table class=\"table mt-2\">");
            sb.Append("<thead>");
            sb.Append("<tr>");
            _headers.ForEach(header => sb.Append($"<th scope=\"col\">{header}</th>"));
            sb.Append("</tr>");
            sb.Append("</thead>");

            sb.Append("<tbody>");
            _source.ForEach(row => {
                sb.Append("<tr>");
                _funcValues.ForEach(value => sb.Append($"<td>{value(row)?.ToString()}</td>"));
                sb.Append("</tr>");

            });
            sb.Append("</tbody>");
            sb.Append("</table>");
            sb.Append("</div>");
            return new HtmlString(sb.ToString());
        }
    }
}
