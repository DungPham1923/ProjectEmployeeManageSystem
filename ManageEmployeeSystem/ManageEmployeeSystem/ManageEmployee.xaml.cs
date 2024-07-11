using ManageEmployeeSystem.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
                Gender = (bool)e.Gender ? "Nam" : "Nữ",
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
            if (idStatus == 1)
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
            if (idStatus == 2)//Nam
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

            if (idStatus == 3)//Nữ
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

            if (idStatus == 4)//trưởng phòng
            {
                FilterPosition(1);
            }
            if (idStatus == 5)//nhân viên chính
            {
                FilterPosition(2);
            }
            if (idStatus == 6) //nhân viên partime
            {
                FilterPosition(3);
            }
            if (idStatus == 7)
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
            if (!int.TryParse(txtEmployeeID.Text, out int idEmployee))
            {
                MessageBox.Show("Vui lòng lựa chọn nhân viên muốn xem chi tiết!", "Thông báo");
                return;
            }
            var employee = database.Employees.Where(e => e.Id == idEmployee).SingleOrDefault();
            if (employee != null)
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
            if (dataSearch == null)
            {
                MessageBox.Show("Vui lòng nhập tìm kiếm!", "Thông báo");
                return;
            }
            if (idFilter == 0) //theo tên
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
            if (idFilter == 2) //theo số điện thoại
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


        private List<Employee> GetEmployeesFromDataGrid()
        {
            var employees = new List<Employee>();

            // Kiểm tra xem dgEmployeeJobs đã được gán ItemsSource chưa
            if (dgEmployee.ItemsSource != null)
            {
                foreach (var item in dgEmployee.ItemsSource)
                {
                    if (item is Employee viewModel) // Thay YourEmployeeViewModel bằng tên viewmodel của bạn
                    {
                        // Tạo đối tượng Employee từ ViewModel hoặc từ item
                        var employee = new Employee
                        {
                            Id = viewModel.Id,
                            FirstName = viewModel.FirstName,
                            LastName = viewModel.LastName,
                            Department = viewModel.Department,
                            Position = viewModel.Position,
                            Email = viewModel.Email,
                            // Thêm các thuộc tính khác tùy vào các trường dữ liệu bạn có
                        };

                        employees.Add(employee);
                    }
                }
            }

            return employees;
        }
        private void SaveEmployee_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog saveFileDialog = new Microsoft.Win32.OpenFileDialog();
            saveFileDialog.Filter = "Excel Files (*.xls;*.xlsx)|*.xls;*.xlsx|All files (*.*)|*.*";
            saveFileDialog.Title = "Tạo mới tệp Excel";
            bool? result = saveFileDialog.ShowDialog();
            var employee = database.Employees.ToList();
            if (result == true)
            {
                foreach (var item in employee)
                {

                }
                try
                {
                    string filePath = saveFileDialog.FileName;
                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Employees");
                        worksheet.Cells[1, 1].Value = "Employee ID";
                        worksheet.Cells[1, 2].Value = "First Name";
                        worksheet.Cells[1, 3].Value = "Last Name";
                        worksheet.Cells[1, 4].Value = "Email";
                        worksheet.Cells[1, 5].Value = "Phone";
                        worksheet.Cells[1, 6].Value = "Salary";
                        worksheet.Cells[1, 7].Value = "RoleID";
                        worksheet.Cells[1, 8].Value = "DepartmentID";
                        worksheet.Cells[1, 9].Value = "ManagerID";
                        worksheet.Cells[1, 10].Value = "PositionID";
                        worksheet.Cells[1, 11].Value = "DateOfBirth";
                        worksheet.Cells[1, 12].Value = "Gender";
                        worksheet.Cells[1, 13].Value = "Address";
                        worksheet.Cells[1, 14].Value = "CreatedAt";
                        worksheet.Cells[1, 15].Value = "UpdatedAt";
                        worksheet.Cells[1, 16].Value = "DeletedAt";
                        worksheet.Cells[1, 17].Value = "IsDelete";
                        worksheet.Cells[1, 18].Value = "DeleteById";
                        // Bạn có thể thêm các cột khác tùy vào yêu cầu của bạn

                        // Lấy danh sách nhân viên từ cơ sở dữ liệu
                        var employees = database.Employees.ToList();

                        // Dữ liệu các nhân viên
                        int rowIndex = 2;
                        foreach (var emp in employees)
                        {
                            worksheet.Cells[rowIndex, 1].Value = emp.Id;
                            worksheet.Cells[rowIndex, 2].Value = emp.FirstName;
                            worksheet.Cells[rowIndex, 3].Value = emp.LastName;
                            worksheet.Cells[rowIndex, 4].Value = emp.Email;
                            worksheet.Cells[rowIndex, 5].Value = emp.Phone;
                            worksheet.Cells[rowIndex, 6].Value = emp.Salary;
                            worksheet.Cells[rowIndex, 7].Value = emp.RoleId;
                            worksheet.Cells[rowIndex, 8].Value = emp.DepartmentId;
                            worksheet.Cells[rowIndex, 9].Value = emp.ManagerId;
                            worksheet.Cells[rowIndex, 10].Value = emp.PositionId;
                            worksheet.Cells[rowIndex, 11].Value = emp.DateOfBirth;
                            worksheet.Cells[rowIndex, 12].Value = emp.Gender;
                            worksheet.Cells[rowIndex, 13].Value = emp.Address;
                            worksheet.Cells[rowIndex, 14].Value = emp.CreatedAt;
                            worksheet.Cells[rowIndex, 15].Value = emp.UpdatedAt;
                            worksheet.Cells[rowIndex, 16].Value = emp.DeletedAt;
                            worksheet.Cells[rowIndex, 17].Value = emp.IsDelete;
                            worksheet.Cells[rowIndex, 18].Value = emp.DeletedById;
                            rowIndex++;
                        }

                        // Lưu tệp Excel vào đường dẫn đã chọn
                        FileInfo excelFile = new FileInfo(filePath);
                        package.SaveAs(excelFile);

                        MessageBox.Show("Đã lưu danh sách nhân viên vào tệp Excel thành công.", "Thông báo", MessageBoxButton.OK);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Đã xảy ra lỗi khi lưu tệp Excel: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
