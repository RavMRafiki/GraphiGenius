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
using System.Windows.Controls;
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
            Departments = new();
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
                _reloadEmployees();
                loadDepartment();
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
        private string departmentNameForm;
        public string DepartmentNameForm
        {
            get { return departmentNameForm; }
            set
            {
                departmentNameForm = value;
                _departmentForm.Name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DepartmentNameForm)));

            }
        }
        private ObservableCollection<int> shiftsForm;
        public ObservableCollection<int> ShiftsForm
        {
            get { return shiftsForm; }
            set
            {
                shiftsForm = value;
                _departmentForm.Shifts = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShiftsForm)));
            }
        }
        private ObservableCollection<int> shiftLenghtsForm;
        public ObservableCollection<int> ShiftLenghtsForm
        {
            get { return shiftLenghtsForm; }
            set
            {
                shiftLenghtsForm = value;
                _departmentForm.ShiftLengths = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShiftLenghtsForm)));
            }
        }
        private ObservableCollection<int> startHoursForm;
        public ObservableCollection<int> StartHoursForm
        {
            get { return shiftLenghtsForm; }
            set
            {
                shiftLenghtsForm = value;
                _departmentForm.StartHours = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartHoursForm)));
            }
        }
        private ObservableCollection<int> startMinutesForm;
        public ObservableCollection<int> StartMinutesForm
        {
            get { return startMinutesForm; }
            set
            {
                startMinutesForm = value;
                _departmentForm.StartMinutes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartMinutesForm)));
            }
        }
        private ObservableCollection<int> endHoursForm;
        public ObservableCollection<int> EndHoursForm
        {
            get { return endHoursForm; }
            set
            {
                endHoursForm = value;
                _departmentForm.EndHours = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EndHoursForm)));
            }
        }
        private ObservableCollection<int> endMinutesForm;
        public ObservableCollection<int> EndMinutesForm
        {
            get { return endMinutesForm; }
            set
            {
                endMinutesForm = value;
                _departmentForm.EndMinutes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EndMinutesForm)));
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
        #region EditDepartments
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
        private ICommand _saveDepartment;
        public ICommand SaveDepartment
        {
            get
            {
                return _saveDepartment ?? (_saveDepartment = new BaseClass.RelayCommand(
                (p) => {
                    saveDepartment();
                }
                ,
            p => true)
                );
            }
        }
        private ICommand _deleteDepartment;
        public ICommand DeleteDepartment
        {
            get
            {
                return _deleteDepartment ?? (_deleteDepartment = new BaseClass.RelayCommand(
                (p) => {
                    deleteDepartment();
                }
                ,
            p => true)
                );
            }
        }
        private void loadDepartment()
        {
            if(currentDepartmentIndex != -1)
            {
                _departmentForm = _departmentsDatabaseAccess.loadDepartment(departmentsIds[currentDepartmentIndex]);
            }
            //MessageBox.Show(messageBoxText: _departmentForm.Name);
            DepartmentNameForm = _departmentForm.Name;
            ShiftsForm= _departmentForm.Shifts;
            ShiftLenghtsForm = _departmentForm.ShiftLengths;
            StartHoursForm= _departmentForm.StartHours;
            StartMinutesForm= _departmentForm.StartMinutes;
            EndHoursForm= _departmentForm.EndHours;
            EndMinutesForm= _departmentForm.EndMinutes;
        }
        private void saveDepartment()
        {
            _departmentsDatabaseAccess.editDepartment(_departmentForm);
            _reloadDepartments();
        }
        private void deleteDepartment()
        {
            _departmentsDatabaseAccess.deleteDepartment(_departmentForm.Id);
            _reloadDepartments();
        }
        private void addDepartment()
        {
            _departmentsDatabaseAccess.addDepartment();
            _reloadDepartments();
            EditDepartment = true;
            CurrentDepartmentIndex= departmentsIds.Length - 1;

        }
        #endregion
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
            if(currentEmployeeIndex != -1)
            {
                _employeeForm = _employeeDatabaseAccess.loadEmployee(employeesIds[currentEmployeeIndex]);
            }

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
            #region Generate
        private ICommand _generatesettings;

        public ICommand GenerateSettings
        {
            get
            {
                // jesli nie jest określone polecenie to tworzymy je i zwracamy poprozez 
                //pomocniczy typ RelayCommand
                return _generatesettings ?? (_generatesettings = new BaseClass.RelayCommand(
                    //co wykonuje polecenie
                    (p) => {
                        generateSettings();
                    }
                    ,
                    //warunek kiedy może je wykonać
                    p => true)
                    );
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
                        generate();
                    }
                    ,
                    //warunek kiedy może je wykonać
                    p => true)
                    );
            }
        }
        private void generateSettings()
        {
            EditSettings = true;
        }

        /*
            public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int IndexOfShift { get; set; }
        public int DayInMonth { get; set; }
        public double ShiftLengthDay { get; set; }
        public int StartHourDay { get; set; }
        public int StartMinuteDay { get; set; }
        public int EndHourDay { get; set; }
        public int EndMinuteDay { get; set; }
        public int ShiftsDay { get; set; }
        */

        private void generate()
        {

           
           


           string generatehtml = @"<!DOCTYPE html>
                                <html lang=""pl"">
                                <head>

                                    <meta charset=""utf-8"">
                                    <title>Pole Minowe</title>
                                    <meta name=""keywords"" content=""javascript, jQuery, game, gwent, memory"">
                                    <meta name=""author"" content=""BB"">
                                    
                                    <meta http-equiv=""X-Ua-Compatible"" content=""IE=edge,chrome=1"">


                                    <link rel=""stylesheet"" href=""grafmain.css"">


                                     <link href=""https://fonts.googleapis.com/css2?family=Rubik+Beastly&display=swap"" rel=""stylesheet""> 

                                    
                                    
                                    <!--[if lt IE 9]>
                                    <script src=""//cdnjs.cloudflare.com/ajax/libs/html5shiv/3.7.3/html5shiv.min.js""></script>
                                    <![endif]-->




                                    <style>

                                    body {
                                        margin: 0;
                                        background-color: #35363a;
                                        color: #fafafa;
                                        font-family: ""Lobster"", sans-serif;
                                        font-size: 18px;
                                        text-align: center;
                                    }

                                    #board {
                                        background-color: #787a80;
                                        justify-content: center;
                                        margin: auto;
                                        height: 540px;
                                        width: 1200px;
                                    }

                                    #monthname {
                                        width: 100px;
                                    }

                                    a:link {
                                        color: #fafafa;
                                        text-decoration: none;
                                    }

                                    a:visited {
                                        color: #fafafa;
                                    }

                                    a:active {
                                        color: #fafafa;
                                    }

                                    a:hover {
                                        color: #e9b64a;
                                    }

                                    h1 {
                                        font-size: 64px;
                                        font-weight: 200;
                                        text-align: center;
                                        letter-spacing: 5px;
                                        margin-top: 20px;
                                        margin-bottom: 0px;
                                        color: #902936;
                                    }

                                    .blad {
                                        background-color: rgb(92, 50, 170);
                                        margin: 0px;
                                        width: 30px;
                                        height: 40px;
                                        cursor: cell;
                                        border-bottom: 1px solid #dadada;
                                    }

                                    .blad2 {
                                        background-color: rgb(255, 168, 55);
                                        margin: 0px;
                                        width: 30px;
                                        height: 40px;
                                        cursor: cell;
                                        border-bottom: 1px solid #dadada;
                                    }

                                    .days {
                                        border: 1px solid #e9b64a;
                                        margin: 0px;
                                        width: 30px;
                                        height: 40px;
                                    }

                                    .weekendchanges {
                                        border: 1px solid #e9b64a;
                                        margin: 0px;
                                        width: 30px;
                                        height: 40px;
                                        background-color: rgb(197, 195, 70);
                                    }

                                    .urlop {
                                        background-color: plum;
                                    }

                                    .changes {
                                        margin: 0px;
                                        width: 30px;
                                        height: 40px;
                                    }

                                    table, th, td {
                                        border-collapse: collapse;
                                    }

                                    .dniowka, .nocka, .changes, .urlop, .check, .weekendchanges {
                                        border: 1px solid #dadada;
                                        cursor: pointer;
                                        color: black;
                                        font-weight: 400;
                                    }

                                    .check {
                                        background-color: rgb(170, 22, 22);
                                        margin: 0px;
                                        width: 30px;
                                        height: 40px;
                                        cursor: cell;
                                    }

                                    .cor {
                                        background-color: rgb(24, 201, 92);
                                        margin: 0px;
                                        width: 30px;
                                        height: 40px;
                                        cursor: cell;
                                    }

                                    .box {
                                        cursor: pointer;
                                        border: 1px solid #dadada;
                                        border-collapse: collapse;
                                    }
                                    </style>

                                </head>

                                <body>

                                    <header>
                                    
                                        <h1>Grafik pracy</h1>
                                    
                                    
                                    </header>
                                    
                                    <main>
                                        
                                        <div id=""board""></div>
                                        
                                    </main>
                                    


                                </body>

                                </html>";
            WebBrowserWindow webBrowserWindow = new();
            webBrowserWindow.Show();
            webBrowserWindow.webBrowser1.NavigateToString(generatehtml);
            //throw new NotImplementedException();
        }
        #endregion
            #region ChooseView
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
