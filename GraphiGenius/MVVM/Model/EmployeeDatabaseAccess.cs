using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GraphiGenius.MVVM.Model
{
    class EmployeeDatabaseAccess : DatabaseAccess
    {
        public int[] loadEmployees(int departmentId)
        {
            List<int> _employees = new List<int>();
            _employees = new List<int>();
            DataTable dt = new DataTable();
            dt = dbConnect($"select Id from Employee where DepartmentId={departmentId};");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                _employees.Add(Convert.ToInt32(dt.Rows[i]["Id"]));
            }
            _employees.Sort();
            Debug.WriteLine("jdfjdskf" + _employees[0]);
            int[] ints = _employees.ToArray();
            return ints;
        }
        public string employeeName(int id)
        {
            DataTable dt = new DataTable();
            dt = dbConnect($"select Name from Employee where Id={id};");
            string nazwa = Convert.ToString(dt.Rows[0]["Name"]);
            return nazwa;
        }
        public void addEmployee(int departmentId)
        {
            dbConnectAdd($"INSERT INTO Employee (Name, HourSalary, WorkingHours, DepartmentId) VALUES ('New Employee', 0, 0, {departmentId});");
        }
        public void editEmployee(Employee employee) 
        {
            dbConnectAdd($"UPDATE Employee SET Name = '{employee.Name}',HourSalary = {employee.HourSalary}, WorkingHours = {employee.WorkingHours} WHERE Id = {employee.Id};");
        }
        public Employee loadEmployee(int employeeId)
        {
            Employee employee = new();
            DataTable dt = new DataTable();
            dt = dbConnect($"select * from Employee where Id={employeeId};");
            employee.Id = employeeId;
            employee.Name = Convert.ToString(dt.Rows[0]["Name"]);
            employee.DepartmentId = Convert.ToInt32(dt.Rows[0]["DepartmentId"]);
            employee.WorkingHours = Convert.ToInt32(dt.Rows[0]["WorkingHours"]);
            employee.HourSalary = Convert.ToInt32(dt.Rows[0]["HourSalary"]);
            return employee;
        }
        public void deleteEmployee(int employeeId)
        {
            dbConnectAdd($"DELETE FROM Employee where Id={employeeId};");
        }
    }
}
