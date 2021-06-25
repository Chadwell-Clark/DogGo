using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models.ViewModels
{
    public class WalksDeleteViewModel
    {
        public List<Walks> Walks { get; set; }

        public int[] WalksToDelete { get; set; }
    }
}
