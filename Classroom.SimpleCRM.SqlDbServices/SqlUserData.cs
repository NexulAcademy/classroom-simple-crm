using System;
using System.Linq;

namespace Classroom.SimpleCRM.SqlDbServices
{
    public class SqlUserData : IUserData
    {
        private readonly CrmIdentityDbContext context;

        public SqlUserData(CrmIdentityDbContext context)
        {
            this.context = context;
        }

        public CrmIdentityUser Get(string id)
        {
            return context.Users.FirstOrDefault(x => x.Id == id);
        }

        public CrmIdentityUser GetSingle(string userName)
        {
            return context.Users.FirstOrDefault(x => x.UserName == userName);
        }
    }
}
