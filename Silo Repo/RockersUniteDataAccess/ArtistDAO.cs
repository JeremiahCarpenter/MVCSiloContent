using RockersUniteDataAcccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockersUniteDataAcccess.Models
{
    public class ArtistDAO : IArtistDO
    {
        public int ArtistBandID { get; set; }
        public string ArtistBandName { get; set; }
        public string ArtistBandLocation { get; set; }
        public DateTime ArtistBandDebutDate { get; set; }
        public string ArtistBandBiography { get; set; }
    }
}
