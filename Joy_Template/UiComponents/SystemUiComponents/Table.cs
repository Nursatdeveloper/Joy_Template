using Joy_Template.UiComponents.Base;
using Microsoft.AspNetCore.Html;
using MVCTemplate.Sources.Repository;
using System.Text;

namespace Joy_Template.UiComponents.SystemUiComponents {
    public class Table<TModel> : PairedHtmlTag where TModel : TbBase {
        private List<TModel> _source;
        private List<string> _headers;
        private List<Func<TModel, object>> _funcValues;

        public Table(List<TModel> source) {
            _source = source;
            _headers = new List<string>();
            _funcValues = new List<Func<TModel, object>>();
        }

        public Table<TModel> Header(string[] headers) {
            _headers.AddRange(headers);
            return this;
        }
        public Table<TModel> Column(params Func<TModel, object>[] values) {
            _funcValues.AddRange(values);
            return this;
        }

        public override HtmlString ToHtmlString() {
            var sb = new StringBuilder();
            sb.Append("<table class=\"table\">");
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
            return new HtmlString(sb.ToString());
        }
    }
}
