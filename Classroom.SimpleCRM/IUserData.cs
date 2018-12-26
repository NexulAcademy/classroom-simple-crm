using System;

namespace Classroom.SimpleCRM
{
    public interface IUserData
    {
        CrmIdentityUser Get(string id);
        CrmIdentityUser GetSingle(string userName);
    }
}
