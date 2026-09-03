using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Core.Common
{
    public interface IValidationFailureFactory<TSelf> where TSelf : Result
    {
        static abstract TSelf ValidationFailure(Error error);
    }
    public class Result : IValidationFailureFactory<Result>
    {
        public bool IsSuccess { get; }
        public Error? Error { get; }
        protected Result(bool isSuccess, Error? error) 
        {
            IsSuccess = isSuccess;
            Error = error;
        }
        public static Result Success() => new(true, null);
        public static Result Failure(Error error) =>
            new(false, error ?? throw new ArgumentNullException(nameof(error)));

        public static Result ValidationFailure(Error error) => Failure(error);

        public static implicit operator Result(Error error) => Failure(error);

    }

    public sealed class Result<T> : Result, IValidationFailureFactory<Result<T>>
    {
        private readonly T? _value;
        private Result(T? value) : base(true, null) => _value = value;
        private Result(Error error) : base(false, error) { }

        public T Value => IsSuccess 
            ? _value! 
            : throw new InvalidOperationException("Unable to obtain value of invalid operation.");
        
        public static Result<T> Success(T value) => new (value);
        public static new Result<T> ValidationFailure(Error error) => new(error);

        public static implicit operator Result<T>(T value) => new(value);

        public static implicit operator Result<T>(Error error) => new(error);
    }
}
