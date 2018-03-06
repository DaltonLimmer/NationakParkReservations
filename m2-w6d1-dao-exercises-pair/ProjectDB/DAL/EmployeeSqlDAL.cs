using ProjectDB.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDB.DAL
{
    public class EmployeeSqlDAL
    {
        private string connectionString;
        private const string SQL_GetAllEmployees = "SELECT * FROM Employee";
        private const string SQL_SearchForEmployee = "SELECT * FROM employee WHERE first_name = @firstName AND last_name = @lastName";
        private const string SQL_GetEmployeesWithoutProjects = "SELECT * From Employee LEFT JOIN project_employee ON employee.employee_id = project_employee.employee_id Where project_employee.project_id IS NULL";

        // Single Parameter Constructor
        public EmployeeSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetAllEmployees, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee item = GetEmployeeFromReader(reader);
                        employees.Add(item);
                    }

                }
            }
            catch (SqlException)
            {

                throw;
            }

            return employees ;
        }

        public List<Employee> Search(string firstname, string lastname)
        {
            List<Employee> employeesFound = new List<Employee>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_SearchForEmployee, conn);
                    cmd.Parameters.AddWithValue("@firstName", firstname);
                    cmd.Parameters.AddWithValue("@lastName", lastname);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee item = GetEmployeeFromReader(reader);
                        employeesFound.Add(item);
                    }
                }
            }
            catch (SqlException)
            {

                throw;
            }

            return employeesFound;
        }

        public List<Employee> GetEmployeesWithoutProjects()
        {
            List<Employee> employeesFound = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_GetEmployeesWithoutProjects, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee item = GetEmployeeFromReader(reader);
                        employeesFound.Add(item);
                    }
                }
            }
            catch (SqlException)
            {

                throw;
            }

            return employeesFound;
        }

        private Employee GetEmployeeFromReader(SqlDataReader reader)
        {
            Employee employee = new Employee()
            {
                EmployeeId = Convert.ToInt32(reader["employee_id"]),
                DepartmentId = Convert.ToInt32(reader["department_id"]),
                FirstName = Convert.ToString(reader["first_name"]),
                LastName = Convert.ToString(reader["last_name"]),
                JobTitle = Convert.ToString(reader["job_title"]),
                BirthDate = Convert.ToDateTime(reader["birth_date"]),
                Gender = Convert.ToString(reader["gender"]),
                HireDate = Convert.ToDateTime(reader["hire_date"])
            };

            return employee;
        }
    }
}
