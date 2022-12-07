using System;
using System.Collections.Generic;

namespace Gameplay
{
    public static class GameplayHelper
    {
        /// <summary>
        /// Calculates the review from 0 to 5 based on the missing and unwanted pizza ingredients.
        /// </summary>
        /// <returns>A decimal value between 0 and 5 with precision of 2 decimal places.</returns>
        public static decimal CalculateStarsReviewForOrder(List<PizzaIngredients> missingIngredients, List<PizzaIngredients> unwantedIngredients)
        {
            decimal review = 5;

            // Decrease the review by 0.5 for each missing ingredient
            review -= ((decimal)0.8 * missingIngredients.Count);

            // Decrease the review by 0.8 for each unwanted ingredient
            review -= ((decimal)1.2 * unwantedIngredients.Count);

            return Math.Max(0, Math.Round(review, 2));
        }
    }
}