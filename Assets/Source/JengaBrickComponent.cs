using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[Serializable]
public class BrickData : IComparable
{
    // Default constructor
    public BrickData()
    {
    }

    // Unique identifier for the brick
    public int id;
    // Subject associated with the brick
    public string subject;
    // Grade level of the brick
    public string grade;
    // Mastery level of the brick
    public int mastery;
    // Identifier for the domain of the brick
    public string domainid;
    // Domain to which the brick belongs
    public string domain;
    // Cluster to which the brick belongs
    public string cluster;
    // Identifier for the standard of the brick
    public string standardid;
    // Description of the standard to which the brick adheres
    public string standarddescription;

    // Comparison method for sorting bricks
    public int CompareTo(object obj)
    {
        var a = this;
        var b = obj as BrickData;

        int result = a.domain.CompareTo(b.domain);
        if (result != 0) return result;

        result = a.cluster.CompareTo(b.cluster);
        if (result != 0) return result;

        result = a.standardid.CompareTo(b.standardid);
        if (result != 0) return result;

        return 0;
    }
}

[RequireComponent(typeof(MeshRenderer))]
public class JengaBrickComponent : MonoBehaviour
{
    [Header("Component References")]
    // Reference to the MeshRenderer component
    public MeshRenderer meshRenderer;
    // Reference to the TextMeshProUGUI component
    public TextMeshProUGUI text;
    // Reference to the Rigidbody component
    public Rigidbody rb;

    [Header("Mastery info")]
    // List of materials representing different mastery levels
    public List<Material> masteryMaterials;
    // List of names associated with different mastery levels
    public List<string> masteryNames;
    
    [HideInInspector]
    // The BrickData object associated with this brick
    public BrickData _brickData;

    void Start()
    {
        // Ensure references are set if they are null
        if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
        if (rb == null) rb = GetComponent<Rigidbody>();
    }

    void OnMouseOver()
    {
        // Display brick information when the mouse is over the brick
        FindObjectOfType<GameManager>().textUI.SetText(_brickData.grade + ": " + _brickData.domain + "\n" +
                                                       _brickData.cluster + "\n" + _brickData.standardid + ": " +
                                                       _brickData.standarddescription);
    }

    // Update the appearance based on the brick's mastery level
    public void UpdateMastery()
    {
        if (meshRenderer != null && masteryMaterials.Count > _brickData.mastery)
            meshRenderer.SetMaterials(new List<Material>() { masteryMaterials[_brickData.mastery] });

        if (text != null && masteryNames.Count > _brickData.mastery)
            text.SetText(masteryNames[_brickData.mastery]);
    }
}