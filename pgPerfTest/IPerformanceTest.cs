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
	/// 	����� ��������� ������
	/// </summary>
	public interface IPerformanceTest {
		/// <summary>
		/// ��������� ���� � ���������� ��������
		/// </summary>
		/// <returns></returns>
		PerformanceResult Execute();
		/// <summary>
		/// ��������� ���� � ����������� ��������
		/// </summary>
		/// <returns></returns>
		Task<PerformanceResult> ExecuteAsync();

		/// <summary>
		/// ����� ��������� ��������� ������ �����������
		/// </summary>
		/// <param name="connection"></param>
		void SetConnection(string connection);
	}
}