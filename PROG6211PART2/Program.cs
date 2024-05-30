/*method for creating recipe
* method for creating, viewing, scaling, clearing, resetting recipe
* call these methods in main method
* use conditional or switch cases
* use a menu-use part 3 2023 as a reference-welcome message
* proper error handling
*/
using System;
using System.Collections.Generic;
using System.Linq;

//Declare the delegate
public delegate void CalorieExceeded(double totalCalories);

namespace RecipeApplication
{
  public class Ingredient
    {
        public string Name { get; set; }
        public double OriginalQuantity { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public double Calories { get; set; }
        public string FoodGroup { get; set; }
    }

   public class RecipeStep
    {
        public string Step { get; set; }
    }

    public class Recipe
    {
        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<RecipeStep> Steps { get; set; }

        //event that notifies the user that the calories exceed 300
        public event CalorieExceeded calories;

        public Recipe()
        {
            Ingredients = new List<Ingredient>();
            Steps = new List<RecipeStep>();
        }

        //create a new recipe
        public void CreateRecipe()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Enter details for the recipe");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Enter recipe name: ");
            Console.ResetColor();
            Name = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Enter number of ingredients: ");
            Console.ResetColor();
            if (!int.TryParse(Console.ReadLine(), out int ingredientCount))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid number. Please enter a valid number of ingredients.");
                Console.ResetColor();
                return;
            }

            //loop for the amount of ingredients there are
            for (int i = 0; i < ingredientCount; i++)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Enter ingredient details {i + 1}:");
                Console.ResetColor();

                Ingredient ingredient = new Ingredient();

                Console.Write("Ingredient name: ");
                ingredient.Name = Console.ReadLine();

                Console.Write("Ingredient quantity (number only): ");
                if (!double.TryParse(Console.ReadLine(), out double quantity))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid quantity. Please enter a valid number.");
                    Console.ResetColor();
                    return;
                }
                ingredient.OriginalQuantity = quantity;
                ingredient.Quantity = quantity;

                Console.Write("Unit of measurement (slice, teaspoon, etc): ");
                ingredient.Unit = Console.ReadLine();

                Console.Write("Number of calories: ");
                if (!double.TryParse(Console.ReadLine(), out double calories))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid calorie count. Please enter a valid number.");
                    Console.ResetColor();
                    return;
                }
                //store the calories
                ingredient.Calories = calories;

                Console.Write("Food group: ");
                ingredient.FoodGroup = Console.ReadLine();

                //https://stackoverflow.com/questions/230454/how-to-fill-an-array-from-user-input-c
                Ingredients.Add(ingredient);
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Enter number of steps: ");
            Console.ResetColor();
            if (!int.TryParse(Console.ReadLine(), out int stepCount))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid number. Please enter a valid number of steps.");
                Console.ResetColor();
                return;
            }
            //loop for the amount of steps
            for (int i = 0; i < stepCount; i++)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"Enter step {i + 1}: ");
                Console.ResetColor();
                string step = Console.ReadLine();
                Steps.Add(new RecipeStep { Step = step });
            }

            //link to the CalorieExceeded event
            calories += OnCalorieNotification;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nNew recipe created! Look at you, MasterChef! =)");
            Console.ResetColor();
        }
        //display the recipe
        public void DisplayRecipe()
        {
            if (Ingredients.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No recipe to display.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nRecipe details:");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Ingredients:");
            Console.ResetColor();
            foreach (var ingredient in Ingredients)
            {
                //https://stackoverflow.com/questions/6482331/how-to-add-different-types-of-objects-in-a-single-array-in-c
                Console.WriteLine($"{ingredient.Quantity} {ingredient.Unit} of {ingredient.Name} ({ingredient.Calories} calories, {ingredient.FoodGroup})");
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nSteps:");
            Console.ResetColor();
            for (int i = 0; i < Steps.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Steps[i].Step}");
            }
            //calculate the total calories of the ingredients 
            double totalCalories = Ingredients.Sum(ing => ing.Calories * ing.Quantity / ing.OriginalQuantity);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"\nTotal Calories: {totalCalories}");
            Console.ResetColor();

            //if calories exceed 300 then the system will notify the user 
            if (totalCalories > 300)
            {
                calories?.Invoke(totalCalories);
            }
        }
        //method for the CalorieExceeded notifaction
        public void OnCalorieNotification(double totalCalories)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Warning: Total calories exceed 300! (Total Calories: {totalCalories})");
            Console.ResetColor();
       // https://www.bytehide.com/blog/eventhandler-csharp
        }
        
        //scale the recipe
        public void ScaleRecipe(double scaleAmount)
        {
            if (Ingredients.Count == 0)
            {
                //https://www.geeksforgeeks.org/c-sharp-how-to-change-foreground-color-of-text-in-console/
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No recipe to scale.");
                Console.ResetColor();
                return;
            }

            if (scaleAmount <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid scaling factor. Please enter a positive number.");
                Console.ResetColor();
                return;
            }
            //loop for the amount of ingredients 
            foreach (var ingredient in Ingredients)
            {
                ingredient.Quantity = ingredient.OriginalQuantity * scaleAmount;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Recipe scaled successfully.");
            Console.ResetColor();
        }

        //reset the scaled quantites
        public void ResetQuantities()
        {
            if (Ingredients.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No recipe to reset.");
                Console.ResetColor();
                return;
            }
            //loop for the amount of ingredients 
            foreach (var ingredient in Ingredients)
            {
                ingredient.Quantity = ingredient.OriginalQuantity;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Quantities reset.");
            Console.ResetColor();
        }

        //clear the recipe
        public void ClearRecipe()
        {
            if (Ingredients.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No recipe to clear.");
                Console.ResetColor();
                return;
            }

            Ingredients.Clear();
            Steps.Clear();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Recipe cleared.");
            Console.ResetColor();
        }
    }

    class Program
    {
        static List<Recipe> recipes = new List<Recipe>();

        //create the menu with all the methods and options
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Welcome to Recipe Book!");
            Console.ResetColor();

            while (true)
            //while(true) will keep the application looping until the user exits(6)
            {
                Console.WriteLine("\nSelect a function:");
                Console.WriteLine("(1) Create a new recipe");
                Console.WriteLine("(2) Display recipes");
                Console.WriteLine("(3) Scale recipe");
                Console.WriteLine("(4) Reset quantities");
                Console.WriteLine("(5) Clear recipe");
                Console.WriteLine("(6) Quit");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Recipe recipe = new Recipe();
                        recipe.CreateRecipe();
                        recipes.Add(recipe);
                        break;
                    case "2":
                        DisplayRecipes();
                        break;
                    case "3":
                        ScaleRecipe();
                        break;
                    case "4":
                        ResetRecipeQuantities();
                        break;
                    case "5":
                        ClearRecipe();
                        break;
                    case "6":
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Happy cooking!");
                        Console.ResetColor();
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option. Please select from options 1-6.");
                        Console.ResetColor();
                        break;
                }
            }
        }
        //display recipe
        static void DisplayRecipes()
        {
            if (recipes.Count == 0)
            //https://www.w3schools.com/cs/cs_conditions.php
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No recipes available.");
                Console.ResetColor();
                return;
            }

            recipes = recipes.OrderBy(r => r.Name).ToList();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Recipe List:");
            Console.ResetColor();
            foreach (var recipe in recipes)
            {
                Console.WriteLine(recipe.Name);
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Enter the name of the recipe you want to view: ");
            Console.ResetColor();
            string recipeName = Console.ReadLine();

            Recipe selectedRecipe = recipes.FirstOrDefault(r => r.Name.Equals(recipeName, StringComparison.OrdinalIgnoreCase));
            if (selectedRecipe != null)
            {
                selectedRecipe.DisplayRecipe();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Recipe not found.");
                Console.ResetColor();
            }
        }
        //scale recipe
        static void ScaleRecipe()
        {
            if (recipes.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No recipes available to scale.");
                Console.ResetColor();
                return;
                //message to be displayed if no quantities have been scaled
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Enter the name of the recipe you want to scale: ");
            Console.ResetColor();
            string recipeName = Console.ReadLine();

            Recipe selectedRecipe = recipes.FirstOrDefault(r => r.Name.Equals(recipeName, StringComparison.OrdinalIgnoreCase));
            if (selectedRecipe != null)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Enter the scaling factor (e.g., 0.5, 2, 3): ");
                Console.ResetColor();
                if (double.TryParse(Console.ReadLine(), out double scaleAmount))
                {
                    selectedRecipe.ScaleRecipe(scaleAmount);
                    selectedRecipe.DisplayRecipe();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid scaling factor. Please enter a valid number.");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Recipe not found.");
                Console.ResetColor();
            }
        }

        //reset quantites
        static void ResetRecipeQuantities()
        //https://stackoverflow.com/questions/39495995/c-sharp-reset-an-array-to-its-initialized-values
        {
            if (recipes.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No recipes available to reset.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Enter the name of the recipe you want to reset: ");
            Console.ResetColor();
            string recipeName = Console.ReadLine();

            Recipe selectedRecipe = recipes.FirstOrDefault(r => r.Name.Equals(recipeName, StringComparison.OrdinalIgnoreCase));
            if (selectedRecipe != null)
            {
                selectedRecipe.ResetQuantities();
                selectedRecipe.DisplayRecipe();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Recipe not found.");
                Console.ResetColor();
            }
        }

        //clear recipe
        static void ClearRecipe()
        {
            //https://stackoverflow.com/questions/39495995/c-sharp-reset-an-array-to-its-initialized-values
            if (recipes.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No recipes available to clear.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Enter the name of the recipe you want to clear: ");
            Console.ResetColor();
            string recipeName = Console.ReadLine();

            Recipe selectedRecipe = recipes.FirstOrDefault(r => r.Name.Equals(recipeName, StringComparison.OrdinalIgnoreCase));
            if (selectedRecipe != null)
            {
                selectedRecipe.ClearRecipe();
                recipes.Remove(selectedRecipe);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Recipe not found.");
                Console.ResetColor();
            }
        }
    }
}

