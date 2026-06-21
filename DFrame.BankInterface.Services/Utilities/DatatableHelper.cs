using System.Data;

namespace ExpenseTracker.Services.Utilities
{
	public static class DatatableHelper
	{

		/// <summary>
		/// Convert Datatble to List of T
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static List<T> BindList<T>(DataTable dt)
		{
			// Get all public fields
			var fields = typeof(T).GetProperties();

			List<T> lst = new List<T>();

			foreach (DataRow dr in dt.Rows)
			{
				// Create the object of T
				var ob = Activator.CreateInstance<T>();

				foreach (var fieldInfo in fields)
				{
					foreach (DataColumn dc in dt.Columns)
					{
						// Matching the columns with fields
						if (fieldInfo.Name == dc.ColumnName)
						{

							// Get the value from the datatable cell
							object? value = dr[dc.ColumnName] != DBNull.Value ? dr[dc.ColumnName] : null;

							// Set the value into the object
							fieldInfo.SetValue(ob, value);
							break;
						}
					}
				}

				lst.Add(ob);
			}

			return lst;
		}
	}
}
