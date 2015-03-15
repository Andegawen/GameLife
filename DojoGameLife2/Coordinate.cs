namespace DojoGameLife2
{
	public class Coordinate
	{
		public Coordinate(int x, int y)
		{
			X = x;
			Y = y;
		}

		public int X { get; private set; }
		public int Y { get; private set; }

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			var c = obj as Coordinate;
			if (c == null)
				return false;

			return c.X == X && c.Y == Y;
		}

		public override int GetHashCode()
		{
			return X ^ Y;
		}

		public override string ToString()
		{
			return string.Format("X: {0}, Y: {1}", X, Y);
		}
	}
}
