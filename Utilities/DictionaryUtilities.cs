namespace AOC.Utilities;

public static class DictionaryUtilities {
    public static TValue GetValueOrAssignDefault<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TValue> defaultValueGetter
    ) {
        if (!dictionary.TryGetValue(key, out var value)) {
            value = defaultValueGetter();
            dictionary[key] = value;
        }

        return value;
    }
}