using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateAdmin.Core.ActiveDirectory
{
    public class ADGroup
    {
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
        }

        private string description;
        public string Description
        {
            get
            {
                return description;
            }
        }

        private Dictionary<string, string> memberDescriptionMap = new Dictionary<string, string>();
        public Dictionary<string, string> MemberDescriptionMap
        {
            get
            {
                return memberDescriptionMap;
            }
        }
        

        public ADGroup(string name, string description, Dictionary<string,string> memberDescMap)
        {
            this.name = name;
            this.description = description;
            this.memberDescriptionMap = memberDescMap;
        }
    }
}
