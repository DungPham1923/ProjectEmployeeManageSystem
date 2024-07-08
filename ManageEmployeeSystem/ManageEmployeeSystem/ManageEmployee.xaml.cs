using ManageEmployeeSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ManageEmployeeSystem
{
    /// <summary>
    /// Interaction logic for ManageEmployee.xaml
    /// </summary>
    public partial class ManageEmployee : Window
    {

        public ManageEmployee()
        {
            InitializeComponent();
        }
        Employee em = new Employee();
        ProjectPrn212Context database = new ProjectPrn212Context();
        public ManageEmployee(Employee employee)
        {
            InitializeComponent();
            em = employee;
            this.Title = "Xin chào admin, " + em.FirstName + " " + em.LastName;
            LoadDataEmployee();
        }

        private void LoadDataEmployee()
        {
            var employee = database.Employees.Where(e => e.IsDelete == false).Select(e => new
            {
                ID = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                DateOfBirth = e.DateOfBirth,
                Gender = (bool) e.Gender ? "Nam": "Nữ",
                Address = e.Address,
                Salary = e.Salary,
                Department = e.Department.Name,
                Manager = e.Manager.FirstName + " " + e.Manager.LastName,
                Position = e.Position.Name,
            }).ToList();
            dgEmployee.ItemsSource = employee.ToList();
        }

        private void cbbFilterStatusEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idStatus = cbbFilterEmployeeStatus.SelectedIndex;
            if (idStatus == 0)
            {
                dgEmployee.ItemsSource = string.Empty;
                var employee = database.Employees.Where(e => e.IsDelete == false).Select(e => new
                {
                    ID = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Phone = e.Phone,
                    DateOfBirth = e.DateOfBirth,
                    Gender = (bool)e.Gender ? "Nam" : "Nữ",
                    Address = e.Address,
                    Salary = e.Salary,
                    Department = e.Department.Name,
                    Manager = e.Manager.FirstName + " " + e.Manager.LastName,
                    Position = e.Position.Name,
                }).ToList();
                if (employee.Any())
                {
                    dgEmployee.ItemsSource = employee.ToList();
                    cbAllEmployee.IsChecked = false;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên nào chưa bị xóa!", "Thông báo");
                    return;
                }
            }
            if(idStatus == 1)
            {
                dgEmployee.ItemsSource = string.Empty;
                var employee = database.Employees.Where(e => e.IsDelete == true).Select(e => new
                {
                    ID = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Phone = e.Phone,
                    DateOfBirth = e.DateOfBirth,
                    Gender = (bool)e.Gender ? "Nam" : "Nữ",
                    Address = e.Address,
                    Salary = e.Salary,
                    Department = e.Department.Name,
                    Manager = e.Manager.FirstName + " " + e.Manager.LastName,
                    Position = e.Position.Name,
                }).ToList();
                if (employee.Any())
                {
                    dgEmployee.ItemsSource = employee.ToList();
                    cbAllEmployee.IsChecked = false;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên nào đã ngừng hoạt động!", "Thông báo");
                    return;
                }
            }
            if(idStatus == 2)//Nam
            {
                dgEmployee.ItemsSource = string.Empty;
                var employee = database.Employees.Where(e => e.Gender == true).Select(e => new
                {
                    ID = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Phone = e.Phone,
                    DateOfBirth = e.DateOfBirth,
                    Gender = (bool)e.Gender ? "Nam" : "Nữ",
                    Address = e.Address,
                    Salary = e.Salary,
                    Department = e.Department.Name,
                    Manager = e.Manager.FirstName + " " + e.Manager.LastName,
                    Position = e.Position.Name,
                }).ToList();
                if (employee.Any())
                {
                    dgEmployee.ItemsSource = employee.ToList();
                    cbAllEmployee.IsChecked = false;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên nào đã bị xóa!", "Thông báo");
                    return;
                }
            }

            if(idStatus == 3)//Nữ
            {
                dgEmployee.ItemsSource = string.Empty;
                var employee = database.Employees.Where(e => e.Gender == false).Select(e => new
                {
                    ID = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Phone = e.Phone,
                    DateOfBirth = e.DateOfBirth,
                    Gender = (bool)e.Gender ? "Nam" : "Nữ",
                    Address = e.Address,
                    Salary = e.Salary,
                    Department = e.Department.Name,
                    Manager = e.Manager.FirstName + " " + e.Manager.LastName,
                    Position = e.Position.Name,
                }).ToList();
                if (employee.Any())
                {
                    dgEmployee.ItemsSource = employee.ToList();
                    cbAllEmployee.IsChecked = false;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên nào đã bị xóa!", "Thông báo");
                    return;
                }
            }

            if(idStatus == 4)//trưởng phòng
            {
                FilterPosition(1);
            }
            if(idStatus == 5)//nhân viên chính
            {
                FilterPosition(2);
            }
            if(idStatus == 6) //nhân viên partime
            {
                FilterPosition(3);
            }
            if(idStatus == 7)
            {
                FilterPosition(4);
            }
        }
        private void FilterPosition(int id)
        {
            dgEmployee.ItemsSource = string.Empty;
            var employee = database.Employees.Where(e => e.PositionId == id).Select(e => new
            {
                ID = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                DateOfBirth = e.DateOfBirth,
                Gender = (bool)e.Gender ? "Nam" : "Nữ",
                Address = e.Address,
                Salary = e.Salary,
                Department = e.Department.Name,
                Manager = e.Manager.FirstName + " " + e.Manager.LastName,
                Position = e.Position.Name,
            }).ToList();
            if (employee.Any())
            {
                dgEmployee.ItemsSource = employee.ToList();
                cbAllEmployee.IsChecked = false;
            }
            else
            {
                MessageBox.Show("Không tìm thấy nhân viên nào đã bị xóa!", "Thông báo");
                return;
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Thông báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Login login = new Login();
                this.Hide();
                login.ShowDialog();
                this.Close();
            }
        }

        private void GoHome_Click(object sender, RoutedEventArgs e)
        {
            Home home = new Home(em);
            this.Hide();
            home.ShowDialog();
            this.Close();
        }

        private void cbAllEmployee_Checked(object sender, RoutedEventArgs e)
        {
            LoadDataEmployee();
        }

        private void DetailEmployee_Click(object sender, RoutedEventArgs e)
        {
            if(!int.TryParse(txtEmployeeID.Text, out int idEmployee))
            {
                MessageBox.Show("Vui lòng lựa chọn nhân viên muốn xem chi tiết!", "Thông báo");
                return;
            }
            var employee = database.Employees.Where(e => e.Id == idEmployee).SingleOrDefault();
            if(employee != null)
            {
                Userprofile userprofile = new Userprofile(em, employee);
                this.Hide();
                userprofile.ShowDialog();
                this.Close();
            }
        }

        private void AddNewEmployee_Click(object sender, RoutedEventArgs e)
        {
            Userprofile userprofile = new Userprofile(em, em);
            this.Hide();
            userprofile.ShowDialog();
            this.Close();
        }

        private void cbbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idFilter = cbbFilter.SelectedIndex;
            string dataSearch = txtDatasearch.Text;
            if(dataSearch == null)
            {
                MessageBox.Show("Vui lòng nhập tìm kiếm!", "Thông báo");
                return;
            }
            if(idFilter == 0) //theo tên
            {
                dgEmployee.ItemsSource = string.Empty;
                var employee = database.Employees.Where(e => (e.FirstName + " " + e.LastName).ToLower().Contains(dataSearch.ToLower())).Select(e => new
                {
                    ID = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Phone = e.Phone,
                    DateOfBirth = e.DateOfBirth,
                    Gender = (bool)e.Gender ? "Nam" : "Nữ",
                    Address = e.Address,
                    Salary = e.Salary,
                    Department = e.Department.Name,
                    Manager = e.Manager.FirstName + " " + e.Manager.LastName,
                    Position = e.Position.Name,
                }).ToList();
                if (employee.Any())
                {
                    dgEmployee.ItemsSource = employee.ToList();
                    cbAllEmployee.IsChecked = false;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên nào có tên " + dataSearch, "Thông báo");
                    return;
                }
            }
            if (idFilter == 1)//theo email
            {
                dgEmployee.ItemsSource = string.Empty;
                var employee = database.Employees.Where(e => e.Email.ToLower().Contains(dataSearch.ToLower())).Select(e => new
                {
                    ID = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Phone = e.Phone,
                    DateOfBirth = e.DateOfBirth,
                    Gender = (bool)e.Gender ? "Nam" : "Nữ",
                    Address = e.Address,
                    Salary = e.Salary,
                    Department = e.Department.Name,
                    Manager = e.Manager.FirstName + " " + e.Manager.LastName,
                    Position = e.Position.Name,
                }).ToList();
                if (employee.Any())
                {
                    dgEmployee.ItemsSource = employee.ToList();
                    cbAllEmployee.IsChecked = false;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên có email " + dataSearch, "Thông báo");
                    return;
                }
            }
            if(idFilter == 2) //theo số điện thoại
            {
                dgEmployee.ItemsSource = string.Empty;
                var employee = database.Employees.Where(e => e.Phone.Equals(dataSearch)).Select(e => new
                {
                    ID = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Phone = e.Phone,
                    DateOfBirth = e.DateOfBirth,
                    Gender = (bool)e.Gender ? "Nam" : "Nữ",
                    Address = e.Address,
                    Salary = e.Salary,
                    Department = e.Department.Name,
                    Manager = e.Manager.FirstName + " " + e.Manager.LastName,
                    Position = e.Position.Name,
                }).ToList();
                if (employee.Any())
                {
                    dgEmployee.ItemsSource = employee.ToList();
                    cbAllEmployee.IsChecked = false;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên nào có số điện thoại " + dataSearch, "Thông báo");
                    return;
                }
            }
        }


        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            Home home = new Home(em);
            this.Hide();
            home.ShowDialog();
            this.Close();
        }

        private void txtDatasearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            cbbFilter.SelectedIndex = -1;
        }
    }
}
