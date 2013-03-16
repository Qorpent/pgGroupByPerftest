#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : AllPartitionsInParallelTest.cs
// Project: pgPerfTest
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Concurrent;
using System.Linq;

namespace pgPerfTest {
	/// <summary>
	/// 	Используя таблицы из testbench.sql выполняет паралельное обращение ко всем частям партиций
	/// </summary>
	public class AllPartitionsInParallelTest : AsyncPerformanceTestBase {
		private const int START_YEAR = 2001;
		private const int END_YEAR = 2007;

		/// <summary>
		/// 	Создает тест с набором партиций и с возможными собственными шаблоном запроса и соединения
		/// </summary>
		/// <param name="query"> </param>
		/// <param name="connection"> </param>
		public AllPartitionsInParallelTest(string query = null, string connection = null) {
			_subtests = new ConcurrentBag<SinglePartitionQueryingTest>();
			for (var year = START_YEAR; year <= END_YEAR; year++) {
				_subtests.Add(new SinglePartitionQueryingTest(year, HalfYear.First, query, connection));
				_subtests.Add(new SinglePartitionQueryingTest(year, HalfYear.Second, query, connection));
			}
		}

		/// <summary>
		/// 	Синхронный вариант вызова
		/// </summary>
		/// <returns> </returns>
		public PerformanceResult Execute() {
			var task = ExecuteQueryAsync();
			task.Wait();
			return task.Result;
		}

		protected override void ExecuteInternalTest(PerformanceResult result) {
			_subtests.AsParallel().ForAll(
				_ =>
					{
						var task = _.ExecuteQueryAsync();
						task.Wait();
						result.Subresults.Add(task.Result);
					}
				);
		}

		private readonly ConcurrentBag<SinglePartitionQueryingTest> _subtests;
	}
}