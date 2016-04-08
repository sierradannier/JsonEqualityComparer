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
                Console.WriteLine("There are differences between both models");
            }
            else
            {
                Console.WriteLine("The models are exactly equals");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(false);
        }
    }
}
