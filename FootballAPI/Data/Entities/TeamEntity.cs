using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballAPI.Data.Entities
{
    public class TeamEntity
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string league { get; set; }
        public int position { get; set; }
        public int points { get; set; }
    }
}
