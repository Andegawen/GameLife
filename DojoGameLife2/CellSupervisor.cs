using System;
using System.Collections.Generic;
using System.Linq;

namespace DojoGameLife2
{
	public class CellSupervisor
	{
		public Action CalculateState;
		public Action ChangeState;
		public Action<IList<Cell>> GetCells;
		public Action<Coordinate, IList<Cell>> Find;

		public Cell InsertCell(Coordinate coordinate, State state)
		{
			DeleteCell(coordinate);
			return new Cell(this, coordinate, state);
		}

		public void DeleteCell(Coordinate coordinate)
		{
			var cells = new List<Cell>();
			if(Find!=null)
				Find(coordinate, cells);
			if(cells.Any())
				cells.First().Unregister();
		}

		public IList<Cell> FindCells(Coordinate coordinate)
		{
			IList<Cell> foundCells = new List<Cell>();
			if(Find!=null)
				Find(coordinate, foundCells);

			return foundCells;
		}

		public void Step()
		{
			var calculateState = CalculateState;
			if (calculateState != null)
				calculateState();

			var changeStateHandler = ChangeState;
			if (changeStateHandler != null)
				changeStateHandler();
		}

		public IList<Cell> GetAlived()
		{
			var collection = new List<Cell>();
			if (GetCells != null)
				GetCells(collection);
			return collection;
		}

		public void Clear()
		{
			foreach (var cell in GetAlived())
			{
				cell.Unregister();
			}
		}
	}
}
