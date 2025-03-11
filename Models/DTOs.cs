namespace CodespringProject.Models
{
    public class IngredientDto
    {
        public string Name { get; set; }
        public List<RecipeShortDto> Recipes { get; set; } = new();
    }
    public class IngredientInDto
    {
        public string Name { get; set; }
    }


    public class RecipeShortDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class RecipeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public List<IngredientDto> Ingredients { get; set; } = new();
    }
    public class RecipeListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }

    public class RecipeInDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public List<IngredientInDto> Ingredients { get; set; }
    }
}