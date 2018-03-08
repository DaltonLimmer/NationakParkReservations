using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ProjectDB.DAL;
using System.Data.SqlClient;
using ProjectDB.Models;

namespace ProjectDB.Tests.DAL
{
    [TestClass]
    public class EmployeeSqlDALTests
    {
        private TransactionScope tran;
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=ProjectOrganizer2;Integrated Security = True";
        private int numberOfEmployees = 0;
        private int newEmployeeID = 0;
        private int numOfEmployeesWithoutProjects = 0;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;
                conn.Open();

                //Total Number of employees
                cmd = new SqlCommand("Select COUNT(*) From Employee", conn);
                numberOfEmployees = (int)cmd.ExecuteScalar();

                //select employee using first and last names
                cmd = new SqlCommand("select * from employee where first_name = 'flo' and last_name = 'henderson'", conn);
                newEmployeeID = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand("SELECT count(*) From Employee LEFT JOIN project_employee ON employee.employee_id = project_employee.employee_id Where project_employee.project_id IS NULL", conn);
                numOfEmployeesWithoutProjects = (int)cmd.ExecuteScalar();
                // Single Parameter Constructor", conn);

            }
        }

        // Cleanup Rollback
        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void GetEmployeesTest()
        {
            //Arrange
            EmployeeSqlDAL employeeDAL = new EmployeeSqlDAL(connectionString);

            //Act
            List<Employee> employees = employeeDAL.GetAllEmployees();

            //Assert
            Assert.AreEqual(12, employees.Count);
        }

        [TestMethod]
        public void SearchEmployeesTest()
        {
            //Arrange
            EmployeeSqlDAL employeeDAL = new EmployeeSqlDAL(connectionString);


            //Assert
            Assert.AreEqual(2, newEmployeeID);
        }

        [TestMethod]
        public void EmployeesWithoutProjectsTest()
        {
            //Arrange
            EmployeeSqlDAL employeeDAL = new EmployeeSqlDAL(connectionString);

            List<Employee> employees = employeeDAL.GetEmployeesWithoutProjects();

            //Assert
            Assert.AreEqual(1, numOfEmployeesWithoutProjects);
        }
    }
}
