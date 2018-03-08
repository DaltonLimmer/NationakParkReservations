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
    public class ProjectSqlDALTests
    {
        private TransactionScope tran;
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=ProjectOrganizer2;Integrated Security = True";

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;
                conn.Open();

                //Total Number of projects
                cmd = new SqlCommand("Select COUNT(*) From Project", conn);
                cmd.ExecuteReader();

            }
        }

        // Cleanup Rollback
        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void GetProjectsTest()
        {
            //Arrange
            ProjectSqlDAL projectDAL = new ProjectSqlDAL(connectionString);

            //Act
            List<Project> projects = projectDAL.GetAllProjects();

            //Assert
            Assert.AreEqual(7, projects.Count);
        }

        //Assign
        [TestMethod]
        public void AssignEmployeeToProjectTest()
        {
            //Arrange
            ProjectSqlDAL projectDAL = new ProjectSqlDAL(connectionString);

            //Act
            bool wasSuccesful = projectDAL.AssignEmployeeToProject(2, 5);

            //Assert
            Assert.AreEqual(true, wasSuccesful);
        }

        //Remove
        [TestMethod]
        public void RemoveEmployeeFromProjectTest()
        {
            //Arrange
            ProjectSqlDAL projectDAL = new ProjectSqlDAL(connectionString);

            //Act
            bool wasSuccesful = projectDAL.RemoveEmployeeFromProject(1, 3);

            //Assert
            Assert.AreEqual(true, wasSuccesful);
        }

        //Create
        [TestMethod]
        public void CreateProjectTest()
        {
            //Arrange
            ProjectSqlDAL projectDAL = new ProjectSqlDAL(connectionString);
            Project newProj = new Project()
            {
                Name = "TechElevator",
                StartDate = new DateTime(2018,01,29),
                EndDate = new DateTime(2018, 05, 04)
            };


            //Act
            bool wasSuccessful = projectDAL.CreateProject(newProj);

            //Assert
            Assert.AreEqual(true, wasSuccessful);
        }
    }   
}
