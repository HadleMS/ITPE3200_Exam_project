using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
   
    public class Item
    {
        
        public int ItemId { get; set; }

        // Validation for the Name property 
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}", ErrorMessage = "Name must be letters and between 2 to 20 characters.")]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        // Validation for the Food_Group property 
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}", ErrorMessage = "Food Group must be letters and between 2 to 20 characters.")]
        [Display(Name = "Food Group")]
        public string Food_Group { get; set; } = string.Empty;

        // Validation for Energi_Kcal 
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ., \-]{2,20}", ErrorMessage = "Energy per 100g must be numbers or letters and between 2 to 20 characters.")]
        [Display(Name = "Energy per 100g")]
        public string Energi_Kcal { get; set; } = string.Empty;

        // Validation for Fett 
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ., \-]{2,20}", ErrorMessage = "Fat per 100g must be numbers or letters and between 2 to 20 characters.")]
        [Display(Name = "Fat per 100g")]
        public string Fett { get; set; } = string.Empty;

        // Validation for Protein 
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ., \-]{2,20}", ErrorMessage = "Protein per 100g must be numbers or letters and between 2 to 20 characters.")]
        [Display(Name = "Protein per 100g")]
        public string Protein { get; set; } = string.Empty;

        // Validation for Karbohydrat 
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ., \-]{2,20}", ErrorMessage = "Carbohydrates per 100g must be numbers or letters and between 2 to 20 characters.")]
        [Display(Name = "Carbohydrates per 100g")]
        public string Karbohydrat { get; set; } = string.Empty;

        // Validation for Salt 
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ., \-]{2,20}", ErrorMessage = "Salt per 100g must be numbers or letters and between 2 to 20 characters.")]
        [Display(Name = "Salt per 100g")]
        public string Salt { get; set; } = string.Empty;

        // Validation for ImageUrl 
        [RegularExpression(@"[a-zA-ZæøåÆØÅ., \-]{2,20}", ErrorMessage = "ImageUrl must be letters and between 2 to 20 characters.")]
        [Display(Name = "ImageUrl")]
        public string ImageUrl { get; set; } = string.Empty;

        public bool HasGreenKeyhole { get; set; }


       
        public virtual List<OrderItem>? OrderItems { get; set; }
    }
}


