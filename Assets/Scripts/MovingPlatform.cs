using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingPlatform : MonoBehaviour
{
   [SerializeField] private Transform posA, posB;
   [SerializeField] private int Speed;
   [SerializeField] private Vector2 targetPos;

   void Start()
   {
      targetPos = posB.position;
   }

   void Update()
   {
      if(Vector2.Distance(transform.position, posA.position) <.1f) targetPos = posB.position;

      if(Vector2.Distance(transform.position, posB.position) <.1f) targetPos = posA.position;

      transform.position=Vector2.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);

   }

  
 


}

