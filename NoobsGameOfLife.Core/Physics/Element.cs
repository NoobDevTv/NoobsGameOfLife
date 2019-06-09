namespace NoobsGameOfLife.Core.Physics
{
    public struct Element
    {
        public string Name { get; private set; }
        public byte Energy { get; set; }

        private Element(string name, byte energy)
        {
            Name = name;
            Energy = energy;
        }

        public static Element Carbon => new Element("carbon", 10);
        public static Element Oxygen => new Element("oxygen", 10);
        public static Element Hydrogen => new Element("hydrogen", 10);


        public override string ToString() 
            => $"{Name} | {Energy}";

        public override bool Equals(object obj) 
            => obj is Element element && Name == element.Name;


        public override int GetHashCode()
            => base.GetHashCode(); //only to prevent the stupid warning
    }
}
