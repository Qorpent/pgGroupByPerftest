namespace pgPerfTest {
	/// <summary>
	/// ��������� GROUP BY �� ������� �������
	/// </summary>
	public class Single10MlnQueryTest : ExecuteQueryTest {
		public const string QUERY = @"select date,count(*) from groupbytest.m10test  group by date";
		public Single10MlnQueryTest():base(QUERY,null) {}	
	}
}