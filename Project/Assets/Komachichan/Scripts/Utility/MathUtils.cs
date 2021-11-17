using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtils
{
    public static int Wrap(int value, int min, int max)
    {
        int n = (value - min) % (max - min);
        return n >= 0 ? n + min : n + max;
    }
}
