using FluentValidation;
using Mediator;
using Moq;
using Shouldly;
using WMS.Application.Behaviors;
using WMS.Core.Common;
using Xunit;

namespace WMS.Tests.Services
{
    public class ValidationBehaviourTests
    {
        private sealed record CommandTest(string Name) : ICommand<Result<int>>;

        private int _nextCalls;

        private ValueTask<Result<int>> Next(CommandTest command, CancellationToken ct)
        {
            _nextCalls++;
            return new ValueTask<Result<int>>(Result<int>.Success(42));
        }
        [Fact]
        public async Task Handle_ValidationErrors_ReturnsValidationErrorAndDoesNotCallNext()
        {
            //arrange
            var sut = CreateSut(NameRequired("Name is required"));
            
            //act
            var result = await sut.Handle(new CommandTest(""),Next,CancellationToken.None);

            //assert
            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldNotBeNull();
            result.Error.Type.ShouldBe(ErrorType.Validation);
            result.Error.Title.ShouldBe("Validation Error");
            result.Error.Description.ShouldBe("Name: Name is required");
            _nextCalls.ShouldBe(0);
        }
        [Fact]

        public async Task Handle_NoValidationErrors_CallsNextAndReturnItsResult()
        {
            //arrange
            var sut = CreateSut(NameRequired("Name is required"));
            
            //act
            var result = await sut.Handle(new CommandTest("name"),Next,CancellationToken.None);

            //assert
            result.IsSuccess.ShouldBeTrue();
            result.Value.ShouldBe(42);
            _nextCalls.ShouldBe(1);
        }

        [Fact]
        public async Task Handle_NoValidatorsRegistered_CallsNext()
        {
            //arrange
            var sut = CreateSut();

            //act
            var result = await sut.Handle(new CommandTest(""), Next, CancellationToken.None);

            //assert
            result.IsSuccess.ShouldBeTrue();
            _nextCalls.ShouldBe(1);
        }
        [Fact]
        public async Task Handle_ErrorsFromMultipleValidatators_AreJoinWithNewLine()
        {
            //arrange
            var sut = CreateSut(NameRequired("Name first is required"), NameRequired("Name second is required"));

            //act
            var result = await sut.Handle(new CommandTest(""), Next, CancellationToken.None);

            //assert
            result.Error.ShouldNotBeNull();
            result.Error.Description.ShouldBe("Name: Name first is required\nName: Name second is required");
            _nextCalls.ShouldBe(0);
        }
        private static InlineValidator<CommandTest> NameRequired(string message)
        {
            var validator = new InlineValidator<CommandTest>();
            validator.RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage(message);
            return validator;
        }
        private static ValidationBehavior<CommandTest,Result<int>> CreateSut(
            params IValidator<CommandTest>[] validators) 
        {
            return new(validators);
        }
    }
}
