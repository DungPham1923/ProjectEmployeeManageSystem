using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageEmployeeSystem.Models
{
    public partial class DepartmentEmployee
    {
        public int DepartmentID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Manager { get; set; }

        public string ManagerEmail { get; set; }

        public DateOnly? CreatedAt { get; set; }

        public DateOnly? UpdatedAt { get; set; }

        public bool? Status { get; set; }
    }
}
