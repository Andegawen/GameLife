using System.Collections.Generic;
using System.Linq;

namespace DojoGameLife2
{
	public class Cell
	{
		public Cell(CellSupervisor supervisor, Coordinate coordinate, State currentState)
		{			
			this.supervisor = supervisor;
			this.coordinate = coordinate;
			this.currentState = currentState;
			Register();
		}

		public void Unregister()
		{
			supervisor.CalculateState -= CalculateState;
			supervisor.ChangeState -= ChangeState;
			supervisor.GetCells -= Get;
			supervisor.Find -= FindCell;
		}

		public bool IsAlive
		{
			get { return currentState == State.Life; }
		}

		public Coordinate Coordinate
		{
			get { return coordinate; }
		}

		public void CalculateState()
		{
			switch (currentState)
			{
				case State.Life:
					CellSurviverMode();
					break;
				case State.Death:
					CellReproductiveMode();
					break;
			}
		}

		private void Register()
		{
			supervisor.CalculateState += CalculateState;
			supervisor.ChangeState += ChangeState;
			supervisor.GetCells += Get;
			supervisor.Find += FindCell;
		}

		private void Get(IList<Cell> collection)
		{
			collection.Add(this);
		}

		private void FindCell(Coordinate coordinatePattern, IList<Cell> foundCells)
		{
			if (coordinate.Equals(coordinatePattern))
				foundCells.Add(this);
		}

		private void ChangeState()
		{
			currentState = nextState;
			if (currentState == State.Death)
				Unregister();
		}

		private void CellSurviverMode()
		{
			var totalNeighbours = GetNumberNeighbours();
			if (totalNeighbours >= 2 && totalNeighbours <= 3)
				nextState = State.Life;
			else
				nextState = State.Death;
		}

		private void CellReproductiveMode()
		{
			var totalNeighbours = GetNumberNeighbours();
			nextState = totalNeighbours == 3
				? State.Life 
				: State.Death;
		}

		private int GetNumberNeighbours()
		{
			var total = 0;
			for (var i = -1; i < 2; i++)
			{
				for (var j = -1; j < 2; j++)
				{
					if (i == 0 && j == 0)
						continue;
					var searchCoordinate = new Coordinate(i + coordinate.X, j + coordinate.Y);
					if (IsNeighbourLive(searchCoordinate))
						total++;
					else if (currentState == State.Life)
						supervisor.InsertCell(searchCoordinate, State.Death).CalculateState();
				}
			}
			return total;
		}

		private bool IsNeighbourLive(Coordinate searchCoordinate)
		{
			var foundCells = supervisor.FindCells(searchCoordinate);

			return foundCells.Any() && foundCells.First().IsAlive;
		}

		private readonly Coordinate coordinate;
		private State currentState;
		private State nextState;
		private readonly CellSupervisor supervisor;
	}
}
