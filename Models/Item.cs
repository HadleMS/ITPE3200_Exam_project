using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
    public class Item
    {
        public int ItemId { get; set; }

        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,20}", ErrorMessage = "The Name must be numbers or letters and between 2 to 20 characters.")]
        [Display(Name = "Item name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]

        public string? Food_Group { get; set; }

        public string? Energi_Kcal { get; set; }
        public string? Fett { get; set; }
        public string? Protein { get; set; }
        public string? Karbohydrat { get; set; }
        public string? Salt { get; set; }


        public string? ImageUrl { get; set; }
        // navigation property
        public virtual List<OrderItem>? OrderItems { get; set; }
    }
}

