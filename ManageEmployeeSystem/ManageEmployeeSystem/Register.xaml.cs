using ManageEmployeeSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
        }
        ProjectPrn212Context database = new ProjectPrn212Context();
        private void ExistButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn thoát app?", "Thông báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void Login_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void Clear_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            txtEmail.Clear();
            txtPassword.Clear();
            txtRePassword.Clear();
        }
        private static bool IsEmailFormatValid(string email)
        {
            // Biểu thức chính quy kiểm tra định dạng email phổ biến
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }

        private static bool IsPasswordValid(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(password);
        }


        private void Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string email = txtEmail.Text;
                string password = txtPassword.Password;
                string repassword = txtRePassword.Password;
                if (email != null && password != null && repassword != null)
                {
                    if (!IsEmailFormatValid(email))
                    {
                        MessageBox.Show("Địa chỉ email không hợp lệ!", "Thông báo");
                        return;
                    }
                    var Employee = database.Employees.SingleOrDefault(em => em.Email.Equals(email));
                    if (Employee != null)
                    {
                        MessageBox.Show("Địa chỉ email đã tồn tại! Vui lòng dùng địa chỉ email khác!", "Thông báo");
                        return;
                    }

                    if (!IsPasswordValid(password))
                    {
                        MessageBox.Show("Mật khẩu bao gồm ít nhất 1 kí tự hoa, 1 kí tự thường, 1 kí tự số!", "Thông báo");
                        return;
                    }
                    else
                    {
                        if (!password.Equals(repassword))
                        {
                            MessageBox.Show("Xác nhận mật khẩu không chính xác", "Thông báo");
                            return;
                        }
                    }
                    Employee employee = new Employee()
                    {
                        Email = email,
                        CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                        UpdatedAt = DateOnly.FromDateTime(DateTime.Now),
                    };
                    database.Employees.Add(employee);
                    database.SaveChanges();
                    Authentication authentication = new Authentication()
                    {
                        EmployeeId = employee.Id,
                        Username = email,
                        PassWord = repassword,
                        CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                        UpdatedAt = DateOnly.FromDateTime(DateTime.Now),
                    };
                    database.Authentications.Add(authentication);
                    if (database.SaveChanges() > 0)
                    {
                        if (MessageBox.Show("Đăng kí thành công, vui lòng đăng nhập để vào ứng dụng!", "Thông báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            Login login = new Login();
                            this.Hide();
                            login.ShowDialog();
                            this.Close();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!","Thông báo");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Cú pháp lỗi, vui lòng nhập đúng thông tin!" + ex.Message, "Thông báo");
            }
        }
    }
}
