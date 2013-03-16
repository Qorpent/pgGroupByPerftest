namespace pgPerfTest {
	/// <summary>
	/// ��������� GROUP BY �� ����� - ������� ����������� ��������
	/// </summary>
	public class ExplicitUnionViewTest : ExecuteQueryTest
	{
		public const string QUERY = @"select date,count(*) from groupbytest.partitionedunion  group by date";
		public ExplicitUnionViewTest() : base(QUERY, null) { }
	}
}