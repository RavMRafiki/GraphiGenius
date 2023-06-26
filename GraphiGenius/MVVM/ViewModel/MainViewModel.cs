using GraphiGenius.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GraphiGenius.MVVM.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //public WindowStateViewModel WindowStateViewModel = new();
        private Model.DepartmentsDatabaseAccess _departmentsDatabaseAccess = new();
        private Model.EmployeeDatabaseAccess _employeeDatabaseAccess= new();
        private Model.Employee _employeeForm = new();
        private Model.Department _departmentForm = new();
        public MainViewModel()
        {
            _reloadDepartments();
            _reloadEmployees();
        }  
        private int[] departmentsIds;
        private void _reloadDepartments()
        {
            //Departments = new List<Department>();
            departmentsIds = _departmentsDatabaseAccess.loadDepartments();
            for (int i =0;i<departmentsIds.Length;i++)
            {
                Departments.Add(_departmentsDatabaseAccess.departmentName(departmentsIds[i]));
            }
        }
        private int[] employeesIds;
        private void _reloadEmployees()
        {
            Employees.Clear();
            employeesIds = _employeeDatabaseAccess.loadEmployees(departmentsIds[currentDepartmentIndex]);
            for (int i = 0; i < employeesIds.Length; i++)
            {
                Employees.Add(_employeeDatabaseAccess.employeeName(employeesIds[i]));
            }

        }
        private int currentDepartmentIndex = 0;
        public int CurrentDepartmentIndex
        {
            get { return currentDepartmentIndex; }
            set
            {
                EditDepartment = true;
                currentDepartmentIndex = value;
                //string ids = String.Join(", ", departmentsIds.ToString());
                _reloadEmployees();
                //zgłoszenie zmiany wartości tej własności
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentDepartmentIndex)));
            }
        }
        private ObservableCollection<string> departments = new ObservableCollection<string>();
        public ObservableCollection<string> Departments
        {
            get
            { return departments; }
            set
            {
                departments = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Departments)));
            }
        }

        private int currentEmployeeIndex = 0;
        public int CurrentEmployeeIndex
        {
            get { return currentEmployeeIndex; }
            set
            {
                if (value != -1)
                {
                    EditEmployee= true;
                }
                currentEmployeeIndex = value;
                loadEmployee();
                //zgłoszenie zmiany wartości tej własności
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentEmployeeIndex)));
            }
        }
        private string employeeNameForm;
        public string EmployeeNameForm
        {
            get { return employeeNameForm; }
            set
            {
                employeeNameForm = value;
                _employeeForm.Name= value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EmployeeNameForm)));

            }
        }
        private string employeeWorkingHoursForm;
        public string EmployeeWorkingHoursForm
        {
            get { return employeeWorkingHoursForm; }
            set { employeeWorkingHoursForm = value;
                _employeeForm.WorkingHours = Int32.Parse(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EmployeeWorkingHoursForm)));
            }
        }
        private ObservableCollection<string> employees = new ObservableCollection<string>();
        public ObservableCollection<string> Employees
        {
            get
            { return employees; }
            set
            {
                employees = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Employees)));
            }
        }
        private ICommand _generate;

        public ICommand Generate
        {
            get
            {
                // jesli nie jest określone polecenie to tworzymy je i zwracamy poprozez 
                //pomocniczy typ RelayCommand
                return _generate ?? (_generate = new BaseClass.RelayCommand(
                    //co wykonuje polecenie
                    (p) => {
                        generateGrahic();
                    }
                    ,
                    //warunek kiedy może je wykonać
                    p => true)
                    );
            }
        }
        private ICommand _addDepartment;


        public ICommand AddDepartment
        {
            get
            {
                // jesli nie jest określone polecenie to tworzymy je i zwracamy poprozez 
                //pomocniczy typ RelayCommand
                return _addDepartment ?? (_addDepartment = new BaseClass.RelayCommand(
                    //co wykonuje polecenie
                    (p) => {
                        addDepartment();
                    }
                    ,
                    //warunek kiedy może je wykonać
                    p => true)
                    );
            }
        }
            #region EditEmployees
        private ICommand _addEmployee;


        public ICommand AddEmployee
        {
            get
            {
                // jesli nie jest określone polecenie to tworzymy je i zwracamy poprozez 
                //pomocniczy typ RelayCommand
                return _addEmployee ?? (_addEmployee = new BaseClass.RelayCommand(
                    //co wykonuje polecenie
                    (p) => {
                        addEmployee();
                    }
                    ,
                    //warunek kiedy może je wykonać
                    p => true)
                    );
            }
        }
        private ICommand _saveEmployee;
        public ICommand SaveEmployee
        {
            get
            {
                return _saveEmployee ?? (_saveEmployee = new BaseClass.RelayCommand(
                (p) => {
                saveEmployee();
            }
                ,
            p => true)
                );
            }
        }
        private ICommand _deleteEmployee;
        public ICommand DeleteEmployee
        {
            get
            {
                return _deleteEmployee ?? (_deleteEmployee = new BaseClass.RelayCommand(
                (p) => {
                    deleteEmployee();
                }
                ,
            p => true)
                );
            }
        }
        private void loadEmployee()
        {
            _employeeForm = _employeeDatabaseAccess.loadEmployee(employeesIds[currentEmployeeIndex]);
            //MessageBox.Show(_employeeForm.Name + _employeeForm.HourSalary.ToString() + _employeeForm.WorkingHours.ToString());

            EmployeeNameForm = _employeeForm.Name;
            EmployeeWorkingHoursForm = _employeeForm.WorkingHours.ToString();
        }
        private void saveEmployee()
        {
            _employeeDatabaseAccess.editEmployee(_employeeForm);
            _reloadEmployees();
        }
        private void deleteEmployee()
        {
            _employeeDatabaseAccess.deleteEmployee(_employeeForm.Id);
            _reloadEmployees();

        }

        private void addEmployee()
        {
            _employeeDatabaseAccess.addEmployee(departmentsIds[currentDepartmentIndex]);
            _reloadEmployees();
            EditEmployee = true;
            CurrentEmployeeIndex = employeesIds.Length -1;

        }
        #endregion
        private void generateGrahic()
        {
            EditSettings = true;
        }
            #region ChooseView
        private void addDepartment()
        {
            EditDepartment= true;

        }
        public void selectedEmployeeChanged()
        {
            EditEmployee= true;
        }
        public void selectedDepartmentChanged()
        {
            EditDepartment = true;
        }
        private bool editEmployee = true;
        public bool EditEmployee
        {
            get { return editEmployee; }
            private set
            {
                editEmployee = value;
                if (value)
                {
                    EditDepartment = false;
                    EditSettings = false;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditEmployee)));
            }
        }
        private bool editDepartment = false;
        public bool EditDepartment
        {
            get { return editDepartment; }
            private set
            {
                editDepartment = value;
                if (value)
                {
                    EditEmployee = false;
                    EditSettings = false;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditDepartment)));
            }
        }
        private bool editSettings = false;
        public bool EditSettings
        {
            get { return editSettings; }
            private set
            {
                editSettings = value;
                if (value)
                {
                    EditEmployee = false;
                    EditDepartment = false;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditSettings)));
            }
        }
        #endregion
    }
}
