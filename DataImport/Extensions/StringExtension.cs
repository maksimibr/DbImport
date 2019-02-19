namespace DataImport.Extensions
{
    internal static class StringExtension
    {
        internal static string RemoveLast(this string text) => text.Length < 1 ? text : text.Remove(text.Length - 1);
    }
}
