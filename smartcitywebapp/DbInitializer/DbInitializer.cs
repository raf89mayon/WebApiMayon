using SmartCityWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SmartCityWebApp.DbInitializer
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {


        protected override void Seed(ApplicationDbContext context)
        {
/*
            Bed bed = new Bed()
            {
                Code = 1,
                Name = "lit1"
            };

            context.BedDB.Add(bed);
            context.SaveChanges(); */
        }

    }

}