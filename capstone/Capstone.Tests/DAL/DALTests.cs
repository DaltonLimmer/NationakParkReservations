using System;
using System.Collections.Generic;
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
                cmd = new SqlCommand("INSERT INTO park VALUES ('park', 'india', '2018-03-08', 3333, 222, 'good park');", conn);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("INSERT INTO campground VALUES (2, 'gramercy', 5, 8, 30.0);", conn);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("INSERT INTO site VALUES (7, 1, 7, 1, 0, 1);", conn);
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

        [TestMethod]
        public void GetCampgroundInfoTest()
        {

            //Arrange
            CampgroundSqlDAL campgroundSqlDAL = new CampgroundSqlDAL();

            //Act
            Campground campground = campgroundSqlDAL.GetCampgroundInfo("gramercy");

            //Assert
            Assert.AreEqual(30.0, campground.Daily_Fee);
        }

        [TestMethod]
        public void GetAllParksAlphabeticallyTest()
        {

            //Arrange
            CampgroundSqlDAL campgroundSqlDAL = new CampgroundSqlDAL();

            //Act
            Dictionary<int, Park> parks = campgroundSqlDAL.GetAllParksAlphabetically();

            //Assert
            Assert.AreEqual(4, parks.Count);
            Assert.AreEqual(3333, parks[4].Area);
        }

        [TestMethod]
        public void GetAllCampgroundsInParkTest()
        {

            //Arrange
            CampgroundSqlDAL campgroundSqlDAL = new CampgroundSqlDAL();

            //Act
            Dictionary<int, Campground> campgrounds = campgroundSqlDAL.GetAllCampgroundsInPark("Arches");

            //Assert
            Assert.AreEqual(4, campgrounds.Count);
            Assert.AreEqual(5, campgrounds[4].Open_From_MM);
        }

        [TestMethod]
        public void GetCampgroundAvailabilityTest()
        {

            //Arrange
            CampgroundSqlDAL campgroundSqlDAL = new CampgroundSqlDAL();
            DateTime dt1 = new DateTime(2018, 3, 3, 0, 0, 0);
            DateTime dt2 = new DateTime(2018, 3, 5, 0, 0, 0);

            //Act
            List<Site> sites = campgroundSqlDAL.GetCampgroundAvailability("Blackwoods", dt1, dt2);

            //Assert
            Assert.AreEqual(5, sites.Count);
        }

        //[TestMethod]
        //public void BookReservationTest()
        //{

        //    //Arrange
        //    CampgroundSqlDAL campgroundSqlDAL = new CampgroundSqlDAL();
        //    DateTime dt1 = new DateTime(2018, 3, 3, 0, 0, 0);
        //    DateTime dt2 = new DateTime(2018, 3, 5, 0, 0, 0);

        //    //Act
        //    int confirmation = campgroundSqlDAL.BookReservation("Dalton", 43, dt1, dt2);

        //    //Assert
        //    Assert.AreEqual(50, confirmation);
        //}

        [TestMethod]
        public void GetParkAvailabilityTest()
        {

            //Arrange
            CampgroundSqlDAL campgroundSqlDAL = new CampgroundSqlDAL();
            DateTime dt1 = new DateTime(2018, 3, 3, 0, 0, 0);
            DateTime dt2 = new DateTime(2018, 3, 5, 0, 0, 0);

            //Act
            List<Site> sites = campgroundSqlDAL.GetParkAvailability("Acadia", dt1, dt2);

            //Assert
            Assert.AreEqual(5, sites.Count);
        }

        [TestMethod]
        public void SearchParkForReservationTest()
        {

            //Arrange
            CampgroundSqlDAL campgroundSqlDAL = new CampgroundSqlDAL();
            DateTime dt1 = new DateTime(2018, 3, 3, 0, 0, 0);
            DateTime dt2 = new DateTime(2018, 3, 5, 0, 0, 0);

            //Act
            List<Site> sites = campgroundSqlDAL.SearchParkForMadeReservations("Acadia");

            //Assert
            Assert.AreEqual(12, sites.Count);
        }

        [TestMethod]
        public void AdvancedSearchTest()
        {

            //Arrange
            CampgroundSqlDAL campgroundSqlDAL = new CampgroundSqlDAL();
            DateTime dt1 = new DateTime(2018, 3, 3, 0, 0, 0);
            DateTime dt2 = new DateTime(2018, 3, 5, 0, 0, 0);

            //Act
            List<Site> sites = campgroundSqlDAL.GetCampgroundAvailabilityAdvancedSearch("Blackwoods", dt1, dt2, 5, true, 0, true);

            //Assert
            Assert.AreEqual(2, sites.Count);
        }

    }
}
