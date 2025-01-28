using System;
using System.Collections.Generic;
using WorldsBelly.DataAccess.Entities;

namespace WorldsBelly.API.Models
{
    public class CountryView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
