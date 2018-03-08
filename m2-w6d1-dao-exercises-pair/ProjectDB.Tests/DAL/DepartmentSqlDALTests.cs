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
    public class DepartmentSqlDALTests
    {
        private TransactionScope tran;
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=ProjectOrganizer2;Integrated Security = True";
        private int numberOfDepartments = 0;
        private int newDaprtmentID = 0;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;
                conn.Open();

                //Total Number of departments
                cmd = new SqlCommand("Select COUNT(*) From Department", conn);
                numberOfDepartments = (int)cmd.ExecuteScalar();

                //Insert Department
                cmd = new SqlCommand("Insert Into Department(name) Values('Sofa'); Select department_id FROM department Where name='sofa'", conn);
                newDaprtmentID = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand("Update Department Set name = 'blah' WHERE name = 'Research And Development'", conn);
                cmd.ExecuteNonQuery();
            }
        }


        [TestMethod]
        public void GetDepartmentsTest()
        {
            //Arrange
            DepartmentSqlDAL departmentDAL = new DepartmentSqlDAL(connectionString);

            //Act
            List<Department> departments = departmentDAL.GetDepartments();

            //Assert
            Assert.AreEqual(7, departments.Count, "Insert: 5 initial departments. 1 Insert in the Initialize");
        }

        [TestMethod]
        public void CreateDepartmentTest()
        {
            DepartmentSqlDAL departmentDAL = new DepartmentSqlDAL(connectionString);

            //Act
            Department newDepartment = new Department()
            {
                Name = "Xbox"
            };

            bool wasSuccessful = departmentDAL.CreateDepartment(newDepartment);

            //Assert
            Assert.AreEqual(true, wasSuccessful);
        }

        [TestMethod]
        public void UpdateDepartmentTest()
        {
            //Arrange
            DepartmentSqlDAL departmentDAL = new DepartmentSqlDAL(connectionString);

            Department updatedDepartment = new Department()
            {
                Id = newDaprtmentID,
                Name = "Playsation"
            };

            bool wasSuccessful = departmentDAL.UpdateDepartment(updatedDepartment);

            //Assert
            Assert.AreEqual(true, wasSuccessful);
        }

        // Cleanup Rollback
        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }
    }
}
