using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace JsonEqualityComparer
{
    internal interface IDecorator
    {
        bool Accept(IVisitor visitor, JToken node);
        JToken Node { get; set; }
        string Path { get; }
    }
}
