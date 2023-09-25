using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Documents;
using MySql.Data.MySqlClient;

namespace IKT_Konyvek_Lazulasz_Partizasz{
    public class Adatbazis{
        
        public static MySqlConnection databaseConnection{
            get{
                MySqlConnection connection = new MySqlConnection();
                String connectionString = "SERVER=192.168.50.13;"+"DATABASE=books_db;"+"UID=root;"+"PASSWORD=password;"+"SSL MODE=none;";
                connection.ConnectionString = connectionString;
                return connection;
            }
        }
        
        public static List<Konyv> SELECT(String select){
            List<Konyv> list = new List<Konyv>();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT "+ select + " FROM books;";
            
            MySqlConnection connection = databaseConnection;
            try{
                connection.Open();
                cmd.Connection = connection;
                MySqlDataReader mySqlDataReader = cmd.ExecuteReader();
                while(mySqlDataReader.Read()){
                    Konyv konyv = new Konyv(
                        Int32.Parse(mySqlDataReader["id"].ToString()),
                            mySqlDataReader["title"].ToString(),
                            mySqlDataReader["author"].ToString(),
                            mySqlDataReader["publisher"].ToString(),
                            double.Parse(mySqlDataReader["price"].ToString()),
                            Int32.Parse(mySqlDataReader["stock"].ToString()));
                    
                    list.Add(konyv);
                }
            }catch(Exception exception){ 
                Console.WriteLine(exception);
            }
            
            connection.Close();
            return list;
        }
        
    }
}
