namespace WMS.Core.Common
{
    public sealed record Error(string Title, ErrorType Type, string Description);
    public static class Errors<T>
    {
        public static Error ObjectExists { get; } = 
            new("Object exists" , ErrorType.Conflict, $"{typeof(T).Name} already exists.");
        public static Error NotFound { get; } =
            new("Object not found", ErrorType.NotFound, $"{typeof(T).Name} not found");
        public static Error ObjectNotExists(int id) =>
            new("Object not exists", ErrorType.NotFound, $"{typeof(T).Name} with ID: {id} not exists");
        public static Error ObjectCanNotBeDelete =>
            new($"Object can not be delete", ErrorType.Conflict, $"{typeof(T).Name} has reference to other object in Database");
    }
}