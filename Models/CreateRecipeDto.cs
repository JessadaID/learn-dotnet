namespace cookapi.Models
{
    public class CreateRecipeDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<IngredientDto> Ingredients { get; set; } = new();
        public List<string> Steps { get; set; } = new();
    }

    public class IngredientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Quantity { get; set; } = "";
    }
}
