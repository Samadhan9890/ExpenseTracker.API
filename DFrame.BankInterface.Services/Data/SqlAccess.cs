using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Reflection;

namespace ExpenseTracker.Services.Data
{
	public class SqlAccess
	{

		private readonly AppDBContext _context;
		public SqlAccess(AppDBContext context)
		{
			_context = context;

		}

		public List<T> ExecuteQuery<T>(string query) where T : class, new()
		{
			using (var command = _context.Database.GetDbConnection().CreateCommand())
			{
				command.CommandText = query;
				command.CommandType = CommandType.Text;

				_context.Database.OpenConnection();

				using (var reader = command.ExecuteReader())
				{
					var lst = new List<T>();
					var lstColumns = new T().GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
					while (reader.Read())
					{
						var newObject = new T();
						for (var i = 0; i < reader.FieldCount; i++)
						{
							var name = reader.GetName(i);
							PropertyInfo prop = lstColumns.FirstOrDefault(a => a.Name.ToLower().Equals(name.ToLower()));
							if (prop == null)
							{
								continue;
							}
							var val = reader.IsDBNull(i) ? null : reader[i];
							prop.SetValue(newObject, val, null);
						}
						lst.Add(newObject);
					}

					return lst;
				}
			}
		}

		public DataTable ExceuteQuery(string query)
		{
			using (var command = _context.Database.GetDbConnection().CreateCommand())
			{
				try
				{
					command.CommandTimeout = 90;
					command.CommandText = query;
					command.CommandType = CommandType.Text;

					_context.Database.OpenConnection();

					using (System.Data.Common.DbDataReader result = command.ExecuteReader())
					{
						var dataTable = new DataTable();
						dataTable.Load(result);
						return dataTable;
					}
				}
				catch (Exception)
				{
					throw;
				}
				finally
				{
					if (command.Connection != null && !command.Connection.State.Equals(ConnectionState.Closed))
					{
						_context.Database.CloseConnection();

					}
				}
			}
		}

		/// <summary>
		/// This method executes a stored procedure and gets back datatable
		/// </summary>
		/// <param name="spName"></param>
		/// 
		/// <param name="parameters"></param>
		/// <returns></returns>
		public DataTable ExecuteScalarSp(string spName, Dictionary<string, string> parameters)
		{

			using (var command = _context.Database.GetDbConnection().CreateCommand())
			{
				try
				{
					command.CommandTimeout = 0;
					command.CommandText = spName;
					command.CommandType = CommandType.StoredProcedure;

					foreach (var param in parameters)
					{
						command.Parameters.Add(new SqlParameter(param.Key, param.Value));

					}

					_context.Database.OpenConnection();

					using (System.Data.Common.DbDataReader result = command.ExecuteReader())
					{
						var dataTable = new DataTable();
						dataTable.Load(result);
						return dataTable;
					}
				}
				catch (Exception)
				{
					throw;
				}
				finally
				{
					if (command.Connection != null && !command.Connection.State.Equals(ConnectionState.Closed))
					{
						_context.Database.CloseConnection();

					}
				}
			}

		}

		/// <summary>
		/// This method executes a stored procedure and gets back dataset
		/// </summary>
		/// <param name="spName"></param>
		/// 
		/// <param name="parameters"></param>
		/// <returns></returns>		
		public DataSet ExecuteMultiScalarSp(string spName, Dictionary<string, string> parameters)
		{
			// Create a DataSet to hold the results
			DataSet dataSet = new DataSet();

			// Create a new SqlCommand
			using (var sqlCmd = _context.Database.GetDbConnection().CreateCommand())
			{
				try
				{
					// Ensure the connection is open
					_context.Database.OpenConnection();

					// Set up the SqlCommand properties
					sqlCmd.CommandTimeout = 0;
					sqlCmd.CommandText = spName;
					sqlCmd.CommandType = CommandType.StoredProcedure;

					// Add parameters to the command
					foreach (var param in parameters)
					{
						var parameter = sqlCmd.CreateParameter();
						parameter.ParameterName = param.Key;
						parameter.Value = param.Value;
						sqlCmd.Parameters.Add(parameter);
					}

					// Use SqlDataAdapter to fill the DataSet
					using (var adapter = new SqlDataAdapter((SqlCommand)sqlCmd))
					{
						adapter.Fill(dataSet); // Fill the DataSet with all result sets
					}
				}
				catch (Exception ex)
				{
					// Handle exceptions, e.g., logging
					throw new Exception("An error occurred while executing the stored procedure", ex);
				}
				finally
				{
					// Ensure the connection is closed and disposed properly
					if (sqlCmd.Connection != null && sqlCmd.Connection.State != ConnectionState.Closed)
					{
						_context.Database.CloseConnection();
					}
				}
			}

			return dataSet;
		}





		//This method executes the command in the command object,this method will be generally used for data manipulation.
		//public void executeNonQuery()
		//{
		//    sqlCmd.Transaction = sqlTrn;
		//    sqlCmd.ExecuteNonQuery();
		//    if (sqlTrn == null)
		//    {
		//        sqlCmd.Connection.Close();
		//    }
		//}

		/// <summary>
		/// executeNonQuery
		/// </summary>
		/// <param name=""></param>
		/// <param name=""></param>
		/// <returns></returns>
		/// 
		public void executeNonQuery(string spName, Dictionary<string, string> parameters, IDbContextTransaction transaction = null)
		{

			using (var command = _context.Database.GetDbConnection().CreateCommand())
			{
				try
				{
					if (transaction != null)
					{
						command.Transaction = transaction.GetDbTransaction();
					}
					command.CommandTimeout = 0;
					command.CommandText = spName;
					command.CommandType = CommandType.StoredProcedure;

					foreach (var param in parameters)
					{
						command.Parameters.Add(new SqlParameter(param.Key, param.Value));

					}
					_context.Database.OpenConnection();
					if (transaction != null)
					{
						command.Transaction = transaction.GetDbTransaction();
					}
					command.ExecuteNonQuery();
				}
				catch (Exception)
				{
					throw;
				}
				finally
				{
					if (command.Connection != null && !command.Connection.State.Equals(ConnectionState.Closed))
					{
						_context.Database.CloseConnection();

					}
				}
			}
		}


	}
}
