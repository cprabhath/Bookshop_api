using Bookshop_api.Models;
using FluentValidation;

namespace Bookshop_api.Validations
{
    public class BookValidations
    {
        public string Image { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public string Language { get; set; }
        public double Price { get; set; }

        public BookValidations(string image, string title, string author, string description, Category category, string language, double price)
        {
            Image = image;
            Title = title;
            Author = author;
            Description = description;
            Category = category;
            Language = language;
            Price = price;
        }
    }

    public class BookValidationsValidator : AbstractValidator<BookValidations>
    {
        public BookValidationsValidator()
        {
            RuleFor(x => x.Image).NotEmpty().WithMessage("Image is required");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
            RuleFor(x => x.Author).NotEmpty().WithMessage("Author is required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
            RuleFor(x => x.Language).NotEmpty().WithMessage("Language is required");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Price is required");
            RuleFor(x => x.Price).Must(x => x.GetType() == typeof(double)).WithMessage("Price must be a number");
        }
    }
}
