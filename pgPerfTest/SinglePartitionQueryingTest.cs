#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : SinglePartitionQueryingTest.cs
// Project: pgPerfTest
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Npgsql;

namespace pgPerfTest {
	/// <summary>
	/// 	Выполняет запрос к отдельной партиции, считывает результат и подсчитывает время
	/// </summary>
	public class SinglePartitionQueryingTest : AsyncPerformanceTestBase {
		public const string DEFAULT_QUERY = "select date,count(data)  from groupbytest.y{0}_{1}test group by date;";
		public const string DEFAULT_CONNECTION = "Server=127.0.0.1;Port=5432;Database=test;Integrated Security=true;";

		public SinglePartitionQueryingTest(int year, HalfYear halfYear, string query = null, string connection = null) {
			HalfYear = halfYear;
			Year = year;
			Query = query;
			if (string.IsNullOrWhiteSpace(Query)) {
				Query = DEFAULT_QUERY;
			}
			Query = string.Format(Query, Year, (int) HalfYear);
			Connection = connection;
			if (string.IsNullOrWhiteSpace(Connection)) {
				Connection = DEFAULT_CONNECTION;
			}
		}

		/// <summary>
		/// 	Год партиции
		/// </summary>
		public int Year { get; private set; }

		public HalfYear HalfYear { get; private set; }

		public string Query { get; private set; }

		public string Connection { get; private set; }

		protected override void ExecuteInternalTest(PerformanceResult result) {
			using (var c = new NpgsqlConnection(Connection)) {
				c.Open();
				var cmd = c.CreateCommand();
				cmd.CommandText = Query;
				using (var r = cmd.ExecuteReader()) {
					while (r.Read()) {}
				}
			}
		}
	}
}