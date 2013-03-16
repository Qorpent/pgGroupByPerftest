using System;
using Npgsql;

namespace pgPerfTest {
	public abstract class ExecuteQueryTest : AsyncPerformanceTestBase {
		public string Query { get; protected set; }
		
		
		protected ExecuteQueryTest(string query,string connection ) {
			Query = query;
			SetConnection(connection);
		}
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