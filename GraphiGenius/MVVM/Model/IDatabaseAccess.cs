using Microsoft.Data.Sqlite;
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
    abstract class IDatabaseAccess
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
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                sqlite.Close();
                return dt;
            }
        }


    }
}