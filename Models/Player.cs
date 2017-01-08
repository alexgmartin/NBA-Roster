using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prac4.Models
{
    public class Player
    {

        public String Registration_ID { get; set; }
        public String Player_name { get; set; }
        public String Team_name { get; set; }
        public DateTime Date_of_birth { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as Player;

            if (item == null)
            {
                return false;
            }

            //assuming registration ID is unique
            return this.Registration_ID.Equals(item.Registration_ID);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode();
        }
    }
    

}