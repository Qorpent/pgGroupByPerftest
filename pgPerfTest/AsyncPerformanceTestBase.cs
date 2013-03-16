#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : AsyncPerformanceTestBase.cs
// Project: pgPerfTest
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace pgPerfTest {
	/// <summary>
	/// 	Общая абстракция теста на производительность в асинхронном режиме
	/// </summary>
	public abstract class AsyncPerformanceTestBase : IPerformanceTest {
		/// <summary>
		/// 	Асинхронный вариант вызова
		/// </summary>
		/// <returns> </returns>
		public Task<PerformanceResult> ExecuteQueryAsync() {
			return Task.Run(() =>
				{
					var result = new PerformanceResult {Source = this};
					var timecheck = Stopwatch.StartNew();
					try {
						ExecuteInternalTest(result);
					}
					catch (Exception e) {
						result.Error = e;
					}
					finally {
						timecheck.Stop();
						result.ExecuteTime = timecheck.Elapsed;
					}
					return result;
				});
		}

		protected abstract void ExecuteInternalTest(PerformanceResult result);
	}
}