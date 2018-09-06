After Modify Database:
1. Excute following Command in 'Package Manager Console' , The value of Default project must be 'PuppetCat.Sample.Data'
Scaffold-DbContext "Server=.;database=puppetcat_sample;uid=sa;password=123456;" Pomelo.EntityFrameworkCore.MySql -OutputDir Sample -Context SampleDbContext -f

2. Find file 'OneCallDbContext' , then delete following code:

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=192.168.30.253;database=SSWChinaWebsite;uid=sa;password=Perfect2018!;");
            }
        }

**************************************************************************************************