using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FootballAPI.Models
{
    public class TeamModel
    {
        public int Id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string league { get; set; }
        
        [Range(1, 20)]
        [Required]
        public int position { get; set; }

        [Required]
        public int points { get; set; }
    }
}