﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_EF.Model
{
    [Table("users")]
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Email { get; set; }                
        public string FullName { get; set; }
        public int Age { get; set; }

    }
}