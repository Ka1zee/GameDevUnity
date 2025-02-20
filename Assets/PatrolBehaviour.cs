using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class PatrolBehaviour : StateMachineBehaviour
{
    float timer;
    List<Transform> points = new List<Transform>();
    NavMeshAgent agent;
    Transform player;
    float chaseRange = 5;

    string patrolTag; // Беремо тег з компонента бота

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;

        // Отримуємо PatrolTag з компонента бота (наприклад, PatrolSettings)
        patrolTag = animator.GetComponent<PatrolSettings>().patrolTag;

        GameObject[] pointObjects = GameObject.FindGameObjectsWithTag(patrolTag);
        foreach (GameObject obj in pointObjects)
        {
            points.Add(obj.transform);
        }

        if (points.Count == 0)
        {
            Debug.LogError("No patrol points found for bot with tag: " + patrolTag);
            return;
        }

        agent = animator.GetComponent<NavMeshAgent>();
        agent.SetDestination(points[Random.Range(0, points.Count)].position);

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (points.Count == 0) return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(points[Random.Range(0, points.Count)].position);
        }

        timer += Time.deltaTime;
        if (timer > 10)
        {
            animator.SetBool("Ispatroling", false);
        }

        float distance = Vector3.Distance(animator.transform.position, player.position);
        if (distance < chaseRange)
        {
            animator.SetBool("isChasing", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent != null)
        {
            agent.SetDestination(agent.transform.position);
        }
    }
}
