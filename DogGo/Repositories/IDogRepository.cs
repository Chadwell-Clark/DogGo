﻿using DogGo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
    public interface IDogRepository
    {
        List<Dog> GetAllDogs();

        Dog GetDogById(int id);
        List<Dog> GetDogsByOwnerId(int ownerId);

        void AddDog(Dog dog);

        void UpdateDog(Dog Dog);

        void DeleteDog(int id);


    }
}
