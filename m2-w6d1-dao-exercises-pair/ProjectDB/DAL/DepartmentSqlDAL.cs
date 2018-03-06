using ProjectDB.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDB.DAL
{
    public class DepartmentSqlDAL
    {
        private string connectionString;
        private const string SQL_GetDepartments = "SELECT * FROM Department";
        private const string SQL_InsertDepartment = "INSERT INTO Department(name) VALUES(@name)";
        private const string SQL_UpdateDepartment = "UPDATE Department SET name = @NewName WHERE department_id = @NewDepartment_ID";


        // Single Parameter Constructor
        public DepartmentSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Department> GetDepartments()
        {
            List<Department> departments = new List<Department>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetDepartments, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Department item = GetDepartmentFromReader(reader);
                        departments.Add(item);
                    }

                }
            }
            catch (SqlException)
            {

                throw;
            }

            return departments;
        }

        public bool CreateDepartment(Department newDepartment)
        {
            bool wasSuccessful = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_InsertDepartment, conn);
                    cmd.Parameters.AddWithValue("@name", newDepartment.Name);
                    var reader = cmd.ExecuteReader();
                    wasSuccessful = reader.RecordsAffected == 0 ? false : true;
                }

            }
            catch (SqlException)
            {

                throw;
            }

            return wasSuccessful;
        }

        public bool UpdateDepartment(Department updatedDepartment)
        {
            bool wasSuccessful = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_UpdateDepartment, conn);
                    cmd.Parameters.AddWithValue("@NewName", updatedDepartment.Name);
                    cmd.Parameters.AddWithValue("@NewDepartment_ID", updatedDepartment.Id );

                    var reader = cmd.ExecuteReader();
                    wasSuccessful = reader.RecordsAffected == 0 ? false : true;
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return wasSuccessful;
        }

        private Department GetDepartmentFromReader(SqlDataReader reader)
        {
            Department item = new Department
            {
                Id = Convert.ToInt32(reader["department_id"]),
                Name = Convert.ToString(reader["name"])
            };

            return item;

        }
    }
}
