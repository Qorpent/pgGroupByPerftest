#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : Program.cs
// Project: pgPerfTest
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace pgPerfTest {
	internal class Program {
		private const int ITERATE_COUNT = 10;
		private static void Main(string[] args) {
			var connection = DEFAULT_CONNECTION;
			if(0!=args.Length) {
				connection = args[0];
			}
			PerformMultiIterationTest<ExplicitUnionViewTest>(connection);
			PerformMultiIterationTest<Single10MlnQueryTest>(connection);
			PerformMultiIterationTest<UserAggregateViewNotUnionAll>(connection);
			PerformMultiIterationTest<UserAggregateViewUnionAll>(connection);
			PerformMultiIterationTest<AllPartitionsInParallelTest>(connection);
			CommonPartitionalTestWithDetails(connection);
		}

		private static void PerformMultiIterationTest<TTest>(string connection) where TTest:IPerformanceTest,new() {
			Console.WriteLine();
			Console.WriteLine("===================================================================================");
			var test = new TTest();
			test.SetConnection(connection);
			Console.WriteLine("Выполняем множественную проверку {0} тестов ({1}) ...", ITERATE_COUNT, test.GetType());
			Console.WriteLine("===================================================================================");
			var totaltime = new TimeSpan();
			for (var i = 0; i < ITERATE_COUNT; i++) {
				if ((i%10) == 0) {
					Console.WriteLine();
				}
				Console.Write(".");
				var result = test.Execute();
				if(null!=result.Error) {
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine();
					
					Console.WriteLine("Ошибка!  {0}:{1} ", result.Error.GetType(), result.Error.Message);
					Console.ResetColor();
					break;
				}
				totaltime+=result.ExecuteTime;
			}
			Console.WriteLine();
			Console.WriteLine("Общее время на {0} тестов : {1}", ITERATE_COUNT, totaltime);

			var avg = TimeSpan.FromMilliseconds(totaltime.TotalMilliseconds/ITERATE_COUNT);
			Console.WriteLine("Среднее время на тест : {0}", avg);
			Console.WriteLine("===================================================================================");
		}

		private static void CommonPartitionalTestWithDetails(string connection) {
			Console.WriteLine("Тестируем распаралелленный вариант с партициями");

			var test = new AllPartitionsInParallelTest(null,connection);
			var result = test.Execute();
			if (null != result.Error) {
				WriteCommonPartiotionalTestError(result);
			}
			else {
				WriteOutCommonTestDetails(result);
			}
		}

		private static void WriteOutCommonTestDetails(PerformanceResult result) {
			Console.WriteLine("Общий результат: {0}", result.ExecuteTime);

			foreach (var subresult in result.Subresults) {
				var parttest = (SinglePartitionQueryingTest) subresult.Source;
				if (null != subresult.Error) {
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Ошибка! Партиция {0}_{1} : {2}:{3}", parttest.Year, parttest.HalfYear,
					                  subresult.Error.GetType(), subresult.Error.Message);
					Console.ResetColor();
				}
				else {
					Console.WriteLine("Партиция {0}_{1} : {2}", parttest.Year, parttest.HalfYear, subresult.ExecuteTime);
				}
			}
		}

		private static void WriteCommonPartiotionalTestError(PerformanceResult result) {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Ошибка! {0}:{1}", result.Error.GetType(), result.Error.Message);
			Console.ResetColor();
		}

		public const string DEFAULT_CONNECTION = "Server=127.0.0.1;Port=5432;Database=test;Integrated Security=true;";
	}
}