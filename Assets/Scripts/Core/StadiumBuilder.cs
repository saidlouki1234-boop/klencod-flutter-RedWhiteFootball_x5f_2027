
using UnityEngine;

namespace RedWhiteFootball.Core
{
    // Builds simple stadium at runtime - no external models needed
    public class StadiumBuilder : MonoBehaviour
    {
        public Material grassMat;
        public Material lineMat;
        public Material redMat;
        public Material whiteMat;

        void Awake() { Build(); }

        void Build()
        {
            // Ground
            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.transform.position = Vector3.zero;
            ground.transform.localScale = new Vector3(7f,1f,9f);
            ground.name = "Ground_Grass";
            if (grassMat) ground.GetComponent<Renderer>().material = grassMat;

            // Field lines via cubes
            CreateLine(new Vector3(0,0.02f,0), new Vector3(50,0.1f,70), "FieldBounds"); // simplified

            // Goals
            CreateGoal(new Vector3(0,1.5f,52f), false);
            CreateGoal(new Vector3(0,1.5f,-52f), true);

            // Walls
            var wallN = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wallN.transform.position = new Vector3(0,2.5f,58f); wallN.transform.localScale = new Vector3(80,5,1); wallN.name="WallN";
            var wallS = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wallS.transform.position = new Vector3(0,2.5f,-58f); wallS.transform.localScale = new Vector3(80,5,1); wallS.name="WallS";
        }

        void CreateLine(Vector3 pos, Vector3 scale, string name)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.position = pos;
            go.transform.localScale = scale;
            var c = go.GetComponent<Collider>(); if(c) Destroy(c);
        }

        void CreateGoal(Vector3 center, bool isTeamA)
        {
            var root = new GameObject(isTeamA?"GoalA":"GoalB");
            root.transform.position = center;
            root.tag = "Untagged";
            var trigger = new GameObject("GoalTrigger");
            trigger.transform.parent = root.transform;
            trigger.transform.localPosition = Vector3.zero;
            trigger.transform.localScale = new Vector3(7.32f, 2.44f, 2f);
            var col = trigger.AddComponent<BoxCollider>();
            col.isTrigger = true;
            var gs = trigger.AddComponent<GoalSystem>();
            gs.isTeamAGoal = isTeamA;

            // Posts
            var postL = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            postL.transform.parent = root.transform;
            postL.transform.localPosition = new Vector3(-3.66f,0,0);
            postL.transform.localScale = new Vector3(0.15f,1.22f,0.15f);
            var postR = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            postR.transform.parent = root.transform;
            postR.transform.localPosition = new Vector3(3.66f,0,0);
            postR.transform.localScale = new Vector3(0.15f,1.22f,0.15f);
            var cross = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cross.transform.parent = root.transform;
            cross.transform.localPosition = new Vector3(0,2.44f,0);
            cross.transform.localScale = new Vector3(7.6f,0.15f,0.15f);
        }
    }
}
