using DogGo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
    public interface IWalksRepository
    {
        List<Walks> GetAll();
        List<Walks> GetWalksByWalkerId(int walkerId);

        void AddWalks(Walks walks);

    }
}
