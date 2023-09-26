using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
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
            MainWindow.StatusTextBlock.Content = "csatlakozás";

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
                            Int32.Parse(mySqlDataReader["price"].ToString()),
                            Int32.Parse(mySqlDataReader["stock"].ToString()));
                    
                    list.Add(konyv);
                }
            }catch(Exception exception){ 
                Console.WriteLine(exception);
                return new List<Konyv>(){new Konyv(-1, exception.Message, "", "", -1,-1)};
            }
            
            connection.Close();
            return list;
        }
        
        public static String UPDATE(int id, Konyv ujKonyv){
            MySqlCommand mySqlCommand = new MySqlCommand();
            mySqlCommand.CommandType = CommandType.Text;
            mySqlCommand.CommandText = "UPDATE books SET booktitle = @CIM, author=@SZERZO, publisher=@KIADO, price=@AR, stock=@RAKTARON WHERE id=@ID";
            
            MySqlConnection connection = databaseConnection;
            try{
                mySqlCommand.Connection = connection;
                mySqlCommand.Parameters.Add(new MySqlParameter("@CIM", MySqlDbType.VarChar)).Value = ujKonyv.Cim;
                mySqlCommand.Parameters.Add(new MySqlParameter("@SZERZO", MySqlDbType.VarChar)).Value = ujKonyv.Szerzo;
                mySqlCommand.Parameters.Add(new MySqlParameter("@KIADO", MySqlDbType.VarChar)).Value = ujKonyv.Kiado;
                mySqlCommand.Parameters.Add(new MySqlParameter("@AR", MySqlDbType.Int32)).Value = ujKonyv.Ar;
                mySqlCommand.Parameters.Add(new MySqlParameter("@RAKTARON", MySqlDbType.Int32)).Value = ujKonyv.Raktaron;
                mySqlCommand.Parameters.Add(new MySqlParameter("@ID", MySqlDbType.Int32)).Value = ujKonyv.getID();

                mySqlCommand.Connection.Open();
                try{
                    mySqlCommand.ExecuteNonQuery();
                }catch(Exception exception){
                    Console.WriteLine(exception);
                    return exception.Message;
                }
            }catch(Exception exception){ 
                Console.WriteLine(exception);
                return exception.Message;
            }
            
            connection.Close();
            return "Siker!";
        }
        
        public static String DELETE(int id){
            MySqlCommand mySqlCommand = new MySqlCommand();
            mySqlCommand.CommandType = CommandType.Text;
            mySqlCommand.CommandText = "DELETE FROM books WHERE id=@ID;";
            
            MySqlConnection connection = databaseConnection;
            try{
                mySqlCommand.Connection = connection;
                mySqlCommand.Parameters.Add(new MySqlParameter("@ID", MySqlDbType.Int32)).Value = id;

                mySqlCommand.Connection.Open();
                try{
                    mySqlCommand.ExecuteNonQuery();
                }catch(Exception exception){
                    Console.WriteLine(exception);
                    return exception.Message;
                }
            }catch(Exception exception){ 
                Console.WriteLine(exception);
                return exception.Message;
            }
            
            connection.Close();
            return "Siker";
        }

        public static String INSERT(Konyv konyv){
            MySqlCommand mySqlCommand = new MySqlCommand();
            mySqlCommand.CommandType = CommandType.Text;
            mySqlCommand.CommandText = "INSERT INTO `books`(`booktitle`, `author`, `publisher`, `price`, `stock`) VALUES (@CIM,@SZERZO,@KIADO,@AR,@RAKTARON);";
            
            MySqlConnection connection = databaseConnection;
            try{
                mySqlCommand.Connection = connection;
                mySqlCommand.Parameters.Add(new MySqlParameter("@CIM", MySqlDbType.VarChar)).Value = konyv.Cim;
                mySqlCommand.Parameters.Add(new MySqlParameter("@SZERZO", MySqlDbType.VarChar)).Value = konyv.Szerzo;
                mySqlCommand.Parameters.Add(new MySqlParameter("@KIADO", MySqlDbType.VarChar)).Value = konyv.Kiado;
                mySqlCommand.Parameters.Add(new MySqlParameter("@AR", MySqlDbType.Int32)).Value = konyv.Ar;
                mySqlCommand.Parameters.Add(new MySqlParameter("@RAKTARON", MySqlDbType.Int32)).Value = konyv.Raktaron;
                mySqlCommand.Connection.Open();
                try{
                    mySqlCommand.ExecuteNonQuery();
                }catch(Exception exception){
                    Console.WriteLine(exception);
                    return exception.Message;
                }
            }catch(Exception exception){ 
                Console.WriteLine(exception);
                MessageBox.Show(exception.Message);
                return exception.Message;
            }
            
            connection.Close();
            return "Siker";
        }
    }
}
