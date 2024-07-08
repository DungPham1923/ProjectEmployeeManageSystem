using ManageEmployeeSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
                Status = (bool) d.IsDelete ? "Ngừng hoạt động" : "Đang hoạt động",
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
                if (department.Status == "Đang hoạt động")
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

                if (manager != null && manager.Fullname != null)
                {
                    cbbManager.SelectedItem = manager;
                }
                else
                {
                    cbbManager.SelectedIndex = -1;
                }
                var manager2 = database.Employees.Where(m => m.RoleId == 3 && m.DepartmentId == idDepartment).FirstOrDefault();
                if (manager2 != null)
                {
                    txtEmail.Text = manager2.Email;
                }

            }
            else
            {
                MessageBox.Show("Null");
            }

        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtDepartmentID.Clear();
            txtDepartmentName.Clear();
            txtEmail.Clear();
            txtDescription.Clear();
            dpCreatedAt.Clear();
            dpUpdatedAt.Clear();
            cbbManager.SelectedIndex = -1;
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
                    txtEmail.Text = employee.Email;
                }
                else
                {
                    MessageBox.Show("Không thể tìm thấy email của nhân viên " + manager.Fullname, "Thông báo");
                }
            }
        }

        private void btnUpdateDepartment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(txtDepartmentID.Text, out int departmentId))
                {
                    MessageBox.Show("Không xử lí được ID phòng ban!", "Thông báo");
                    return;
                }
                string name = txtDepartmentName.Text;
                string description = txtDescription.Text;
                bool isDelete = true;
                if (radioActive.IsChecked == true)
                {
                    isDelete = false;
                }
                else if (radioInactive.IsChecked == true)
                {
                    isDelete = true;
                }
                //DateOnly updatedAt = DateOnly.Parse(DateTime.Now());
                if(cbbManager.SelectedItem != null)
                {
                    int idEmployee= 0;
                    var manager = cbbManager.SelectedItem as dynamic;
                    if (manager != null)
                    {
                        idEmployee = manager.ID;
                    }
                    var manageFuture = database.Employees.FirstOrDefault(e => e.Id == idEmployee);
                    var manageCurrent = database.Employees.FirstOrDefault(e => e.DepartmentId == departmentId && e.RoleId == 3); //trưởng phòng của phòng này
                    if (manageFuture != null && manageCurrent != null)
                    {
                        if(manageFuture != manageCurrent && manageFuture.RoleId != 3) //nếu nhân viên được chọn không phải trưởng phỏng của phòng này và cũng không phải trưởng phòng 
                        {
                            if(MessageBox.Show("Thay thế trưởng phòng \"" + manageCurrent.FirstName + " " + manageCurrent.LastName + "\" thành \"" + manageFuture.FirstName + " " + manageFuture.LastName + "\"?", "Thông báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                //Bãi nhiệm:
                                manageCurrent.PositionId = null;
                                manageCurrent.RoleId = 2; //role nhân viên
                                manageCurrent.ManagerId = manageFuture.Id;
                                database.Employees.Update(manageCurrent); 

                                //Lên chức:
                                manageFuture.DepartmentId = departmentId; //chuyển phòng
                                manageFuture.RoleId = 3; //cấp quyền trưởng phòng
                                manageFuture.PositionId = 1; //trưởng phòng
                                manageFuture.ManagerId = manageFuture.Id;
                                database.Employees.Update(manageFuture);

                                //tất cả nhân viên phòng hiện tại có quản lí mới
                                var employeeOfDepart = database.Employees.Where(e => e.DepartmentId == departmentId).ToList();
                                employeeOfDepart.ForEach(e => e.ManagerId = manageFuture.Id);
                                employeeOfDepart.ForEach(e =>  database.Employees.Update(e));

                            }
                            else
                            {
                                return;
                            }
                        }
                        else if (manageFuture != manageCurrent && manageFuture.RoleId == 3)// khác và cũng là trưởng phòng
                        {
                            if(MessageBox.Show("\"" + manageFuture.FirstName + " " + manageFuture.LastName+"\" hiện đang là trưởng phòng của phòng của 1 phòng ban khác, xác nhận thay thế ?", "Thông báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                //Bãi nhiệm:
                                manageCurrent.PositionId = null;
                                manageCurrent.RoleId = 2; //role nhân viên
                                manageCurrent.ManagerId = manageFuture.Id;
                                database.Employees.Update(manageCurrent);

                                //tất cả nhiên viên ở phòng của trưởng phòng mới bị mất trưởng phòng
                                var employeeOfDepartOfManageFuture = database.Employees.Where(e => e.DepartmentId == manageFuture.DepartmentId).ToList();
                                employeeOfDepartOfManageFuture.ForEach(e => e.ManagerId = null);
                                employeeOfDepartOfManageFuture.ForEach(e => database.Employees.Update(e));

                                //Lên chức:
                                manageFuture.DepartmentId = departmentId; //chuyển phòng
                                manageFuture.ManagerId = manageFuture.Id;
                                database.Employees.Update(manageFuture);


                                //tất cả nhân viên phòng hiện tại có quản lí mới
                                var employeeOfDepart = database.Employees.Where(e => e.DepartmentId == departmentId).ToList();
                                employeeOfDepart.ForEach(e => e.ManagerId = manageFuture.Id);
                                employeeOfDepart.ForEach(e => database.Employees.Update(e));
       
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                }

                var departmentUpdate = database.Departments.SingleOrDefault(d => d.Id == departmentId);
                if (departmentUpdate != null)
                {
                    departmentUpdate.Name = name;
                    departmentUpdate.Description = description;
                    departmentUpdate.IsDelete = isDelete;
                    departmentUpdate.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);
                    database.Departments.Update(departmentUpdate);
                    if (database.SaveChanges() > 0)
                    {
                        MessageBox.Show("Cập nhật thông tin phòng ban thành công!", "Thông báo");
                        LoadDataDepartment();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật thông tin phòng ban thất bại", "Thông báo");
                    }
                }
                else
                {
                    MessageBox.Show("Không thể tìm thấy phòng ban này!", "Thông báo");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật phòng ban!" + ex.Message, "Thông báo");
                return;
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string datasearch = txtDatasearch.Text;
                if (datasearch.IsNullOrEmpty())
                {
                    MessageBox.Show("Vui lòng điền gợi í tìm kiếm!", "Thông báo");
                    return;
                }
                var department = database.Departments.Where(d => d.Name.ToLower().Contains(datasearch) || d.Description.ToLower().Contains(datasearch)).Select(d => new
                {
                    DepartmentID = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt,
                    Status = d.IsDelete,
                }).ToList();
                if (department != null)
                {
                    dgDepartment.ItemsSource = department;
                    cbAllDepartment.IsChecked = false;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy phòng ban!", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm " + ex.Message, "Thông báo");
            }
        }

        private void cbAllEmployee_Checked(object sender, RoutedEventArgs e)
        {
            if (cbAllDepartment.IsChecked == true)
            {
                LoadDataDepartment();
            }
        }

        private void cbbFilterEmployeeStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbbFilterStatus.Items == null)
            {
                return;
            }
            if (cbbFilterStatus.SelectedIndex == 0)
            {
                var department = database.Departments.Where(d => d.IsDelete == false).Select(d => new
                {
                    DepartmentID = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt,
                    Status = d.IsDelete,
                }).ToList();
                if (department.Any())
                {
                    dgDepartment.ItemsSource = department;
                    cbAllDepartment.IsChecked = false;
                }
                else
                {
                    MessageBox.Show("Không có phòng ban nào hoạt động!", "Thông báo");
                    return;
                }

            }
            if (cbbFilterStatus.SelectedIndex == 1)
            {
                var department = database.Departments.Where(d => d.IsDelete == true).Select(d => new
                {
                    DepartmentID = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt,
                    Status = d.IsDelete,
                }).ToList();
                if (department.Any())
                {
                    dgDepartment.ItemsSource = department;
                    cbAllDepartment.IsChecked = false;
                }
                else
                {
                    MessageBox.Show("Không có phòng ban nào ngừng hoạt động!", "Thông báo");
                    return;
                }
            }
        }

        private void btnAddDepartment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = txtDepartmentName.Text;
                string description = txtDescription.Text;
                bool isDelete = true;
                if (radioActive.IsChecked == true)
                {
                    isDelete = false;
                }
                else if (radioInactive.IsChecked == true)
                {
                    isDelete = true;
                }
                Department department = new Department();
                department.Name = name;
                department.Description = description;
                department.IsDelete = isDelete;
                department.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
                department.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);
                database.Departments.Add(department);
                if (database.SaveChanges() > 0)
                {
                    MessageBox.Show("Thêm mới phòng ban thành công!", "Thông báo");
                    LoadDataDepartment();
                }
                else
                {
                    MessageBox.Show("Thêm mới phòng ban thất bại", "Thông báo");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm mới phòng ban!" + ex.Message, "Thông báo");
                return;
            }

        }

        private void btnDeleteDepartment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(txtDepartmentID.Text, out int Id))
                {
                    MessageBox.Show("Lỗi xử lí ID!", "Thông báo");
                    return;
                }
                var department = database.Departments.SingleOrDefault(d => d.Id == Id);
                if (department != null)
                {
                    var employee = database.Employees.Where(e => e.DepartmentId == Id).ToList();
                    var job = database.Jobs.Where(j =>j.DepartmentId == Id).ToList();
                    if (employee.Count != 0)
                    {
                        if (MessageBox.Show("Xóa phòng " + department.Name + " sẽ làm cho các nhân viên hiện tại của phòng ban này không có phòng làm việc và xóa hết công việc của phòng này! Chắc chắn muốn xóa phòng " + department.Name + " ?", "Thông báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            employee.ForEach(e => { e.DepartmentId = null; });
                            employee.ForEach(e => database.Employees.Update(e));

                            job.ForEach(j => { j.DepartmentId = null; });
                            job.ForEach(j => database.Jobs.Update(j));
                            database.Departments.Remove(department);
                            if (database.SaveChanges() > 0)
                            {
                                MessageBox.Show("Xóa phòng ban thành công!", "Thông báo");
                                LoadDataDepartment();
                            }
                            else
                            {
                                MessageBox.Show("Xóa phòng ban thất bại!", "Thông báo");
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (MessageBox.Show("Bạn chắc chắn muốn xóa phòng " + department.Name + " ?", "Thông báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            database.Departments.Remove(department);
                            if (database.SaveChanges() > 0)
                            {
                                MessageBox.Show("Xóa phòng ban thành công!", "Thông báo");
                                LoadDataDepartment();
                            }
                            else
                            {
                                MessageBox.Show("Xóa phòng ban thất bại!", "Thông báo");
                                return;
                            }
                        }
                    }


                }
                else
                {
                    MessageBox.Show("Không tìm thấy phòng ban trong hệ thông!", "Thông báo");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo");
                return;
            }
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            Home home = new Home(em);
            this.Hide();
            home.ShowDialog();
            this.Close();
        }
    }
}
