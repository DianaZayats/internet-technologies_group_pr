using System.Resources;
using System.Reflection;
using System.Globalization;

namespace WebAppCore.Resources
{
    
    public class ValidationMessages
    {
        private static ResourceManager GetResourceManager()
        {
            
            var culture = CultureInfo.CurrentUICulture;
            
         
            return new ResourceManager("WebAppCore.Resources.ValidationMessages", 
                typeof(ValidationMessages).Assembly);
        }

      
        
        public static string FullName_Required => 
            GetResourceManager().GetString("FullName_Required") ?? "FullName_Required";
        
        public static string FullName_StringLength => 
            GetResourceManager().GetString("FullName_StringLength") ?? "FullName_StringLength";
        
        public static string Email_Required => 
            GetResourceManager().GetString("Email_Required") ?? "Email_Required";
        
        public static string Email_EmailAddress => 
            GetResourceManager().GetString("Email_EmailAddress") ?? "Email_EmailAddress";
        
        public static string Age_Required => 
            GetResourceManager().GetString("Age_Required") ?? "Age_Required";
        
        public static string Age_Range => 
            GetResourceManager().GetString("Age_Range") ?? "Age_Range";
        
        public static string Password_Required => 
            GetResourceManager().GetString("Password_Required") ?? "Password_Required";
        
        public static string Password_StringLength => 
            GetResourceManager().GetString("Password_StringLength") ?? "Password_StringLength";
        
        public static string ConfirmPassword_Required => 
            GetResourceManager().GetString("ConfirmPassword_Required") ?? "ConfirmPassword_Required";
        
        public static string ConfirmPassword_Compare => 
            GetResourceManager().GetString("ConfirmPassword_Compare") ?? "ConfirmPassword_Compare";
        
        public static string OrderAmount_Required => 
            GetResourceManager().GetString("OrderAmount_Required") ?? "OrderAmount_Required";
        
        public static string OrderAmount_Range => 
            GetResourceManager().GetString("OrderAmount_Range") ?? "OrderAmount_Range";
        
        public static string PromoCode_Remote => 
            GetResourceManager().GetString("PromoCode_Remote") ?? "PromoCode_Remote";
        
        public static string RecipeName_Required => 
            GetResourceManager().GetString("RecipeName_Required") ?? "RecipeName_Required";
        
        public static string RecipeName_StringLength => 
            GetResourceManager().GetString("RecipeName_StringLength") ?? "RecipeName_StringLength";
        
        public static string Description_Required => 
            GetResourceManager().GetString("Description_Required") ?? "Description_Required";
        
        public static string Description_StringLength => 
            GetResourceManager().GetString("Description_StringLength") ?? "Description_StringLength";
        
        public static string Category_Required => 
            GetResourceManager().GetString("Category_Required") ?? "Category_Required";
        
        public static string Difficulty_Required => 
            GetResourceManager().GetString("Difficulty_Required") ?? "Difficulty_Required";
        
        public static string CookingTime_Required => 
            GetResourceManager().GetString("CookingTime_Required") ?? "CookingTime_Required";
        
        public static string CookingTime_Range => 
            GetResourceManager().GetString("CookingTime_Range") ?? "CookingTime_Range";
        
        public static string CookingTime_Remote => 
            GetResourceManager().GetString("CookingTime_Remote") ?? "CookingTime_Remote";
        
        public static string Ingredients_Required => 
            GetResourceManager().GetString("Ingredients_Required") ?? "Ingredients_Required";
        
        public static string Ingredients_StringLength => 
            GetResourceManager().GetString("Ingredients_StringLength") ?? "Ingredients_StringLength";
        
        // Display name properties
        public static string RecipeName => 
            GetResourceManager().GetString("RecipeName") ?? "RecipeName";
        
        public static string Description => 
            GetResourceManager().GetString("Description") ?? "Description";
        
        public static string Category => 
            GetResourceManager().GetString("Category") ?? "Category";
        
        public static string Difficulty => 
            GetResourceManager().GetString("Difficulty") ?? "Difficulty";
        
        public static string PreparationTime => 
            GetResourceManager().GetString("PreparationTime") ?? "PreparationTime";
        
        public static string Servings => 
            GetResourceManager().GetString("Servings") ?? "Servings";
        
        public static string ImageUrl => 
            GetResourceManager().GetString("ImageUrl") ?? "ImageUrl";
        
        public static string DietType => 
            GetResourceManager().GetString("DietType") ?? "DietType";
        
        public static string Cuisine => 
            GetResourceManager().GetString("Cuisine") ?? "Cuisine";
        
        public static string BudgetLevel => 
            GetResourceManager().GetString("BudgetLevel") ?? "BudgetLevel";
        
        public static string Calories => 
            GetResourceManager().GetString("Calories") ?? "Calories";
        
        public static string Ingredients => 
            GetResourceManager().GetString("Ingredients") ?? "Ingredients";
    }
}

