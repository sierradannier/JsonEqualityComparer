using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonEqualityComparer
{
    public class ComparisonResult
    {
        private bool _areEquals;

        public bool AreEquals
        {
            get { return _areEquals; }
        }

        private IDictionary<string, string> _leftMemberMissingNodes;

        public IDictionary<string, string> LeftMemberMissingNodes
        {
            get { return _leftMemberMissingNodes ?? (_leftMemberMissingNodes = new Dictionary<string, string>()); }
        }

        private IDictionary<string, string> _rightMemberMissingNodes;

        public IDictionary<string, string> RightMemberMissingNodes
        {
            get { return _rightMemberMissingNodes ?? (_rightMemberMissingNodes = new Dictionary<string, string>()); }
        }

        public ComparisonResult()
        {
            _areEquals = true;
        }

        public ComparisonResult(IDictionary<string, string> leftMemberMissingNodes, IDictionary<string, string> rightMemberMissingNodes)
        {
            _areEquals = (leftMemberMissingNodes == null || leftMemberMissingNodes.Count == 0) &&
                         (rightMemberMissingNodes == null || rightMemberMissingNodes.Count == 0);
            _leftMemberMissingNodes = leftMemberMissingNodes;
            _rightMemberMissingNodes = rightMemberMissingNodes;
        }
    }
}
