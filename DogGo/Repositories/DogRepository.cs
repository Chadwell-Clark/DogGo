using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
    public class DogRepository : IDogRepository
    {
        private readonly IConfiguration _config;
        public DogRepository(IConfiguration config)
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

        public List<Dog> GetAllDogs()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT d.Id, d.[Name], d.Breed, d.ImageUrl, d.Notes, d.OwnerId, o.Name as OwnerName
                        FROM Dog d
                        JOIN Owner o On d.OwnerId = o.Id
                       
                    ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Dog> dogs = new List<Dog>();
                    while (reader.Read())
                    {
                        Dog dog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl")),
                            Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null :reader.GetString(reader.GetOrdinal("Notes")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId"))
                        };
                        Owner owner = new Owner
                        {
                            Id = dog.OwnerId,
                            Name = reader.GetString(reader.GetOrdinal("OwnerName"))

                        };
                        dog.Owner = owner;

                        dogs.Add(dog);
                    }

                    reader.Close();

                    return dogs;
                }
            }
        }

        public Dog GetDogById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT d.Id, d.[Name], d.Breed, d.ImageUrl, d.Notes, d.OwnerId, o.Name as OwnerName
                        FROM Dog d
                        JOIN Owner o On d.OwnerId = o.Id
                        Where d.Id = @id
                       
                    ";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();


                    if (reader.Read())
                    {
                        Dog dog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl")),
                            Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId"))
                        };
                        Owner owner = new Owner
                        {
                            Id = dog.OwnerId,
                            Name = reader.GetString(reader.GetOrdinal("OwnerName"))

                        };
                        dog.Owner = owner;


                        reader.Close();

                        return dog;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }

                }
            }

        }

        public List<Dog> GetDogsByOwnerId(int ownerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT Id, Name, Breed, Notes, ImageUrl, OwnerId 
                FROM Dog
                WHERE OwnerId = @ownerId
            ";

                    cmd.Parameters.AddWithValue("@ownerId", ownerId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Dog> dogs = new List<Dog>();

                    while (reader.Read())
                    {
                        Dog dog = new Dog()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId"))
                        };

                        // Check if optional columns are null
                        if (reader.IsDBNull(reader.GetOrdinal("Notes")) == false)
                        {
                            dog.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                        }
                        if (reader.IsDBNull(reader.GetOrdinal("ImageUrl")) == false)
                        {
                            dog.ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"));
                        }

                        dogs.Add(dog);
                    }
                    reader.Close();
                    return dogs;
                }
            }
        }


        public void AddDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Dog ([Name], Breed, Notes, ImageUrl, OwnerId)
                    OUTPUT INSERTED.ID
                    VALUES (@name, @breed, @notes, @imageurl, @ownerId);
                ";

                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    _ = (dog.Notes == null) ? cmd.Parameters.AddWithValue("@notes", DBNull.Value) 
                        : cmd.Parameters.AddWithValue("@notes", dog.Notes);

                    _ = (dog.ImageUrl == null) ? cmd.Parameters.AddWithValue("@imageurl", DBNull.Value) :
                    cmd.Parameters.AddWithValue("@imageurl", dog.ImageUrl);
                    
                    cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);

                    int id = (int)cmd.ExecuteScalar();

                    dog.Id = id;
                }
            }
        }

        public void UpdateDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            Update Dog
                            SET
                                [Name] = @name,
                                Breed = @breed,
                                Notes = @notes,
                                ImageUrl = @imageurl,
                                OwnerId = @ownerid
                            WHERE Id = @id
                            ";

                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    _ = (dog.Notes == null) ? cmd.Parameters.AddWithValue("@notes", DBNull.Value)
                        : cmd.Parameters.AddWithValue("@notes", dog.Notes);

                    _ = (dog.ImageUrl == null) ? cmd.Parameters.AddWithValue("@imageurl", DBNull.Value) :
                    cmd.Parameters.AddWithValue("@imageurl", dog.ImageUrl);

                    cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);
                    cmd.Parameters.AddWithValue("@id", dog.Id);

                    cmd.ExecuteNonQuery();

                    
                }
            }
        }

        public void DeleteDog(int dogId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Dog
                            WHERE Id = @id
                            ";

                    cmd.Parameters.AddWithValue("@id", dogId);

                    cmd.ExecuteScalar();

                   
                }
            }

        }

    }
}
