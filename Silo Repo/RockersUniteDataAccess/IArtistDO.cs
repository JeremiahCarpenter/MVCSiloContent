using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockersUniteDataAcccess.Interfaces
{
    public interface IArtistDO
    {
        int ArtistBandID { get; set; }
        string ArtistBandName { get; set; }
        string ArtistBandLocation { get; set; }
        DateTime ArtistBandDebutDate { get; set; }
        string ArtistBandBiography { get; set; }
    }
}
