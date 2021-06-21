using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {

        private readonly IConfiguration _config;

        public OwnerRepository(IConfiguration config)
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

        public List<Owner> GetAllOwners()
        { 
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT o.Id, o.[Name], o.Email, o.Address, o.Phone, o.NeighborhoodId, n.[Name] As HoodName
                        FROM Owner o
                        LEFT JOIN Neighborhood n ON o.NeighborhoodId = n.Id
                    ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Owner> owners = new List<Owner>();
                    while (reader.Read())
                    {
                        Owner owner = new Owner
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            Phone = reader.GetString(reader.GetOrdinal("Phone")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                        };
                        Neighborhood neighborhood = new Neighborhood
                        {
                            Id = owner.NeighborhoodId,
                            Name = reader.GetString(reader.GetOrdinal("HoodName"))

                        };


                        owner.Neighborhood = neighborhood;
                        owners.Add(owner);
                    }

                    reader.Close();

                    return owners;
                }
            }
        
        }

        public Owner GetOwnerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT o.Id, o.[Name], o.Email, o.Address, o.Phone, o.NeighborhoodId, n.[Name] As HoodName, d.Name as DogName
                        FROM Owner o
                        LEFT JOIN Neighborhood n ON o.NeighborhoodId = n.Id
                        LEFT JOIN Dog d ON o.id = d.OwnerId
                        WHERE o.Id = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Owner owner = new Owner
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            Phone = reader.GetString(reader.GetOrdinal("Phone")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                        };
                        Neighborhood neighborhood = new Neighborhood
                        {
                            Id = owner.NeighborhoodId,
                            Name = reader.GetString(reader.GetOrdinal("HoodName"))
                        };
                       
                        owner.Neighborhood = neighborhood;


                        reader.Close();
                        return owner;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }
        }

    }
}
