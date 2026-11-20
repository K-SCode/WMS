namespace WMS.Api.Extensions
{
    public  static class ETagHelper
    {
        public static string ToEtag(this byte[] version) =>
            $"\"{Convert.ToBase64String(version)}\"";
        
        public static byte[] Parse(string? ifMatch)
        {
            if (string.IsNullOrWhiteSpace(ifMatch))
                return [];
            try
            {
                return Convert.FromBase64String(ifMatch.Trim('"'));
            }
            catch(FormatException)
            {
                return [];
            }
        }
    }
}
