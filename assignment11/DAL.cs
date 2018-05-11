using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment11
{
    class DAL : IDAL
    {
        public string ConnectionString { get; set; }

        public IEnumerable<T> GetData<T>(string code, ICollection<KeyValuePair<string, object>> parameters)
        {
            List<T> list = new List<T>();
            
            using (var connection = new SqlConnection(this.ConnectionString))
            {
                SqlCommand command = new SqlCommand(code, connection);
                var c = code.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if(c.Length == 1)
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                        }
                    }
                }
                else if(c.Length > 1)
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = code;
                }
                else
                {
                    
                }
                //command.CommandType = this.CommandType;
                //if(this.CommandType == CommandType.StoredProcedure)
                //{
                //    if(parameters != null)
                //    {
                //        foreach(var parameter in parameters)
                //        {
                //            command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                //        }
                //    }
                //}
                //else
                //{
                //    command.CommandText = code;
                //}

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        T obj = (T)typeof(T).GetConstructor(new Type[] { }).Invoke(null);
                        foreach (var propertyInfo in obj.GetType().GetProperties())
                        {
                            propertyInfo.SetValue(obj, reader[propertyInfo.Name]);
                        }

                        list.Add(obj);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                    

            }

            return list;
        }
    }
}
