using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;

namespace Joy_Template.UiComponents.Base {
    public interface IHtmlHelperFactory {
        IHtmlHelper Create();

    }

    public class HtmlHelperFactory: IHtmlHelperFactory {
        private readonly IHttpContextAccessor _contextAccessor;

        public class JoyView: IView {
            public Task RenderAsync(ViewContext context) {
                return Task.CompletedTask;
            }

            public string Path { get; } = "View";
        }

        public HtmlHelperFactory(IHttpContextAccessor contextAccessor) {
            _contextAccessor = contextAccessor;
        }

        public IHtmlHelper Create() {
            var modelMetadataProvider = _contextAccessor.HttpContext.RequestServices.GetRequiredService<IModelMetadataProvider>();
            var tempDataProvider = _contextAccessor.HttpContext.RequestServices.GetRequiredService<ITempDataProvider>();
            var htmlHelper = _contextAccessor.HttpContext.RequestServices.GetRequiredService<IHtmlHelper>();
            var viewContext = new ViewContext(
                new ActionContext(_contextAccessor.HttpContext, _contextAccessor.HttpContext.GetRouteData(), new ControllerActionDescriptor()),
                new JoyView(),
                new ViewDataDictionary(modelMetadataProvider, new ModelStateDictionary()),
                new TempDataDictionary(_contextAccessor.HttpContext, tempDataProvider),
                TextWriter.Null,
                new HtmlHelperOptions()
            );

            ((IViewContextAware)htmlHelper).Contextualize(viewContext);
            return htmlHelper;
        }
    }

    public interface IHtmlHelperFactory<TModel> {
        IHtmlHelper<TModel> Create();
    }

    public class HtmlHelperFactory<TModel>: IHtmlHelperFactory<TModel> {
        private readonly IHttpContextAccessor _contextAccessor;

        public class JoyView: IView {
            public Task RenderAsync(ViewContext context) {
                return Task.CompletedTask;
            }

            public string Path { get; } = "View";
        }

        public HtmlHelperFactory(IHttpContextAccessor contextAccessor) {
            _contextAccessor = contextAccessor;
        }


        IHtmlHelper<TModel> IHtmlHelperFactory<TModel>.Create() {
            var modelMetadataProvider = _contextAccessor.HttpContext.RequestServices.GetRequiredService<IModelMetadataProvider>();
            var tempDataProvider = _contextAccessor.HttpContext.RequestServices.GetRequiredService<ITempDataProvider>();
            var htmlHelper = _contextAccessor.HttpContext.RequestServices.GetRequiredService<IHtmlHelper<TModel>>();
            var viewContext = new ViewContext(
                new ActionContext(_contextAccessor.HttpContext, _contextAccessor.HttpContext.GetRouteData(), new ControllerActionDescriptor()),
                new JoyView(),
                new ViewDataDictionary<TModel>(modelMetadataProvider, new ModelStateDictionary()),
                new TempDataDictionary(_contextAccessor.HttpContext, tempDataProvider),
                TextWriter.Null,
                new HtmlHelperOptions()
            );

            ((IViewContextAware)htmlHelper).Contextualize(viewContext);
            return htmlHelper;
        }
    }
}
