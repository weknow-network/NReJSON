using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;

namespace NReJSON
{
    public static partial class DatabaseExtensions
    {
        private static string[] CombineArguments(params object[] args)
        {
            IEnumerable<string> ArgumentCombiner()
            {
                if (args == null)
                {
                    yield break;
                }

                foreach (var arg in args)
                {
                    if (arg.GetType() == typeof(RedisKey[]))
                    {
                        foreach (var aa in (RedisKey[])arg)
                        {
                            if (TryGetValidString(aa, out var result))
                            {
                                yield return result;
                            }
                        }
                    }
                    else if (arg.GetType().IsArray)
                    {
                        foreach (var aa in (object[])arg)
                        {
                            if (TryGetValidString(aa, out var result))
                            {
                                yield return result;
                            }
                        }
                    }
                    else
                    {
                        if (TryGetValidString(arg, out var result))
                        {
                            yield return result;
                        }
                    }
                }
            }

            return ArgumentCombiner().ToArray();
        }

        private static bool TryGetValidString(RedisKey key, out string result)
        {
            result = key.ToString();

            return result.Length > 0;
        }

        private static bool TryGetValidString(object arg, out string result)
        {
            result = arg.ToString();

            return result.Length > 0;
        }

        private static string[] PathsOrDefault(string[] paths, string[] @default) =>
            paths == null || paths.Length == 0 ? @default : paths;

        private static string GetSetOptionString(SetOption setOption)
        {
            switch (setOption)
            {
                case SetOption.Default:
                    return string.Empty;
                case SetOption.SetIfNotExists:
                    return "NX";
                case SetOption.SetOnlyIfExists:
                    return "XX";
                default:
                    return string.Empty;
            }
        }

        private static readonly string[] EmptyIndexSpecification = new string[1] {string.Empty};

        private static string[] ResolveIndexSpecification(string index)
        {
            if (string.IsNullOrEmpty(index))
            {
                return EmptyIndexSpecification;
            }
            else
            {
                return new[] { "INDEX", index };
            }
        }
    }
}
