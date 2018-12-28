namespace RunescapeMVC.Controllers
{
    using RunescapeMVC.Custom;
    using RunescapeMVC.Models;
    using RunescapeMVC.ViewModels;
    using RunescapeMVC_DAL;
    using RunescapeMVC_BLL;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.IO;

    public class AreasController : Controller
    {
        //instantiate Data access layers
        AreasDataAccess AreaData = new AreasDataAccess();
        BeastDataAccess BeastData = new BeastDataAccess();
        AreaBusinessLogic AreaLogic = new AreaBusinessLogic();

        // GET: Areas
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateArea()
        {
            ActionResult oResponse = null;

            //check session for poweruser or higher before allowing access
            if (Session["UserName"] != null && (int)Session["Role"] != 1)
            {
                //instantiate viewmodel to use in form
                AreasInfoVM areaInfoVM = new AreasInfoVM();
                oResponse = View(areaInfoVM);
            }
            else
            {
                //redirect to homepage if role is 1 or guest
                oResponse = RedirectToAction("Index", "Home");
            }
            return oResponse;
        }

        //method for creating a new Area
        [HttpPost]
        public ActionResult CreateArea(AreasInfoVM iViewModel)
        {
            ActionResult oResponse = null;

            //verify high enough Access level
            if (Session["UserName"] != null && (int)Session["Role"] != 1)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        //map data from PO to DO
                        IAreaInfoDO lAreaForm = Map.MapAreaPOtoDO(iViewModel.Area);

                        //call DataAccess method for inserting new row, passing the completed forms values
                        AreaData.InsertArea(lAreaForm);
                    }
                    catch (Exception ex)
                    {
                        //catch, log, store error message
                        using (StreamWriter fileWriter = new StreamWriter(@"C:\Users\Lukus\Documents\BootCamp\Projects\Visual Studio\RunescapeMVC\ErrorLogs\Logs.txt"))
                        {
                            fileWriter.WriteLine("{0}-{1}",
                                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), ex.Message, true);
                        }
                        iViewModel.ErrorMessage = "We apologize, but we were unable to handle your request.";
                    }
                    finally
                    {
                        //If null, then there was no error
                        if (iViewModel.ErrorMessage == null)
                        {
                            //Display areas if there was no error
                            oResponse = RedirectToAction("ViewAllAreas", "Areas");
                        }
                        //If not null, there WAS an error
                        else
                        {
                            //if error occurred, redirect back to form, keeping users data input 
                            oResponse = View(iViewModel);
                        }
                    }
                }
                else
                {
                    oResponse = View(iViewModel);
                }
            }
            else
            {
                //redirect to homepage if role is 1 or guest
                oResponse = RedirectToAction("Index", "Home");
            }
            return oResponse;
        }

        //method to Display entire areas table
        [HttpGet]
        public ActionResult ViewAllAreas()
        {
            AreasInfoVM AreasVM = new AreasInfoVM();
            ActionResult oResponse = null;

            //verify user role
            if (Session["UserName"] != null && (int)Session["Role"] >= 1)
            {
                try
                {
                    //map allneeded data to appropriate datatypes to perform BeastsInArea calculation
                    List<IAreaInfoDO> allAreasDO = AreaData.ViewAllAreas();

                    List<IBeastDO> allBeastsDO = BeastData.ViewAllBeasts();

                    List<IBeastBO> allBeastsBO = BeastMap.MapListOfDOsToListOfBOs(allBeastsDO);

                    List<IAreaInfoBO> allAreasBO = Map.MapListOfDOsToListOfBOs(allAreasDO);
                                        
                    List<IAreaInfoBO> areas = AreaLogic.CalculateBeastsInArea(allBeastsBO, allAreasBO);

                    List<AreaInfoPO> listOFAreaPOs = Map.MapListOfBOsToListOfPOs(areas);

                    AreasVM.ListOfAreasPO = listOFAreaPOs;
                    oResponse = View(AreasVM);
                }
                catch (Exception ex)
                {
                    //Catch the exception and show error message to user
                    using (StreamWriter fileWriter = new StreamWriter(@"C:\Users\Lukus\Documents\BootCamp\Projects\Visual Studio\RunescapeMVC\ErrorLogs\Logs.txt"))
                    {
                        fileWriter.WriteLine("{0}-{1}",
                            DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), ex.Message, true);
                    }

                    AreasVM.ErrorMessage = "We apologize, but we were unable to handle your request.";
                    oResponse = View(AreasVM);
                }
            }
            else
            {
                //redirect to home if invalid user
                oResponse = RedirectToAction("Index", "Home");
            }

            return oResponse;
        }

        //method to delete area if users role is high enough
        [HttpGet]
        public ActionResult DeleteArea(long areaID)
        {
            ActionResult oResponse = null;

            if (Session["UserName"] != null && (int)Session["Role"] != 1)
            {
                try
                {
                    AreaData.DeleteArea(areaID);
                }
                catch (Exception ex)
                {
                    AreasInfoVM areaVM = new AreasInfoVM();
                    areaVM.ErrorMessage = "Sorry, something went wrong. Please try again."; ;
                    //log error message to file
                    using (StreamWriter fileWriter = new StreamWriter(@"C:\Users\Lukus\Documents\BootCamp\Projects\Visual Studio\RunescapeMVC\ErrorLogs\Logs.txt"))
                    {
                        fileWriter.WriteLine("{0}-{1}",
                            DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), ex.Message, true);
                    }
                }
                finally
                {
                    //always redirect to viewall areas to confirm delete
                    oResponse = RedirectToAction("ViewAllAreas", "Areas");
                }
            }
            else
            {
                oResponse = RedirectToAction("Index", "Home");
            }
            return oResponse;
        }

        //method to  updateArea 
        [HttpGet]
        public ActionResult UpdateArea(long areaID)
        {
            ActionResult oResponse = null;

            //verifiy user role to be high enough
            if (Session["UserName"] != null && (int)Session["Role"] != 1)
            {
                AreasInfoVM areaInfoVM = new AreasInfoVM();
                IAreaInfoDO areaDO = AreaData.ViewAreaByID(areaID);
                areaInfoVM.Area = Map.MapAreaDOtoPO(areaDO);
                oResponse = View(areaInfoVM);
            }
            else
            {
                oResponse = RedirectToAction("Index", "Home");
            }
            return oResponse;
        }

        //method to  updateArea 
        [HttpPost]
        public ActionResult UpdateArea(AreasInfoVM iViewModel)
        {
            ActionResult oResponse = null;

            if (Session["UserName"] != null && (int)Session["Role"] != 1)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        IAreaInfoDO lAreaForm = Map.MapAreaPOtoDO(iViewModel.Area);

                        //call method from DAL to update database
                        AreaData.UpdateArea(lAreaForm);

                        oResponse = RedirectToAction("ViewAllAreas", "Areas");
                    }
                    catch (Exception ex)
                    {
                        //write ex.message to log file
                        using (StreamWriter fileWriter = new StreamWriter(@"C:\Users\Lukus\Documents\BootCamp\Projects\Visual Studio\RunescapeMVC\ErrorLogs\Logs.txt"))
                        {
                            fileWriter.WriteLine("{0}-{1}",
                                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), ex.Message, true);
                        }

                        iViewModel.ErrorMessage = "Sorry, something went wrong. Please try again.";

                        oResponse = View(iViewModel);
                    }
                    finally
                    {
                    }
                }
                else
                {
                    oResponse = View(iViewModel);
                }
            }
            else
            {
                oResponse = RedirectToAction("Index", "Home");
            }
            return oResponse;
        }
    }
}