namespace PizzeriaContracts.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public string Title { get; private set; }

        public bool Visible { get; private set; }

        public int Width { get; private set; }

        public GridViewAutoSize GridViewAutoSize { get; private set; }

        public bool IsUseAutoSize { get; private set; }

        public string Format { get; private set; }

        public ColumnAttribute(string title = "", bool visible = true, int width = 0,
            GridViewAutoSize gridViewAutoSize = GridViewAutoSize.None, bool isUseAutoSize = false,
            string format = "")
        {
            Title = title;
            Visible = visible;
            Width = width;
            GridViewAutoSize = gridViewAutoSize;
            IsUseAutoSize = isUseAutoSize;
            Format = format;
        }
    }
}
