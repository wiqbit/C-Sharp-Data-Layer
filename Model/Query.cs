namespace Model
{
	public class Query<T>
	{
		public Query()
		{
			Results = new List<T>();
		}

		public List<T> Results { get; set; }
		public bool Success { get; set; }
	}
}