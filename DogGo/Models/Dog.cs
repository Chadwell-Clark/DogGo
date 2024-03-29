﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models
{
    public class Dog
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="You're dog may have fleas but it really needs a name")]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public string Breed { get; set; }

        public string Notes { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        [DisplayName("Owner")]
        public int OwnerId { get; set; }

        public Owner Owner { get; set; }
    }
}
