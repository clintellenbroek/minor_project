using System.Collections.Generic;
using UnityEngine;

public class SituationTrigger : MonoBehaviour
{
    public List<Situation> situations;

    public void TriggerSituation(int index)
    {
        Situation situation = situations[index];

        Debug.Log(situation.title);
        Debug.Log(situation.description);
    }
}