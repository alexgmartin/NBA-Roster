using Prac4.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Prac4.Controllers
{

    public class PlayerController : ApiController
    {
        List<Player> players = new List<Player>();

        public IHttpActionResult getAllPlayers()
        {
            string line;

            System.IO.StreamReader file = new System.IO.StreamReader(@"H:\\players.txt");
            while ((line = file.ReadLine()) != null)
            {
                String[] playerInfo = line.Split(',');
                String[] dobsplit = playerInfo[3].Split('-');

                int year = Int32.Parse(dobsplit[0]);
                int month = Int32.Parse(dobsplit[1]);
                int day = Int32.Parse(dobsplit[2]);

                DateTime dob = new DateTime(year, month, day);
  
                Player player = new Player { Registration_ID = playerInfo[0], Player_name = playerInfo[1], Team_name = playerInfo[2], Date_of_birth = dob };
                players.Add(player);
            }

            file.Close();

            return Ok(players);
        }

        /**
         * Searches for the player
         * */
        [HttpGet]
        [Route("api/player/{field}/{value}")]
       // [ResponseType(typeof(UserItem))]
        public IHttpActionResult searchPlayer(string field, string value)
        {
            getAllPlayers();
            List<Player> matchedPlayers = new List<Player>();

            if (field == null || value == null)
            {
                return NotFound();
            }

            switch (field.ToLower())
                {
                    case "id":
                         searchID(matchedPlayers, value);
                        break;
                    case "name":
                         searchName(matchedPlayers, value);
                         break;
                    case "team":
                         searchTeam(matchedPlayers, value);
                         break;
                    case "dob":
                         searchDOB(matchedPlayers, value);
                         break;
                default:
                    return NotFound();
                }

            if (matchedPlayers == null)
            {
                return NotFound();
            }

            return Ok(matchedPlayers);
        }


        public void searchID(List<Player> matchedPlayers, string value)
        {

            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Registration_ID == value)
                {
                    matchedPlayers.Add(players[i]);
                }
            }
        }

        public void searchName(List<Player> matchedPlayers, string value)
        {

            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Player_name.ToLower().Contains(value))
                {
                    matchedPlayers.Add(players[i]);
                }
            }
        }

        public void searchTeam(List<Player> matchedPlayers, string value)
        {

            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Team_name.Contains(value))
                {
                    matchedPlayers.Add(players[i]);
                }
            }
        }

        public void searchDOB(List<Player> matchedPlayers, string value)
        {
            //DD/MM/YYYY
            value = value.Replace('-', '/');
            value = reverse(value);
            for (int i = 0; i < players.Count; i++)
            {
              
                if (players[i].Date_of_birth.ToString().Contains(value))
                {
                    matchedPlayers.Add(players[i]);
                }
            }
        }

        //helper string reverse function
        public static string reverse(string str)
        {
            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }


        [HttpDelete]
        [Route("api/player/delete/{field}/{value}")]
        // [ResponseType(typeof(UserItem))]
        public IHttpActionResult deletePlayer(string field, string value)
        {
            getAllPlayers();

            if (field == null || value == null)
            {
                return NotFound();
            }

            switch (field.ToLower())
            {
                case "id":
                    deleteID(value);
                    break;
                case "name":
                    deleteName(value);
                    break;
                case "team":
                    deleteTeam(value);
                    break;
                case "dob":
                    deleteDOB( value);
                    break;
                default:
                    return NotFound();
            }

            if (players == null)
            {
                return NotFound();
            }

            using (StreamWriter writer = new StreamWriter(@"H:\\players.txt", false))
            {
                for(int i=0; i < players.Count; i++) {
                    string year = players[i].Date_of_birth.Year.ToString();
                    string mon = players[i].Date_of_birth.Month.ToString();
                    string day = players[i].Date_of_birth.Day.ToString();
                     writer.WriteLine(players[i].Registration_ID +","+ players[i].Player_name + "," + players[i].Team_name + "," + year+"-"+mon+"-"+day);
                }
            }
            
            return Ok(players);
        }


        public void deleteID(string value)
        {

            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Registration_ID == value)
                {
                    players.Remove(players[i]);
                }
            }
        }

        public void deleteName(string value)
        {

            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Player_name.ToLower().Contains(value))
                {
                    players.Remove(players[i]);
                }
            }
        }

        public void deleteTeam(string value)
        {

            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Team_name.ToLower().Contains(value))
                {
                    players.Remove(players[i]);
                }
            }
        }

        public void deleteDOB(string value)
        {
            //DD/MM/YYYY
    
        String[] dateParsed = value.Split('-');
        String year = dateParsed[0];
        String mon = dateParsed[1];
        String day = dateParsed[2];
        String date = day + "/" + mon + "/" + year;

            for (int i = 0; i < players.Count; i++)
            {

                if (players[i].Date_of_birth.ToString().Contains(date))
                {
                    players.Remove(players[i]);
                }
            }
        }


   
        [HttpPost]
        [Route("api/player/register/{registerId}/{registerFirstName}/{registerLastName}/{registerTeamName}/{registerDOB}")]
        public IHttpActionResult registerPlayer(string registerId, string registerFirstName, string registerLastName,
           string registerTeamName, string registerDOB)
        {

            getAllPlayers();

            if (registerId == null || registerFirstName == null || registerLastName == null || registerTeamName == null || registerDOB == null) {
                return NotFound();
            }

            String[] dateParsed = registerDOB.Split('-');
            int year = Int32.Parse(dateParsed[0]);
            int mon = Int32.Parse(dateParsed[1]);
            int day = Int32.Parse(dateParsed[2]);
            DateTime dob = new DateTime(year, mon, day);
            
            Player player = new Player { Registration_ID = registerId, Player_name = registerFirstName+ " " + registerLastName, Team_name = registerTeamName, Date_of_birth = dob };

            if (!(players.Contains(player))) {

                players.Add(player);

                using (StreamWriter writer = File.AppendText(@"H:\\players.txt"))
                {
                    writer.WriteLine(player.Registration_ID + "," + player.Player_name + "," + player.Team_name + "," + player.Date_of_birth.Year + "-" +
                      player.Date_of_birth.Month + "-" + player.Date_of_birth.Day);
                }


            } else {
                             
                string[] arrLine = File.ReadAllLines((@"H:\\players.txt"));

                for (int i = 0; i < arrLine.Length; i++)
                {
                    if(arrLine[i].Contains(player.Registration_ID)) {
                        arrLine[i] = player.Registration_ID + "," + player.Player_name + "," + player.Team_name + "," + player.Date_of_birth.Year + "-" +
                        player.Date_of_birth.Month + "-" + player.Date_of_birth.Day;
                   
                    }

                }

                File.WriteAllLines(@"H:\\players.txt", arrLine);
            }

            return Ok(players);
        }
    }
}
