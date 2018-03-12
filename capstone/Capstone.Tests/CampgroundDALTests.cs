using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone.Tests
{
    public class CampgroundDALTests
    {
        private TransactionScope tran;
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Campground;Integrated Security = True";


        /*
        * SETUP
        * Create a fake country ('ABC Country').
        * Add an OfficialLanguage to ABC Country
        * Add an UnofficialLanguage to ABC Country
        */
        [TestInitialize]
        public void Initialize()
        {
            SqlCommand cmd;
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //Insert a Dummy Record for Country                
                cmd = new SqlCommand("INSERT INTO park VALUES ('park', 'india', 2018/03/08, 3333, 222, 'good park');", conn);
                cmd.ExecuteNonQuery();

                //Insert a Dummy Record for City that belongs to 'ABC Country'
                //If we want to the new id of the record inserted we can use
                // SELECT CAST(SCOPE_IDENTITY() as int) as a work-around
                // This will get the newest identity value generated for the record most recently inserted
                //cmd = new SqlCommand("INSERT INTO City VALUES (5000, 'Test City', 'ABC', 'Test District', 1);", conn); // SELECT CAST(SCOPE_IDENTITY() as int);
                //cityId = 5000;// (int)cmd.ExecuteScalar();
                //cmd.ExecuteNonQuery();
            }
        }
        

        /*
        * CLEANUP
        * Rollback the Transaction and get rid of the new records added for the test.        
        */
        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }



        /*
        * TEST:
        * Using ABC Country, validate that there is only one official language.
        */
        [TestMethod()]
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
