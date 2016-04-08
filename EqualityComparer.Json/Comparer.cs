using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonEqualityComparer.Json.Linq;
using Newtonsoft.Json.Linq;

namespace JsonEqualityComparer
{
    public class Comparer
    {
        public static ComparisonResult HasDifferences(JToken x, JToken y)
        {
            if(x == null) throw new ArgumentNullException("x");
            if(y == null) throw new ArgumentNullException("y");
            var v = new JsonDifferencesVisitor();
            var w = new JsonDifferencesVisitor();
            switch (x.Type)
            {
                case JTokenType.Object:
                    v.Visit(new JObjectDecorator(x as JObject), y);
                    w.Visit(new JObjectDecorator(y as JObject), x);
                    break;
                case JTokenType.Constructor:
                    v.Visit(new JConstructorDecorator(x as JConstructor), y);
                    v.Visit(new JConstructorDecorator(y as JConstructor), x);
                    break;
                case JTokenType.Array:
                    v.Visit(new JArrayDecorator(x as JArray), y);
                    v.Visit(new JArrayDecorator(y as JArray), x);
                    break;
                case JTokenType.Property:
                    v.Visit(new JPropertyDecorator(x as JProperty), y);
                    v.Visit(new JPropertyDecorator(y as JProperty), x);
                    break;
                default:
                    v.Visit(new JValueDecorator(x as JValue), y);
                    v.Visit(new JValueDecorator(y as JValue), x);
                    break;
            }
            return
                new ComparisonResult(w.Differences, v.Differences);
        }
    }
}
