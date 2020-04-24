using UnityEngine;
using UnityEngine.UI;
using MLAgents;
using MLAgents.Sensors;

public class Robot : Agent
{
    private Rigidbody Robot_rig, Ball_rig;
    [Header("速度"), Range(1, 1000)]
    public float speed;
    private Animator ani;


    private void Start()
    {
        ani = GetComponent<Animator>();
        Robot_rig = GetComponent<Rigidbody>();
        Ball_rig = GameObject.Find("Ball").GetComponent<Rigidbody>();
    }
    /// <summary>
    /// 每次開始時重新設定人跟球的位置
    /// </summary>
    public override void OnEpisodeBegin()
    {
        //把加速度跟角度加速度歸零
        Robot_rig.velocity = Vector3.zero;
        Robot_rig.angularVelocity = Vector3.zero;
        Ball_rig.velocity = Vector3.zero;
        Ball_rig.angularVelocity = Vector3.zero;
        //隨機出現人的位置
        Vector3 posRobot = new Vector3(Random.Range(-2f, 2f), 0.06f, Random.Range(-2f, 1.5f));
        transform.position = posRobot;
        //隨機出現球的位置
        Vector3 posBall = new Vector3(1f, 0.06f, 0f);
        Ball_rig.position = posBall;

        Ball.conplate = false;
    }
    /// <summary>
    /// 收集資料
    /// </summary>
    /// <param name="sensor"></param>
    public override void CollectObservations(VectorSensor sensor)
    {
        //加入觀測資料
        sensor.AddObservation(transform.position);
        sensor.AddObservation(Ball_rig.position);
        sensor.AddObservation(Robot_rig.velocity.x);
        sensor.AddObservation(Robot_rig.velocity.z);
    }
    public override void OnActionReceived(float[] vectorAction)
    {
        Vector3 control = Vector3.zero;
        control.x = vectorAction[0];
        control.z = vectorAction[1];
        Robot_rig.AddForce(control * speed);


        if (Ball.conplate)
        {
            SetReward(1);
            EndEpisode();
        }
        if (transform.position.y < 0 || Ball_rig.position.y < 0)
        {
            SetReward(-1);
            EndEpisode();
        }
    }
    public override float[] Heuristic()
    {
        var action = new float[2];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        return action;
    }
}
