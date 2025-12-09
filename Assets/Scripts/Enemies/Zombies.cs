using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XR;

public class Zombies : MonoBehaviour
{
    private Animator animator;
    private CharacterController controller;
    
    public float transitionTime = 0.25f;
    public int health = 3;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // timer start
    }

    void Update()
    {
        /*every time update is called, timer ticks by
        every 10 seconds, spawn rate is upped by 0.5f*/

        /*Zombie is idle
        if (player gets too close to zombie) {
            Zombie animation switches to walking
            zombie nav mesh starts chasing player
        }*/

        /*a new zombie is spawned from a random spawn point every X seconds
        as the time ticks by, the spawn rate goes up*/

        // If a zombie reaches the player, switch to bite animation

       /* If (a zombie gets shot) {
            activate shot animation once
            zombies health goes down by one
            if (Zombies health reaches 2, and it is close enough) {
                switch to jump animation
            }
            if (Zombies health reaches 0) {
                switch to zombie dies animation
                stop movement
                after X seconds, zombies body disappears
            }
        }*/
    }
}
