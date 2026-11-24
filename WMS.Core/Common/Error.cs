namespace WMS.Core.Common
{
    public sealed record Error(string Title, ErrorType Type, string Description);
    public static class Errors<T>
    {
        public static Error ObjectExists =>
            new("Object exists" , ErrorType.Conflict, $"{typeof(T).Name} already exists.");
        public static Error NotFound =>
            new("Object not found", ErrorType.NotFound, $"{typeof(T).Name} not found");
        public static Error ObjectNotExists(int id) =>
            new("Object not exists", ErrorType.NotFound, $"{typeof(T).Name} with ID: {id} not exists");
        public static Error ObjectCanNotBeDelete =>
            new($"Object can not be delete", ErrorType.Conflict, $"{typeof(T).Name} has reference to other object in Database");
        public static Error ValidationProductFailure(string message) =>
            new("Validation invalid", ErrorType.Failure, $"Product has invalid data. {message}");
        public static Error InvalidProperty(string propertyName) =>
            new("Invalid format", ErrorType.Validation, $"{typeof(T).Name} - {propertyName} has invalid format");
        public static Error ObjectWithIdExists(object id) =>
            new("Object exists", ErrorType.Conflict, $"{typeof(T).Name} with id: {id} already exists.");
        public static Error OperationFailed =>
            new("Operation Failed", ErrorType.Failure, $"Operation on {typeof(T).Name} failed");
        public static Error InsufficeintQuantity =>
            new("InsufficeintQ uantity", ErrorType.Failure, $"{typeof(T).Name} has not enough qunaitity.");
    }
    public static class Errors
    {
        public static Error InvalidFileFormat(string extension) =>
            new("Invalid format", ErrorType.Failure, $"Extension: '{extension}' is not supported");
        public static Error Unauthorized =>
            new("Invalid data", ErrorType.AccessUnAuthorized, $"Invalid authorize data");
        public static Error NotExists =>
            new("Bad Request", ErrorType.BadRequest, $"Object not exists");
        public static Error CanNotChangeStatus =>
            new("Bad Request", ErrorType.BadRequest, "Cannot change order status.");
        public static Error RoleNotExists =>
            new("Bad Request", ErrorType.BadRequest, "Role not exists.");
        public static Error ConcurrencyConflict =>
             new("Concurrency conflict", ErrorType.Conflict, "The data was modified by another operation. Please try again.");

        public static Error NotAssignedToYou =>
             new("Task not assigned to you", ErrorType.AccessForbidden, "Only the assigned warehouseman can work on this task.");
        public static Error InvalidStatus(string status) =>
            new("Invalid task status", ErrorType.Conflict, $"The task cannot be {status} in its current status.");
        public static Error WorkerNotAvailable(int userId) =>
            new("Worker not available", ErrorType.BadRequest, $"User with ID: {userId} is not an active warehouseman.");
        public static Error CannotAssign =>
            new("Cannot assign task", ErrorType.Conflict, "Only a pending or assigned task can be assigned to a warehouseman.");
        public static Error InvalidDataFormat =>
            new("Invalid data", ErrorType.BadRequest, "Input date has invalid format");
    }
}