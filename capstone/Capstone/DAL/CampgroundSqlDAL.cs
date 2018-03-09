using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Capstone.Models;
using System.Configuration;

namespace Capstone.DAL
{

    public class CampgroundSqlDAL
    {
        //string connectionString = ConfigurationManager.ConnectionStrings["Campground"].ConnectionString;
        private const string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Campground;Integrated Security = True";
        private const string SQL_GetParks = "select * from park order by name asc";
        private const string SQL_GetParkInfo = "select name, location, establish_date, area, visitors, description from park";
        private const string SQL_GetCamgroundByPark = "SELECT * FROM campground JOIN park ON park.park_id = campground.park_id WHERE  park.name = @parkName;";

        public Park GetParkInfo(string parkName)
        {
            Park park = new Park();

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand(SQL_GetParks, conn);

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            park = GetParksFromReader(reader);
                        }

                    }
                }
                catch (SqlException)
                {

                    throw;
                }

                return park;
        }

        
        public List<Park> GetAllParksAlphabetically()
        {
            List<Park> parks = new List<Park>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetParks, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Park item = GetParksFromReader(reader);
                        parks.Add(item);
                    }

                }
            }
            catch (SqlException)
            {

                throw;
            }

            return parks;

        }

        //As a user of the system, I need the ability to select a park that
        //my customer is visiting and see a list of all campgrounds for that available park.
        public Dictionary<int, Campground> GetAllCampgroundsInPark(string parkName)
        {
            Dictionary<int, Campground> campgrounds = new Dictionary<int, Campground>();
            int count = 1;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetCamgroundByPark, conn);
                    cmd.Parameters.AddWithValue("@parkname", parkName);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Campground item = GetCampgroundFromReader(reader);
                        campgrounds.Add(count, item);
                        count++;
                    }

                }
            }
            catch (SqlException)
            {

                throw;
            }

            return campgrounds;

        }

        //As a user of the system, I need the ability to select a campground and
        //search for date availability so that I can make a reservation.
        public bool CampgroundAvailability(string campground, DateTime startDate, DateTime endDate)
        {

            return true;
        }

        public bool BookReservation(string personName, DateTime startDate, DateTime endDate)
        {
            return true;
        }

        //As a user of the system, I want the ability to select a park and search for campsite
        //availability across the entire park so that I can make a reservation.
        public bool SearchParkForAvailability(string parkName, DateTime startDate, DateTime endDate)
        {
            return true;
        }

        ////BONUS: As a user of the system, I would like the ability to see a list
        ////of all upcoming reservations within the next 30 days for a selected national park.
        //public bool SearchParkForMadeReservations(string parkName)
        //{
            
        //}

        ////Provide an advanced search functionality allowing users to indicate any
        ////requirements they have for maximum occupancy, requires wheelchair 
        ////accessible site, an rv and its length if required, and if a utility hookup is necessary.
        //public List<Site> AdvancedSearch(int maxOccupancy, bool wheelchairAccessible, bool hasRV, int length, bool utilityHookupRequired)
        //{
            
        //}

        private Park GetParksFromReader(SqlDataReader reader)
        {
            Park item = new Park
            {
                ParkID = Convert.ToInt32(reader["park_id"]),
                Name = Convert.ToString(reader["name"]),
                Location = Convert.ToString(reader["location"]),
                Establish_Date = Convert.ToDateTime(reader["establish_date"]),
                Area = Convert.ToString(reader["area"]),
                Visitors = Convert.ToInt32(reader["visitors"]),
                Description = Convert.ToString(reader["description"]),

            };

            return item;

        }

        private Campground GetCampgroundFromReader(SqlDataReader reader)
        {
            Campground item = new Campground
            {
                CampgroundId = Convert.ToInt32(reader["campground_id"]),
                ParkId = Convert.ToInt32(reader["park_id"]),
                Name = Convert.ToString(reader["name"]),
                Open_From_MM = Convert.ToInt32(reader["open_from_mm"]),
                Open_To_MM = Convert.ToInt32(reader["open_to_mm"]),
                Daily_Fee = Convert.ToDouble(reader["daily_fee"]),

            };

            return item;

        }
    }
}