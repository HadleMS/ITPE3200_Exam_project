using System;
using Exam.Models;

namespace Exam.Utilities
{

    public static class NokkelhullValidator
    {
        // Checks whether a given item qualifies for the Nøkkelhullet label based on thresholds for various food groups.
        public static bool IsEligibleForNokkelhull(Item item)
        {
            // Define thresholds based on the Nøkkelhullet regulations
            return item.Food_Group.ToLower() switch
            {
                "Dairy" => item.Fett <= 1.5 && item.Salt <= 0.1 && item.Energi_Kj <= 150,
                "Meat" => item.Fett <= 10 && item.Salt <= 1.0 && item.Protein >= 12,
                "Vegetables" => item.Fett <= 5 && item.Salt <= 0.2 && item.Energi_Kj <= 300,
                "Nuts" => item.Fett <= 30 && item.Salt <= 0.5 && item.Karbohydrat <= 15,
                "Sauce" => item.Fett <= 5 && item.Salt <= 0.5 && item.Energi_Kj <= 100,
                "Berries" => item.Fett <= 0.5 && item.Salt <= 0.1 && item.Karbohydrat <= 8,
                "Beverages" => item.Salt <= 0.02 && item.Energi_Kj <= 5,
                _ => false // Default: not eligible if the food group is unknown
            };
        }
    }

}

