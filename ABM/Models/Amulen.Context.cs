﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ABM.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AmulenEntities : DbContext
    {
        public AmulenEntities()
            : base("name=AmulenEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<HomePageData> HomePageData { get; set; }
        public virtual DbSet<HomePageImage> HomePageImage { get; set; }
        public virtual DbSet<Proyect> Proyect { get; set; }
        public virtual DbSet<ProyectState> ProyectState { get; set; }
        public virtual DbSet<SuscriptorProyect> SuscriptorProyect { get; set; }
        public virtual DbSet<TypeUser> TypeUser { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<ImportantFile> ImportantFile { get; set; }
    }
}
