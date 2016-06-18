using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;


namespace Modelowanie
{
    class Database
    {
        public static string connectionString = "Data Source=DESKTOP-6NU2EAE;Initial Catalog=uniterm;Password=modelowanie; User Id=modelowanie";
        private SqlConnection conString;

        public Database() {
            this.conString = new SqlConnection(Database.connectionString);
        }

        public Database(string conStr)
        {
            this.conString = new SqlConnection(conStr);
        }


        public SqlConnection ConnectionString
        {
            get
            {
                return this.conString;
            }
            set
            {
                this.conString = value;
            }
        }


        public void Connect()
        {
            try
            {
                this.conString.Open();
            }
            catch (InvalidCastException ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void Disconnect()
        {
            try
            {
                this.conString.Close();
            }
            catch
            {

            }
        }


        public SqlDataAdapter CreateAdapter(string query) {
            return new SqlDataAdapter(query, this.ConnectionString);
        }

        public DataTable CreateDataTable(string query)
        {
            DataTable tab = new DataTable();
            this.Connect();

            if (this.ConnectionString.State == ConnectionState.Open)
            {
                SqlDataAdapter ad = CreateAdapter(query);
                ad.Fill(tab);
            } else
            {
                throw new Exception("Nie mozna polaczyc z baza");
            }

            this.Disconnect();

            return tab;
        }

        public DataRow CreateDataRow(string query)
        {
            DataTable tab = new DataTable();

            DataRow row;

            this.Connect();

            if (this.ConnectionString.State == ConnectionState.Open)
            {

                SqlDataAdapter ad = CreateAdapter(query);

                ad.Fill(tab);
                try
                {
                    row = tab.Rows[0];
                }
                catch
                {
                    row = null;
                }
            }
            else
            {
                throw new Exception("Nie można połączyć się z bazą daych");
            }

            this.Disconnect();

            return row;
        }

        public void RunQuery(string query)
        {
            Console.WriteLine(query);
            this.Connect();

            if (this.ConnectionString.State == ConnectionState.Open)
            {
                SqlCommand cmd = new SqlCommand(query, this.ConnectionString);

                cmd.ExecuteNonQuery();
            }
            else
            {
                throw new Exception("Nie można połączyć się z bazą daych");
            }

            this.Disconnect();
        }


    }
}
