namespace DojoGameLife2
{
	class Program
	{
		static void Main(string[] args)
		{
			var cellCollector = new CellSupervisor();
			cellCollector.Step();
		}
	}
}
