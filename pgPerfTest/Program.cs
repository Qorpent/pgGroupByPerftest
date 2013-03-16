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
		private const int ITERATE_COUNT = 100;
		private static void Main(string[] args) {
			var test = new AllPartitionsInParallelTest();
			var result = test.Execute();
			if (null != result.Error) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Ошибка! {0}:{1}", result.Error.GetType(), result.Error.Message);
				Console.ResetColor();
			}
			else {
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
				Console.WriteLine("Выполняем множественную проверку {0} тестов...",ITERATE_COUNT);
				var totaltime = new TimeSpan();
				for (var i=0;i<ITERATE_COUNT;i++) {
					if((i % 10)==0)Console.WriteLine();
					Console.Write(".");
					totaltime += test.Execute().ExecuteTime;
				}
				Console.WriteLine();
				Console.WriteLine("Общее время на {0} тестов : {1}",ITERATE_COUNT,totaltime);

				var avg = TimeSpan.FromMilliseconds(totaltime.TotalMilliseconds/ITERATE_COUNT);
				Console.WriteLine("Среднее время на тест : {0}",avg);
			}
		}
	}
}