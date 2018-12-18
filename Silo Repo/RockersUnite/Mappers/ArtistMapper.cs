using RockersUnite.Models;
using RockersUniteBusiness.Interfaces;
using RockersUniteBusiness.Models;
using RockersUniteDataAcccess.Interfaces;
using RockersUniteDataAcccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RockersUnite.Mappers
{
    public class ArtistMapper
    {
        // Maps Artist PO to an Artist DO using the IArtistDO interface.
        public static IArtistDO MapArtistPOtoDO(ArtistPO iArtist)
        {
            IArtistDO oArtist = new ArtistDAO();
            oArtist.ArtistBandID = iArtist.ArtistBandID;
            oArtist.ArtistBandName = iArtist.ArtistBandName;
            oArtist.ArtistBandLocation = iArtist.ArtistBandLocation;
            oArtist.ArtistBandDebutDate = iArtist.ArtistBandDebutDate;
            oArtist.ArtistBandBiography = iArtist.ArtistBandBiography;

            return oArtist;
        }

        // Maps Artist DO to Artist PO using the IArtistDO interface
        public static ArtistPO MapArtistDOtoPO(IArtistDO iArtist)
        {
            var oArtist = new ArtistPO();
            oArtist.ArtistBandID = iArtist.ArtistBandID;
            oArtist.ArtistBandName = iArtist.ArtistBandName;
            oArtist.ArtistBandLocation = iArtist.ArtistBandLocation;
            oArtist.ArtistBandDebutDate = iArtist.ArtistBandDebutDate;
            oArtist.ArtistBandBiography = iArtist.ArtistBandBiography;

            return oArtist;
        }

        // Maps Artist BO to Artist BO using the IArtistDO interface
        public static IArtistBO MapArtistDOtoBO(IArtistDO iArtist)
        {
            IArtistBO oArtist = new ArtistBO();
            oArtist.ArtistBandID = iArtist.ArtistBandID;
            oArtist.ArtistBandName = iArtist.ArtistBandName;
            oArtist.ArtistBandLocation = iArtist.ArtistBandLocation;
            oArtist.ArtistBandDebutDate = iArtist.ArtistBandDebutDate;
            oArtist.ArtistBandBiography = iArtist.ArtistBandBiography;

            return oArtist;
        }

        // Maps Artist BO to Artist PO using the ArtistBO instance
        public static ArtistPO MapArtistBOtoPO(ArtistBO iArtist)
        {
            var oArtist = new ArtistPO();
            oArtist.ArtistBandID = iArtist.ArtistBandID;
            oArtist.ArtistBandName = iArtist.ArtistBandName;
            oArtist.ArtistBandLocation = iArtist.ArtistBandLocation;
            oArtist.ArtistBandDebutDate = iArtist.ArtistBandDebutDate;
            oArtist.ArtistBandBiography = iArtist.ArtistBandBiography;

            return oArtist;
        }

        // Maps list of Artist DO to Artist PO using the IArtistDO interface
        public static List<ArtistPO> MapListOfDOsToListOfPOs(List<IArtistDO> iArtistDOs)
        {
            List<ArtistPO> listOfArtistPOs = new List<ArtistPO>();
            
            // Maps each object in the list
            foreach(IArtistDO artist in iArtistDOs)
            {
                var artistPO = MapArtistDOtoPO(artist);
                listOfArtistPOs.Add(artistPO);
            }

            return listOfArtistPOs;
        }

        // Maps list of Artist DO to Artist BO using IArtistDO interface
        public static List<IArtistBO> MapListOfDOsToListOfBOs(List<IArtistDO> iArtistDOs)
        {
            var listOfArtistBOs = new List<IArtistBO>();

            // Loops to map each object in the list
            foreach(IArtistDO artist in iArtistDOs)
            {
                var artistBO = MapArtistDOtoBO(artist);
                listOfArtistBOs.Add(artistBO);
            }
            return listOfArtistBOs;
        }
    }
}