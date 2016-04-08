using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace JsonEqualityComparer.Json.Linq
{
    internal class JPropertyDecorator : JProperty, IDecorator
    {
        public JToken Node { get; set; }

        public new string Path
        {
            get { return Node.Path; }
        }

        public JPropertyDecorator(JProperty node)
            : base(node)
        {
            Node = node;
        }

        public bool Accept(IVisitor visitor, JToken node)
        {
            return visitor.Visit(this, node);
        }

        public IList<JToken> ChildrenTokens
        {
            get
            {
                return base.ChildrenTokens;
            }
        }
    }
}
