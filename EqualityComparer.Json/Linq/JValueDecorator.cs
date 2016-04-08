using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace JsonEqualityComparer.Json.Linq
{
    internal class JValueDecorator : JValue, IDecorator
    {
        public JToken Node { get; set; }

        public new string Path
        {
            get { return Node.Path; }
        }

        public JValueDecorator(JValue node)
            : base(node)
        {
            Node = node;
        }

        public bool Accept(IVisitor visitor, JToken node)
        {
            return visitor.Visit(this, node);
        }
    }
}
