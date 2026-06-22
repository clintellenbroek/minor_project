using UnityEngine;

public class SituationTrigger : MonoBehaviour
{
    public Situation situation;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

            hasTriggered = true;
            TriggerSituation();

    }

    void TriggerSituation()
    {
        Debug.Log("Vraag: " + situation.title);
        Debug.Log("Beschrijving: " + situation.description);

        for (int i = 0; i < situation.choices.Length; i++)
        {
            Debug.Log(situation.choices[i].text + " kost " + situation.choices[i].waterCost + " water");
        }

        // Later kan je hier je UI openen:
        // SituationUI.instance.ShowSituation(situation);
    }
}