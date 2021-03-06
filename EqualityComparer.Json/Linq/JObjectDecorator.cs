﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace JsonEqualityComparer.Json.Linq
{
    internal class JObjectDecorator : JObject, IDecorator
    {
        public JToken Node { get; set; }

        public new string Path
        {
            get { return Node.Path; }
        }

        public JObjectDecorator(JObject node)
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
