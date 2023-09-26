using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballRanking
{
    public class Team
    {
        private string name;
        private int points;

        public Team(string name, int points)
        {
            this.name = name;
            this.points = points;
        }
    }
}
