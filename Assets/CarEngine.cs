using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngine : MonoBehaviour
{
    public Transform path;
    public float maxSteerAngle = 10f;
    public float _motorForce = 1500f;
    public float steerAngl;
    public float newSteer;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    float h,v;

    private List<Transform> nodes;
    private int currectNode = 0;

    void Start(){
      Transform[] pathTransforms =
      path.GetComponentsInChildren<Transform>();
      nodes = new List <Transform>();

      for (int i = 0; i< pathTransforms.Length; i++){
        if (pathTransforms[i] != path.transform){
          nodes.Add(pathTransforms[i]);
        }
      }

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
      //Inputs();
      ApplySteer();
      //Drive();
      CheckWaypointDistance();
    }

    void Inputs()
    {
      h = Input.GetAxis("Horizontal");
      v = Input.GetAxis("Vertical");
      print(h);
    }

    private void ApplySteer(){
    //  if (Input.GetKeyUp(KeyCode.UpArrow)){
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currectNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude)*maxSteerAngle;
        wheelFL.steerAngle= newSteer;
        wheelFR.steerAngle= newSteer;
        wheelFL.motorTorque = _motorForce;
        wheelFR.motorTorque = _motorForce;
      }
    //   } else {
    //
    //   steerAngl = maxSteerAngle * h;
    //   wheelFR.steerAngle = steerAngl;
    //   wheelFL.steerAngle = steerAngl;
    //   wheelFL.motorTorque = v * _motorForce;
    //   wheelFR.motorTorque = v * _motorForce;
    //   }
    // }


    // private void Drive() {
    //   // if (Input.GetKeyUp(KeyCode.UpArrow))
    //   //   {
    //     // wheelFL.motorTorque = v * _motorForce;
    //     // wheelFR.motorTorque = v * _motorForce;
    // }

    private void CheckWaypointDistance() {
      print(Vector3.Distance(transform.position, nodes[currectNode].position));
      if (Vector3.Distance(transform.position, nodes[currectNode].position) < 1.1f) {
        if(currectNode == nodes.Count - 1) {
          currectNode = 0;
        } else {
          currectNode++;
        }
      }
    }
}
