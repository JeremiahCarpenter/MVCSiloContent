using RockersUnite.Mappers;
using RockersUnite.Models;
using RockersUniteDataAcccess;
using RockersUniteDataAcccess.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RockersUnite.Controllers
{
    public class ArtistController : Controller
    {
        ArtistDataAccess artistDA = new ArtistDataAccess();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        /// <summary>
        ///  Retrieves all of the Artists that are stored within the Site's database
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewAllArtists()
        {
            var ViewAllArtistVM = new ArtistViewModel();
            ActionResult oResponse = null;

            // Ensures both an Authenticated and Non-Registered use can view all of the artists
            if (Session["Email"] != null || Session["Email"] == null)
            {
                try
                {
                    // Calls ViewAllArtists from Data Access layer and stores into allArtists
                    List<IArtistDO> allArtists = artistDA.ViewAllArtists();

                    // Maps from Data Objects to Presentation Objects. Passes allArtists properties for mapping
                    ViewAllArtistVM.ListOfArtistPO = ArtistMapper.MapListOfDOsToListOfPOs(allArtists);

                    oResponse = View(ViewAllArtistVM);
                }
                catch (Exception ex)
                {
                    // Catch exception and show error message to user
                    using (StreamWriter fileWriter = new StreamWriter(@"C:\Course Content\ForgetTheMilk\RockersUnite\Logger\Log.txt"))
                    {
                        fileWriter.WriteLine("{0}-{1}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), ex.Message, true);
                    }
                    ViewAllArtistVM.ErrorMessage = "We apologize, but we were unable to handle your request";

                    oResponse = View(ViewAllArtistVM);
                }
            }
            else
            {
                oResponse = RedirectToAction("Index", "Home");
            }
            return oResponse;
        }

        [HttpGet]
        /// <summary>
        ///  Retrieves the form for creating a new artist
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateArtist()
        {
            ActionResult oResponse = null;

            // Ensures the user is authenticated and must be an Admin/SuperUser to create an artist
            if (Session["Email"] != null && (int)Session["Role"] >= 4)
            {
                var artistVM = new ArtistViewModel();

                oResponse = View(artistVM);
            }
            else
            {
                // User doesn't have access, redirect to home
                oResponse = RedirectToAction("Index", "Home");
            }
            return oResponse;
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        /// <summary>
        ///  Creates and stores the artist submitted to the site
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateArtist(ArtistViewModel iViewModel)
        {
            ActionResult oResponse = null;

            // Ensures the user is authenticated and is an Admin/SuperUser for creating an artist
            if (Session["Email"] != null && (int)Session["Role"] >= 4)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Maps Artist properties from Presentation Objects to Data Objects for creation
                        IArtistDO lArtistForm = ArtistMapper.MapArtistPOtoDO(iViewModel.Artist);
                        
                        // Passes lArtistForm to the AddArtist method
                        artistDA.AddArtist(lArtistForm);
                        oResponse = RedirectToAction("ViewAllArtists", "Artist");
                    }
                    catch (Exception ex)
                    {
                        using (StreamWriter fileWriter = new StreamWriter(@"C:\Course Content\ForgetTheMilk\RockersUnite\Logger\Log.txt"))
                        {
                            fileWriter.WriteLine("{0}-{1}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), ex.Message, true);
                        }
                        iViewModel.ErrorMessage = "We apologize, but are unable to handle request at this time..";

                        // Map data to populate form drop down
                        //PopulateDropDownLists(iViewModel);
                        oResponse = View(iViewModel);
                    }
                }
                else
                {                
                    oResponse = View(iViewModel);
                }
            }
            else
            {
                // User doesn't have access redirect to home
                oResponse = RedirectToAction("Index", "Home");
            }
            return oResponse;
        }

        [HttpGet]
        /// <summary>
        ///  Retrieves the form/data for the selected artist based off of Artist ID
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateArtist(int artistID)
        {
            ActionResult oResponse = null;
            
            // Ensures user is authenticated and is an Admin/SuperUser for Updating Priveleges
            if (Session["Email"] != null && (int)Session["Role"] >= 4)
            {
                var artistVM = new ArtistViewModel();

                // Retrieves an artist by its ID
                IArtistDO artistDO = artistDA.ViewArtistByID(artistID);

                // Maps artistDO from Data Object to Presentation Objects for Updating
                artistVM.Artist = ArtistMapper.MapArtistDOtoPO(artistDO);
                //PopulateDropDownLists(artistVM);

                oResponse = View(artistVM);
            }
            else
            {
                // User doesn't have priveleges to this page redirect to home
                oResponse = RedirectToAction("Index", "Home");
            }
            return oResponse;
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        /// <summary>
        ///  Will modify the changes made to a given artist based off of their Artist ID
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateArtist(ArtistViewModel iViewModel)
        {
            ActionResult oResponse = null;

            // Ensures the user is Authenticated and is an Admin/SuperUser for Updating an Artist
            if (Session["Email"] != null && (int)Session["Role"] >= 4)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Maps Artist from Presenation Object to Data Object
                        IArtistDO lArtistForm = ArtistMapper.MapArtistPOtoDO(iViewModel.Artist);
                        
                        // Passes lArtistForm properties to map From Presentation Objects to Data Objects for Update submission
                        artistDA.UpdateArtistInformation(lArtistForm);
                        oResponse = RedirectToAction("ViewAllArtists", "Artist");
                    }
                    catch (Exception ex)
                    {
                        using (StreamWriter fileWriter = new StreamWriter(@"C:\Course Content\ForgetTheMilk\RockersUnite\Logger\Log.txt"))
                        {
                            fileWriter.WriteLine("{0}-{1}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), ex.Message, true);
                        }
                        iViewModel.ErrorMessage = "Sorry, something went wrong. Please try again.";

                        // Mapp all data to appropriate types to populate drop down
                        //PopulateDropDownLists(iViewModel);
                        oResponse = View(iViewModel);
                    }
                }
                else
                {
                    oResponse = View(iViewModel);
                }
            }
            else
            {
                // User doesn't have privileges to Update an Artist, redirect to home
                oResponse = RedirectToAction("Index", "Home");
            }
            return oResponse;
        }

        [HttpGet]
        [Authorize]
        [ValidateAntiForgeryToken]
        /// <summary>
        ///  Deletes a given artist from the database/site. Only Admin or Higher.
        /// </summary>
        /// <returns></returns>
        public ActionResult RemoveArtist(int artistID)
        {
            ActionResult oResponse = null;

            // Ensure user is Authenticated and is an Admin/SuperUser
            if (Session["Email"] != null && (int)Session["Role"] >= 4)
            {
                try
                {
                    // Removes artist by ID
                    artistDA.RemoveArtistByID(artistID);
                }
                catch (Exception ex)
                {
                    var error = new ArtistViewModel();
                    error.ErrorMessage = "Sorry we are unable to handle your request right now.";

                    using (StreamWriter fileWriter = new StreamWriter(@"C:\Course Content\ForgetTheMilk\RockersUnite\Logger\Log.txt"))
                    {
                        fileWriter.WriteLine("{0}-{1}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), ex.Message, true);
                    }
                }
                finally
                {
                    // Always return to ViewAll to confirm delete
                    oResponse = RedirectToAction("ViewAllArtists", "Artist");
                }
            }
            else
            {
                // User doesn't have privileges to delete artists, redirect to home
                oResponse = RedirectToAction("Index", "Home");
            }
            return oResponse;
        }

        /// <summary>
        ///  Retrieves the details for the selected Artist based off of their Artist ID
        /// </summary>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            var selectedArtist = new ArtistViewModel();
            ActionResult oResponse = null;

            if (ModelState.IsValid)
            {
                try
                {
                    // Stores Artist Details by ID to iArtist
                    IArtistDO iArtist = (IArtistDO)artistDA.ArtistDetails(id);

                    // Maps iArtist from Data Objects to Presentation Objects
                    selectedArtist.Artist = ArtistMapper.MapArtistDOtoPO(iArtist);

                    oResponse = View(selectedArtist);
                }
                catch (Exception ex)
                {
                    // Catch the exception; log it and store in View Model
                    using (StreamWriter fileWriter = new StreamWriter(@"C:\Course Content\ForgetTheMilk\RockersUnite\Logger\Log.txt"))
                    {
                        fileWriter.WriteLine("{0}-{1}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), ex.Message, true);
                    }

                    selectedArtist.ErrorMessage = "We apologize, but we are unable to handle your request at this time.";

                    oResponse = View(selectedArtist);
                }
            }
            else
            {
                oResponse = View(selectedArtist);
            }

            return oResponse;
        }
    }
}
