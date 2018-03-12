using System;
using System.Data.SqlClient;
using System.Transactions;
using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Capstone.Tests
{
    [TestClass]
    public class DALTests
    {
        private TransactionScope tran;
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Campground;Integrated Security = True";

        [TestInitialize]
        public void Initialize()
        {
            SqlCommand cmd;
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                //Insert a Dummy Record for Country                
                cmd = new SqlCommand("INSERT INTO park VALUES ('park', 'india', 2018/03/08, 3333, 222, 'good park');", conn);
                cmd.ExecuteNonQuery();
            }

        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void GetParkInfoTest()
        {

            //Arrange
            CampgroundSqlDAL campgroundSqlDAL = new CampgroundSqlDAL();

            //Act
            Park park = campgroundSqlDAL.GetParkInfo("park");

            //Assert
            Assert.AreEqual(3333, park.Area);
        }
    }
}
