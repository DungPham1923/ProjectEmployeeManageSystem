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
        Employee em = new Employee();
        ProjectPrn212Context database = new ProjectPrn212Context();
        public Userprofile(Employee employee)
        {
            InitializeComponent();
            em = employee;
            this.Title = "Hello " + employee.FirstName + " " + employee.LastName;
            LoadData();
            LoadDataDepartment();
            LoadDataAccount();
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


            var department = database.Departments.FirstOrDefault(d => d.Id == em.DepartmentId);
            if (department != null)
            {
                txtDepartment.Text = department.Name.ToString();
                txtDepartmentID.Text = department.Id.ToString();
            }

            txtCreatedAt.Text = em.CreatedAt.ToString();
            string salary = FormatNumber((double)em.Salary);
            txtSalary.Text = salary + " VNĐ";//em.Salary.ToString();
            var position = database.Positions.FirstOrDefault(p => p.Id == em.PositionId);
            if (position != null)
            {
                txtPosition.Text = position.Name.ToString();
            }

            var manager = database.Employees.FirstOrDefault(m => m.Id == em.Id);
            if (manager != null)
            {
                txtManager.Text = manager.FirstName.ToString() + " " + manager.LastName.ToString();
            }

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

        private void LoadDataDepartment()
        {
            var department = database.Departments.Select(d => new
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
            }).ToList();
            //var department = database.Departments.ToList();
            dgDepartments.ItemsSource = department.ToList();
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
            Home home = new Home(em);
            this.Hide();
            home.ShowDialog();
            this.Close();
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

        private void dgDepartments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgDepartments == null)
            {
                return;
            }
            var selectedItem = dgDepartments.SelectedItem as dynamic;
            if (selectedItem != null)
            {
                txtDepartment.Text = selectedItem.Name.ToString();
                txtDepartmentID.Text = selectedItem.Id.ToString();
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
                //Employee employee = new Employee()
                //{
                //    FirstName = firstName,
                //    LastName = lastName,
                //    Email = email,
                //    Phone = phone,
                //    Address = address,
                //    DateOfBirth = dateofbirth,
                //    Gender = gender,
                //};
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



                //if(txtFirstname.Text == null) { MessageBox.Show("Tên không được để trống!", "Thông báo");  return; }
                //else { firstName = txtFirstname.Text; }

                //if(txtLastname.Text == null) { MessageBox.Show("Họ không được để trống!", "Thông báo"); return; } 
                //else{ lastName = txtLastname.Text; }

                //if(txtEmail.Text == null) { MessageBox.Show("Email không được để trống!", "Thông báo"); return; }
                //else { email = txtEmail.Text; }

                //if (txtPhone.Text == null) { MessageBox.Show("Số điện thoại không được để trống!", "Thông báo"); return; }


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
    }
}
