using System;
using System.Collections.Generic;
using System.Text;

namespace Avaruz.FrameWork.Collections
{
    /// <summary>
    /// A simple type of domain object rule that uses a delegate for validation. 
    /// </summary>
    /// <returns>True if the rule has been followed, or false if it has been broken.</returns>
    /// <remarks>
    /// Usage:
    /// <code>
    ///     this.Rules.Add(new SimpleRule("Name", "The customer name must be at least 5 letters long.", delegate { return this.Name &gt; 5; } ));
    /// </code>
    /// </remarks>
    public delegate bool SimpleRuleDelegate();

    /// <summary>
    /// A class to define a simple rule, using a delegate for validation.
    /// </summary>
    public class SimpleRule : 
        Rule {
        private SimpleRuleDelegate _ruleDelegate;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="propertyName">The name of the property this rule validates for. This may be blank.</param>
        /// <param name="brokenDescription">A description message to show if the rule has been broken.</param>
        /// <param name="ruleDelegate">A delegate that takes no parameters and returns a boolean value, used to validate the rule.</param>
        public SimpleRule(string propertyName, string brokenDescription, SimpleRuleDelegate ruleDelegate):
            base(propertyName, brokenDescription) {
            this.RuleDelegate = ruleDelegate;
        }

        /// <summary>
        /// Gets or sets the delegate used to validate this rule.
        /// </summary>
        protected virtual SimpleRuleDelegate RuleDelegate {
            get { return _ruleDelegate; }
            set { _ruleDelegate = value; }
        }

        /// <summary>
        /// Validates that the rule has not been broken.
        /// </summary>
        /// <param name="domainObject">The domain object being validated.</param>
        /// <returns>True if the rule has not been broken, or false if it has.</returns>
        public override bool ValidateRule(BaseObject domainObject) {
            return RuleDelegate();
        }
    }
}
