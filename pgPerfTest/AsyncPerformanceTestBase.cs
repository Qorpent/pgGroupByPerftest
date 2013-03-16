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
	/// 	����� ���������� ����� �� ������������������ � ����������� ������
	/// </summary>
	public abstract class AsyncPerformanceTestBase : IPerformanceTest {
		

		/// <summary>
		/// 	����������� ������� ������
		/// </summary>
		/// <returns> </returns>
		public Task<PerformanceResult> ExecuteAsync() {
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

		/// <summary>
		/// ����� ��������� ��������� ������ �����������
		/// </summary>
		/// <param name="connection"></param>
		public virtual void SetConnection(string connection) {
			this.Connection = connection;
		}

		protected string Connection { get;  private set; }

		protected abstract void ExecuteInternalTest(PerformanceResult result);

		/// <summary>
		/// 	���������� ������� ������
		/// </summary>
		/// <returns> </returns>
		public PerformanceResult Execute() {
			var task = ExecuteAsync();
			task.Wait();
			return task.Result;
		}
	}
}