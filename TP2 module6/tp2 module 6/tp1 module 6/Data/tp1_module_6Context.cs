﻿using BO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace tp1_module_6.Data
{
    public class tp1_module_6Context : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public tp1_module_6Context() : base("name=tp1_module_6Context")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {   

            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
            modelBuilder.Entity<Samourai>().Ignore(s => s.potentiel);
            modelBuilder.Entity<Samourai>().HasMany(s => s.ArtMartials).WithMany();
            base.OnModelCreating(modelBuilder);
        }
        public System.Data.Entity.DbSet<BO.Samourai> Samourais { get; set; }

        public System.Data.Entity.DbSet<BO.Arme> Armes { get; set; }
        public System.Data.Entity.DbSet<BO.ArtMartial> ArtMartials { get; set; }
    }
}
