using Microsoft.AspNetCore.Http;
using Exam.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.Models
{
    public class Item : IValidatableObject
    {
        public int ItemId { get; set; }

        // Validation for the Name property
        [Required(ErrorMessage = "Name is required.")]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        // Validation for the Food_Group property
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}", ErrorMessage = "Food Group must be letters and between 2 to 20 characters.")]
        [Display(Name = "Food Group")]
        public string Food_Group { get; set; } = string.Empty;

        // Validation for Energi_Kj
        [RegularExpression(@"[0-9., \-]{1,20}", ErrorMessage = "Energy per 100g must be numbers and between 1 to 20 characters.")]
        [Display(Name = "Energy per 100g")]
        public double Energi_Kj { get; set; } = 0;

        // Validation for Fett
        [RegularExpression(@"[0-9., \-]{1,20}", ErrorMessage = "Fat per 100g must be numbers and between 1 to 20 characters.")]
        [Display(Name = "Fat per 100g")]
        public double Fett { get; set; } = 0;

        // Validation for Protein
        [RegularExpression(@"[0-9., \-]{1,20}", ErrorMessage = "Protein per 100g must be numbers and between 1 to 20 characters.")]
        [Display(Name = "Protein per 100g")]
        public double Protein { get; set; } = 0;

        // Validation for Karbohydrat
        [RegularExpression(@"[0-9., \-]{1,20}", ErrorMessage = "Carbohydrates per 100g must be numbers and between 1 to 20 characters.")]
        [Display(Name = "Carbohydrates per 100g")]
        public double Karbohydrat { get; set; } = 0;

        // Validation for Salt
        [RegularExpression(@"[0-9., \-]{1,20}", ErrorMessage = "Salt per 100g must be numbers and between 1 to 20 characters.")]
        [Display(Name = "Salt per 100g")]
        public double Salt { get; set; } = 0;

        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = string.Empty;

        public bool HasGreenKeyhole { get; set; }

        // Property to handle file upload, not mapped to the database
        [NotMapped]
        [AllowedFileExtensions(new string[] { ".jpg", ".jpeg", ".png" }, ErrorMessage = "Only image files (.jpg, .jpeg, .png) are allowed.")]
        public IFormFile? ImageFile { get; set; }

        // Custom validation logic
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var isCreating = ItemId == 0; // Assuming ItemId is 0 for new items (creating)

            if (isCreating && (ImageFile == null || ImageFile.Length == 0))
            {
                yield return new ValidationResult("File is required.", new[] { nameof(ImageFile) });
            }
        }
    }
}
