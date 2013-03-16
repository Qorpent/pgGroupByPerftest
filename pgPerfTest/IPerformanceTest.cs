#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IPerformanceTest.cs
// Project: pgPerfTest
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Threading.Tasks;

namespace pgPerfTest {
	/// <summary>
	/// 	Общий интерфейс тестов
	/// </summary>
	public interface IPerformanceTest {
		Task<PerformanceResult> ExecuteQueryAsync();
	}
}