using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using assignment11;

namespace DataAccessLayer
{
    /// <summary>
    /// Data Access Layer.
    /// </summary>
    class DAL : IDAL
    {
        public string ConnectionString { get; set; }
        /// <summary>
        /// The method executes the code and returns the result.
        /// </summary>
        /// <typeparam name="T">Return type of a method.</typeparam>
        /// <param name="code">SQL code.</param>
        /// <param name="parameters">Parametrs for code.</param>
        /// <returns>IEnumerable<T></returns>
        public IEnumerable<T> GetData<T>(string code, params KeyValuePair<string, object>[] parameters)
        {
            
            //In a debug folder.
            var inFile = File.ReadLines(@"test.txt");

            //IEnumerable<T> for returning values.
            List<T> result = new List<T>();

            //Creating new SQLconaction.
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = null;

                string[] c = null;

                foreach (var line in inFile)
                {
                    c = line.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if(c[0].Split(new string[] { "=>" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim() == code)
                    {
                        query = c[0].Split(new string[] { "=>" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
                        break;
                    }
                }

                SqlCommand command = new SqlCommand(query, connection);

                if (c.Length > 1)
                {
                    command.CommandType = CommandType.StoredProcedure;
                    for(var i = 1; i < c.Length; i++)
                    {
                        var haveParametrs = false;
                        var key = c[i].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();                      
                        foreach(var keyValuePair in parameters)
                        {
                            if(key == keyValuePair.Key)
                            {
                                command.Parameters.AddWithValue(c[i].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1].Trim(), keyValuePair.Value);
                                haveParametrs = true;
                                break;
                            }
                        }
                        if(!haveParametrs)
                        {
                            throw new ArgumentException("Parameter has not been given or been invalid");
                        }
                    }
                }
               
                //Opening connection.
                connection.Open();

                //Start reading data.
                using (SqlDataReader reader = command.ExecuteReader())
                { 
                    while (reader.Read())
                    {
                        T data = (T)typeof(T).GetConstructor(new Type[] { }).Invoke(null);
                        foreach (var propertyInfo in data.GetType().GetProperties())
                        {
                            propertyInfo.SetValue(data, reader[propertyInfo.Name]);
                        }

                        result.Add(data);
                    }
                }
                              
            }

            return result;
        }
    }
}