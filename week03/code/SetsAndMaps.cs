using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

public static class SetsAndMaps
{
    /// <summary>
    /// The words parameter contains a list of two character 
    /// words (lower case, no duplicates). Using sets, find an O(n) 
    /// solution for returning all symmetric pairs of words.  
    ///
    /// For example, if words was: [am, at, ma, if, fi], we would return :
    ///
    /// ["am & ma", "if & fi"]
    ///
    /// The order of the array does not matter, nor does the order of the specific words in each string in the array.
    /// at would not be returned because ta is not in the list of words.
    ///
    /// As a special case, if the letters are the same (example: 'aa') then
    /// it would not match anything else (remember the assumption above
    /// that there were no duplicates) and therefore should not be returned.
    /// </summary>
    /// <param name="words">An array of 2-character words (lowercase, no duplicates)</param>
    public static string[] FindPairs(string[] words)
    {
        HashSet<string> seenWords = new HashSet<string>();
        List<string> result = new List<string>();

        foreach (string word in words)
        {
            // Skip words with identical characters (e.g., "aa")
            if (word[0] == word[1])
                continue;

            string reversed = $"{word[1]}{word[0]}";

            // If we've seen the reversed version already, add the pair
            if (seenWords.Contains(reversed))
            {
                result.Add($"{reversed} & {word}");
            }
            else
            {
                seenWords.Add(word);
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Read a census file and summarize the degrees (education)
    /// earned by those contained in the file.  The summary
    /// should be stored in a dictionary where the key is the
    /// degree earned and the value is the number of people that 
    /// have earned that degree.  The degree information is in
    /// the 4th column of the file.  There is no header row in the
    /// file.
    /// </summary>
    /// <param name="filename">The name of the file to read</param>
    /// <returns>fixed array of divisors</returns>
    public static Dictionary<string, int> SummarizeDegrees(string filename)
    {
        var degrees = new Dictionary<string, int>();
        foreach (var line in File.ReadLines(filename))
        {
            var fields = line.Split(',');
            // Ensure the line has at least 4 fields to access the degree
            if (fields.Length >= 4)
            {
                string degree = fields[3].Trim(); // Get degree from 4th column and trim any whitespace
                if (!string.IsNullOrEmpty(degree)) // Skip empty degree fields
                {
                    if (degrees.ContainsKey(degree))
                    {
                        degrees[degree]++; // Increment count if degree exists
                    }
                    else
                    {
                        degrees[degree] = 1; // Initialize count for new degree
                    }
                }
            }
        }

        return degrees;
    }

    /// <summary>
    /// Determine if 'word1' and 'word2' are anagrams.  An anagram
    /// is when the same letters in a word are re-organized into a 
    /// new word.  A dictionary is used to solve the problem.
    /// 
    /// Examples:
    /// is_anagram("CAT","ACT") would return true
    /// is_anagram("DOG","GOOD") would return false because GOOD has 2 O's
    /// 
    /// Important Note: When determining if two words are anagrams, you
    /// should ignore any spaces.  You should also ignore cases.  For 
    /// example, 'Ab' and 'Ba' should be considered anagrams
    /// 
    /// Reminder: You can access a letter by index in a string by 
    /// using the [] notation.
    /// </summary>
    public static bool IsAnagram(string word1, string word2)
    {
        // Normalize the strings: remove spaces and convert to invariant case
        word1 = word1.Replace(" ", "", StringComparison.Ordinal);
        word2 = word2.Replace(" ", "", StringComparison.Ordinal);

        // If lengths differ, they can't be anagrams
        if (word1.Length != word2.Length)
        {
            return false;
        }

        // Optimized path for ASCII-only strings
        if (IsAscii(word1) && IsAscii(word2))
        {
            int[] charCounts = new int[128]; // Covers standard ASCII

            foreach (char c in word1)
            {
                charCounts[char.ToLowerInvariant(c)]++;
            }

            foreach (char c in word2)
            {
                if (--charCounts[char.ToLowerInvariant(c)] < 0)
                {
                    return false;
                }
            }
        }
        else
        {
            // Fallback for Unicode strings
            var charCounts = new Dictionary<char, int>();

            foreach (char c in word1)
            {
                char lower = char.ToLowerInvariant(c);
                if (charCounts.ContainsKey(lower))
                    charCounts[lower]++;
                else
                    charCounts[lower] = 1;
            }

            foreach (char c in word2)
            {
                char lower = char.ToLowerInvariant(c);
                if (!charCounts.ContainsKey(lower) || --charCounts[lower] < 0)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static bool IsAscii(string s)
    {
        foreach (char c in s)
        {
            if (c > 127)
                return false;
        }
        return true;
    }

    public class FeatureCollection
    {
        [JsonPropertyName("features")]
        public List<Feature> Features { get; set; }
    }

    public class Feature
    {
        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }
    }

    public class Properties
    {
        [JsonPropertyName("place")]
        public string Place { get; set; }

        [JsonPropertyName("mag")]
        public double? Mag { get; set; }
    }

    /// <summary>
    /// This function will read JSON (Javascript Object Notation) data from the 
    /// United States Geological Service (USGS) consisting of earthquake data.
    /// The data will include all earthquakes in the current day.
    /// 
    /// JSON data is organized into a dictionary. After reading the data using
    /// the built-in HTTP client library, this function will return a list of all
    /// earthquake locations ('place' attribute) and magnitudes ('mag' attribute).
    /// Additional information about the format of the JSON data can be found 
    /// at this website:  
    /// 
    /// https://earthquake.usgs.gov/earthquakes/feed/v1.0/geojson.php
    /// 
    /// </summary>
    public static string[] EarthquakeDailySummary()
    {
        try
        {
            const string uri = "https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_day.geojson";
            using var client = new HttpClient();
            using var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            using var jsonStream = client.Send(getRequestMessage).Content.ReadAsStream();
            using var reader = new StreamReader(jsonStream);
            var json = reader.ReadToEnd();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json, options);

            // Check for null and extract valid results
            if (featureCollection?.Features == null)
            {
                return Array.Empty<string>();
            }

            var summaries = new List<string>();
            foreach (var feature in featureCollection.Features)
            {
                if (feature?.Properties != null && !string.IsNullOrEmpty(feature.Properties.Place))
                {
                    var mag = feature.Properties.Mag?.ToString("0.00") ?? "unknown";
                    summaries.Add($"{feature.Properties.Place} - Mag {mag}");
                }
            }

            return summaries.ToArray();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching earthquake data: {ex.Message}");
            return Array.Empty<string>();
        }
    }
}