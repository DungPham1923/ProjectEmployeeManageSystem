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
using System.Windows.Shapes;

namespace ManageEmployeeSystem
{
    /// <summary>
    /// Interaction logic for EmployeeJobs.xaml
    /// </summary>
    public partial class EmployeeJobs : Window
    {
        Employee em = new Employee();
        ProjectPrn212Context database = new ProjectPrn212Context();
        public EmployeeJobs()
        {
            InitializeComponent();
            LoadDataJobs();
            LoadDataJobStatus();
        }
        public EmployeeJobs(Employee employee)
        {
            InitializeComponent();
            em = employee;
            this.Title = "Hello " + employee.FirstName + " " + employee.LastName;
            LoadAssignBy();
            LoadDataJobStatus();
            LoadEmployee();
            if (em != null)
            {
                if (em.RoleId == 2)
                {
                    LoadIndiJobs();             
                    cbAllJob.Visibility = Visibility.Visible;
                }
                if (em.RoleId == 3) 
                {
                    LoadDataJobs();
                    cbAllJob.Visibility = Visibility.Visible;
                    cbIndiJob.Visibility = Visibility.Visible;
                }
            }
        }

        private void LoadEmployee()
        {
            if (em.RoleId == 2) 
            {
                var employeedata = database.Employees.Where(e => e.Id == em.Id && e.DepartmentId == em.DepartmentId).Select(e => new
                {
                    ID = e.Id,
                    FullName = $"{e.FirstName} {e.LastName}"
                }).ToList();
                cbbSelectEmployee.ItemsSource = employeedata;
                cbbSelectEmployee.DisplayMemberPath = "FullName";
                cbbSelectEmployee.SelectedValuePath = "ID";
            }
            if (em.RoleId == 3)
            {
                var employeedata = database.Employees.Where(e => e.DepartmentId == em.DepartmentId).Select(e => new
                {
                    ID = e.Id,
                    FullName = $"{e.FirstName} {e.LastName}"
                }).ToList();
                cbbSelectEmployee.ItemsSource = employeedata.ToList();
                cbbSelectEmployee.DisplayMemberPath = "FullName";
                cbbSelectEmployee.SelectedValuePath = "ID";
            }
        }

        private void LoadAssignBy()
        {
            var employeedata = database.Employees.Where(e => e.Id == em.Id && e.DepartmentId == em.DepartmentId).Select(e => new
            {
                ID = e.Id,
                FullName = $"{e.FirstName} {e.LastName}"
            }).ToList();
            cbbSelectAssign.ItemsSource = employeedata;
            cbbSelectAssign.DisplayMemberPath = "FullName";
            cbbSelectAssign.SelectedValuePath = "ID";
        }

        private void LoadDataJobs()
        {
            var job = database.EmployeeJobs.Where(j => j.Job.DepartmentId == em.DepartmentId).Select(j => new
            {
                ID = j.EmployeeJobId,
                JobID = j.JobId,
                JobName = j.Job.Title,
                Description = j.Job.Description,
                EmployeeName = j.Employee.FirstName + " " + j.Employee.LastName,
                AssignBy = j.Job.AssignedByNavigation.FirstName + " " + j.Job.AssignedByNavigation.LastName,
                StartDate = j.Job.StartDate,
                EndDate = j.Job.EndDate,
                AssigmentDate = j.AssignmentDate,
                Status = j.Job.JobStatus.Name,
            }).ToList();
            dgEmployeeJobs.ItemsSource = job.ToList();
        }
        private void LoadIndiJobs()
        {
            dgEmployeeJobs.ItemsSource = string.Empty;
            var job = database.EmployeeJobs.Where(j => j.Job.DepartmentId == em.DepartmentId && j.EmployeeId == em.Id).Select(j => new
            {
                ID = j.EmployeeJobId,
                JobID = j.JobId,
                JobName = j.Job.Title,
                Description = j.Job.Description,
                EmployeeName = j.Employee.FirstName + " " + j.Employee.LastName,
                AssignBy = j.Job.AssignedByNavigation.FirstName + " " + j.Job.AssignedByNavigation.LastName,
                StartDate = j.Job.StartDate,
                EndDate = j.Job.EndDate,
                AssigmentDate = j.AssignmentDate,
                Status = j.Job.JobStatus.Name,
            }).ToList();
            dgEmployeeJobs.ItemsSource = job.ToList();
        }
        private void LoadDataJobStatus()
        {
            var jobStatus = database.JobStatuses.ToList();
            cbbFilterJobStatus.ItemsSource = jobStatus.ToList();
            cbbFilterJobStatus.DisplayMemberPath = "Name";
            cbbFilterJobStatus.SelectedValuePath = "ID";
            cbbStatus.ItemsSource = jobStatus.ToList();
            cbbStatus.DisplayMemberPath = "Name";
            cbbStatus.SelectedValuePath = "ID";
        }

        private void dgEmployeeJobs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgEmployeeJobs.SelectedItem == null)
            {
                return;
            }
            var employeJob = dgEmployeeJobs.SelectedItem as dynamic;
            if (employeJob != null)
            {
                //var employeeDetail = database.EmployeeJobs.FirstOrDefault(x => x.EmployeeJobId);
                //txtJobID.Text = employeJob.JobId.ToString();
                //txtJobName.Text = employeJob.Job.Title.ToString();
                //txtDesription.Text = employeJob.Job.Description.ToString();
                //txtStatus.Text = employeJob.Job.JobStatus.Name.ToString();
                //txtEmployee.Text = employeJob.Employee.LastName + " " + employeJob.Employee.FirstName;
                //txtAssignBy.Text = employeJob.Job.AssignedByNavigation.LastName + " " + employeJob.Job.AssignedByNavigation.FirstName;
                DateOnly startDate = DateTime.Parse(employeJob.Job.StartDate.ToString());
                txtStartDate.Text = startDate.ToString();
                DateOnly endDate = DateTime.Parse(employeJob.Job.EndDate.ToString());
                txtEndDate.Text = endDate.ToString();
                txtAssignDate.Text = employeJob.AssignmentDate.ToString();
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

        private void AllJobOfDepartment_Click(object sender, RoutedEventArgs e)
        {
            LoadDataJobs();
        }

        private void MyJobs_Click(object sender, RoutedEventArgs e)
        {
            dgEmployeeJobs.ItemsSource = string.Empty;
            var job = database.EmployeeJobs.Where(j => j.Job.DepartmentId == em.DepartmentId && j.EmployeeId == em.Id).Select(j => new
            {
                ID = j.EmployeeJobId,
                JobID = j.JobId,
                JobName = j.Job.Title,
                Description = j.Job.Description,
                EmployeeName = j.Employee.FirstName + " " + j.Employee.LastName,
                AssignBy = j.Job.AssignedByNavigation.FirstName + " " + j.Job.AssignedByNavigation.LastName,
                StartDate = j.Job.StartDate,
                EndDate = j.Job.EndDate,
                AssigmentDate = j.AssignmentDate,
                Status = j.Job.JobStatus.Name,
            }).ToList();
            dgEmployeeJobs.ItemsSource = job.ToList();
        }

        private void cbbFilterJobStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (em.RoleId == 3)
            {
                if (cbIndiJob.IsChecked == true)
                {
                    var statusId = cbbFilterJobStatus.SelectedIndex + 1;
                    dgEmployeeJobs.ItemsSource = string.Empty;
                    var job = database.EmployeeJobs.Where(j => j.Job.DepartmentId == em.DepartmentId && j.Job.JobStatusId == statusId && j.EmployeeId == em.Id).Select(j => new
                    {
                        ID = j.EmployeeJobId,
                        JobID = j.JobId,
                        JobName = j.Job.Title,
                        Description = j.Job.Description,
                        EmployeeName = j.Employee.FirstName + " " + j.Employee.LastName,
                        AssignBy = j.Job.AssignedByNavigation.FirstName + " " + j.Job.AssignedByNavigation.LastName,
                        StartDate = j.Job.StartDate,
                        EndDate = j.Job.EndDate,
                        AssigmentDate = j.AssignmentDate,
                        Status = j.Job.JobStatus.Name,
                    }).ToList();
                    dgEmployeeJobs.ItemsSource = job.ToList();
                    cbAllJob.IsChecked = false;
                }
                else
                {
                    var statusId = cbbFilterJobStatus.SelectedIndex + 1;
                    dgEmployeeJobs.ItemsSource = string.Empty;
                    var job = database.EmployeeJobs.Where(j => j.Job.DepartmentId == em.DepartmentId && j.Job.JobStatusId == statusId).Select(j => new
                    {
                        ID = j.EmployeeJobId,
                        JobID = j.JobId,
                        JobName = j.Job.Title,
                        Description = j.Job.Description,
                        EmployeeName = j.Employee.FirstName + " " + j.Employee.LastName,
                        AssignBy = j.Job.AssignedByNavigation.FirstName + " " + j.Job.AssignedByNavigation.LastName,
                        StartDate = j.Job.StartDate,
                        EndDate = j.Job.EndDate,
                        AssigmentDate = j.AssignmentDate,
                        Status = j.Job.JobStatus.Name,
                    }).ToList();
                    dgEmployeeJobs.ItemsSource = job.ToList();
                    cbAllJob.IsChecked = false;
                    cbIndiJob.IsChecked = false;
                }
            }
            if (em.RoleId == 2)
            {
                var statusId = cbbFilterJobStatus.SelectedIndex + 1;
                dgEmployeeJobs.ItemsSource = string.Empty;
                var job = database.EmployeeJobs.Where(j => j.Job.DepartmentId == em.DepartmentId && j.Job.JobStatusId == statusId && j.EmployeeId == em.Id).Select(j => new
                {
                    ID = j.EmployeeJobId,
                    JobID = j.JobId,
                    JobName = j.Job.Title,
                    Description = j.Job.Description,
                    EmployeeName = j.Employee.FirstName + " " + j.Employee.LastName,
                    AssignBy = j.Job.AssignedByNavigation.FirstName + " " + j.Job.AssignedByNavigation.LastName,
                    StartDate = j.Job.StartDate,
                    EndDate = j.Job.EndDate,
                    AssigmentDate = j.AssignmentDate,
                    Status = j.Job.JobStatus.Name,
                }).ToList();
                dgEmployeeJobs.ItemsSource = job.ToList();
                cbAllJob.IsChecked = false;
            }

        }
        private void cbIndiJob_Checked(object sender, RoutedEventArgs e)
        {
            dgEmployeeJobs.ItemsSource = string.Empty;
            var job = database.EmployeeJobs.Where(j => j.Job.DepartmentId == em.DepartmentId && j.EmployeeId == em.Id).Select(j => new
            {
                ID = j.EmployeeJobId,
                JobID = j.JobId,
                JobName = j.Job.Title,
                Description = j.Job.Description,
                EmployeeName = j.Employee.FirstName + " " + j.Employee.LastName,
                AssignBy = j.Job.AssignedByNavigation.FirstName + " " + j.Job.AssignedByNavigation.LastName,
                StartDate = j.Job.StartDate,
                EndDate = j.Job.EndDate,
                AssigmentDate = j.AssignmentDate,
                Status = j.Job.JobStatus.Name,
            }).ToList();
            dgEmployeeJobs.ItemsSource = job.ToList();
            cbAllJob.IsChecked = false;
        }

        private void cbAllJob_Checked(object sender, RoutedEventArgs e)
        {
            if (em.RoleId == 3)
            {
                LoadDataJobs();
            }
            if (em.RoleId == 2)
            {
                LoadIndiJobs();
            }
            cbIndiJob.IsChecked = false;
            cbbFilterJobStatus.SelectedIndex = -1;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            txtJobID.Text = string.Empty;
            txtJobName.Text = string.Empty;
            txtDesription.Text = string.Empty;
            txtEndDate.Text = string.Empty;
            txtStartDate.Text = string.Empty;
            cbbStatus.Text = string.Empty;
            cbbSelectAssign.Text = string.Empty;
            txtAssignDate.Text = string.Empty;
            cbbSelectEmployee.Text = string.Empty;
        }
    }
}
