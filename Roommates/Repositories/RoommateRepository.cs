using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, FirstName, LastName, RentPortion, MoveInDate
                                        FROM Roommate";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Roommate> roommates = new List<Roommate>();
                        while (reader.Read())
                        {
                            Roommate roommate = new Roommate
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                                MoveInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate"))

                            };
                            roommates.Add( roommate );
                        }
                        return roommates;
                    }
                }
            }
        }

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Roommate.Id, FirstName, LastName, RentPortion, MoveInDate, Name, RoomId
                                        FROM Roommate
                                        LEFT JOIN Room on RoomId = Room.Id
                                        WHERE Roommate.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Roommate roommate = null;
                        if (reader.Read())
                        {
                            roommate = new Roommate
                            {
                                Id = id,
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                                MoveInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                                Room = new Room
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("RoomId")),
                                    Name = reader.GetString(reader.GetOrdinal("Name"))
                                },


                            };
                        }
                        return roommate;
                    }
                }
            }
        }
    }
}