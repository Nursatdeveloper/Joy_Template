using Joy_Template.UiComponents.Base;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;

namespace Joy_Template.UiComponents.SystemUiComponents.Table {
    public class HtmlTable<T>: HtmlBase {
        internal List<T> _source;
        private List<string> _headers;
        private List<Func<T, object>> _funcValues;

        public HtmlTable() {
            _headers = new List<string>();
            _funcValues = new List<Func<T, object>>();
        }

        public HtmlTable<T> SetData(List<T> source) {
            _source = source;
            return this;
        }

        public virtual HtmlTable<T> Header(string[] headers) {
            _headers.AddRange(headers);
            return this;
        }
        public virtual HtmlTable<T> Presentation(params Func<T, object>[] values) {
            _funcValues.AddRange(values);
            return this;
        }

        public override HtmlString ToHtmlString(IHtmlHelper html) {
            var sb = new StringBuilder();
            sb.Append("<table class='table'>");
            sb.Append(getTableHeader());
            sb.Append(getTableBody());
            sb.Append("</table>");
            return new HtmlString(sb.ToString());
        }

        private string getTableHeader() {
            var sb = new StringBuilder();
            sb.Append("<thead>");
            sb.Append("<tr>");
            foreach(var header in _headers) {
                sb.Append($"<th scope=\"scope\">{header}</th>");
            }
            sb.Append("</tr>");
            sb.Append("</thead>");
            return sb.ToString();
        }

        private string getTableBody() {
            var sb = new StringBuilder();
            sb.Append("<tbody>");
            for(int i = 0; i < _source.Count; i++) {
                sb.Append("<tr>");
                foreach(var col in _funcValues) {
                    sb.Append($"<td>{col(_source[i]).ToString()}</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</tbody>");
            return sb.ToString();
        }
    }
}
