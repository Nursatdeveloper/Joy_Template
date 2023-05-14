using Joy_Template.UiComponents.Base;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using MVCTemplate.Sources.Repository;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;

namespace Joy_Template.UiComponents.SystemUiComponents.Table {
    public class SearchTable<T> : HtmlTable<T> where T : TbBase {
        private List<FilterData<T>> _filters;
        private SubmitArgs _submitArgs;
        private DbContext _dbContext;
        private IFormCollection _form;

        public SearchTable(DbContext dbContext) {
            _dbContext = dbContext;
            _filters = new List<FilterData<T>>();
        }
        public SearchTable<T> OnSearch(SubmitArgs submitArgs) {
            _submitArgs = submitArgs;
            return this;
        }

        public SearchTable<T> Filter(FilterData<T>[] filters) {
            _filters.AddRange(filters);
            return this;
        }

        public override SearchTable<T> Header(string[] headers) {
            base.Header(headers);
            return this;
        }
        public override SearchTable<T> Presentation(params Func<T, object>[] values) {
            base.Presentation(values);
            return this;
        }
        public void Print(IHtmlHelper htmlHelper, HttpContext httpContext, IFormCollection formCollection, out HtmlString html) {
            _form = formCollection;
            setData();

            new FormComponent(htmlHelper, httpContext)
                .OnSubmit(_submitArgs)
                .OnRefresh(new SubmitArgs(_submitArgs.Controller, _submitArgs.Action, "Refresh", "btn btn-outline-secondary btn-sm"))
                .Render(() => getFilteringPanel())
            .ToHtmlString(out var filterPanel);

            var table = base.ToHtmlString(htmlHelper);

            var panel = new StringBuilder();
            panel.Append("<div class='shadow-sm p-3 mt-3 mb-3 bg-white rounded'>");
            panel.Append(filterPanel.Value);
            panel.Append(table.Value);
            panel.Append("</div>");

            html = new HtmlString(panel.ToString());
        }

        private void setData() {
            var objects = _dbContext.Set<T>().ToList();
            var tryGetIsRefreshClicked = _form.TryGetValue("Refresh", out var value);
            if(value == "true") {
                base.SetData(objects);
                return;
            }
            var filteredObjects = new List<T>();
            if(_form == null) {
                throw new Exception("formCollection is null!");
            }
            foreach(var obj in objects) {
                filteredObjects.AddIf(obj, filter(obj));
            }

            base.SetData(filteredObjects);
        }

        private bool filter(T obj) {
            var ret = false;
            foreach(var filter in _filters) {
                var valueExistsInForm = _form.TryGetValue(filter.Field, out var value);
                if(valueExistsInForm) {
                    switch(filter.FieldType) {
                        case FieldType.Text:
                            var objectPropertyValue = filter.PropertyGetFunc(obj)?.ToString() ?? string.Empty;
                            if(filter.SearchType == SearchType.Equals) {
                                if(objectPropertyValue == value) {
                                    return true;
                                }
                            } else {
                                if(objectPropertyValue.Contains(value)) {
                                    return true;
                                }
                            }
                            break;
                        case FieldType.DateTime:
                            break;
                    }
                } else {
                    ret = true;
                }
            }
            return ret;
        }

        private HtmlBase getFilteringPanel() {
            var div = new Div();
            var filterChunkies = _filters.Chunk(4);
            foreach(var filterChunk in filterChunkies) {
                var row = new Div("row");
                foreach(var filter in filterChunk) {
                    row.Append(new Div("col")
                        .Append(new Div("form-group")
                            .Append(new Label(text: filter.Label).WithAttr("asp-for", filter.Field))
                            .Append(new Input(cssClass: "form-control").WithAttr("name", filter.Field))
                        )
                    );
                }
                div.Append(row);
            }
            return div;
        }
    }
}
