using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonEqualityComparer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EqualityComparer.Json.Samples
{
    class Program
    {
        private static string _A = "{"
                                       + "\"glossary\": {"
                                       +     "\"topic\": \"example glossary\","
		                               +     "\"GlossDiv\": {"
                                       +         "\"title\": \"S\","
			                           +         "\"GlossList\": {"
                                       +             "\"GlossEntry\": {"
                                       +                 "\"ID\": \"SGML\","
					                   +                 "\"SortAs\": \"SGML\","
					                   +                 "\"GlossTerm\": \"Standard Generalized Markup Language\","
					                   +                 "\"Acronym\": \"SGML\","
					                   +                 "\"Abbrev\": \"ISO 8879:1986\","
					                   +                 "\"GlossDef\": {"
                                       +                     "\"para\": \"A meta-markup language, used to create markup languages such as DocBook.\","
						               +                     "\"GlossSeeAlso\": [\"GML\", \"XML\"]"
                                       +                 "},"
					                   +                 "\"GlossSee\": \"markup\""
                                       +             "}"
                                       +         "}"
                                       +     "}"
                                       + "}"
                                    + "}";
        private static string _B = "{"
                                       + "\"glossary\": {"
                                       + "\"title\": \"example glossary\","
                                       + "\"GlossDiv\": {"
                                       + "\"title\": \"S\","
                                       + "\"GlossList\": {"
                                       + "\"GlossEntry\": {"
                                       + "\"ID\": \"SGML\","
                                       + "\"SortAs\": \"SGML\","
                                       + "\"GlossTerm\": \"Standard Generalized Markup Language\","
                                       + "\"Acronym\": \"SGML\","
                                       + "\"Abbrev\": \"ISO 8879:1986\","
                                       + "\"GlossDef\": {"
                                       + "\"para\": \"A meta-markup language, used to create markup languages such as DocBook.\","
                                       + "\"GlossSeeAlso\": [\"GML\", \"XML\"]"
                                       + "},"
                                       + "\"GlossSee\": \"markup\""
                                       + "}"
                                       + "}"
                                       + "}"
                                       + "}"
                                    + "}";
        static void Main(string[] args)
        {
            var jOExpected = JsonConvert.DeserializeObject(_A) as JToken;
            var jOReceived = JsonConvert.DeserializeObject(_B) as JToken;

            var comparisonResult = Comparer.HasDifferences(jOExpected, jOReceived);
            if (!comparisonResult.AreEquals)
            {
                Console.WriteLine("There are differences between both models\n");

                Func<ICollection<string>, int> biggestString = collection =>
                {
                    return collection.Select(item => item.Length).Max();
                };

                var numeratorSize = Math.Max(comparisonResult.LeftMemberMissingNodes.Count,
                    comparisonResult.RightMemberMissingNodes.Count).ToString().Length + 1;
                var valueFieldSize = Math.Max(biggestString(comparisonResult.LeftMemberMissingNodes.Keys),
                    biggestString(comparisonResult.RightMemberMissingNodes.Keys));

                var colls = new[] {"Path", "Value"};
                var pathMarginLeft = numeratorSize + 1 + colls[0].Length;
                var valueMarginLeft = valueFieldSize + colls[1].Length - colls[0].Length + 1;

                var gridHeaderFormat = "{0," + pathMarginLeft + "}{1," + valueMarginLeft + "}";
                var gridRowFormat = "{0,-" + (numeratorSize + 1) + "}{1,-" + (valueFieldSize + 1) + "}{2}";

                var i = 1;
                if (comparisonResult.LeftMemberMissingNodes.Count != 0)
                {
                    Console.WriteLine("The following nodes does not exists in first json model:");
                    Console.WriteLine(gridHeaderFormat, "Path", "Value");
                    foreach (var item in comparisonResult.LeftMemberMissingNodes)
                    {
                        Console.WriteLine(gridRowFormat, (i++) + ".", item.Key, item.Value);
                    }
                    Console.WriteLine();
                }
                if (comparisonResult.RightMemberMissingNodes.Count != 0)
                {
                    Console.WriteLine("The following nodes does not exists in second json model:");
                    Console.WriteLine(gridHeaderFormat, "Path", "Value");
                    i = 1;
                    foreach (var item in comparisonResult.RightMemberMissingNodes)
                    {
                        Console.WriteLine(gridRowFormat, (i++) + ".", item.Key, item.Value);
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("The models are exactly equals\n");
            }

            Console.Write("Press any key to continue...");
            Console.ReadKey(false);
        }
    }
}
