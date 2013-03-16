#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : PerformanceResult.cs
// Project: pgPerfTest
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;

namespace pgPerfTest {
	/// <summary>
	/// 	Описывает результат теста
	/// </summary>
	public class PerformanceResult {
		public PerformanceResult() {
			Subresults = new List<PerformanceResult>();
		}

		public IPerformanceTest Source { get; set; }
		public TimeSpan ExecuteTime { get; set; }
		public Exception Error { get; set; }
		public List<PerformanceResult> Subresults { get; private set; }
	}
}