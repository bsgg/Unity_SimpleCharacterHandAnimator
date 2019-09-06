using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleCharacterHandAnimator
{
    public class HandModifier : MonoBehaviour
    {
        private Animator animator;

        [Header("Override Hands Rotation")]
        [SerializeField] private bool overrideHandRotation = false;
        [SerializeField] private Vector3 leftHandRotation = new Vector3(0.0f, 0.0f, 0.0f);
        [SerializeField] private Vector3 rightHandRotation = new Vector3(0.0f, 0.0f, 0.0f);
        private Transform leftHand;
        private Transform rightHand;


        private List<Transform> lhProximalBones;
        private List<Transform> rhProximalBones;

        private List<Transform> lhIntermediateBones;
        private List<Transform> rhIntermediateBones;

        private List<Transform> lhDistalBones;
        private List<Transform> rhDistalBones;

        [Header("Left Finger Rotation (Mirror for the other right hand)")]

        [SerializeField] private Vector3 lhProximalRotation = new Vector3(0.0f, 0.0f, 15.0f);

        [SerializeField] private Vector3 lhIntermediateRotation = new Vector3(0.0f, 0.0f, 30.0f);

        [SerializeField] private Vector3 lhDistalRotation = new Vector3(0.0f, 0.0f, 15.0f);


        [Header("Left Thumb Finger Bone Rotation (Mirror for the other right hand)")]

        [SerializeField] private Vector3 lhThumbProximalRotation = new Vector3(-7.0f, 6.0f, 0.0f);
        private Transform lhThumbProximal;

        private Transform rhThumbProximal;

        [SerializeField] private Vector3 lhThumbIntermediateRotation = new Vector3(0.0f, 30.0f, 0.0f);
        private Transform lhThumbIntermediate;
        private Transform rhThumbIntermediate;

        [SerializeField] private Vector3 lhThumbDistalRotation = new Vector3(0.0f, -18.0f, 0.0f);
        private Transform lhThumbDistal;
        private Transform rhThumbDistal;


        void Awake()
        {
            animator = GetComponent<Animator>();

            if (animator == null) return;

            leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
            rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);


            lhProximalBones = new List<Transform>();
            rhProximalBones = new List<Transform>();            
            
            // Proximal finger bones (left and right hand)
            lhProximalBones.Add(animator.GetBoneTransform(HumanBodyBones.LeftIndexProximal));
            lhProximalBones.Add(animator.GetBoneTransform(HumanBodyBones.LeftMiddleProximal));
            lhProximalBones.Add(animator.GetBoneTransform(HumanBodyBones.LeftRingProximal));
            lhProximalBones.Add(animator.GetBoneTransform(HumanBodyBones.LeftLittleProximal));
            lhThumbProximal = animator.GetBoneTransform(HumanBodyBones.LeftThumbProximal);  

            rhProximalBones.Add(animator.GetBoneTransform(HumanBodyBones.RightIndexProximal));
            rhProximalBones.Add(animator.GetBoneTransform(HumanBodyBones.RightMiddleProximal));
            rhProximalBones.Add(animator.GetBoneTransform(HumanBodyBones.RightRingProximal));
            rhProximalBones.Add(animator.GetBoneTransform(HumanBodyBones.RightLittleProximal));
            rhThumbProximal = animator.GetBoneTransform(HumanBodyBones.RightThumbProximal);

            // Intermediate finger bones
            lhIntermediateBones = new List<Transform>();
            rhIntermediateBones = new List<Transform>();

            lhIntermediateBones.Add(animator.GetBoneTransform(HumanBodyBones.LeftIndexIntermediate));
            lhIntermediateBones.Add(animator.GetBoneTransform(HumanBodyBones.LeftMiddleIntermediate));
            lhIntermediateBones.Add(animator.GetBoneTransform(HumanBodyBones.LeftRingIntermediate));
            lhIntermediateBones.Add(animator.GetBoneTransform(HumanBodyBones.LeftLittleIntermediate));
            lhThumbIntermediate = animator.GetBoneTransform(HumanBodyBones.LeftThumbIntermediate);

            rhIntermediateBones.Add(animator.GetBoneTransform(HumanBodyBones.RightIndexIntermediate));
            rhIntermediateBones.Add(animator.GetBoneTransform(HumanBodyBones.RightMiddleIntermediate));
            rhIntermediateBones.Add(animator.GetBoneTransform(HumanBodyBones.RightRingIntermediate));
            rhIntermediateBones.Add(animator.GetBoneTransform(HumanBodyBones.RightLittleIntermediate));
            rhThumbIntermediate = animator.GetBoneTransform(HumanBodyBones.RightThumbIntermediate);


            // Distal bones
            lhDistalBones = new List<Transform>();
            rhDistalBones = new List<Transform>();

            lhDistalBones.Add(animator.GetBoneTransform(HumanBodyBones.LeftIndexDistal));
            lhDistalBones.Add(animator.GetBoneTransform(HumanBodyBones.LeftMiddleDistal));
            lhDistalBones.Add(animator.GetBoneTransform(HumanBodyBones.LeftRingDistal));
            lhDistalBones.Add(animator.GetBoneTransform(HumanBodyBones.LeftLittleDistal));
            lhThumbDistal = animator.GetBoneTransform(HumanBodyBones.LeftThumbDistal);

            rhDistalBones.Add(animator.GetBoneTransform(HumanBodyBones.RightIndexDistal));
            rhDistalBones.Add(animator.GetBoneTransform(HumanBodyBones.RightMiddleDistal));
            rhDistalBones.Add(animator.GetBoneTransform(HumanBodyBones.RightRingDistal));
            rhDistalBones.Add(animator.GetBoneTransform(HumanBodyBones.RightLittleDistal));
            rhThumbDistal = animator.GetBoneTransform(HumanBodyBones.RightThumbDistal);
        }

        private void LateUpdate()
        {
            if (animator == null) return;

            // Override Hand Rotation
            if (overrideHandRotation)
            {
                leftHand.localRotation = Quaternion.Euler(new Vector3(leftHandRotation.x, leftHandRotation.y, leftHandRotation.z));
                rightHand.localRotation = Quaternion.Euler(new Vector3(rightHandRotation.x, rightHandRotation.y, rightHandRotation.z));
            }

            // Update leftHand Proximal
            for (int i=0; i< lhProximalBones.Count; i++)
            {
                lhProximalBones[i].localRotation = Quaternion.Euler(lhProximalRotation); 
            }

            lhThumbProximal.localRotation = Quaternion.Euler(lhThumbProximalRotation); 

            // Mirror the other hand (oposite angles)           
            for (int i = 0; i < rhProximalBones.Count; i++)
            {
                rhProximalBones[i].localRotation = Quaternion.Euler(new Vector3(-lhProximalRotation.x, -lhProximalRotation.y, -lhProximalRotation.z)); 
            }

            rhThumbProximal.localRotation = Quaternion.Euler(new Vector3(-lhThumbProximalRotation.x, -lhThumbProximalRotation.y, -lhThumbProximalRotation.z));

            // Update leftHand Intermediate
            for (int i = 0; i < lhIntermediateBones.Count; i++)
            {
                lhIntermediateBones[i].localRotation = Quaternion.Euler(lhIntermediateRotation);
            }
            lhThumbIntermediate.localRotation = Quaternion.Euler(lhThumbIntermediateRotation);

            // Mirror the other hand (oposite angles)       
            for (int i = 0; i < rhIntermediateBones.Count; i++)
            {
                rhIntermediateBones[i].localRotation = Quaternion.Euler(new Vector3(-lhIntermediateRotation.x, -lhIntermediateRotation.y, -lhIntermediateRotation.z));
            }
            lhThumbIntermediate.localRotation = Quaternion.Euler(new Vector3(-lhThumbIntermediateRotation.x, -lhThumbIntermediateRotation.y, -lhThumbIntermediateRotation.z));


            // Distal bones
            for (int i = 0; i < lhDistalBones.Count; i++)
            {
                lhDistalBones[i].localRotation = Quaternion.Euler(lhDistalRotation);
            }

            lhThumbDistal.localRotation = Quaternion.Euler(lhThumbDistalRotation);

            // Mirror the other hand (oposite angles) 
            for (int i = 0; i < rhDistalBones.Count; i++)
            {
                rhDistalBones[i].localRotation = Quaternion.Euler(new Vector3(-lhDistalRotation.x, -lhDistalRotation.y, -lhDistalRotation.z));
            }

            rhThumbDistal.localRotation = Quaternion.Euler(new Vector3(-lhThumbDistalRotation.x, -lhThumbDistalRotation.y, -lhThumbDistalRotation.z));

        }

    }
}
