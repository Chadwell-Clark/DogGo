using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models.ViewModels
{
    public class WalksFormViewModel
    {
        public Walks Walks { get; set; }
        public List<Walker> Walkers { get; set; }

        public List<Dog> Dogs { get; set; }

        public int[] MultiSelectDogs { get; set; }

    }
}
