using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SimpleCharacterHandAnimator
{
    public class RecordManualAnimation : MonoBehaviour
    {
        [SerializeField] private SkeletonMapper skeleton;
        [SerializeField] private Animator skeletonAnimator;

        [SerializeField] private BVHTools.BVHRecorder bvhRecorder;

        public int framesCaptured { get; private set; }

        public bool isReadyToRecordFrame = false;

        public string motionCaptureDataDirectory;
        

        void Start()
        {           
            StartCoroutine(Initialize());
        }


        private IEnumerator Initialize()
        {
            isReadyToRecordFrame = false;

            yield return new WaitForSeconds(3.0f);

            skeleton.GenerateBoneMap(skeletonAnimator);

            yield return new WaitForEndOfFrame();

            bvhRecorder.targetAvatar = skeletonAnimator;
            bvhRecorder.enforceHumanoidBones = true;
            bvhRecorder.scripted = true;

            yield return new WaitForSeconds(1.0f);

            // Retarget bones
            bvhRecorder.bones = new List<Transform>();
            for (int i = 0; i < skeleton.BoneNumber; i++)
            {
                HumanoidBone bone = skeleton.GetBoneByIndex(i);
                if (bone != null)
                {
                    bvhRecorder.bones.Add(bone.BoneTransform);
                }
            }

            yield return new WaitForSeconds(1.0f);

            bvhRecorder.buildSkeleton();
            bvhRecorder.genHierarchy();

            bvhRecorder.blender = false;

            yield return new WaitForEndOfFrame();

            bvhRecorder.targetAvatar = skeletonAnimator;
            bvhRecorder.enforceHumanoidBones = true;
            bvhRecorder.scripted = true;

            // Retarget bones
            bvhRecorder.bones = new List<Transform>();
            for (int i = 0; i < skeleton.BoneNumber; i++)
            {
                HumanoidBone bone = skeleton.GetBoneByIndex(i);
                if (bone != null)
                {
                    bvhRecorder.bones.Add(bone.BoneTransform);
                }
            }

            // Always record full body
            bvhRecorder.InitializeBonesToRecord(skeleton.fullBodyBones);
            bvhRecorder.buildSkeleton();
            bvhRecorder.genHierarchy();

            framesCaptured = 0;
            isReadyToRecordFrame = true;
        }

        public void SaveFile()
        {
            isReadyToRecordFrame = false;

            string time = DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
            string clipName = "session_" + DateTime.Now.ToString("dd MM yyyy_HH mm ss");
            string extensionBVH = ".bvh";

            string directoryName = clipName;

            string sessionPath = Path.Combine(motionCaptureDataDirectory, directoryName);
            Directory.CreateDirectory(sessionPath);

            string lastSessionFile = Path.Combine(sessionPath, clipName + extensionBVH);

            bvhRecorder.directory = sessionPath;
            bvhRecorder.filename = lastSessionFile;
            bvhRecorder.saveBVH();


            string bacFileName = Path.Combine(sessionPath, clipName + ".bac");
            string bvhFileToSerialize = Path.Combine(sessionPath, clipName + ".bvh");

            //bool serializeBAC = AnimationClipBinaryWritter.Serialize(bvhFileToSerialize, bacFileName, skeleton);

            //Debug.Log("<color=cyan>" + "Serialized Result: " + serializeBAC + "</color>");

            
        }
       
        public void CaptureFrames()
        {
            isReadyToRecordFrame = false;

            framesCaptured++;
            Debug.Log("<color=cyan>" + framesCaptured + "Frames Captured " + "</color>");

            bvhRecorder.captureFrame();
           
            isReadyToRecordFrame = true;
        }        

        private void Update()
        {
            if (!isReadyToRecordFrame) return;

            if (Input.GetKeyUp(KeyCode.R))
            {
                CaptureFrames();
            }
        }

    }
}
