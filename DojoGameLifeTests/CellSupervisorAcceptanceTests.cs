using System.Collections.Generic;
using System.Linq;
using DojoGameLife2;
using NUnit.Framework;

namespace DojoGameLifeTests
{
	[TestFixture]
	public class CellSupervisorAcceptanceTests
	{
		[Test, TestCaseSource("ImmortalLayouts")]
		public void Step_ImmortalCells(Coordinate[] coordinates)
		{
			var collector = InitializeCollector(coordinates);

			collector.Step();
			Assert.That(collector.GetAlived().Select(x => x.Coordinate), Is.EquivalentTo(coordinates));
			collector.Step();
			Assert.That(collector.GetAlived().Select(x => x.Coordinate), Is.EquivalentTo(coordinates));
		}

		[Test, TestCaseSource("OscillatorLayouts")]
		public void Step_OscillatorLayouts(Coordinate[] state1, Coordinate[] state2)
		{
			var collector = InitializeCollector(state1);

			collector.Step();
			Assert.That(collector.GetAlived().Select(x => x.Coordinate), Is.EquivalentTo(state2));
			collector.Step();
			Assert.That(collector.GetAlived().Select(x => x.Coordinate), Is.EquivalentTo(state1));
		}

		private static readonly object[] OscillatorLayouts =
		{
			//kreska
			new object[] 
			{ 
				new[]{ new Coordinate(0, 0), new Coordinate(1, 0), new Coordinate(2,0)},
				new[]{ new Coordinate(1, 1), new Coordinate(1, 0), new Coordinate(1,-1)}
			},
		};
		private static readonly object[] ImmortalLayouts =
		{
			//klocek
			new object[] { new[]{
				new Coordinate(0, 0), new Coordinate(1, 0),
				new Coordinate(0,-1), new Coordinate(1,-1) }},
			//≥Ûdü
			new object[] { new[]
			{
				new Coordinate(0, 0), new Coordinate(1, 0),
				new Coordinate(0, -1), new Coordinate(2, -1),
				new Coordinate(1, -2)
			} },
			//koniczynka
			new object[] { new[]
			{
				new Coordinate(1, 0),
				new Coordinate(0, -1), new Coordinate(2, -1),
				new Coordinate(1, -2)
			}},
			//kryszta≥
			new object[] { new[]
			{
				new Coordinate(1, 0),
				new Coordinate(0, -1), new Coordinate(2, -1),
				new Coordinate(0, -2), new Coordinate(2, -2),
				new Coordinate(1, -3)
			}}
		};

		private static CellSupervisor InitializeCollector(IEnumerable<Coordinate> coordinates)
		{
			var collector = new CellSupervisor();
			foreach (var coordinate in coordinates)
				collector.InsertCell(coordinate, State.Life);
			return collector;
		}
	}
}