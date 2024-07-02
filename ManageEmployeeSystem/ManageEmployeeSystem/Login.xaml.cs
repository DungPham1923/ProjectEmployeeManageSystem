using ManageEmployeeSystem.Models;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ManageEmployeeSystem
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
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

        private void Register_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Register register = new Register();
            register.Show();
            this.Close();
        }

        private void ClearForm_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            txtUsername.Text = string.Empty;
            txtPassword.Clear();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = txtUsername.Text;
                string password = txtPassword.Password;
                if (string.IsNullOrEmpty(username))
                {
                    MessageBox.Show("Tên đăng nhập không được để trống!", "Thông báo", MessageBoxButton.OK);
                    return;
                }
                Authentication account = database.Authentications.FirstOrDefault(a => a.Username.Equals(username) & a.PassWord.Equals(password));
                if (account == null)
                {
                    MessageBox.Show("Tài khoản không tồn tại!", "Thông báo", MessageBoxButton.OK);
                }
                else
                {
                    Employee employee = database.Employees.FirstOrDefault(em => em.Id == account.EmployeeId);

                    if (employee != null)
                    {
                        if (employee.RoleId == 2)
                        {
                            Home home = new Home(employee);
                            home.userFunc.Visibility = Visibility.Visible;
                            this.Hide();
                            home.ShowDialog();
                            this.Close();
                            //LoginBack.NavigationService.Navigate(home);
                        }
                        if(employee.RoleId == 3)
                        {
                            Home home = new Home(employee);
                            home.manageFunc.Visibility = Visibility.Visible;
                            this.Hide();
                            home.ShowDialog();
                            this.Close();
                        }
                        if (employee.RoleId == 1)
                        {
                            Home home = new Home(employee);
                            home.adminFunc.Visibility = Visibility.Visible;
                            this.Hide();
                            home.ShowDialog();
                            this.Close();
                            //LoginBack.NavigationService.Navigate(home);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy thông tin nhân viên cho tài khoản này!", "Thông báo", MessageBoxButton.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đăng nhập lỗi: " + ex.Message);
            }
        }
    }
}
