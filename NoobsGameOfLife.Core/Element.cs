namespace NoobsGameOfLife.Core
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
    }
}
