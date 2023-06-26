using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphiGenius.MVVM.Model
{
    class DepartmentsDatabaseAccess : DatabaseAccess
    {
        public List<int> _departments = new List<int>();
        public int[] loadDepartments(int index = 0)
        {
            DataTable dt = new DataTable();
            dt = dbConnect("select Id from Department;");
            _departments.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                _departments.Add(Convert.ToInt32(dt.Rows[i]["Id"]));
            }
            _departments.Sort();
            //System.Windows.MessageBox.Show(string.Join(", ", _departments));
            int[] ints = _departments.ToArray();
            return ints;
        }
        public string departmentName(int id)
        {
            DataTable dt = new DataTable();
            dt = dbConnect($"select Name from Department where Id={id};");
            string nazwa = Convert.ToString(dt.Rows[0]["Name"]);
            //MessageBox.Show(nazwa);
            return nazwa;
        }
        public void addDepartment()
        {
            dbConnectAdd($"INSERT INTO Department (Name)VALUES ('New Department');");
            DataTable dt = new DataTable();
            dt = dbConnect($"select Id from Department where Name='New Department';");
            string id = Convert.ToString(dt.Rows[0]["Id"]);
            dbConnectAdd($"INSERT INTO Day (DepartmentId, ShiftLength, DayOfWeek, StartHour, StartMinute, EndHour, EndMinute, Shifts ) VALUES ({id}, 0, 0, 0,0 , 0, 0, 0),({id}, 0, 1, 0, 0, 0, 0, 0), ({id}, 0, 2, 0, 0, 0, 0, 0),    ({id}, 0, 3, 0, 0, 0, 0, 0), ({id}, 0, 4, 0, 0, 0, 0, 0), ({id}, 0, 5, 0, 0, 0, 0, 0), ({id}, 0, 6, 0, 0, 0, 0, 0);");
        }
        public void editDepartment(Department department)
        {
            dbConnectAdd($"UPDATE Department SET Name = {department.Name} WHERE Id={department.Id};");
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
        public Department loadEmployee(int departmentId)
        {
            Department department = new();
            DataTable dt = new DataTable();
            dt = dbConnect($"select * from Department where Id={departmentId};");
            department.Id = departmentId;
            department.Name = Convert.ToString(dt.Rows[0]["Name"]);
            dt = dbConnect($"select * from Day where DepartmentId={departmentId} ORDER by DayOfWeek;");
            for( int i = 0; i < 7; i++)
            {
                department.Shifts.Add(Convert.ToInt32(dt.Rows[i]["Shifts"]));
                department.ShiftLengths.Add(Convert.ToInt32(dt.Rows[i]["ShiftLengths"]));
                department.StartHours.Add(Convert.ToInt32(dt.Rows[i]["StartHours"]));
                department.StartHours.Add(Convert.ToInt32(dt.Rows[i]["StartHours"]));
                department.StartMinutes.Add(Convert.ToInt32(dt.Rows[i]["StartMinutes"]));
                department.EndHours.Add(Convert.ToInt32(dt.Rows[i]["EndHours"]));
                department.EndMinutes.Add(Convert.ToInt32(dt.Rows[i]["EndMinutes"]));
            }
            return department;
        }
        public void deleteDepartment(int departmentId)
        {
            dbConnectAdd($"DELETE FROM Days where DepartmentId={departmentId};");
            dbConnectAdd($"DELETE FROM Department where Id={departmentId};");
        }
    }
}
