using Model;

namespace Data
{
	public interface IData
	{
		Task<T> Add<T>(T model) where T : Base;
		Task<bool> ChangeRowKey(Base oldModel, Base newModel);
		Task<bool> Delete(Base model);
		Task<T> Get<T>(string rowKey) where T : Base;
		Task<Query<T>> Query<T>(string filter = null) where T : Base;
		Task<T> Update<T>(T model) where T : Base;
	}
}