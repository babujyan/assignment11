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

        public CommandType CommandType { get; set; }

        public DAL()
        {
            this.CommandType = CommandType.Text;
        }

        public IEnumerable<T> GetData<T>(string code, ICollection<KeyValuePair<string, object>> parameters)
        {
            List<T> list = new List<T>();
            
            using (var connection = new SqlConnection(this.ConnectionString))
            {
                SqlCommand command = new SqlCommand(code, connection);
                //command.CommandType = this.CommandType;

                string query = "select * from sysobjects where type='P' and name=" + code;
                bool spExists = false;

                //
                
                
                connection.Open();
                using (SqlCommand command1 = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader1 = command1.ExecuteReader())
                    {
                        while (reader1.Read())
                        {
                            spExists = true;
                            break;
                        }
                    }
                }
                

                try
                {
                   // connection.Open();
                    
                    


                    if(spExists)
                    {
                        if(parameters != null)
                        {
                            foreach(var parameter in parameters)
                            {
                                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                            }
                        }
                    }

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
