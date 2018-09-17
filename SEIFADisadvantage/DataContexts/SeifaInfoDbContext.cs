using Microsoft.EntityFrameworkCore;
using SEIFADisadvantage.Models._2011;
using SEIFADisadvantage.Models._2016;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SEIFADisadvantage.DataContexts
{
    public class SeifaInfoDbContext : DbContext
    {
        public SeifaInfoDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<SeifaInfo2016> Data2016 { get; set; }

        public DbSet<SeifaInfo2011> Data2011 { get; set; }
    }
}
