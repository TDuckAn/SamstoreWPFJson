using SamStoreWPFJson_BOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamStoreWPFJson_Repositories.Interfaces
{
    public interface IMemberRepo
    {
        Member? GetMemberByLogin(string email, string password);
    }
}
