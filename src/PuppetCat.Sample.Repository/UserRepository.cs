using System;
using PuppetCat.Sample.Data;
using System.Linq;

namespace PuppetCat.Sample.Repository
{
    public partial class UserRepository : BaseRepository<User, SampleDbContext>
    {
        public UserRepository(SampleDbContext context) : base(context)
        {

        }
    }
}

