namespace PrintEngine.Core
{
    public class TemplateAttribute : Attribute
    {
        public string[] Templates { get; set; }
        public string[] Files { get; set; }
    }
}
