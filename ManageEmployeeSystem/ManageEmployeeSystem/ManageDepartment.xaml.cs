using ManageEmployeeSystem.Models;
using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for ManageDepartment.xaml
    /// </summary>
    public partial class ManageDepartment : Window
    {
        Employee em;
        ProjectPrn212Context database = new ProjectPrn212Context();
        public ManageDepartment()
        {
            InitializeComponent();
        }
        public ManageDepartment(Employee employeeAdmin)
        {
            InitializeComponent();
            em = employeeAdmin;
            this.Title = "Xin chào admin, " + em.FirstName + " " + em.LastName;
            LoadDataDepartment();
            LoadEmployees();
        }

        private void LoadDataDepartment()
        {
            var departmentData = database.Departments.Select(d => new
            {
                DepartmentID = d.Id,
                Name = d.Name,
                Description = d.Description,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt,
                Status = d.IsDelete,
            }).ToList();
            //var departmentData = database.Employees.Where(e => e.RoleId == 3).Include(d => d.Department).Select(d => new
            //{
            //    DepartmentID = d.DepartmentId,
            //    Name = d.Department.Name,
            //    Description = d.Department.Description,
            //    Manager = d.Manager.FirstName + " " + d.Manager.LastName,
            //    ManagerEmail = d.Manager.Email,
            //    CreatedAt = d.Department.CreatedAt,
            //    UpdatedAt = d.Department.UpdatedAt,
            //    //Status = d.Department.IsDelete,
            //    Status = true,
            //});
            dgDepartment.ItemsSource = departmentData.ToList();
        }

        private void LoadEmployees()
        {
            var employee = database.Employees.Select(e => new
            {
                ID = e.Id,
                Fullname = e.FirstName + " " + e.LastName,
            }).ToList();
            cbbManager.ItemsSource = employee;
            cbbManager.DisplayMemberPath = "Fullname";
            cbbManager.SelectedValuePath = "ID";
        }
        private void GoHome_Click(object sender, RoutedEventArgs e)
        {
            if (em.RoleId == 1)
            {
                Home home = new Home(em);
                this.Hide();
                home.ShowDialog();
                this.Close();
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

        //private void dgDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (dgDepartment.SelectedItem == null)
        //    {
        //        return;
        //    }
        //    //DataGridRow dataGridRow = dgDepartment.SelectedItem as DataGridRow;
        //    //var dataGridRow = dgDepartment.SelectedItem as DataRowView;
        //    //if (dataGridRow != null)
        //    //{
        //    //    //var isDelete = (dataGridRow.Item as DataRowView)["Status"];
        //    //    var isDelete = dataGridRow["Status"] as string;
        //    //    if (isDelete != null)
        //    //    {
        //    //        if (isDelete == "false")
        //    //        {
        //    //            radioActive.IsChecked = true;
        //    //        }
        //    //        if(isDelete == "true")
        //    //        {

        //    //        }

        //    //    }
        //    //}
        //    var department = dgDepartment.SelectedItem as DepartmentEmployee;
        //    if (department != null)
        //    {
        //        if (department.Status == true)
        //        {
        //            radioActive.IsChecked = true;
        //        }
        //        else
        //        {
        //            radioInactive.IsChecked = true;
        //        }
        //    }
        //}
        private void dgDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgDepartment.SelectedItem == null)
            {
                return;
            }

            var department = dgDepartment.SelectedItem as dynamic;
            if (department != null)
            {
                if (department.Status == false)
                {
                    radioActive.IsChecked = true;
                }
                else
                {
                    radioInactive.IsChecked = true;
                }
                int idDepartment = department.DepartmentID;
                var manager = database.Employees.Where(m => m.RoleId == 3 && m.DepartmentId == idDepartment).Select(e => new
                {
                    ID = e.Id,
                    Fullname = e.FirstName + " " + e.LastName,
                }).FirstOrDefault();
                
                if(manager != null)
                {
                    cbbManager.SelectedItem = manager;
                }
                var manager2 = database.Employees.Where(m => m.RoleId == 3 && m.DepartmentId == idDepartment).FirstOrDefault();
                txtEmail.Text = manager2.Email;
            }
            else
            {
                MessageBox.Show("Null");
            }

        }

    }
}
