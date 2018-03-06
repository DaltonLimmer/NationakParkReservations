using ProjectDB.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDB.DAL
{
    public class ProjectSqlDAL
    {
        private string connectionString;
        private const string SQL_GetAllProjects = "SELECT * FROM project";
        private const string SQL_AssignEmployeeToProject = "INSERT INTO project_employee (project_id, employee_id) VALUES (@projectId, @employeeId)";
        private const string SQL_RemoveEmployeeFromProject = "DELETE FROM project_employee WHERE employee_id = @employeeID AND project_id = @projectID";
        private const string SQL_CreateNewProject = "INSERT INTO project (name, from_date, to_date) VALUES (@name, @fromDate, @toDate)";

        // Single Parameter Constructor
        public ProjectSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Project> GetAllProjects()
        {
            List<Project> projects = new List<Project>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_GetAllProjects, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Project item = GetProjectsFromReader(reader);
                        projects.Add(item);
                    }
                }
            }
            catch (SqlException)
            {

                throw;
            }

            return projects;
        }

        public bool AssignEmployeeToProject(int projectId, int employeeId)
        {
            bool wasSuccessful = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_AssignEmployeeToProject, conn);
                    cmd.Parameters.AddWithValue("@projectId", projectId);
                    cmd.Parameters.AddWithValue("@employeeId", employeeId);
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

        public bool RemoveEmployeeFromProject(int projectId, int employeeId)
        {
            bool wasSuccessful = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_RemoveEmployeeFromProject, conn);
                    cmd.Parameters.AddWithValue("@projectID", projectId);
                    cmd.Parameters.AddWithValue("@employeeID", employeeId);

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

        public bool CreateProject(Project newProject)
        {
            bool wasSuccessful = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_CreateNewProject, conn);
                    cmd.Parameters.AddWithValue("@name", newProject.Name);
                    cmd.Parameters.AddWithValue("@fromdate", newProject.StartDate);
                    cmd.Parameters.AddWithValue("@todate", newProject.EndDate);

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

        private Project GetProjectsFromReader(SqlDataReader reader)
        {
            Project project = new Project
            {
                ProjectId = Convert.ToInt32(reader["project_id"]),
                Name = Convert.ToString(reader["name"]),
                StartDate = Convert.ToDateTime(reader["from_date"]),
                EndDate = Convert.ToDateTime(reader["to_date"])
            };

            return project;
        }

    }
}
