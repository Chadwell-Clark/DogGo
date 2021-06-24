using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
    public class WalksRepository : IWalksRepository
    {
        private readonly IConfiguration _config;

        public WalksRepository(IConfiguration config)
        {
            _config = config;
        }
        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        public List<Walks> GetAllWalks()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Date, Duration, WalkerId, DogId FROM Walks";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walks> walks = new List<Walks>();

                    while (reader.Read())
                    {
                        Walks walk = new Walks()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration"))/60,
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId"))
                        };
                        //Walker walker = new()
                        //{

                        //};

                        walks.Add(walk);
                    }

                    reader.Close();

                    return walks;
                }
            }
        }

        public List<Walks> GetWalksByWalkerId(int walkerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT w.Id, w.Duration, w.Date, w.WalkerId, w.DogId, o.Name AS OwnerName
                        FROM Walks w
                        LEFT JOIN Dog d ON w.DogId = d.Id
                        JOIN Owner o ON d.OwnerId = o.Id
                        WHERE w.WalkerId = @walkerId
                        ORDER BY o.Name
                    ";

                    cmd.Parameters.AddWithValue("@WalkerId", walkerId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walks> walks = new List<Walks>();

                    while(reader.Read())
                    {
                        Walks walk = new Walks
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")) / 60,
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                            Owner = new Owner

                            {
                                Name = reader.GetString(reader.GetOrdinal("OwnerName"))

                            }


                        };

                        walks.Add(walk);
                        
                    }
                    reader.Close();
                    return walks;
                    
                }
            }
        }

        public void AddWalks(Walks walks)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            INSERT INTO Walks(Date, Duration, WalkerId, DogId)
                            OUTPUT INSERTED.ID
                            VALUES (@date, @duration, @walkerId, @dogId)";
                    cmd.Parameters.AddWithValue("@date", walks.Date);
                    cmd.Parameters.AddWithValue("@duration", walks.Duration);
                    cmd.Parameters.AddWithValue("@walkerId", walks.WalkerId);
                    cmd.Parameters.AddWithValue("@dogId", walks.DogId);
                    int id = (int)cmd.ExecuteScalar();
                    walks.Id = id;

                }
            
            }
        }
    }
}
