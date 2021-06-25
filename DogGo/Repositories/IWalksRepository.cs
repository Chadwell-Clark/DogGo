using DogGo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
    public interface IWalksRepository
    {
        List<Walks> GetAllWalks();
        List<Walks> GetWalksByWalkerId(int walkerId);

        void AddWalks(Walks walks);

        void DeleteWalks(int id);

    }
}
