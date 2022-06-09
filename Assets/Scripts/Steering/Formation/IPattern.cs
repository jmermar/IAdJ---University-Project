using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern {
    private Static[] positions;
    private float scale;

    public float Scale {
        get => scale;
        set => scale = Mathf.Max(0.00001f, value);
    }

    public Pattern(Static[] p, float scale = 1f) {
        positions = (Static[]) p.Clone();
        Scale = scale;
    }


    public int GetNumberOfSlots(List<Formation.Assigment> assigments) {
        return assigments.Count;
    }

    public bool SupportSlots(int nSlots) {
        return nSlots <= positions.Length;
    }
    public Static GetSLocation(int slot) {
        Static s = positions[slot];
        s.position *= Scale;
        return s;
    }
    public Static GetDriftOffset(List<Formation.Assigment> assigments) {
        int n = assigments.Count;
        Vector3 position = Vector3.zero;
        Vector3 orientation = Vector3.zero;
        foreach(Formation.Assigment a in assigments) {
            position += GetSLocation(a.number).position;
            orientation += Bodi.AngleToVector(GetSLocation(a.number).orientation);
        }

        position /= n;
        orientation /= n;

        Static center = new Static();
        center.position = position;
        center.orientation = Vector3.SignedAngle(Vector3.forward, orientation, Vector3.up) * Mathf.Rad2Deg;
        return center;
    }
}