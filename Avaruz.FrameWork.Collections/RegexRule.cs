using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Avaruz.FrameWork.Collections
{
    public class RegexRule : Rule {

        private string _regex;

        /// <summary>
        /// Constructor.
        /// </summary>
        public RegexRule(string propertyName, string description, string regex) : base(propertyName, description) {
            _regex = regex;
        }


        public override bool ValidateRule(BaseObject domainObject) {
            PropertyInfo pi = domainObject.GetType().GetProperty(this.PropertyName);
            Match m = Regex.Match(pi.GetValue(domainObject, null).ToString(), _regex);
            if (m.Success) {
                return true;
            } else return false;
        }
    }
}
