using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using RecipeApplication; 

namespace MyPOETest
//https://www.youtube.com/watch?v=mI7fRxy44Y0
//https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions-1/unit-testing/creating-unit-tests-for-asp-net-mvc-applications-cs
{
    [TestClass]
    public class UnitTest1
    {
        private Recipe CreateSampleRecipe(double sugarQuantity, double butterQuantity)
        {
            //create test data
            Recipe recipe = new Recipe();
            recipe.calories += recipe.OnCalorieNotification; 
            recipe.Ingredients.Add(new Ingredient { Name = "Sugar", Quantity = sugarQuantity, OriginalQuantity = sugarQuantity, Unit = "g", Calories = 400, FoodGroup = "Carbs" });
            recipe.Ingredients.Add(new Ingredient { Name = "Butter", Quantity = butterQuantity, OriginalQuantity = butterQuantity, Unit = "g", Calories = 700, FoodGroup = "Fat" });
            recipe.Steps.Add(new RecipeStep { Step = "Combine ingredients in bowl" });
            recipe.Steps.Add(new RecipeStep { Step = "Bake" });
            return recipe;
        }

        //test the calorie calculator
        [TestMethod]
        public void TestTotalCalories()
        {
            
            Recipe recipe = CreateSampleRecipe(100, 50);

         
            double totalCalories = recipe.Ingredients.Sum(ing => ing.Calories * ing.Quantity / ing.OriginalQuantity);

            //Assert data to test
            Assert.AreEqual(1100, totalCalories);
        }

        //test the notification event
        [TestMethod]
        public void TestCalorieNotificationTriggered()
        {
           
            Recipe recipe = CreateSampleRecipe(100, 50);
            bool eventTriggered = false;
            recipe.calories += (totalCalories) => eventTriggered = true;

       
            recipe.DisplayRecipe();

          
            Assert.IsTrue(eventTriggered);
        }

        //test the trigger if calories are below 300
        [TestMethod]
        public void TestCalorieNotificationNotTriggered()
        {
      
            Recipe recipe = CreateSampleRecipe(30, 20);
            bool eventTriggered = false;
            recipe.calories += (totalCalories) => eventTriggered = false;

        
            recipe.DisplayRecipe();

          
            Assert.IsFalse(eventTriggered);
        }
    }
}
//All tests passed