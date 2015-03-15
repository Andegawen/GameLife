using System.Collections.Generic;
using System.Linq;
using DojoGameLife2;
using NUnit.Framework;

namespace DojoGameLifeTests
{
	[TestFixture]
	public class CellSupervisorUnitTests
	{
		[Test]
		public void Step_NoAlived_WhenNothingLivedBefore()
		{
			var collector = new CellSupervisor();

			collector.Step();

			Assert.That(collector.GetAlived().Count(), Is.EqualTo(0));
		}

		[Test, TestCaseSource("CoordinatesToDie")]
		public void Step_CellsDies_WhenTheyAreLoneliness(Coordinate[] coordinates)
		{
			var collector = InitializeCollector(coordinates);

			collector.Step();

			Assert.That(collector.GetAlived().Count(), Is.EqualTo(0));
		}

		static readonly object[] CoordinatesToDie =
		{
			new object[] { new[]{new Coordinate(1,0)}},
			new object[] { new[]{new Coordinate(0,0), new Coordinate(1,0) } }
		};

		//x
		// s
		//  x
		[Test]
		public void Step_CellSurvives_WhenHas2Neighbours()
		{
			var expectedSurvivedCoordinates = new[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(2, 2) };
			var collector = InitializeCollector(expectedSurvivedCoordinates);

			collector.Step();

			Assert.That(collector.GetAlived().Select(c => c.Coordinate), Is.EquivalentTo(new[] { new Coordinate(1, 1) }));
		}

		// n
		//xsx
		// n
		[Test]
		public void Step_CellGeneratesNewCell_WhenHas3Neighbours()
		{
			var coordinates = new[] { new Coordinate(0, 0), new Coordinate(1, 0), new Coordinate(2, 0) };
			var collector = InitializeCollector(coordinates);

			collector.Step();

			var expectedSurvivedCoordinates = new[] { new Coordinate(1, -1), new Coordinate(1, 0), new Coordinate(1, 1) };
			Assert.That(collector.GetAlived().Select(x => x.Coordinate), Is.EquivalentTo(expectedSurvivedCoordinates));
		}

		[Test]
		public void Step_CellDies_WhenThereIsOverpopulation()
		{
			var coordinates = new[]
			{
				new Coordinate(0, 0), new Coordinate(1, 0),
				new Coordinate(0, -1), new Coordinate(1, -1), new Coordinate(2, -1)
			};
			var collector = InitializeCollector(coordinates);

			collector.Step();

			Assert.That(collector.GetAlived().Any(x => x.Coordinate.Equals(new Coordinate(1,-1))), Is.False);
		}


		[Test]
		public void Clear_ReturnNoAlivedCells()
		{
			var coordinates = new[]
			{
				new Coordinate(0, 0), new Coordinate(1, 0),
				new Coordinate(0, -1), new Coordinate(1, -1), new Coordinate(2, -1)
			};
			var collector = InitializeCollector(coordinates);

			collector.Clear();

			Assert.That(collector.GetAlived().Count, Is.EqualTo(0));
		}

		[Test]
		public void DeleteCell_DeleteCertainCell()
		{
			var coordinates = new[]{ new Coordinate(0, 0), new Coordinate(1, 0) };
			var collector = InitializeCollector(coordinates);

			collector.DeleteCell(new Coordinate(1, 0));

			Assert.That(collector.GetAlived().Select(c => c.Coordinate), Is.EquivalentTo(new[] { new Coordinate(0, 0) }));
		}

		private static CellSupervisor InitializeCollector(IEnumerable<Coordinate> coordinates)
		{
			var collector = new CellSupervisor();
			foreach (var coordinate in coordinates)
				collector.InsertCell(coordinate, State.Life);
			return collector;
		}
	}
}
