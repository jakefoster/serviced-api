using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.ncore.Exceptions;

namespace org.ncore.ServicedApi.Exceptions
{
    public class DataValidationCondition : RuntimeCondition
    {
        private IEnumerable<RuleViolation> _violations;

        private new const string DEFAULT_MESSAGE = "Validation of data failed on one or more class members.";

        public IEnumerable<RuleViolation> Violations
        {
            get
            {
                return _violations;
            }
        }

        public DataValidationCondition( IEnumerable<RuleViolation> violations )
            : base( DEFAULT_MESSAGE )
        {
            _violations = violations;
        }

        public DataValidationCondition( IEnumerable<RuleViolation> violations, string defaultMessage )
            : base( defaultMessage )
        {
            _violations = violations;
        }
    }
}
