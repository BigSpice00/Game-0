using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderGroundChecker : MonoBehaviour
{
    

}

/*public List<SpiderConstraintController> footIKTargets = new List<SpiderConstraintController>(4);

    public Transform skeleton;//the root of the skeleton
    public Transform rig; //the parent of the IK targets

    private Vector3 defaultBodyPosOffset; // the initial body pos offset, stored because the body pos offset can shange
    private Vector3 bodyPosOffset; //the bodyPos offset is the differnece between the average stable feet position and the body position at the start.
    private Quaternion bodyRotOffset; //the body rotation offset is the differnece between the average stable feet rotation and the body rotation at the start.
    private Quaternion rigRotOffset; //the rig rotation offset is the differnece between the average stable feet rotation and the rig rotation at the start.

    private void Update()
    {        
        //transform.Translate(transform.forward);
        BodyControl();
    }
    private void BodyControl() // Simply updates the position and rotation of the body based on the new feet positions
    {
        //Store all foot target positions before moving their parent, which is this transform
        Vector3[] tempTargetPositions = new Vector3[footIKTargets.Count];

        //Calculate the average stable feet position
        Vector3 cummulativeStableFeetPos = Vector3.zero;

        // Calculating the averages
        for (int i = 0; i < footIKTargets.Count; i++)
        {
            tempTargetPositions[i] = footIKTargets[i].position;

            cummulativeStableFeetPos += footIKTargets[i].position;
        }

        Vector3 averageStableFeetPos = cummulativeStableFeetPos / (footIKTargets.Count);

        //The position of the body is the average stable feet positions + the body offset
       // transform.position = bodyPosOffset + averageStableFeetPos;

        //Uncomment the code below to draw a line that shows how the body moves realtive to the feet
        //debugLine.SetPosition(0, averageStableFeetPos);
        //debugLine.SetPosition(1, bodyPosOffset + averageStableFeetPos);

        //Restore foot target positions to cancel out parent's movement
        for (int i = 0; i < footIKTargets.Count; i++)
        {
            footIKTargets[i].position = tempTargetPositions[i];
        }

        RotationControl(); // updates the rotation of the body
    }

    private void RotationControl()
    {
        //Store all foot target positions before moving their parent, which is this transform
        Vector3[] tempTargetPositions = new Vector3[footIKTargets.Count];

        Vector3 cummulativeLeftFeetPos = Vector3.zero;
        Vector3 cummulativeRightFeetPos = Vector3.zero;

        Vector3 cummulativeFrontFeetPos = Vector3.zero;
        Vector3 cummulativeHindFeetPos = Vector3.zero;

        // Calculating the averages
        for (int i = 0; i < footIKTargets.Count; i++)
        {
            tempTargetPositions[i] = footIKTargets[i].position;

            if (footIKTargets[i].limbSide == 1)
            {
                cummulativeLeftFeetPos += footIKTargets[i].position;
            }
            else if (footIKTargets[i].limbSide == 0)
            {
                cummulativeRightFeetPos += footIKTargets[i].position;
            }

            if (i < footIKTargets.Count / 2)
            {
                cummulativeFrontFeetPos += footIKTargets[i].position;
            }
            else if (i >= footIKTargets.Count / 2)
            {
                cummulativeHindFeetPos += footIKTargets[i].position;
            }
        }

        Vector3 averageLeftFeetPos = cummulativeLeftFeetPos / (footIKTargets.Count / 2);
        Vector3 averageRightFeetPos = cummulativeRightFeetPos / (footIKTargets.Count / 2);

        Vector3 averageFrontFeetPos = cummulativeFrontFeetPos / (footIKTargets.Count / 2);
        Vector3 averageHindFeetPos = cummulativeHindFeetPos / (footIKTargets.Count / 2);

        //The azimuth vector is like the z rotation which is gotten by knowing wether the height difference between the average left feet positions and the average right feet positions
        Vector3 azimuthVector = (averageLeftFeetPos - averageRightFeetPos);
        //The elevation vector is like the x rotation which is gotten by knowing wether the height difference between the average front feet positions and the average hind feet positions
        Vector3 elevationVector = (averageFrontFeetPos - averageHindFeetPos);

        float azimuthAngle = Vector3.SignedAngle(transform.right, azimuthVector, transform.forward);
        float elevationAngle = Vector3.SignedAngle(transform.forward, elevationVector, transform.right);

        //Uncomment to three lines below see how the average front and hind feet positions affect the elevation
        //debugLine.SetPosition(0, averageFrontFeetPos);
        //debugLine.SetPosition(1, averageHindFeetPos);
        //Debug.Log("elevation: " + elevationAngle);

        Quaternion azimuth = Quaternion.AngleAxis(azimuthAngle, transform.forward);
        Quaternion elevation = Quaternion.AngleAxis(elevationAngle, transform.right);

        skeleton.rotation = Quaternion.Lerp(skeleton.rotation, azimuth * elevation * bodyRotOffset, Time.deltaTime);
        rig.rotation = Quaternion.Lerp(rig.rotation, azimuth * elevation * rigRotOffset, Time.deltaTime);

        //2 lines below is just some fine tuning, it can be ignored.
        Vector3 afterRotation = rig.up;
        bodyPosOffset = RotateByQuaternion(defaultBodyPosOffset, Quaternion.FromToRotation(Vector3.up, afterRotation));


        //Restore foot target positions to cancel out parent's movement
        for (int i = 0; i < footIKTargets.Count; i++)
        {
            footIKTargets[i].position = tempTargetPositions[i];
        }
    }

    private void GetBodyOffset() // called at the beginning to get both the positional and the rotational offset between the body and the feet.
    {
        Vector3 cummulativeStableFeetPos = Vector3.zero;

        for (int i = 0; i < footIKTargets.Count; i++)
        {
            cummulativeStableFeetPos += footIKTargets[i].position;
        }

        Vector3 averageStableFeetPos = cummulativeStableFeetPos / (footIKTargets.Count);

        defaultBodyPosOffset = transform.position - averageStableFeetPos;

        bodyPosOffset = defaultBodyPosOffset;

        bodyRotOffset = skeleton.rotation;

        rigRotOffset = rig.rotation;
    }

    Vector3 RotateByQuaternion(Vector3 vector, Quaternion quaternion)// A helper function that I wrote to rotate a vector by a quaternion. I almost never use it!
    {
        Quaternion inverseQ = Quaternion.Inverse(quaternion);
        Quaternion compVector = new Quaternion(vector.x, vector.y, vector.z, 0);
        Quaternion qNion = quaternion * compVector * inverseQ;
        return new Vector3(qNion.x, qNion.y, qNion.z);
    }*/
