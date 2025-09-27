using UnityEngine;
using UnityEngine.UI;

public class MachineProgress : MonoBehaviour
{
    [Header("References")]
    public CoffeeMachine machine;
    public Image progressFill;        // assign your ProgressFill image
    public GameObject progressRoot;   // the whole bar group, so we can hide it

    void Start()
    {
        if (machine != null)
        {
            machine.onProgress += OnProgress;
            machine.onProcessingChanged += OnProcessingChanged;
        }

        if (progressRoot != null)
            progressRoot.SetActive(false);
    }

    void OnProgress(float value)
    {
        if (progressFill != null)
            progressFill.fillAmount = value;
    }

    void OnProcessingChanged(bool started)
    {
        if (progressRoot != null)
            progressRoot.SetActive(started);

        if (!started && progressFill != null)
            progressFill.fillAmount = 0f;
    }

    void OnDestroy()
    {
        if (machine != null)
        {
            machine.onProgress -= OnProgress;
            machine.onProcessingChanged -= OnProcessingChanged;
        }
    }
}
