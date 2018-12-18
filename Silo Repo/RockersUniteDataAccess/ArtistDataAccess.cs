using RockersUniteDataAcccess.Interfaces;
using RockersUniteDataAcccess.Models;
using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockersUniteDataAcccess
{
    public class ArtistDataAccess
    {
        public void AddArtist(IArtistDO iArtist)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RockersUnite"].ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("CreateArtistInformation", conn))
                    {
                        try
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandTimeout = 15;

                            // SqlDbType followed by datatype is whitelisting to tell MVC what to expect for the parameter
                            command.Parameters.AddWithValue("@ArtistBandName", SqlDbType.VarChar).Value = iArtist.ArtistBandName;
                            command.Parameters.AddWithValue("@ArtistBandLocation", SqlDbType.VarChar).Value = iArtist.ArtistBandLocation;
                            command.Parameters.AddWithValue("@ArtistBandDebutDate", SqlDbType.DateTime).Value = iArtist.ArtistBandDebutDate;
                            command.Parameters.AddWithValue("@ArtistBandBiography", SqlDbType.VarChar).Value = iArtist.ArtistBandBiography;

                            conn.Open();
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                        finally
                        {
                            conn.Close();
                            conn.Dispose();
                            command.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log ex
                throw;
            }
            finally
            {
                // Connection/Command already disposed, do nothing
            }
        }

        // Retrieves Artist Information from DB by ID
        public IArtistDO ViewArtistByID(int IArtistID)
        {
            IArtistDO artistDO = new ArtistDAO();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RockersUnite"].ConnectionString))
                {
                    using (SqlCommand viewComm = new SqlCommand("ViewArtistInformation", conn))
                    {
                        viewComm.CommandType = CommandType.StoredProcedure;
                        viewComm.CommandTimeout = 35;
                        viewComm.Parameters.AddWithValue("@ArtistBandID", SqlDbType.Int).Value = IArtistID;

                        conn.Open();

                        using (SqlDataReader reader = viewComm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                artistDO.ArtistBandID = reader.GetInt32(0);
                                artistDO.ArtistBandName = reader.GetString(1);
                                artistDO.ArtistBandLocation = reader.GetString(2);
                                artistDO.ArtistBandDebutDate = reader.GetDateTime(3);
                                artistDO.ArtistBandBiography = reader.GetString(4);

                                var artistBandID = artistDO.ArtistBandID;

                                // Ensures the ArtistBandID is a valid number or throws an exception
                                // This is known as whitelisting; pretty much says what to expect that is trusted
                                if (!int.TryParse(artistBandID.ToString(), out IArtistID))
                                {
                                    throw new ApplicationException("Artist ID was not an integer");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return artistDO;
        }

        public List<IArtistDO> ViewAllArtists()
        {
            var listOfArtistDOs = new List<IArtistDO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RockersUnite"].ConnectionString))
                {
                    using (SqlCommand viewComm = new SqlCommand("ViewAllArtistsInformation", conn))
                    {
                        viewComm.CommandType = CommandType.StoredProcedure;
                        viewComm.CommandTimeout = 35;
                        conn.Open();

                        using (SqlDataReader reader = viewComm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                IArtistDO artistDO = new ArtistDAO();

                                artistDO.ArtistBandID = reader.GetInt32(0);
                                artistDO.ArtistBandName = reader.GetString(1);
                                artistDO.ArtistBandLocation = reader.GetString(2);
                                artistDO.ArtistBandDebutDate = reader.GetDateTime(3);
                                artistDO.ArtistBandBiography = reader.GetString(4);

                                listOfArtistDOs.Add(artistDO);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return listOfArtistDOs;
        }

        public void RemoveArtistByID(int iArtistID)
        {
            try
            {
                using(SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RockersUnite"].ConnectionString))
                {
                    using(SqlCommand deleteComm = new SqlCommand("DeleteArtistInformation", conn))
                    {
                        try
                        {
                            deleteComm.CommandType = CommandType.StoredProcedure;
                            deleteComm.CommandTimeout = 35;
                            deleteComm.Parameters.AddWithValue("@ArtistBandID", SqlDbType.Int).Value = iArtistID;

                            var artistBandID = iArtistID;
                            var id = 0;

                            // Ensures the ID is a valid integer prior to retrieving information based off of ID
                            // This is known as whitelisting; pretty much says what to expect that is trusted
                            if (!int.TryParse(artistBandID.ToString(), out id))
                            {
                                throw new ApplicationException("Artist ID was not an integer");
                            }

                            conn.Open();
                            deleteComm.ExecuteNonQuery();
                        }
                        catch(Exception ex)
                        {
                            throw;
                        }
                        finally
                        {
                            conn.Close();
                            conn.Dispose();
                            deleteComm.Dispose();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            finally
            {
                // Connection/Command already disposed, do nothing
            }
        }

        // Updates the Artist based off the form data
        public void UpdateArtistInformation(IArtistDO iArtist)
        {
            var selectedArtist = new ArtistDAO();
            try
            {
                using(SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RockersUnite"].ConnectionString))
                {
                    using(SqlCommand updateComm = new SqlCommand("UpdateArtistInformation", conn))
                    {
                        try
                        {
                            updateComm.CommandType = CommandType.StoredProcedure;
                            updateComm.CommandTimeout = 35;

                            updateComm.Parameters.AddWithValue("@ArtistBandID", SqlDbType.Int).Value = iArtist.ArtistBandID;
                            updateComm.Parameters.AddWithValue("@ArtistBandName", SqlDbType.VarChar).Value = iArtist.ArtistBandName;
                            updateComm.Parameters.AddWithValue("@ArtistBandLocation", SqlDbType.VarChar).Value = iArtist.ArtistBandLocation;
                            updateComm.Parameters.AddWithValue("@ArtistBandDebutDate", SqlDbType.DateTime).Value = iArtist.ArtistBandDebutDate;
                            updateComm.Parameters.AddWithValue("@ArtistBandBiography", SqlDbType.VarChar).Value = iArtist.ArtistBandBiography;

                            var artistBandID = selectedArtist.ArtistBandID;
                            var id = 0;

                            // Ensures the ArtistBandID is a valid number before executing the request
                            // This is known as whitelisting; pretty much says what to expect that is trusted
                            if (!int.TryParse(artistBandID.ToString(), out id))
                            {
                                throw new ApplicationException("Artist ID was not an integer");
                            }

                            conn.Open();
                            updateComm.ExecuteNonQuery();
                        }
                        catch(Exception ex)
                        {
                            throw;
                        }
                        finally
                        {
                            conn.Close();
                            conn.Dispose();
                            updateComm.Dispose();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public ArtistDAO ArtistDetails(int id)
        {
            var selectedArtist = new ArtistDAO();
            var adapter = new SqlDataAdapter();
            var myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["RockersUnite"].ConnectionString);
            var cmd = new SqlCommand("ViewArtistInformation", myConn);
            
            try
            {                
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ArtistBandID", SqlDbType.Int).Value = id;

                var artistBandID = selectedArtist.ArtistBandID;

                // Ensures the ArtistBandID is a valid number or throws an exception
                // This is known as whitelisting; pretty much says what to expect that is trusted
                if (!int.TryParse(artistBandID.ToString(), out id))
                {
                    throw new ApplicationException("Artist ID was not an integer");
                }

                myConn.Open();
                adapter.SelectCommand = cmd;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        selectedArtist.ArtistBandID = reader.GetInt32(0);
                        selectedArtist.ArtistBandName = reader.GetString(1);
                        selectedArtist.ArtistBandLocation = reader.GetString(2);
                        selectedArtist.ArtistBandDebutDate = reader.GetDateTime(3);
                        selectedArtist.ArtistBandBiography = reader.GetString(4);
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                myConn.Close();
                myConn.Dispose();
                cmd.Dispose();
            }
            return selectedArtist;
        }
    }
}
