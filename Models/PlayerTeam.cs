using System.ComponentModel.DataAnnotations;

namespace SportsORM.Models
{
    public class PlayerTeam
    {
        [Key]
        public int PlayerTeamId {get;set;}
        public int PlayerId {get;set;}
        public Player PastPlayerOnTeam {get;set;}
        public int TeamId {get;set;}
        public Team PastTeamOfPlayer {get;set;}
    }
}