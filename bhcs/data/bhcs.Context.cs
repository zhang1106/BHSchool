﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class bhcsEntities : DbContext
    {
        public bhcsEntities()
            : base("name=bhcsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<classroom> classrooms { get; set; }
        public DbSet<course> courses { get; set; }
        public DbSet<member> members { get; set; }
        public DbSet<message> messages { get; set; }
        public DbSet<timeslot> timeslots { get; set; }
        public DbSet<address> addresses { get; set; }
        public DbSet<bhclass> bhclasses { get; set; }
        public DbSet<transactiondetail> transactiondetails { get; set; }
        public DbSet<class_student> class_students { get; set; }
        public DbSet<transaction> transactions { get; set; }
        public DbSet<image> images { get; set; }
        public DbSet<item> items { get; set; }
        public DbSet<item_sharing> item_sharing { get; set; }
    }
}
