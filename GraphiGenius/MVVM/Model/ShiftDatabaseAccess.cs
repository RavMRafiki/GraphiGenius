using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphiGenius.MVVM.Model
{
    class ShiftDatabaseAccess : DatabaseAccess
    {
        List<Shift> loadShifts()
        {
            return loadShifts("%");
        }
        List<Shift> loadShifts(string graphiName)
        {
            List<Shift> result = new List<Shift>();
            DataTable dt = new DataTable();
            dt = dbConnect($"SELECT s.Id AS ShiftId," +
                $" s.EmployeeId," +
                $" e.Name AS EmployeeName," +
                $" s.NumberOfShifts," +
                $" s.DayInMonth," +
                $" d.ShiftLength," +
                $" d.StartHour," +
                $" d.StartMinute," +
                $" d.EndHour," +
                $" d.EndMinute," +
                $" s.Shifts" +
                $" FROM  Shifts s " +
                $"INNER JOIN Employee e ON s.EmployeeId = e.Id " +
                $"INNER JOIN Day d ON s.DayId = d.Id " +
                $"INNER JOIN Graphi g ON s.GraphiId = g.Id " +
                $"WHERE g.Name like {graphiName};");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Shift shift = new();
                shift.Id = Convert.ToInt32(dt.Rows[i]["ShiftId"]);
                shift.EmployeeId = Convert.ToInt32(dt.Rows[0]["EmployeeId"]);
                shift.EmployeeName = Convert.ToString(dt.Rows[0]["EmployeeName"]);
                shift.IndexOfShift = Convert.ToInt32(dt.Rows[0]["NumberOfShifts"]);
                shift.DayInMonth = Convert.ToInt32(dt.Rows[0]["DayInMonth"]);
                shift.ShiftLengthDay = Convert.ToInt32(dt.Rows[0]["ShiftLength"]);
                shift.StartHourDay = Convert.ToInt32(dt.Rows[0]["StartHour"]);
                shift.StartMinuteDay = Convert.ToInt32(dt.Rows[0]["StartMinute"]);
                shift.EndHourDay = Convert.ToInt32(dt.Rows[0]["EndHour"]);
                shift.EndMinuteDay = Convert.ToInt32(dt.Rows[0]["EndMinute"]);
                shift.ShiftsDay = Convert.ToInt32(dt.Rows[0]["Shifts"]);
                result.Add( shift );
            }


            return result;
        }
        public void addShift(Shift shift)
        {
            dbConnectAdd($"INSERT INTO Shifts " +
                $"(EmployeeId, DayId, NumberOfShifts, DayInMonth, GraphiId)" +
                $" VALUES ({shift.EmployeeId}, {shift.DayId}, {shift.IndexOfShift}, {shift.DayInMonth}, {shift.GraphId});");
        }
        public Graphi loadGraphi(string name)
        {
            Graphi graphi = new();
            DataTable dt = new DataTable();
            dt = dbConnect($"SELECT Id, Name, Month, Year FROM Graphi WHERE Name={name};");
            graphi.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
            graphi.Name = Convert.ToString(dt.Rows[0]["Name"]);
            graphi.Month = Convert.ToInt32(dt.Rows[0]["Month"]);
            graphi.Year = Convert.ToInt32(dt.Rows[0]["Year"]);
            return graphi;
        }
        public void addGraphi(Graphi graphi)
        {
            dbConnectAdd($"INSERT INTO Graphi " +
                $"(Name, Month, Year) VALUES " +
                $"('{graphi.Name}',{graphi.Month},{graphi.Year});");
        }
    }
}
