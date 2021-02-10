using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V2Sharp.Models;

namespace V2Sharp.Models
{
    public class V2SharpContext : DbContext
    {
        public V2SharpContext(DbContextOptions<V2SharpContext> options) : base(options)
        {
        }

        protected V2SharpContext()
        {
        }

        public DbSet<User> User { get; set; }
    }
}
