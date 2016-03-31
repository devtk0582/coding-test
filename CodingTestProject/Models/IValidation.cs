using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodingTestProject.Models
{
    public interface IValidation
    {
        void AddError(string key, string errorMessage);
        bool IsValid { get; }
    }
}