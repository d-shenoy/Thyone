using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Utils
{
    public static Dictionary<K, V> HashtableToDictionary<K, V>(Hashtable table)
    {
        return table.Cast<DictionaryEntry>()
        .ToDictionary(kvp => (K)kvp.Key, kvp => (V)kvp.Value);
    }
}