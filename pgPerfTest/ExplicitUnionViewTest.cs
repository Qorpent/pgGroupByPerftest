namespace pgPerfTest {
	/// <summary>
	/// Тестирует GROUP BY на вьюхе - простом объединении партиций
	/// </summary>
	public class ExplicitUnionViewTest : ExecuteQueryTest
	{
		public const string QUERY = @"select date,count(*) from groupbytest.partitionedunion  group by date";
		public ExplicitUnionViewTest() : base(QUERY, null) { }
	}
}