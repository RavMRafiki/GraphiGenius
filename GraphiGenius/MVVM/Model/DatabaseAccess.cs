using Microsoft.Data.Sqlite;
using Microsoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GraphiGenius.MVVM.Model
{
    abstract class DatabaseAccess
    {
        public DataTable dbConnect(string command)
        {
            using (SQLiteConnection sqlite = new SQLiteConnection("Data Source=GraphiGeniusDB.db; Version=3;"))
            {
                SQLiteDataAdapter ad;
                DataTable dt = new DataTable();
                try
                {
                    SQLiteCommand cmd;
                    sqlite.Open();
                    cmd = sqlite.CreateCommand();
                    cmd.CommandText = command;
                    ad = new SQLiteDataAdapter(cmd);
                    ad.Fill(dt);
                    //MessageBox.Show(dt.ToString());
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                sqlite.Close();
                return dt;
            }
        }
        public void dbConnectAdd(string command)
        {
            using (SQLiteConnection sqlite = new SQLiteConnection("Data Source=GraphiGeniusDB.db; Version=3;"))
            {
                DataTable dt = new DataTable();
                try
                {
                    SQLiteCommand cmd;
                    sqlite.Open();
                    cmd = sqlite.CreateCommand();
                    cmd.CommandText = command;
                    cmd.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                sqlite.Close();
            }
        }


    }
}