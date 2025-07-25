using SamStoreWPFJson_DAOs;
using SamStoreWPFJson_BOs;
using SamStoreWPFJson_Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamStoreWPFJson_Repositories.Implements
{
    public class MemberRepo : IMemberRepo
    {
        private readonly MemberDAO _memberDAO;

        public MemberRepo()
        {
            _memberDAO = new MemberDAO();
        }

        public Member? GetMemberByLogin(string email, string password)
        {
            return _memberDAO.GetMemberByLogin(email, password);
        }
    }
}
