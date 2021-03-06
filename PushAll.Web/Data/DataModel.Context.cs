﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PushAll.Web.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class PushAllEntities : DbContext
    {
        public PushAllEntities()
            : base("name=PushAllEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<UserTag> UserTags { get; set; }
        public virtual DbSet<PushAllUser> PushAllUsers { get; set; }
    
        public virtual ObjectResult<GetTags_Result> GetTags(string userId, Nullable<bool> includeImage)
        {
            var userIdParameter = userId != null ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(string));
    
            var includeImageParameter = includeImage.HasValue ?
                new ObjectParameter("IncludeImage", includeImage) :
                new ObjectParameter("IncludeImage", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetTags_Result>("GetTags", userIdParameter, includeImageParameter);
        }
    }
}
