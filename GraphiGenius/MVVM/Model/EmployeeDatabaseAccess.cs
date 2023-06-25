using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphiGenius.MVVM.Model
{
    class EmployeeDatabaseAccess : DatabaseAccess
    {
        public List<int> _employees = new List<int>();
        public int[] loadEmployees(int departmentId)
        {
            _employees = new List<int>();
            DataTable dt = new DataTable();
            dt = dbConnect($"select Id from Employee where DepartmentId={departmentId};");
            _employees.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                _employees.Add(Convert.ToInt32(dt.Rows[i]["Id"]));
            }
            _employees.Sort();
            //System.Windows.MessageBox.Show(string.Join(", ", _departments));
            int[] ints = _employees.ToArray();
            return ints;
        }
        public string employeeName(int id)
        {
            DataTable dt = new DataTable();
            dt = dbConnect($"select Name from Employee where Id={id};");
            string nazwa = Convert.ToString(dt.Rows[0]["Name"]);
            //System.Windows.MessageBox.Show(nazwa);
            return nazwa;
        }
    }
}
