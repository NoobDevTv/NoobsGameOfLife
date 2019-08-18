using NoobsGameOfLife.Core.Biology;

namespace NoobsGameOfLife.Core.Information
{
    public struct CellInfo
    {
        public ulong CellId { get; }

        public Location Position { get; }
        public double Energy { get; }

        public CellInfo(Cell cell)
        {
            Position = cell.Position;
            Energy = cell.Energy;
            CellId = cell.Id;
        }

        public override bool Equals(object obj)
        {
            if (obj is CellInfo cellInfo)
                return CellId == cellInfo.CellId;

            return false;
        }

        public override int GetHashCode() 
            => -1453718448 + CellId.GetHashCode();
    }
}