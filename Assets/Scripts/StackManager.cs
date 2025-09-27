using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StackManager : MonoBehaviour
{
    [Header("Stack")]
    public Transform stackRoot;
    public GameObject bagPrefab;
    public float bagHeight = 0.18f;
    public int maxBags = 3;

    [Header("Tween (optional visual)")]
    public float tweenDuration = 0.22f;
    public AnimationCurve addEasing = AnimationCurve.EaseInOut(0, 0, 1, 1);

    List<GameObject> bags = new List<GameObject>();
    public int BagCount => bags.Count;

    public bool TryAddBag()
    {
        if (bags.Count >= maxBags) return false;
        var b = Instantiate(bagPrefab, stackRoot);
        b.transform.localPosition = Vector3.up * (bags.Count * bagHeight);
        b.transform.localRotation = Quaternion.identity;
        bags.Add(b);
        return true;
    }

    public bool TryAddBagFrom(Vector3 worldStartPos)
    {
        if (bags.Count >= maxBags) return false;
        GameObject b = Instantiate(bagPrefab);
        b.transform.position = worldStartPos;
        StartCoroutine(DoTweenToStack(b));
        return true;
    }

    IEnumerator DoTweenToStack(GameObject bag)
    {
        Vector3 targetLocal = Vector3.up * (bags.Count * bagHeight);
        Vector3 startWorld = bag.transform.position;
        Vector3 targetWorld = stackRoot.TransformPoint(targetLocal);
        Quaternion startRot = bag.transform.rotation;
        Quaternion targetRot = stackRoot.rotation;

        float t = 0f;
        while (t < tweenDuration)
        {
            t += Time.deltaTime;
            float f = addEasing.Evaluate(Mathf.Clamp01(t / tweenDuration));
            bag.transform.position = Vector3.Lerp(startWorld, targetWorld, f);
            bag.transform.rotation = Quaternion.Slerp(startRot, targetRot, f);
            yield return null;
        }

        bag.transform.SetParent(stackRoot, worldPositionStays: false);
        bag.transform.localPosition = targetLocal;
        bag.transform.localRotation = Quaternion.identity;
        bags.Add(bag);
    }

    public GameObject RemoveTopBag()
    {
        if (bags.Count == 0) return null;
        var top = bags[bags.Count - 1];
        bags.RemoveAt(bags.Count - 1);
        top.transform.SetParent(null);
        return top;
    }

    public void RemoveAndDestroyTopBag()
    {
        var top = RemoveTopBag();
        if (top) Destroy(top);
    }
}
