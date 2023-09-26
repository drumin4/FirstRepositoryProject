using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballRanking
{
    public class Ranking
    {
        private Team[] teams;
        private int numTeam = 0;

        public void AddTeam(Team team)
        {
            teams[numTeam++] = team;
        }

        public int ReportPositionOfTeam(string teamName)
        {
            
        }

        public void UpdateRanking(string winningTeam, string losingTeam)
        {
            
        }

        private void SorteazaEchipeDupaPunctaj()
        {
            for (int i = 0; i < numTeam - 1; i++)
            {
                for (int j = i + 1; j < numTeam; j++)
                {
                    if (teams[i]. < teams[j].Punctaj)
                    {
                        var temp = teams[i];
                        teams[i] = teams[j];
                        teams[j] = temp;
                    }
                }
            }
        }
    }
}
