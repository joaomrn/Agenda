using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace MyFInance.Util
{
    public class DAL
    {
        private static String server = "localhost";
        private static String database = "financeiro";
        private static String user = "root";
        private static String password = "";
        //Cria String de conexao
        private String connectionString = $"Server={server};Database={database};Uid={user};Pad={password};SslMode=none";
        private MySqlConnection connection;

        public DAL()
        {
            //Recebe a string de conexao no construtor
            connection = new MySqlConnection(connectionString);
            //Abri a conexao
            connection.Open();
        }

        //Executa SELECTs
        public DataTable RetDataTable(String sql)
        {
            DataTable dataTable = new DataTable();
            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataAdapter da = new MySqlDataAdapter(command);
            da.Fill(dataTable);
            return dataTable;
        }

        //Executa INSERTs, UPDATs e DELETEs
        public void ExecutarComandoSql(String sql)
        {
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.ExecuteNonQuery();
        }
    }
}
