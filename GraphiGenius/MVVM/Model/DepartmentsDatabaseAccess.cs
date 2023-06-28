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
