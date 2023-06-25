using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphiGenius.MVVM.Model
{
    class DepartmentsDatabaseAccess : IDatabaseAccess
    {
        public List<int> _employees = new List<int>();
        public List<int> _departments = new List<int>();
        public int[] loadDepartments(int index = 0)
        {
            DataTable dt = new DataTable();
            dt = dbConnect("select distinct id from department;");
            _employees.Clear();
            _departments.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                _departments.Add(Convert.ToInt32(dt.Rows[i]["id"]));
            }
            _departments.Sort();
            System.Windows.MessageBox.Show(string.Join(", ", _departments));
            int[] ints = _departments.ToArray();
            return ints;
        }
        public string departmentName(int id)
        {
            DataTable dt = new DataTable();
            dt = dbConnect($"select Name from department where id={id};");
            string nazwa = Convert.ToString(dt.Rows[0]["Name"]);
            //MessageBox.Show(nazwa);
            return nazwa;
        }
    }
}
