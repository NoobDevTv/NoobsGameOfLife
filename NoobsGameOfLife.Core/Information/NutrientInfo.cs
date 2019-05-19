namespace NoobsGameOfLife.Core.Information
{
    public struct NutrientInfo
    {
        public Location Position { get; set; }
        public byte Carbon { get; set; }
        public byte Oxygen { get; set; }
        public byte Hydrogen { get; set; }

        public NutrientInfo(Nutrient nutrient)
        {
            Position = nutrient.Position;
            Carbon = nutrient.Elements[Element.Carbon];
            Oxygen = nutrient.Elements[Element.Oxygen];
            Hydrogen = nutrient.Elements[Element.Hydrogen];
        }
    }
}