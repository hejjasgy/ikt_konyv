using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Documents;
using MySql.Data.MySqlClient;

namespace IKT_Konyvek_Lazulasz_Partizasz{
    public class Adatbazis{
        
        static MySqlConnection databaseConnection{
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
                            mySqlDataReader["booktitle"].ToString(),
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
        
        public static Boolean UPDATE(int id, Konyv ujKonyv){
            List<Konyv> list = new List<Konyv>();
            MySqlCommand mySqlCommand = new MySqlCommand();
            mySqlCommand.CommandType = CommandType.Text;
            mySqlCommand.CommandText = "UPDATE books SET booktitle = @CIM, author=@SZERZO, publisher=@KIADO, price=@AR, stock=@RAKTARON WHERE id=@ID";
            
            MySqlConnection connection = databaseConnection;
            try{
                mySqlCommand.Connection = connection;
                mySqlCommand.Parameters.Add(new MySqlParameter("@CIM", MySqlDbType.VarChar)).Value = ujKonyv.Cim;
                mySqlCommand.Parameters.Add(new MySqlParameter("@SZERZO", MySqlDbType.VarChar)).Value = ujKonyv.Szerzo;
                mySqlCommand.Parameters.Add(new MySqlParameter("@KIADO", MySqlDbType.VarChar)).Value = ujKonyv.Kiado;
                mySqlCommand.Parameters.Add(new MySqlParameter("@AR", MySqlDbType.Decimal)).Value = ujKonyv.Ar;
                mySqlCommand.Parameters.Add(new MySqlParameter("@RAKTARON", MySqlDbType.Int32)).Value = ujKonyv.Raktaron;
                mySqlCommand.Parameters.Add(new MySqlParameter("@ID", MySqlDbType.Int32)).Value = ujKonyv.getID();

                mySqlCommand.Connection.Open();
                try{
                    mySqlCommand.ExecuteNonQuery();
                }catch(Exception exception){
                    Console.WriteLine(exception);
                    return false;
                }
            }catch(Exception exception){ 
                Console.WriteLine(exception);
                return false;
            }
            
            connection.Close();
            return true;
        }
        
    }
}
