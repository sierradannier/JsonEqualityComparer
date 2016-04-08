using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonEqualityComparer.Json.Linq;
using Newtonsoft.Json.Linq;

namespace JsonEqualityComparer
{
    internal interface IVisitor
    {
        bool Visit(JObjectDecorator decorator, JToken node1);
        bool Visit(JConstructorDecorator decorator, JToken node1);
        bool Visit(JArrayDecorator decorator, JToken node1);
        bool Visit(JPropertyDecorator decorator, JToken node1);
        bool Visit(JValueDecorator decorator, JToken node1);
    }
}
