namespace NoobsGameOfLife.Core.Information
{
    public struct CellInfo
    {
        public Location Position { get; private set; }
        public int Energy { get; private set; }

        public CellInfo(Cell cell)
        {
            Position = cell.Position;
            Energy = cell.Energy;
        }
    }
}