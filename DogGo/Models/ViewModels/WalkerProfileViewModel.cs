﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models.ViewModels
{
    public class WalkerProfileViewModel
    {
       public  Walker Walker { get; set; }
       public List<Walks> Walks { get; set; }
        public Dog Dog { get; set; }
    }
}
