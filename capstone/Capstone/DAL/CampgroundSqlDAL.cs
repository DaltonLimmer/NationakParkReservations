﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone.DAL
{
    public class CampgroundSqlDAL
    {
        public string connectionString;
        private const string SQL_GetTeams = "SELECT * FROM Park";

        public List<Park> GetAllParksAlphabetically()
        {
            List<Park> parks = new List<Park>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetTeams, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Team item = GetTeamsFromReader(reader);
                        teams.Add(item);
                    }

                }
            }
            catch (SqlException)
            {

                throw;
            }

            return teams;

        }

        public List<Campground> GetParkWithAvailableCampgrounds(string parkName)
        {

        }

        public bool CampgroundAvailability(string campground, DateTime startDate, DateTime endDate)
        {

        }

        public bool BookReservation(string personName, DateTime startDate, DateTime endDate)
        {

        }

        //As a user of the system, I want the ability to select a park and search for campsite
        //availability across the entire park so that I can make a reservation.
        public bool SearchParkForAvailability(string parkName, DateTime startDate, DateTime endDate)
        {

        }

        //BONUS: As a user of the system, I would like the ability to see a list
        //of all upcoming reservations within the next 30 days for a selected national park.
        public bool SearchParkForMadeReservations(string parkName)
        {

        }

        //Provide an advanced search functionality allowing users to indicate any
        //requirements they have for maximum occupancy, requires wheelchair 
        //accessible site, an rv and its length if required, and if a utility hookup is necessary.
        public List<Site> AdvancedSearch(int maxOccupancy, bool wheelchairAccessible, bool hasRV, int length, bool utilityHookupRequired)
        {

        }

    }
}
