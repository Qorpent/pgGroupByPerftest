#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : SinglePartitionQueryingTest.cs
// Project: pgPerfTest
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace pgPerfTest {
	/// <summary>
	/// 	Выполняет запрос к отдельной партиции, считывает результат и подсчитывает время
	/// </summary>
	public class SinglePartitionQueryingTest : ExecuteQueryTest {
		public const string DEFAULT_QUERY = "select date,count(data)  from groupbytest.y{0}_{1}test group by date;";

		public SinglePartitionQueryingTest(int year, HalfYear halfYear, string query = null,string connection=null):base(query,connection) {
			HalfYear = halfYear;
			Year = year;
			if (String.IsNullOrWhiteSpace(Query)) {
				Query = DEFAULT_QUERY;
			}
			Query = String.Format(Query, Year, (int) HalfYear);
			
		}

		/// <summary>
		/// 	Год партиции
		/// </summary>
		public int Year { get; private set; }

		public HalfYear HalfYear { get; private set; }
	}
}