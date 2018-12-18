using RockersUnite.Models;
using RockersUniteBusiness.Models;
using RockersUniteDataAcccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RockersUnite.Models
{
    public class ArtistViewModel
    {
        public ArtistViewModel()
        {
            Artist = new ArtistPO();
            ListOfArtistPO = new List<ArtistPO>();
            ListOfArtistDO = new List<ArtistDAO>();
        }

        public ArtistPO Artist { get; set; }
        public List<ArtistPO> ListOfArtistPO { get; set; }
        public List<ArtistDAO> ListOfArtistDO { get; set; }
        public string ErrorMessage { get; set; }
    }
}