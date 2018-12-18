using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RockersUnite.Models
{
    public class ArtistPO
    {
        public int ArtistBandID { get; set; }
        [DisplayName("Name: ")]
        [Required(ErrorMessage ="Artist/Band name required to process request!")]
        public string ArtistBandName { get; set; }
        [DisplayName("Location: ")]
        [Required(ErrorMessage = "Artist/Band location required to process request!")]
        public string ArtistBandLocation { get; set; }
        [DisplayName("Debut Album: ")]
        [Required(ErrorMessage = "Artist/Band name required to process request!")]
        public DateTime ArtistBandDebutDate { get; set; }
        [DisplayName("Biography: ")]
        [Required(ErrorMessage = "Artist/Band name required to process request!")]
        public string ArtistBandBiography { get; set; }
    }
}