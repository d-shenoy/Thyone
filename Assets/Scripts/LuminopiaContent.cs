using System.Collections.Generic;
using System.Collections;

public struct LuminopiaContent
{
    public string name;
    public List<string> videoIds;

    public LuminopiaContent(string name, List<string> videoIds)
    {
        this.videoIds = new List<string>(videoIds);
        this.name = name;
    }
}