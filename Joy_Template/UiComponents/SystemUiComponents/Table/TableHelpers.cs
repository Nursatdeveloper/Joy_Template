namespace Joy_Template.UiComponents.SystemUiComponents.Table {
    public record FilterData<T>(string Label, string Field, FieldType FieldType, SearchType SearchType, Func<T, object> PropertyGetFunc);
    public record SubmitArgs(string Controller, string Action, string BtnText = "Submit", string BtnCssClass = "btn btn-primary btn-sm");
    public enum FieldType {
        Text,
        DateTime
    }

    public enum SearchType {
        Equals,
        LessThan,
        GreaterThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        InRange,
        Contains
    }
}
