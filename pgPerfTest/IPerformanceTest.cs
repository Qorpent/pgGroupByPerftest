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
		/// <summary>
		/// Выполнить тест в синхронном варианте
		/// </summary>
		/// <returns></returns>
		PerformanceResult Execute();
		/// <summary>
		/// Выполнить тест в асинхронном варианте
		/// </summary>
		/// <returns></returns>
		Task<PerformanceResult> ExecuteAsync();

		/// <summary>
		/// Метод установки кастомной строки подключения
		/// </summary>
		/// <param name="connection"></param>
		void SetConnection(string connection);
	}
}