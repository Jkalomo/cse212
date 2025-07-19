using System.Collections.Generic;

public class FeatureCollection
{
    public List<Feature> Features { get; set; }
}

public class Feature
{
    public Properties Properties { get; set; }
}

public class Properties
{
    public double? Mag { get; set; }   // Magnitude can be null
    public string Place { get; set; }
}