using ManageEmployeeSystem.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Userprofile.xaml
    /// </summary>
    public partial class Userprofile : Window
    {
        public Userprofile()
        {
            InitializeComponent();
        }
        Employee em;
        Employee employeeAdmin;
        ProjectPrn212Context database = new ProjectPrn212Context();
        public Userprofile(Employee employee)
        {
            InitializeComponent();
            em = employee;
            this.Title = "Xin chào, " + employee.FirstName + " " + employee.LastName;
            LoadData();
            LoadDataAccount();
        }

        public Userprofile(Employee employeeAdmin, Employee employee)
        {
            InitializeComponent();
            this.employeeAdmin = employeeAdmin;
            this.em = employee;
            this.Title = "Xin chào, " + employeeAdmin.FirstName + " " + employeeAdmin.LastName;
            LoadData();
            //LoadDepartmentList();
            LoadDataAccount();
            if (employeeAdmin != null)
            {
                btnAddEmployee.Visibility = Visibility.Visible;
                btnDeleteEmployee.Visibility = Visibility.Visible;
                txtSalary.IsReadOnly = false;
            }
        }

        public string FormatNumber(double number)
        {
            return number.ToString("#,###,###");
        }

        private void LoadData()
        {
            txtID.Text = em.Id.ToString();
            txtFirstname.Text = em.FirstName.ToString();
            txtLastname.Text = em.LastName.ToString();
            txtEmail.Text = em.Email.ToString();
            txtBirthdate.Text = em.DateOfBirth.ToString();
            txtAddress.Text = em.Address.ToString();
            txtPhone.Text = em.Phone.ToString();

            if (employeeAdmin != null)
            {
                var departments = database.Departments.ToList();
                cbbDepartment.ItemsSource = departments;
                cbbDepartment.DisplayMemberPath = "Name";
                cbbDepartment.SelectedValuePath = "ID";

                var positions = database.Positions.ToList();
                cbbPosition.ItemsSource = positions;
                cbbPosition.DisplayMemberPath = "Name";
                cbbPosition.SelectedValuePath = "ID";

                var managers = database.Employees.Where(m => m.RoleId == 3).Select(m => new
                {
                    ID = m.Id,
                    Fullname = m.FirstName + " " + m.LastName
                }).ToList();
                cbbManager.ItemsSource = managers.ToList();
                cbbManager.DisplayMemberPath = "Fullname";
                cbbManager.SelectedValuePath = "ID";
            }
            else
            {
                if (em != null)
                {
                    var departments = database.Departments.Where(d => d.Id == em.DepartmentId).ToList();
                    cbbDepartment.ItemsSource = departments;
                    cbbDepartment.DisplayMemberPath = "Name";
                    cbbDepartment.SelectedValuePath = "ID";

                    var positions = database.Positions.Where(p => p.Id == em.PositionId).ToList();
                    cbbPosition.ItemsSource = positions;
                    cbbPosition.DisplayMemberPath = "Name";
                    cbbPosition.SelectedValuePath = "ID";

                    var managers = database.Employees.Where(m => m.Id == em.ManagerId).Select(m => new
                    {
                        ID = m.Id,
                        Fullname = m.FirstName + " " + m.LastName
                    }).ToList();
                    cbbManager.ItemsSource = managers.ToList();
                    cbbManager.DisplayMemberPath = "Fullname";
                    cbbManager.SelectedValuePath = "ID";
                }

            }

            var selectedDepartment = database.Departments.FirstOrDefault(d => d.Id == em.DepartmentId);
            if (selectedDepartment != null)
            {
                cbbDepartment.SelectedItem = selectedDepartment;
            }
            var selectedPosition = database.Positions.FirstOrDefault(p => p.Id == em.PositionId);
            if (selectedPosition != null)
            {
                cbbPosition.SelectedItem = selectedPosition;
            }
            var selectedManager = database.Employees.Select(m => new
            {
                ID = m.Id,
                Fullname = m.FirstName + " " + m.LastName,
            }).FirstOrDefault(m => m.ID == em.ManagerId);
            if (selectedManager != null)
            {
                cbbManager.SelectedItem = selectedManager;
            }

            txtCreatedAt.Text = em.CreatedAt.ToString();
            string salary = FormatNumber((double)em.Salary);
            txtSalary.Text = salary;//em.Salary.ToString();
            var position = database.Positions.FirstOrDefault(p => p.Id == em.PositionId);


            var manager = database.Employees.FirstOrDefault(m => m.Id == em.Id);


            bool gender = em.Gender.HasValue;
            if (gender)
            {
                radioBoy.IsChecked = gender;
            }
            else if (!gender)
            {
                radioGirl.IsChecked = !gender;
            }

            bool status = em.IsDelete.Value;
            if (status == false)
            {
                txtStatus.Text = "Đang hoạt động";
            }
            else if (status = false)
            {
                txtStatus.Text = "Đã xóa";
            }
        }

        private void LoadDepartmentList()
        {
            var department = database.Departments.Select(d => new
            {
                ID = d.Id,
                Name = d.Name,
            }).ToList();
            cbbDepartment.ItemsSource = department;
            cbbDepartment.DisplayMemberPath = "Name";
            cbbDepartment.SelectedValuePath = "ID";
        }

        private void LoadDataAccount()
        {
            var account = database.Authentications.SingleOrDefault(a => a.EmployeeId == em.Id);
            if (account != null)
            {
                txtUsername.Text = account.Username.ToString();
            }
        }

        private void GoHome_Click(object sender, RoutedEventArgs e)
        {
            if (employeeAdmin != null)
            {
                Home home = new Home(employeeAdmin);
                this.Hide();
                home.ShowDialog();
                this.Close();
            }
            else
            {
                if (em != null)
                {
                    Home home = new Home(em);
                    this.Hide();
                    home.ShowDialog();
                    this.Close();
                }
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

        private void UpdateProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string firstName = txtFirstname.Text;
                string lastName = txtLastname.Text;
                string email = txtEmail.Text;
                string phone = txtPhone.Text;
                string address = txtAddress.Text;
                string birthdate = txtBirthdate.Text;
                DateOnly dateofbirth = DateOnly.Parse(birthdate);
                bool gender = true;
                if (radioBoy.IsChecked == true)
                {
                    gender = true;
                }
                else if (radioGirl.IsChecked == true)
                {
                    gender = false;
                }

                //double salary = 0;
                if (employeeAdmin != null)
                {
                    if (!double.TryParse(txtSalary.Text, out double salary))
                    {
                        MessageBox.Show("Vui lòng nhập lương của nhân viên!", "Thông báo");
                        return;
                    }
                    var selectedDepartment = cbbDepartment.SelectedItem as Department;
                    int idDepartment = selectedDepartment.Id;

                    var selectedPosition = cbbPosition.SelectedItem as Position;
                    int idPosition = selectedPosition.Id;

                    if (idPosition == 1 && em.PositionId != 1 && em.ManagerId != em.Id) //nếu nó không phải là trưởng phòng và quản lí cũng khác nó
                    {
                        if (MessageBox.Show("Giao cho nhân viên " + em.FirstName + " " + em.LastName + " thành trưởng phòng?", "Thông báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            //cho thằng trưởng phòng hiện tại thành nhân viên và chịu sự trách nhiệm quả lí của thằng được xếp làm trường phòng
                            var changeManager = database.Employees.Where(e => e.Id == em.ManagerId).SingleOrDefault();
                            if (changeManager != null)
                            {
                                changeManager.PositionId = null;
                                changeManager.ManagerId = em.Id;
                                changeManager.RoleId = 2;
                                database.Employees.Update(changeManager);
                                database.SaveChanges();
                            }
                        }
                        var employeeUpdate = database.Employees.FirstOrDefault(e => e.Id == em.Id);
                        if (employeeUpdate != null)
                        {
                            employeeUpdate.FirstName = firstName;
                            employeeUpdate.LastName = lastName;
                            employeeUpdate.Email = email;
                            employeeUpdate.Phone = phone;
                            employeeUpdate.Address = address;
                            employeeUpdate.DateOfBirth = dateofbirth;
                            employeeUpdate.Gender = gender;
                            employeeUpdate.PositionId = idPosition;
                            employeeUpdate.DepartmentId = idDepartment;
                            employeeUpdate.ManagerId = em.Id;
                            employeeUpdate.RoleId = 3;

                            database.Employees.Update(employeeUpdate);
                            if (database.SaveChanges() > 0)
                            {
                                MessageBox.Show("Thông tin cá nhân đã được cập nhật!", "Thông báo");
                                LoadData();
                            }
                            else
                            {
                                MessageBox.Show("Không có thay đổi nào được lưu!", "Thông báo");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy nhân viên!", "Thông báo");
                        }
                    }
                    else
                    {
                        var selectedManager = cbbManager.SelectedItem as dynamic;
                        int idManager = selectedManager.ID;

                        var employeeUpdate = database.Employees.FirstOrDefault(e => e.Id == em.Id);
                        if (employeeUpdate != null)
                        {
                            employeeUpdate.FirstName = firstName;
                            employeeUpdate.LastName = lastName;
                            employeeUpdate.Email = email;
                            employeeUpdate.Phone = phone;
                            employeeUpdate.Address = address;
                            employeeUpdate.DateOfBirth = dateofbirth;
                            employeeUpdate.Gender = gender;
                            employeeUpdate.PositionId = idPosition;
                            employeeUpdate.DepartmentId = idDepartment;
                            employeeUpdate.ManagerId = idManager;

                            database.Employees.Update(employeeUpdate);
                            if (database.SaveChanges() > 0)
                            {
                                MessageBox.Show("Thông tin cá nhân đã được cập nhật!", "Thông báo");
                                LoadData();
                            }
                            else
                            {
                                MessageBox.Show("Không có thay đổi nào được lưu!", "Thông báo");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy nhân viên!", "Thông báo");
                        }
                    }


                }
                else
                {
                    var employeeUpdate = database.Employees.FirstOrDefault(e => e.Id == em.Id);
                    if (employeeUpdate != null)
                    {
                        employeeUpdate.FirstName = firstName;
                        employeeUpdate.LastName = lastName;
                        employeeUpdate.Email = email;
                        employeeUpdate.Phone = phone;
                        employeeUpdate.Address = address;
                        employeeUpdate.DateOfBirth = dateofbirth;
                        employeeUpdate.Gender = gender;

                        database.Employees.Update(employeeUpdate);
                        if (database.SaveChanges() > 0)
                        {
                            MessageBox.Show("Thông tin cá nhân đã được cập nhật!", "Thông báo");
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Không có thay đổi nào được lưu!", "Thông báo");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy nhân viên!", "Thông báo");
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message, "Thông báo");
            }
        }
        private static bool IsPasswordValid(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(password);
        }

        private void Changepassword_Click(object sender, RoutedEventArgs e)
        {
            string password = txtOldpassword.Password;
            string newpassword = txtNewpassword.Password;
            string repassword = txtRepassword.Password;
            if (password.IsNullOrEmpty()) { MessageBox.Show("Vui lòng điền mật khẩu!", "Thông báo"); return; }
            if (newpassword.IsNullOrEmpty()) { MessageBox.Show("Vui lòng điền mật khẩu mới!", "Thông báo"); return; }
            if (repassword.IsNullOrEmpty()) { MessageBox.Show("Vui lòng điền xác nhận mật khẩu!", "Thông báo"); return; }

            var authentication = database.Authentications.SingleOrDefault(a => a.EmployeeId == em.Id);
            if (password.Equals(authentication.PassWord))
            {
                if (!IsPasswordValid(newpassword))
                {
                    MessageBox.Show("Mật khẩu bao gồm ít nhất 1 kí tự hoa, 1 kí tự thường, 1 kí tự số!", "Thông báo");
                    return;
                }
                else
                {
                    if (!newpassword.Equals(repassword))
                    {
                        MessageBox.Show("Vui lòng điền 'Mật khẩu' giống với 'Xác nhận mật khẩu'!", "Thông báo");
                        return;
                    }
                    else
                    {
                        if (authentication != null)
                        {
                            if (MessageBox.Show("Bạn chắc chắn muốn đổi mật khẩu?", "Thông bảo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                authentication.PassWord = newpassword;
                                if (database.SaveChanges() > 0)
                                {
                                    MessageBox.Show("Cập nhật mật khẩu thành công!", "Thông báo");
                                    ClearChangePassWord();
                                }
                                else
                                {
                                    MessageBox.Show("Cập nhật mật khẩu không thành công!", "Thông báo");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Lỗi không tìm thấy tài khoản cho người dùng này, vui lòng thử lại sau!", "Thông báo");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Mật khẩu cũ không chính xác!", "Thông báo");
                return;
            }
        }
        private void ClearChangePassWord()
        {
            txtOldpassword.Clear();
            txtNewpassword.Clear();
            txtRepassword.Clear();
        }

        private void cbbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int departmentId = 0;
            var department = cbbDepartment.SelectedItem as Department;
            if (department != null)
            {
                departmentId = department.Id;
                var employeee = database.Employees.Where(e => e.DepartmentId == departmentId && e.RoleId == 3).Select(e => new
                {
                    ID = e.Id,
                    Fullname = e.FirstName + " " + e.LastName,
                }).FirstOrDefault();
                if (employeee != null)
                {
                    cbbManager.SelectedItem = employeee;
                }
            }
        }

        private void cbbManager_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int managerId = 0;
            var manager = cbbManager.SelectedItem as dynamic;
            if (manager != null)
            {
                managerId = manager.ID;
                var employee = database.Employees.FirstOrDefault(emp => emp.Id == managerId);
                if (employee != null)
                {
                    var department = database.Departments.FirstOrDefault(d => d.Id == employee.DepartmentId);
                    if (department != null)
                    {
                        cbbDepartment.SelectedItem = department;
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy phòng ban!", "Thông báo");
                    }
                }
                else
                {
                    MessageBox.Show("Lỗi tìm quản lý!", "Thông báo");
                }
            }
        }

    }
}
