using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace PuppetCat.Sample.Data
{
    public partial class SampleDbContext : DbContext
    {
        public static string ConStr { get; set; }

        public SampleDbContext()
        {

        }

        public SampleDbContext(DbContextOptions<SampleDbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConStr);
        }
    }
}
