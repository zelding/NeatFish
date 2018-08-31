using UnityEngine;
using UnityEngine.UI;
using NeatFish.Simulation.NEAT;
using System.Collections.Generic;

public class Inspector : MonoBehaviour {

    public NeuralNet Brain;

    public GameObject panel;

    private Dictionary<uint, Text> NeuronTexts;

    private void Start()
    {
        NeuronTexts = new Dictionary<uint, Text>();

        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

        foreach (Node n in Brain.Neurons) {
            var o = new GameObject("N " + n.Id.ToString() + " " + n.type);

            o.transform.localPosition = Vector3.zero;
            o.transform.localRotation = Quaternion.identity;

            var t = o.AddComponent<Text>();
            t.rectTransform.localPosition = new Vector3(n.Position.x * 150, 0, n.Position.y * 50);
            t.rectTransform.localRotation = Quaternion.identity;

            t.font = ArialFont;
            t.material = ArialFont.material;
            t.fontSize = 7;
            t.alignment = TextAnchor.UpperLeft;
            t.text = n.GetValue().ToString();

            NeuronTexts.Add(n.Id, t);

            o.transform.SetParent(panel.transform);
        }
    }

    private void OnGUI()
    {
        foreach (Node n in Brain.Neurons) {
            NeuronTexts[n.Id].text = n.GetValue().ToString();
        }
    }
}
