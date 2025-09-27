//-----------------------------------------------------------------------
// <copyright file="ObjectController.cs" company="Google LLC">
// Copyright 2020 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections;
using UnityEngine;

/// <summary>
/// Controls target object's behavior for gaze-based interaction.
/// </summary>
public class ObjectController : MonoBehaviour
{
    public Material InactiveMaterial;
    public Material GazedAtMaterial;

    private const float _minObjectDistance = 2.5f;
    private const float _maxObjectDistance = 3.5f;
    private const float _minObjectHeight = 0.5f;
    private const float _maxObjectHeight = 3.5f;

    private Renderer _myRenderer;
    private Vector3 _startingPosition;

    void Start()
    {
        _startingPosition = transform.parent.localPosition;
        _myRenderer = GetComponent<Renderer>();
        SetMaterial(false);
    }

    /// <summary>
    /// Called when gaze enters this object.
    /// </summary>
    public void OnPointerEnterXR()
    {
        SetMaterial(true);
    }

    /// <summary>
    /// Called when gaze exits this object.
    /// </summary>
    public void OnPointerExitXR()
    {
        SetMaterial(false);
    }

    /// <summary>
    /// Called when object is clicked while being gazed at.
    /// </summary>
    public void OnPointerClickXR()
    {
        TeleportRandomly();
    }

    /// <summary>
    /// Teleports the object randomly within predefined bounds.
    /// </summary>
    public void TeleportRandomly()
    {
        int sibIdx = transform.GetSiblingIndex();
        int numSibs = transform.parent.childCount;
        sibIdx = (sibIdx + Random.Range(1, numSibs)) % numSibs;
        GameObject randomSib = transform.parent.GetChild(sibIdx).gameObject;

        float angle = Random.Range(-Mathf.PI, Mathf.PI);
        float distance = Random.Range(_minObjectDistance, _maxObjectDistance);
        float height = Random.Range(_minObjectHeight, _maxObjectHeight);
        Vector3 newPos = new Vector3(Mathf.Cos(angle) * distance, height,
                                     Mathf.Sin(angle) * distance);

        transform.parent.localPosition = newPos;

        randomSib.SetActive(true);
        gameObject.SetActive(false);
        SetMaterial(false);
    }

    /// <summary>
    /// Changes material based on gaze status.
    /// </summary>
    private void SetMaterial(bool gazedAt)
    {
        if (_myRenderer != null && InactiveMaterial != null && GazedAtMaterial != null)
        {
            _myRenderer.material = gazedAt ? GazedAtMaterial : InactiveMaterial;
        }
    }
}
