using NoobsGameOfLife.Core.Biology;
using NoobsGameOfLife.Core.Physics;

namespace NoobsGameOfLife.Core.Information
{
    public struct NutrientInfo
    {
        public Location Position { get; }
        public byte Carbon { get;  }
        public byte Oxygen { get;  }
        public byte Hydrogen { get;  }

        public NutrientInfo(Nutrient nutrient)
        {
            Position = nutrient.Position;
            Carbon = nutrient.Elements[Element.Carbon];
            Oxygen = nutrient.Elements[Element.Oxygen];
            Hydrogen = nutrient.Elements[Element.Hydrogen];
        }
    }
}