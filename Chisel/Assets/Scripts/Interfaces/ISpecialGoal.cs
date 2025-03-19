using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpecialGoal
{
    // called when the special level starts so the goal can set up references, moves counters, and custom levels.
    void InitializeGoal();

    // returns true if the goal has been completed (required blocks broken, boss HP = 0, score >= scoreCap, comboMultiplier > comboMultCap)
    bool IsGoalMet();

    // does things when the goal is completed (remove assets, win screen, move to next level... etc)
    void OnGoalComplete();
    // each goal needs a description for tasks text.
    string GetGoalDescription();

}
