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

    // Returns a list of random colors of size `size` but ensures that each color is used at least once
    public IEnumerable<Color> GetSemiRandomDistribution(int size)
    {
        List<Color> randomColors = new List<Color>();
        HashSet<Color> colorsRemaining = colors.ToHashSet();

        if(size < colorsRemaining.Count)
        {
            Debug.Log("Size given is smaller than set of colors. Unable to fit every color in");
        }

        for(int i = 0;i < size;i++)
        {
            // if there are less remaining colors than their are slots to fill, continue with random colors
            if(colorsRemaining.Count < size - i)
            {
                // get a random color and add it to the list
                Color randomColor = GetRandomColor();
                randomColors.Add(randomColor);

                // remove it from the set (if it hasn't already been removed)
                colorsRemaining.Remove(randomColor);
            }
            else
            {
                // there are equal or more remaining colors than slots to fill, so fill directly from set
                // to ensure (as much as possible) that every color is used at least once
                randomColors.AddRange(colorsRemaining.Take(size - i));
                break;
            }
        }

        return randomColors;
    }
}
