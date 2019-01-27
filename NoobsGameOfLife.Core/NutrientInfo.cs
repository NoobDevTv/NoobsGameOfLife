namespace NoobsGameOfLife.Core
{
    public struct NutrientInfo
    {
        public Location Position { get; set; }

        public NutrientInfo(Nutrient nutrient)
            => Position = nutrient.Position;
    }
}