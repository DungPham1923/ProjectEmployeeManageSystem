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
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
        }
        Employee em = new Employee();
        public Home(Employee employee)
        {
            InitializeComponent();
            this.Title = "Hello " + employee.FirstName + " " + employee.LastName;
            em = employee;
            if (em != null)
            {
                if (em.RoleId == 2)
                {
                    userFunc.Visibility = Visibility.Visible;
                }
                if (em.RoleId == 1)
                {
                    adminFunc.Visibility = Visibility.Visible;
                }
                if (em.RoleId == 3)
                {
                    manageFunc.Visibility = Visibility.Visible;
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

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (HomeBack.CanGoBack)
            {
                HomeBack.NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Không có trang quay lại!", "Thông báo");
            }
        }

        private void ProfileDetail_Click(object sender, RoutedEventArgs e)
        {
            Userprofile userprofile = new Userprofile(em);
            this.Hide();
            userprofile.ShowDialog();
            this.Close();
        }

        private void Department_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EmployeeJobs_Click(object sender, RoutedEventArgs e)
        {
            EmployeeJobs employeejobs = new EmployeeJobs(em);
            this.Hide();
            employeejobs.ShowDialog();
            this.Close();

        }
    }
}
