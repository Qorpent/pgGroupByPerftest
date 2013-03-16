namespace pgPerfTest {
	/// <summary>
	/// “естирует пользовательский агрегат - GROUP на партиции - UNION
	/// </summary>
	public class UserAggregateViewNotUnionAll : ExecuteQueryTest
	{
		public const string QUERY = @"
		select date,count(data)  from groupbytest.y2001_1test group by date
union	select date,count(data)  from groupbytest.y2001_2test group by date
union	select date,count(data)  from groupbytest.y2002_1test group by date
union	select date,count(data)  from groupbytest.y2002_2test group by date
union	select date,count(data)  from groupbytest.y2003_1test group by date
union	select date,count(data)  from groupbytest.y2003_2test group by date
union	select date,count(data)  from groupbytest.y2004_1test group by date
union	select date,count(data)  from groupbytest.y2004_2test group by date
union	select date,count(data)  from groupbytest.y2005_1test group by date
union	select date,count(data)  from groupbytest.y2005_2test group by date
union	select date,count(data)  from groupbytest.y2006_1test group by date
union	select date,count(data)  from groupbytest.y2006_2test group by date
union	select date,count(data)  from groupbytest.y2007_1test group by date
union	select date,count(data)  from groupbytest.y2007_2test group by date";
		public UserAggregateViewNotUnionAll() : base(QUERY, null) { }
	}
}