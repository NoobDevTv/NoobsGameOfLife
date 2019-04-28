namespace NoobsGameOfLife.Core.Information
{
    public struct NutrientInfo
    {
        public Location Position { get; set; }

        public NutrientInfo(Nutrient nutrient)
            => Position = nutrient.Position;
    }
}