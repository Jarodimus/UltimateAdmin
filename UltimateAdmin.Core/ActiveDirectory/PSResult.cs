using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateAdmin.Core.ActiveDirectory
{
    public class PSResult
    {
        private string _name;
        private bool _success;
        private string _message;

        public string Name
        {
            get
            {
                return _name;
            }
        }
        public bool Success
        {
            get
            {
                return _success;
            }
        }
        public string Message
        {
            get
            {
                return _message;
            }
        }

        public PSResult(string name)
        {
            _name = name;
        }

        public void SetSuccessValue(bool isSuccess)
        {
            _success = isSuccess;
        }

        public void SetMessage(string message)
        {
            _message = message;
        }

    }
}
