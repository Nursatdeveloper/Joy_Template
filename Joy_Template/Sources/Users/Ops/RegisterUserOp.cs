using BCrypt.Net;
using Joy_Template.Controllers;
using Joy_Template.Data.Tables;
using Joy_Template.Sources.Base;
using Joy_Template.Sources.Repository;
using Joy_Template.UiComponents.SystemUiComponents;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCTemplate.Data;

namespace Joy_Template.Sources.Users.Ops {
    //public record RegisterUserOpModel(
    //    string FirstName,
    //    string LastName,
    //    string FatherName,
    //    DateTime BirthDate,
    //    string Iin,
    //    string Email,
    //    string Password
    //);

    public class RegisterUserOpModel {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Iin { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterUserOp: RenderableOperation<RegisterUserOpModel, ApplicationDbContext> {
        public RegisterUserOp(IHtmlHelper htmlHelper, HttpContext httpContext) : base(htmlHelper, httpContext) {
            Render(() =>
                new Div()
                    .Append(
                        new Div("text-danger")
                            .WithAttr("validation-summary", "ModelOnly")
                    )
                    .Append(
                        new Div("form-group")
                            .Append(new Label(text: "FirstName").WithAttr("asp-for", "FirstName"))
                            .Append(new Input(cssClass: "form-control").WithAttr("name", "FirstName"))
                    )
                    .Append(
                        new Div("form-group")
                            .Append(new Label(text: "LastName").WithAttr("for", "LastName"))
                            .Append(new Input(cssClass: "form-control").WithAttr("name", "LastName"))
                    )
                    .Append(
                        new Div("form-group")
                            .Append(new Label(text: "FatherName").WithAttr("for", "FatherName"))
                            .Append(new Input(cssClass: "form-control").WithAttr("name", "FatherName"))
                    )
                    .Append(
                        new Div("form-group")
                            .Append(new Label(text: "BirthDate").WithAttr("for", "BirthDate"))
                            .Append(new Input(type: "date", cssClass: "form-control").WithAttr("name", "BirthDate"))
                    )
                    .Append(
                        new Div("form-group")
                            .Append(new Label(text: "Iin").WithAttr("for", "Iin"))
                            .Append(new Input(cssClass: "form-control").WithAttr("name", "Iin"))
                    )
                    .Append(
                        new Div("form-group")
                            .Append(new Label(text: "Email").WithAttr("for", "Email"))
                            .Append(new Input(cssClass: "form-control").WithAttr("name", "Email"))
                    )
                    .Append(
                        new Div("form-group")
                            .Append(new Label(text: "Password").WithAttr("for", "Password"))
                            .Append(new Input(type: "password", cssClass: "form-control").WithAttr("name", "Password"))
                    )
            );
        }

        public override async Task SetModel(RegisterUserOpModel model, ApplicationDbContext context) {
            var tbUserModel = new TbUser() {
                Firstname = model.FirstName,
                Lastname = model.LastName,
                Fathername = model.FatherName,
                BirthDate = model.BirthDate,
                Iin = model.Iin,
                Email = model.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Roles = "User",
                CreatedAt = DateTime.UtcNow,
                RowVersion = 1,
                UpdatedAt = null
            };

            await context.TbUsers.AddAsync(tbUserModel);
            await context.SaveChangesAsync();
        }
    }
    
}
