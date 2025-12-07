using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetGrooming.BLL
{
    public class ExceptionsBLL : Exception
    {
        public ExceptionsBLL(string message) : base(message) { }
        public ExceptionsBLL(string message, Exception? inner) : base(message, inner) { }
    }

    public class ValidationException : ExceptionsBLL
    {
        public ValidationException(string message) : base(message) { }
    }
    public class BusinessException : ExceptionsBLL
    {
        public BusinessException(string message, Exception? inner = null) : base(message, inner) { }
    }
}