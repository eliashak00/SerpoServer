// ################################################
// ##@project SerpoCMS.Core
// ##@filename JwtToken.cs
// ##@author Elias Håkansson
// ##@license MIT - License(see license.txt)
// ################################################

namespace SerpoCMS.Core.Security
{
    public struct JwtToken
    {
        public string Email;
        public string Exp;
        public string Key;
    }
}