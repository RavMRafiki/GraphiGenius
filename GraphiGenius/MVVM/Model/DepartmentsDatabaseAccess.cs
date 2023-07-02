using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace GraphiGenius.MVVM.Model
{
    class DepartmentsDatabaseAccess : DatabaseAccess
    {
        public int[] loadDepartments(int index = 0)
        {
            List<int> _departments = new List<int>();
            DataTable dt = new DataTable();
            dt = dbConnect("select Id from Department;");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                _departments.Add(Convert.ToInt32(dt.Rows[i]["Id"]));
            }
            _departments.Sort();
            int[] ints = _departments.ToArray();
            return ints;
        }
        public List<List<int>> DepartmentInfo()
        {
            DataTable dt = new DataTable();
            int employees_per_shift = 0;
            int days_per_week = 0;
            int shift_length = 0;
            List<List<int>> ints = new List<List<int>>();
            dt = dbConnect($"SELECT Department.id, d1.ShiftLength AS SL1, d1.Shifts AS SH1, d2.ShiftLength AS SL2, d2.Shifts AS SH2, d3.ShiftLength AS SL3, d3.Shifts AS SH3, d4.ShiftLength AS SL4, d4.Shifts AS SH4, d5.ShiftLength AS SL5, d5.Shifts AS SH5, d6.ShiftLength AS SL6, d6.Shifts AS SH6, d7.ShiftLength AS SL7, d7.Shifts AS SH7\r\nFROM Department, Day as d1, Day as d2, Day as d3, Day as d4, Day as d5, Day as d6, Day as d7\r\nWHERE\r\nDepartment.Id = d1.DepartmentId AND Department.Id = d2.DepartmentId AND Department.Id = d3.DepartmentId AND Department.Id = d4.DepartmentId AND Department.Id = d5.DepartmentId AND Department.Id = d6.DepartmentId AND Department.Id = d7.DepartmentId\r\nAND d1.DayOfWeek = 0 AND d2.DayOfWeek = 1 AND d3.DayOfWeek = 2 AND d4.DayOfWeek = 3 AND d5.DayOfWeek = 4 AND d6.DayOfWeek = 5 AND d7.DayOfWeek = 6;");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                days_per_week = 0;
                employees_per_shift = Convert.ToInt32(dt.Rows[i]["SH1"]);
                if(Convert.ToInt32(dt.Rows[i]["SH1"]) > 0) days_per_week++;
                if (Convert.ToInt32(dt.Rows[i]["SH2"]) > 0) days_per_week++;
                if (Convert.ToInt32(dt.Rows[i]["SH3"]) > 0) days_per_week++;
                if (Convert.ToInt32(dt.Rows[i]["SH4"]) > 0) days_per_week++;
                if (Convert.ToInt32(dt.Rows[i]["SH5"]) > 0) days_per_week++;
                if (Convert.ToInt32(dt.Rows[i]["SH6"]) > 0) days_per_week++;
                if (Convert.ToInt32(dt.Rows[i]["SH7"]) > 0) days_per_week++;
                shift_length = Convert.ToInt32(dt.Rows[i]["SL1"]);
                ints.Add(new List<int>());
                ints[i].Add(employees_per_shift);
                ints[i].Add(days_per_week);
                ints[i].Add(shift_length);
                ints[i].Add(Convert.ToInt32(dt.Rows[i]["Id"]));
            }
            return ints;
        }
        public string departmentName(int id)
        {
            DataTable dt = new DataTable();
            dt = dbConnect($"select Name from Department where Id={id};");
            string nazwa = Convert.ToString(dt.Rows[0]["Name"]);
            return nazwa;
        }
        public void addDepartment()
        {
            dbConnectAdd($"INSERT INTO Department (Name) VALUES ('New Department');");
            DataTable dt = new DataTable();
            dt = dbConnect($"select Id from Department where Name='New Department';");
            string id = Convert.ToString(dt.Rows[0]["Id"]);
            dbConnectAdd($"INSERT INTO Day (DepartmentId, ShiftLength, DayOfWeek, StartHour, StartMinute, EndHour, EndMinute, Shifts ) VALUES ({id}, 0, 0, 0,0 , 0, 0, 0),({id}, 0, 1, 0, 0, 0, 0, 0), ({id}, 0, 2, 0, 0, 0, 0, 0),    ({id}, 0, 3, 0, 0, 0, 0, 0), ({id}, 0, 4, 0, 0, 0, 0, 0), ({id}, 0, 5, 0, 0, 0, 0, 0), ({id}, 0, 6, 0, 0, 0, 0, 0);");
        }
        public void editDepartment(Department department)
        {
            dbConnectAdd($"UPDATE Department SET Name = '{department.Name}' WHERE Id='{department.Id}';");
            for(int i = 0; i < 7; i++)
            {
                dbConnectAdd($"UPDATE Day SET " +
                    $"Shifts = {department.Shifts[i]}, " +
                    $"ShiftLength = {department.ShiftLengths[i]}, " +
                    $"StartHour = {department.StartHours[i]}, " +
                    $"StartMinute = {department.StartMinutes[i]}, " +
                    $"EndHour = {department.EndHours[i]}, " +
                    $"EndMinute = {department.EndMinutes[i]} " +
                    $"WHERE DayOfWeek = {i} AND " +
                    $"DepartmentId = {department.Id};");
            }
        }
        public Department loadDepartment(int departmentId)
        {
            Department department = new();
            DataTable dt = new DataTable();
            dt = dbConnect($"select * from Department where Id={departmentId};");
            department.Id = departmentId;
            department.Name = Convert.ToString(dt.Rows[0]["Name"]);
            dt = dbConnect($"select Shifts, ShiftLength, StartHour, StartMinute, EndHour, EndMinute from Day where DepartmentId={departmentId} ORDER by DayOfWeek;");
            if (dt.Rows.Count != 0)
            {
            ObservableCollection<int> _shifts = new ObservableCollection<int>();
            ObservableCollection<int> _shiftLenghts = new ObservableCollection<int>();
            ObservableCollection<int> _startHours = new ObservableCollection<int>();
            ObservableCollection<int> _startMinutes = new ObservableCollection<int>();
            ObservableCollection<int> _endHours = new ObservableCollection<int>();
            ObservableCollection<int> _endMinutes = new ObservableCollection<int>();

            for (int i = 0; i < 7; i++)
            {
                _shifts.Add(Convert.ToInt32(dt.Rows[i]["Shifts"]));
                _shiftLenghts.Add(Convert.ToInt32(dt.Rows[i]["ShiftLength"]));
                _startHours.Add(Convert.ToInt32(dt.Rows[i]["StartHour"]));
                _startMinutes.Add(Convert.ToInt32(dt.Rows[i]["StartMinute"]));
                _endHours.Add(Convert.ToInt32(dt.Rows[i]["EndHour"]));
                _endMinutes.Add(Convert.ToInt32(dt.Rows[i]["EndMinute"]));
            }
            department.Shifts= _shifts;
            department.ShiftLengths= _shiftLenghts;
            department.StartHours= _startHours;
            department.StartMinutes= _startMinutes;
            department.EndHours= _endHours;
            department.EndMinutes= _endMinutes;
            }
            return department;
        }
        public void deleteDepartment(int departmentId)
        {
            dbConnectAdd($"DELETE FROM Day where DepartmentId={departmentId};");
            dbConnectAdd($"DELETE FROM Employee where DepartmentId={departmentId};");
            dbConnectAdd($"DELETE FROM Department where Id={departmentId};");
        }
    }
}
