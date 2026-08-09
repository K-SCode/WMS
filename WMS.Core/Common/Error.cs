namespace WMS.Core.Common
{
    public sealed record Error(string Title, ErrorType Type, string Description);
    public static class Errors<T>
    {
        public static Error ObjectExists { get; } = 
            new("Object exists" , ErrorType.Conflict, $"{typeof(T).Name} already exists.");
        
    }
}