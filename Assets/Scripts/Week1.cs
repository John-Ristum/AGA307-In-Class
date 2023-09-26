using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Week1 : MonoBehaviour
{
    public GameObject player;

    public int number1 = 3;
    public int number2 = 6;
    public int newNumber;

    void Start()
    {
        //player.transform.position = new Vector3(number2, number1, number2);
        player.GetComponent<Renderer>().material.color = Color.red;
        AddNumbers(number1, number2);
        AddNumbers(9, 11);

        newNumber = AddNumbers2(7, 7);
    }

    int AddNumbers2(int _one, int _two)
    {
        return _one + _two;
    }

    void AddNumbers(int _one, int _two)
    {
        newNumber = _one + _two;
        print(newNumber);
    }

    /*void AddNumbers()
    {
        newNumber = number1 + number2;
        print(newNumber);
    }*/
}
