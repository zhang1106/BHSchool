//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class item_sharing
    {
        public int id { get; set; }
        public int itemId { get; set; }
        public Nullable<int> borrowerId { get; set; }
        public string updatedBy { get; set; }
        public Nullable<System.DateTime> updatedAt { get; set; }
        public Nullable<System.DateTime> borrowDate { get; set; }
        public Nullable<System.DateTime> returnDate { get; set; }
    }
}