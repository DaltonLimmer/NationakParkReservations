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
        string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;
        private const string SQL_GetParks = "select * from park order by name asc";
        private const string SQL_GetParkInfo = "select descript from park order by name asc";

        public Park GetParkInfo(string parkName)
        {
            Park park = new Park();

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

        public Dictionary<int,Campground> GetAllCampgroundsInPark(string parkName)
        {
            Dictionary<int, Campground> list = new Dictionary<int, Campground>();

            return list;

        }

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

        //BONUS: As a user of the system, I would like the ability to see a list
        //of all upcoming reservations within the next 30 days for a selected national park.
        public bool SearchParkForMadeReservations(string parkName)
        {
            return true;
        }

        //Provide an advanced search functionality allowing users to indicate any
        //requirements they have for maximum occupancy, requires wheelchair 
        //accessible site, an rv and its length if required, and if a utility hookup is necessary.
        //public List<Site> AdvancedSearch(int maxOccupancy, bool wheelchairAccessible, bool hasRV, int length, bool utilityHookupRequired)
        //{
        //    List<Site> site = new List<Site>();
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
    }
}