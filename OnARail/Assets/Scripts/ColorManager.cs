using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ColorManager : MonoBehaviour
{
    public List<Color> colors;

    public Color GetColor(int index)
    {
        return colors[index];
    }

    // Returns a random color from the list
    public Color GetRandomColor()
    {
        int random = Random.Range(0, colors.Count);
        return colors[random];
    }

    // Returns a random color from the list excluding any colors that were passed in
    public Color GetRandomColor(params Color[] exclude)
    {
        int random = Random.Range(0, colors.Count - exclude.Length);
        return colors.Where(color => !exclude.Contains(color)).ToList()[random];
    }
}
