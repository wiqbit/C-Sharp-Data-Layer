using Azure;
using Azure.Data.Tables;

namespace Model
{
	public enum TableName
	{
	}

	public abstract class Base : ITableEntity
	{
		public Base(TableName tableName)
		{
			_tableName = tableName;
			_partitionKey = "Default";
		}

		private TableName _tableName;
		private string _partitionKey;
		private string _rowKey;
		private DateTimeOffset? _timestamp;
		private ETag _eTag;
		private bool _success;
		private string _message;

		public TableName TableName
		{
			get
			{
				return _tableName;
			}
		}

		public string PartitionKey 
		{
			get
			{
				return _partitionKey;
			}
			set
			{
				_partitionKey = value;
			}
		}

		public string RowKey
		{ 
			get
			{
				return _rowKey;
			}
			set
			{
				_rowKey = value;
			}
		}

		public DateTimeOffset? Timestamp 
		{ 
			get
			{
				return _timestamp;
			}
			set
			{
				_timestamp = value;
			}
		}

		public ETag ETag
		{
			get
			{
				return _eTag;
			}
			set
			{
				_eTag = value;
			}
		}

		public bool Success
		{
			get
			{
				return _success;
			}
			set
			{
				_success = value;
			}
		}

		public string Message
		{
			get
			{
				return _message;
			}
			set
			{
				_message = value;
			}
		}
	}
}