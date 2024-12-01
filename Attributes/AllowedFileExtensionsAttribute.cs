using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace Exam.Attributes
{

    // Custom validation attribute to restrict uploaded files to specific allowed extensions.
    public class AllowedFileExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        // Constructor to initialize the allowed extensions.
        public AllowedFileExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        // Override to perform the validation logic for file extensions.
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!_extensions.Contains(extension))
                {
                    return new ValidationResult($"Only image files ({string.Join(", ", _extensions)}) are allowed.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
