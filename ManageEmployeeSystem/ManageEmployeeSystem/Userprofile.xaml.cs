using ManageEmployeeSystem.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
            if (em.RoleId == 1)
            {
                this.Title = "Xin chào admin, " + employee.FirstName + " " + employee.LastName;
            }
            else
            {
                this.Title = "Xin chào, " + employee.FirstName + " " + employee.LastName;
            }
            radioActive.IsEnabled = false;
            radioInactive.IsEnabled = false;
            LoadData(em.Id);
            LoadDataAccount();
        }
        public Userprofile(Employee employeeAdmin, Employee employee)
        {
            InitializeComponent();
            this.employeeAdmin = employeeAdmin;
            this.em = employee;
            LoadData(em.Id);
            //LoadDepartmentList();
            LoadDataAccount();
            if (employeeAdmin != null)
            {
                this.Title = "Xin chào admin, " + employeeAdmin.FirstName + " " + employeeAdmin.LastName;
                //btnAddEmployee.Visibility = Visibility.Visible;
                btnDeleteEmployee.Visibility = Visibility.Visible;
                txtSalary.IsReadOnly = false;
            }
            else
            {
                this.Title = "Xin chào, " + employee.FirstName + " " + employee.LastName;
            }
            if (employeeAdmin == em)
            {
                btnAddEmployee.Visibility = Visibility.Visible;
                btnDeleteEmployee.Visibility = Visibility.Collapsed;
                btnUpdateEmployee.Visibility = Visibility.Collapsed;

                ClearForm();
                lbInfoLogin.Visibility = Visibility.Collapsed;
                lbUsername.Visibility = Visibility.Collapsed;
                lbOldpass.Visibility = Visibility.Collapsed;
                lbNewpass.Visibility = Visibility.Collapsed;
                lbRepass.Visibility = Visibility.Collapsed;
                txtUsername.Visibility = Visibility.Collapsed;
                txtOldpassword.Visibility = Visibility.Collapsed;
                txtNewpassword.Visibility = Visibility.Collapsed;
                txtRepassword.Visibility = Visibility.Collapsed;
                btnRepass.Visibility = Visibility.Collapsed;
            }
        }
        public void ClearForm()
        {
            txtID.Clear();
            txtFirstname.Clear();
            txtLastname.Clear();
            txtAddress.Clear();
            txtBirthdate.SelectedDate = null;
            txtEmail.Clear();
            //cbbDepartment.Items.Clear();
            //cbbManager.Items.Clear();
            //cbbPosition.Items.Clear();
            txtSalary.Clear();
            //radioActice.IsChecked = true;
            txtUsername.Clear();
            txtCreatedAt.Clear();
            txtPhone.Clear();
        }
        public string FormatNumber(double number)
        {
            return number.ToString("#,###,###");
        }
        private void LoadDataSub()
        {
            var employee = database.Employees.Where(e => e.Id == em.Id).SingleOrDefault();
            if (employeeAdmin != null) //load hết
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
            else //load objects current
            {
                if (em != null)
                {
                    var departments = database.Departments.Where(d => d.Id == employee.DepartmentId).ToList();
                    cbbDepartment.ItemsSource = departments;
                    cbbDepartment.DisplayMemberPath = "Name";
                    cbbDepartment.SelectedValuePath = "ID";

                    var positions = database.Positions.Where(p => p.Id == employee.PositionId).ToList();
                    cbbPosition.ItemsSource = positions;
                    cbbPosition.DisplayMemberPath = "Name";
                    cbbPosition.SelectedValuePath = "ID";

                    var managers = database.Employees.Where(m => m.Id == employee.ManagerId).Select(m => new
                    {
                        ID = m.Id,
                        Fullname = m.FirstName + " " + m.LastName
                    }).ToList();
                    cbbManager.ItemsSource = managers.ToList();
                    cbbManager.DisplayMemberPath = "Fullname";
                    cbbManager.SelectedValuePath = "ID";
                }
                //if (em != null && em.RoleId == 1)
                //{
                //    var departments = database.Departments.ToList();
                //    cbbDepartment.ItemsSource = departments;
                //    cbbDepartment.DisplayMemberPath = "Name";
                //    cbbDepartment.SelectedValuePath = "ID";

                //    var positions = database.Positions.ToList();
                //    cbbPosition.ItemsSource = positions;
                //    cbbPosition.DisplayMemberPath = "Name";
                //    cbbPosition.SelectedValuePath = "ID";

                //    var managers = database.Employees.Where(m => m.RoleId == 3).Select(m => new
                //    {
                //        ID = m.Id,
                //        Fullname = m.FirstName + " " + m.LastName
                //    }).ToList();
                //    cbbManager.ItemsSource = managers.ToList();
                //    cbbManager.DisplayMemberPath = "Fullname";
                //    cbbManager.SelectedValuePath = "ID";
                //}

            }
        }
        private void LoadData(int Id)
        {
            var employee = database.Employees.Where(e => e.Id == Id).SingleOrDefault();
            if (employee != null)
            {
                txtID.Text = employee.Id.ToString();
                if (employee.FirstName != null)
                {
                    txtFirstname.Text = employee.FirstName.ToString();
                }
                if (employee.LastName != null)
                {
                    txtLastname.Text = employee.LastName.ToString();
                }
                if (employee.Email != null)
                {
                    txtEmail.Text = employee.Email.ToString();
                }
                if (employee.Phone != null)
                {
                    txtPhone.Text = employee.Phone.ToString();
                }
                if (employee.DateOfBirth != null)
                {
                    txtBirthdate.Text = employee.DateOfBirth.ToString();
                }
                if (employee.Address != null)
                {
                    txtAddress.Text = employee.Address.ToString();
                }

                LoadDataSub();
                if (employee.DepartmentId != null)
                {
                    var selectedDepartment = database.Departments.FirstOrDefault(d => d.Id == employee.DepartmentId);
                    if (selectedDepartment != null)
                    {
                        cbbDepartment.SelectedItem = selectedDepartment;
                    }
                }
                if (employee.PositionId != null)
                {
                    var selectedPosition = database.Positions.FirstOrDefault(p => p.Id == employee.PositionId);
                    if (selectedPosition != null)
                    {
                        cbbPosition.SelectedItem = selectedPosition;
                    }
                }
                if (employee.ManagerId != null)
                {
                    var selectedManager = database.Employees.Select(m => new
                    {
                        ID = m.Id,
                        Fullname = m.FirstName + " " + m.LastName,
                    }).FirstOrDefault(m => m.ID == employee.ManagerId);
                    if (selectedManager != null)
                    {
                        cbbManager.SelectedItem = selectedManager;
                    }
                }

                if (employee.CreatedAt != null)
                {
                    txtCreatedAt.Text = employee.CreatedAt.ToString();
                }
                if (employee.Salary != null)
                {
                    string salary = FormatNumber((double)employee.Salary);
                    txtSalary.Text = salary;//em.Salary.ToString();
                }

                //var position = database.Positions.FirstOrDefault(p => p.Id == employee.PositionId);


                //var manager = database.Employees.FirstOrDefault(m => m.Id == employee.Id);

                
                bool gender = employee.Gender.HasValue && employee.Gender.Value;
                if (gender == false)
                {
                    radioGirl.IsChecked = true;
                }
                else if (gender == true)
                {
                    radioBoy.IsChecked = true;
                }

                bool status = employee.IsDelete.HasValue && employee.IsDelete.Value;

                if (status == false)
                {
                    radioActive.IsChecked = true;
                }
                else
                {
                    radioInactive.IsChecked = true;
                }
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

                bool active = false;
                if (radioActive.IsChecked == true)
                {
                    active = false;
                }
                else if (radioInactive.IsChecked == true)
                {
                    active = true;
                }

                //MessageBox.Show("Gender "+ gender + " Active:" + active);

                //double salary = 0;
                if (employeeAdmin != null)
                {
                    if (!double.TryParse(txtSalary.Text, out double salary))
                    {
                        MessageBox.Show("Vui lòng nhập lương của nhân viên!", "Thông báo");
                        return;
                    }
                    var selectedDepartment = cbbDepartment.SelectedItem as Department;
                    if (selectedDepartment == null)
                    {
                        MessageBox.Show("Vui lòng lựa chọn phòng ban!", "Thông báo");
                        return;
                    }
                    int idDepartment = selectedDepartment.Id;

                    var selectedPosition = cbbPosition.SelectedItem as Position;
                    if(selectedPosition == null)
                    {
                        MessageBox.Show("Vui lòng lựa chọn vị trí công việc!", "Thông báo");
                        return ;
                    }
                    int idPosition = selectedPosition.Id;

                    if (idPosition == 1 && em.PositionId != 1 && em.ManagerId != em.Id) //nếu nó không phải là trưởng phòng và quản lí cũng khác nó
                    {
                        if (MessageBox.Show("Giao cho nhân viên " + em.FirstName + " " + em.LastName + " thành trưởng phòng?", "Thông báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            //cho thằng trưởng phòng hiện tại thành nhân viên và chịu sự trách nhiệm quả lí của thằng được xếp làm trường phòng
                            var changeManager = database.Employees.Where(e => e.Id == em.ManagerId).SingleOrDefault();
                            List<Employee> changeManagerEmployee = new List<Employee>();
                            if (changeManager != null) //nếu nó có quản lí
                            {
                                changeManagerEmployee = database.Employees.Where(e => e.ManagerId == changeManager.Id).ToList();
                            }
                            else
                            {
                                changeManagerEmployee = database.Employees.Where(e => e.DepartmentId == idDepartment).ToList();
                            }
                            
                            if (changeManager != null)
                            {
                                changeManager.PositionId = null;
                                changeManager.ManagerId = em.Id;
                                changeManager.RoleId = 2;
                                database.Employees.Update(changeManager);
                                database.SaveChanges();
                            }
                            if (changeManagerEmployee != null)
                            {
                                changeManagerEmployee.ForEach(e => { e.ManagerId = em.Id; });
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
                            employeeUpdate.Salary = (decimal)salary;
                            employeeUpdate.PositionId = idPosition;
                            employeeUpdate.DepartmentId = idDepartment;
                            employeeUpdate.ManagerId = em.Id;
                            employeeUpdate.RoleId = 3;
                            employeeUpdate.IsDelete = active;
                            employeeUpdate.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);
                            database.SaveChanges();
                            var authenticaton = database.Authentications.SingleOrDefault(a => a.EmployeeId == em.Id);
                            authenticaton.IsDelete = active;
                            database.Employees.Update(employeeUpdate);
                            if (database.SaveChanges() > 0)
                            {
                                MessageBox.Show("Thông tin cá nhân đã được cập nhật!", "Thông báo");
                                LoadData(em.Id);
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
                            employeeUpdate.Salary = (decimal)salary;
                            employeeUpdate.Gender = gender;
                            employeeUpdate.PositionId = idPosition;
                            employeeUpdate.DepartmentId = idDepartment;
                            employeeUpdate.ManagerId = idManager;
                            employeeUpdate.IsDelete = active;
                            employeeUpdate.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);
                            database.SaveChanges();
                            var authenticaton = database.Authentications.SingleOrDefault(a => a.EmployeeId == em.Id);
                            authenticaton.IsDelete = active;
                            database.Employees.Update(employeeUpdate);
                            if (database.SaveChanges() > 0)
                            {
                                MessageBox.Show("Thông tin cá nhân đã được cập nhật!", "Thông báo");
                                LoadData(em.Id);
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
                        employeeUpdate.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);

                        database.Employees.Update(employeeUpdate);
                        if (database.SaveChanges() > 0)
                        {
                            MessageBox.Show("Thông tin cá nhân đã được cập nhật!", "Thông báo");
                            LoadData(em.Id);
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
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }
        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
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
                bool active = true;
                if (radioActive.IsChecked == true)
                {
                    active = false;
                }
                else if (radioInactive.IsChecked == true)
                {
                    active = true;
                }

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
                        var changeManagerEmployee = database.Employees.Where(e => e.ManagerId == changeManager.Id).ToList();
                        if (changeManager != null)
                        {
                            changeManager.PositionId = null;
                            changeManager.ManagerId = em.Id;
                            changeManager.RoleId = 2;
                            database.Employees.Update(changeManager);
                            database.SaveChanges();
                        }
                        if (changeManagerEmployee != null)
                        {
                            changeManagerEmployee.ForEach(e => { e.ManagerId = em.Id; });
                        }
                    }
                    Employee newEmployee = new Employee();
                    newEmployee.FirstName = firstName;
                    newEmployee.LastName = lastName;
                    newEmployee.Email = email;
                    newEmployee.Phone = phone;
                    newEmployee.Address = address;
                    newEmployee.DateOfBirth = dateofbirth;
                    newEmployee.Salary = (decimal)salary;
                    newEmployee.Gender = gender;
                    newEmployee.PositionId = idPosition;
                    newEmployee.DepartmentId = idDepartment;
                    newEmployee.ManagerId = em.Id;
                    newEmployee.RoleId = 3;
                    newEmployee.IsDelete = active;
                    newEmployee.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
                    newEmployee.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);

                    database.Employees.Add(newEmployee);
                    database.SaveChanges();

                    Authentication empAuthentication = new Authentication();
                    empAuthentication.EmployeeId = newEmployee.Id;
                    empAuthentication.Username = email;
                    empAuthentication.PassWord = "12345678";
                    empAuthentication.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
                    empAuthentication.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);

                    database.Authentications.Add(empAuthentication);
                    if (database.SaveChanges() > 0)
                    {
                        MessageBox.Show("Nhân viên mới đã được thêm thành công!", "Thông báo");
                        LoadData(newEmployee.Id);
                    }
                    else
                    {
                        MessageBox.Show("Lỗi thêm nhân viên thất bại!", "Thông báo");
                    }
                }
                else
                {
                    var selectedManager = cbbManager.SelectedItem as dynamic;
                    int idManager = selectedManager.ID;
                    Employee newEmployee = new Employee();
                    newEmployee.FirstName = firstName;
                    newEmployee.LastName = lastName;
                    newEmployee.Email = email;
                    newEmployee.Phone = phone;
                    newEmployee.Address = address;
                    newEmployee.DateOfBirth = dateofbirth;
                    newEmployee.Gender = gender;
                    newEmployee.Salary = (decimal)salary;
                    newEmployee.PositionId = idPosition;
                    newEmployee.DepartmentId = idDepartment;
                    newEmployee.ManagerId = idManager;
                    newEmployee.IsDelete = active;
                    newEmployee.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
                    newEmployee.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);
                    database.Employees.Add(newEmployee);
                    database.SaveChanges();

                    Authentication empAuthentication = new Authentication();
                    empAuthentication.EmployeeId = newEmployee.Id;
                    empAuthentication.Username = email;
                    empAuthentication.PassWord = "12345678";
                    empAuthentication.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
                    empAuthentication.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);

                    database.Authentications.Add(empAuthentication);
                    if (database.SaveChanges() > 0)
                    {
                        MessageBox.Show("Nhân viên mới đã được thêm thành công!", "Thông báo");
                        LoadData(newEmployee.Id);
                    }
                    else
                    {
                        MessageBox.Show("Lỗi thêm nhân viên thất bại!", "Thông báo");
                    }

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm mới nhân viên!", "Thông báo");
            }
        }
        private void btnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(txtID.Text, out int employeeId))
                {
                    MessageBox.Show("Xử lí tìm kiếm ID nhân viên lỗi!", "Thông báo");
                    return;
                }
                var employee = database.Employees.Where(e => e.Id == employeeId).SingleOrDefault();
                if (employee != null)
                {
                    if (MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên " + employee.FirstName + " " + employee.LastName, "Thông báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        employee.IsDelete = true;
                        var authentication = database.Authentications.Where(a => a.EmployeeId == employee.Id).SingleOrDefault();
                        if (authentication != null)
                        {
                            authentication.IsDelete = true;
                        }
                        if (database.SaveChanges() > 0)
                        {
                            MessageBox.Show("Đã xóa thành công nhân viên!", "Thông báo");
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Xóa nhân viên không thành công!", "Thông báo");
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên!", "Thông báo");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa nhân viên không thành công!", "Thông báo");
            }
        }
    }
}
