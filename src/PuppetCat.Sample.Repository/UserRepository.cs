using System;
using PuppetCat.Sample.Data;
using System.Linq;

namespace PuppetCat.Sample.Repository
{
    public partial class UserRepository : BaseRepository<User, SampleDbContext>
    {
        public static readonly UserRepository Intance = new UserRepository(); 

        public UserRepository()
        {

        }

        internal UserRepository(SampleDbContext context) : base(context)
        {

        }
    }
}

