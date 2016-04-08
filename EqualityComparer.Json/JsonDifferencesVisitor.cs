using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonEqualityComparer.Json.Linq;
using Newtonsoft.Json.Linq;

namespace JsonEqualityComparer
{
    internal class JsonDifferencesVisitor : IVisitor
    {
        private IDictionary<string, string> _differences;

        public IDictionary<string, string> Differences
        {
            get { return _differences ?? (_differences = new Dictionary<string, string>()); }
        }

        public bool Visit(JObjectDecorator decorator, JToken node1)
        {
            var areEquals = true;
            var jobject = node1 as JObject;
            if (jobject == null)
            {
                var fullPath = GetFullPath(decorator.Node);
                Differences.Add(fullPath, decorator.Node.ToString());
                return false;
            }
            var a = decorator.ChildrenTokens;
            var objDecorator = new JObjectDecorator(jobject);
            var b = objDecorator.ChildrenTokens;
            if (a == b)
                return true;
            if (a == null && b == null)
                return true;
            if (a == null)
            {
                return true;
            }
            foreach (JToken item in a)
            {
                var tokenKey = ((JProperty)item).Name;
                JToken jtoken = b.FirstOrDefault(x => ((JProperty)x).Name == tokenKey);
                if (jtoken == null)
                {
                    var fullPath = GetFullPath((JProperty)item);
                    Differences.Add(fullPath, ((JProperty)item).ToString());
                    areEquals = false;
                    continue;
                }
                JProperty jproperty1 = (JProperty)item;
                JProperty jproperty2 = (JProperty)jtoken;
                if (jproperty1.Value == null)
                {
                    continue;
                }
                if (!ResolveDecorator(jproperty1.Value).Accept(this, jproperty2.Value))
                    areEquals = false;
            }
            return areEquals;
        }
        public bool Visit(JConstructorDecorator decorator, JToken node1)
        {
            JConstructor jconstructor = node1 as JConstructor;
            if (jconstructor != null && decorator.Name == jconstructor.Name)
                return ContentsEqual(decorator.ChildrenTokens, new JConstructorDecorator(jconstructor).ChildrenTokens);
            var fullPath = GetFullPath(decorator.Node);
            Differences.Add(fullPath, decorator.Node.ToString());
            return false;
        }
        public bool Visit(JArrayDecorator decorator, JToken node1)
        {
            JArray jarray = node1 as JArray;
            if (jarray != null)
                return ContentsEqual(decorator.ChildrenTokens, new JArrayDecorator(jarray).ChildrenTokens);
            var fullPath = GetFullPath(decorator.Node);
            Differences.Add(fullPath, decorator.Node.ToString());
            return false;
        }
        public bool Visit(JPropertyDecorator decorator, JToken node1)
        {
            JProperty jproperty = node1 as JProperty;
            if (jproperty != null && (decorator.Node as JProperty).Name == jproperty.Name)
                return ContentsEqual(decorator.ChildrenTokens, new JPropertyDecorator(jproperty).ChildrenTokens);
            var fullPath = GetFullPath(decorator.Node);
            Differences.Add(fullPath, decorator.Node.ToString());
            return false;
        }
        public bool Visit(JValueDecorator decorator, JToken node1)
        {
            return true;
        }

        private bool ContentsEqual(IList<JToken> childrenTokens1, IList<JToken> childrenTokens2)
        {
            if (childrenTokens1 == childrenTokens2)
                return true;
            if (childrenTokens1.Count > childrenTokens2.Count)
            {
                for (int i = Math.Min(childrenTokens1.Count, childrenTokens2.Count); i < childrenTokens1.Count; i++)
                {
                    var fullPath = GetFullPath(childrenTokens1[i]);
                    Differences.Add(fullPath, childrenTokens1[i].ToString());
                }
                return false;
            }
            var result = true;
            for (int i = 0; i < Math.Min(childrenTokens1.Count, childrenTokens2.Count); ++i)
            {
                if (!ResolveDecorator(childrenTokens1[i]).Accept(this, childrenTokens2[i]))
                    result = false;
            }
            return result;
        }

        private IDecorator ResolveDecorator(JToken node)
        {
            switch (node.Type)
            {
                case JTokenType.Object:
                    return new JObjectDecorator(node as JObject);
                case JTokenType.Constructor:
                    return new JConstructorDecorator(node as JConstructor);
                case JTokenType.Array:
                    return new JArrayDecorator(node as JArray);
                case JTokenType.Property:
                    return new JPropertyDecorator(node as JProperty);
                default:
                    return new JValueDecorator(node as JValue);
            }
        }

        private string GetFullPath(JToken node)
        {
            var fullPath = GetPath(node);
            var parentNode = node;
            while (parentNode.Parent != null)
            {
                if (parentNode.Parent is IDecorator)
                    parentNode = ((IDecorator)parentNode.Parent).Node;
                else if (parentNode.Parent is JToken)
                    parentNode = parentNode.Parent;

                fullPath = GetPath(parentNode) + fullPath;
            }
            return fullPath;
        }

        private string GetPath(JToken node)
        {
            if (node.Parent == null) return "/";
            var path = string.Empty;
            switch (node.Type)
            {
                case JTokenType.Property:
                    path = node.Path + "/" + path;
                    break;
                case JTokenType.Object:
                    if (node.Parent.Type == JTokenType.Array)
                        path = node.Path + "/" + path;
                    break;
                case JTokenType.Array:
                case JTokenType.Constructor:
                default:
                    break;
            }
            return path;
        }
    }
}
