using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Model;

namespace Data
{
	public class AzureTableData : IData
	{
		private readonly IConfiguration _configuration;
		private readonly string _connectionString;

		public AzureTableData(IConfiguration configuration)
		{
			_configuration = configuration;
			_connectionString = _configuration.GetConnectionString("<YOUR AZURE CONNECTION STRING NAME>");
		}

		public async Task<T> Add<T>(T model) where T : Base
		{
			T result = default;

			TableServiceClient tableServiceClient = new TableServiceClient(_connectionString);

			TableClient tableClient = tableServiceClient.GetTableClient(model.TableName.ToString());

			await tableClient.CreateIfNotExistsAsync();

			try
			{
				await tableClient.AddEntityAsync(model);

				result = await Get<T>(model.RowKey);
			}
			catch
			{
			}

			return result;
		}

		public async Task<bool> ChangeRowKey(Base oldModel, Base newModel)
		{
			bool result = false;

			TableServiceClient tableServiceClient = new TableServiceClient(_connectionString);

			TableClient tableClient = tableServiceClient.GetTableClient(oldModel.TableName.ToString());

			await tableClient.CreateIfNotExistsAsync();

			try
			{
				List<TableTransactionAction> tableTransactionActions = new List<TableTransactionAction>();

				tableTransactionActions.Add(new TableTransactionAction(TableTransactionActionType.Delete, oldModel));
				tableTransactionActions.Add(new TableTransactionAction(TableTransactionActionType.Add, newModel));

				await tableClient.SubmitTransactionAsync(tableTransactionActions);

				result = true;
			}
			catch
			{
			}

			return result;
		}

		public async Task<bool> Delete(Base model)
		{
			TableServiceClient tableServiceClient = new TableServiceClient(_connectionString);

			TableClient tableClient = tableServiceClient.GetTableClient(model.TableName.ToString());

			await tableClient.CreateIfNotExistsAsync();

			Response response = await tableClient.DeleteEntityAsync(model);

			return response.Status != 404;
		}

		public async Task<T> Get<T>(string rowKey) where T : Base
		{
			T result = default;
			T temp = Activator.CreateInstance<T>();

			TableServiceClient tableServiceClient = new TableServiceClient(_connectionString);

			TableClient tableClient = tableServiceClient.GetTableClient(temp.TableName.ToString());

			await tableClient.CreateIfNotExistsAsync();

			try
			{
				result = await tableClient.GetEntityAsync<T>(temp.PartitionKey, rowKey);
			}
			catch
			{
			}

			return result;
		}

		public async Task<Query<T>> Query<T>(string filter = null) where T : Base
		{
			Query<T> result = Activator.CreateInstance<Query<T>>();
			T temp = Activator.CreateInstance<T>();

			TableServiceClient tableServiceClient = new TableServiceClient(_connectionString);

			TableClient tableClient = tableServiceClient.GetTableClient(temp.TableName.ToString());

			await tableClient.CreateIfNotExistsAsync();

			try
			{
				AsyncPageable<T> pages = tableClient.QueryAsync<T>(filter);

				await foreach (Page<T> page in pages.AsPages())
				{
					foreach (T value in page.Values)
					{
						result.Results.Add(value);
					}
				}
			}
			catch
			{
			}

			return result;
		}

		public async Task<T> Update<T>(T model) where T : Base
		{
			T result = default;

			TableServiceClient tableServiceClient = new TableServiceClient(_connectionString);

			TableClient tableClient = tableServiceClient.GetTableClient(model.TableName.ToString());

			await tableClient.CreateIfNotExistsAsync();

			await tableClient.UpdateEntityAsync(model, model.ETag);

			result = await Get<T>(model.RowKey);

			return result;
		}
	}
}