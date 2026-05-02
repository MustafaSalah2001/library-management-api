using FluentValidation;
using Library_Management_System_API.Dto;

namespace Library_Management_System_API.Validators;

public class CreateBookDtoValidator : AbstractValidator<CreateBookDto>
{
    public CreateBookDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required");

        RuleFor(x => x.Author)
            .NotEmpty().WithMessage("Author is required");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category is required");

        RuleFor(x => x.ISBN)
            .NotEmpty().WithMessage("ISBN is required");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative");
    }
}