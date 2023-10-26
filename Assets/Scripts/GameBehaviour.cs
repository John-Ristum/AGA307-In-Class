using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    protected static GameManager _GM { get { return GameManager.INSTANCE; } }
    protected static EnemyManager _EM { get { return EnemyManager.INSTANCE; } }


    /// <summary>
    /// Scales all objects in a list to a new scale
    /// </summary>
    /// <param name="_objects">The list of objects</param>
    /// <param name="_scale">The new scale we wish to scale to</param>
    public void ScaleObjects(List<GameObject> _objects, Vector3 _scale)
    {
        for(int i = 0; i < _objects.Count; i++)
        {
            _objects[i].transform.localScale = _scale;
        }
    }

    /// <summary>
    /// Scales all objects in an array to a new scale
    /// </summary>
    /// <param name="_objects">The list of objects</param>
    /// <param name="_scale">The new scale we wish to scale to</param>
    public void ScaleObjects(GameObject[] _objects, Vector3 _scale)
    {
        for (int i = 0; i < _objects.Length; i++)
        {
            _objects[i].transform.localScale = _scale;
        }
    }

    /// <summary>
    /// Scales aan objects to a new scale
    /// </summary>
    /// <param name="_objects">The list of objects</param>
    /// <param name="_scale">The new scale we wish to scale to</param>

    public void ScaleObject(GameObject _object, Vector3 _scale)
    {
        _object.transform.localScale = _scale;
    }
}
