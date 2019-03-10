namespace NoobsGameOfLife.Core
{
    public struct Element
    {
        public string Name { get; private set; }

        private Element(string name)
        {
            Name = name;
        }

        public static Element Carbon => new Element("carbon");
        public static Element Oxygen => new Element("oxygen");
        public static Element Hydrogen => new Element("hydrogen");
    }
}
