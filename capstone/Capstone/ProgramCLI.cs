using Capstone;
using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class ProgramCLI
    {
        private const string command_Cancel = "0";
        private const string command_SelectAcadia = "1";
        private const string command_SelectArches = "2";
        private const string command_SelectCuyahoga = "3";
        private const string command_Quit = "q";
        private const string command_ViewCampgrounds = "1";
        private const string command_SearchReservations = "2";
        private const string command_ReturnToPreviousScreen = "3";
        private const string command_SearchAvailableReservatgions = "1";
        private const string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Campground;Integrated Security = True";
        private Dictionary<int, string> Parks = new Dictionary<int, string>()
        {
            {1, "Acadia" },
            {2, "Arches" },
            {3, "Cuyahoga National Valley" }
        };
        



        public void RunCLI()
        {
            PrintMainMenu();

            while (true)
            {
                ConsoleKeyInfo userInput = Console.ReadKey();
                string command = userInput.ToString();

                switch (command)
                {
                    case command_SelectAcadia:
                        int park = int.Parse(command);
                        GetParkInfo(park);
                        break;
                    case command_SelectArches:
                        park = int.Parse(command);
                        GetParkInfo(park);
                        break;
                    case command_SelectCuyahoga:
                        park = int.Parse(command);
                        GetParkInfo(park);
                        break;
                    case command_Quit:
                        park = int.Parse(command);
                        GetParkInfo(park);
                        return;
                    default:
                        Console.WriteLine("The command provided was not a valid command, please try again.");
                        break;
                }
            }
        }

        //Menus
        private void PrintMainMenu()
        {
            Console.WriteLine("Select a Park for further Details");
            Console.WriteLine(" 1) Acadia");
            Console.WriteLine(" 2) Arches");
            Console.WriteLine(" 3) Cuyahoga NAtional Valley Park");
            Console.WriteLine();

            Console.WriteLine(" Q) Quit");
            Console.WriteLine();

        }

        private void PrintParkInfoMenu()
        {
            Console.WriteLine("Select a Park for further Details");
            Console.WriteLine(" 1) View Campgrounds");
            Console.WriteLine(" 2) Search for Reservations");
            Console.WriteLine(" 3) Return to Previous Screen");
            Console.WriteLine();

        }

        private void PrintCampgroundMenu()
        {
            Console.WriteLine("Select a Park for further Details");
            Console.WriteLine(" 1) Search for Available Reservation");
            Console.WriteLine(" 2) Return to Previous Screen");
            Console.WriteLine();

        }

        // Search
        private void GetAllParks()
        {
            CampgroundSqlDAL campgroundDAL = new CampgroundSqlDAL();

        }

        private void GetParkWithCampgrounds()
        {
            CampgroundSqlDAL campgroundDAL = new CampgroundSqlDAL();

        }

        //Get Park Info
        private void GetParkInfo(int parkDictionaryKey)
        {
            CampgroundSqlDAL campgroundDAL = new CampgroundSqlDAL();
            Park park = campgroundDAL.GetParkInfo(Parks[parkDictionaryKey]);


        }

        //Availabilities
        private void GetParkWithAvailableDates()
        {

        }

        private void GetParkCampsiteAdvancedSearch()
        {

        }

        private void SearchParkForAvailability()
        {

        }

        //Booking
        private void BookReservation()
        {

        }

        private void SearchParkForMadeReservations()
        {

        }

    }
}
