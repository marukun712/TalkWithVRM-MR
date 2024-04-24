using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomMove : MonoBehaviour
{
    [SerializeField] Transform central;

    [SerializeField] GameObject target;

    private NavMeshAgent agent;
    [SerializeField] float radius = 3;

    float time = 0;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();

        //目標地点に近づいても速度を落とさなくなる
        agent.autoBraking = false;

        gameObject.transform.position = target.transform.position;

        //目標地点を決める
        GotoNextPoint();
    }
    void GotoNextPoint()
    {
        //NavMeshAgentのストップを解除
        agent.isStopped = false;

        //目標地点のX軸、Z軸をランダムで決める
        float posX = Random.Range(-1 * radius, radius);
        float posZ = Random.Range(-1 * radius, radius);

        //CentralPointの位置にPosXとPosZを足す
        Vector3 pos = central.position;
        pos.x += posX;
        pos.z += posZ;

        //NavMeshAgentに目標地点を設定する
        agent.destination = pos;
    }

    void StopHere()
    {
        //NavMeshAgentを止める
        agent.isStopped = true;
        //待ち時間を数える
        time += Time.deltaTime;

        //待ち時間が設定された数値を超えると発動
        if (time > Random.Range(3, 5))
        {
            //目標地点を設定し直す
            GotoNextPoint();
            time = 0;
        }
    }

    void Update()
    {
        //経路探索の準備ができておらず
        //目標地点までの距離が0.5m未満ならNavMeshAgentを止める
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            StopHere();

        //NavMeshAgentのスピードの2乗でアニメーションを切り替える
        anim.SetFloat("Forward", agent.velocity.sqrMagnitude);
    }
}
