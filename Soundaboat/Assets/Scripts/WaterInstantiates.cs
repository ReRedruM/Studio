using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterInstantiates{

    public Object _water;
    public List<Object> _islands;

    public WaterInstantiates(List<Object> islands, Object water)
    {
        _water = water;
        _islands = islands;
    }
}
