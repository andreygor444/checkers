using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pair<T1, T2>
{
    public T1 first { get; private set; }
    public T2 second { get; private set; }
    internal Pair(T1 first, T2 second)
    {
        this.first = first;
        this.second = second;
    }
}
