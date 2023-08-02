using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnhancable 
{
    bool TryEnhance(uint darkForceCount);
    float CalculateSuccessRate(uint darkForceCount);
}
