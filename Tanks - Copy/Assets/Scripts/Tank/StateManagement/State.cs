using UnityEngine;
using System.Collections;
using System;

public interface State
{
    void updateState();
    void executeState();
}
