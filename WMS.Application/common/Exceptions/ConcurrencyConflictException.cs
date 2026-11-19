namespace WMS.Application.common.Exceptions
{
    public sealed class ConcurrencyConflictException(
        string message,
        Exception? innerException = null)
        : Exception(message, innerException);
}
