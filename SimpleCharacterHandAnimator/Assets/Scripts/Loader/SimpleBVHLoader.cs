using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Winterdust;

namespace SimpleCharacterHandAnimator
{
    public class SimpleBVHLoader : MonoBehaviour
    {
        [SerializeField] private string inputBVHFile;

        private AnimationClip loadedClip;

        [SerializeField] private Animation anim;


        [SerializeField] int nBones;
        [SerializeField] int nFramesCount;
        [SerializeField] double animationDuration;
        [SerializeField] BVH.BVHBone[] bones;



        EditorCurveBinding[] editorCurvesToWrite;
        AnimationCurve[] animatedCurvesToWrite;

        private bool readyToPlay = false;

        void Start()
        {
            StartCoroutine(Load());
        }

        private IEnumerator Load()
        {
            readyToPlay = false;

            yield return new WaitForSeconds(1.0f);

            BVH winterdustBvh = new BVH(inputBVHFile, -10, false, 0, -1, false, true, true, true, null);

            yield return new WaitForEndOfFrame();

            nBones = winterdustBvh.boneCount;

            bones = winterdustBvh.allBones;

            nFramesCount = winterdustBvh.frameCount;
            animationDuration = winterdustBvh.getDurationSec();

            yield return new WaitForSeconds(1.0f);

            loadedClip = winterdustBvh.makeAnimationClip();

            yield return new WaitForEndOfFrame();

            yield return new WaitForSeconds(1.0f);

            loadedClip.legacy = true;

            //anim = targetAvatar.gameObject.GetComponent<Animation>();

            anim.AddClip(loadedClip, loadedClip.name);
            anim.clip = loadedClip;
            anim.playAutomatically = false;


#if UNITY_EDITOR
            //AssetDatabase.CreateAsset(newClip, "Assets/" + newClip + ".anim");
            // AssetDatabase.SaveAssets();


#endif

            readyToPlay = true;

            Debug.Log("<color=cyan>" + "READY TO PLAY" + "</color>");
        }

        [SerializeField] string folderFileName = "D:\\Download\\WinterdustTest";

        private IEnumerator WriteClipToDirectory()
        {
            editorCurvesToWrite = AnimationUtility.GetCurveBindings(loadedClip);

            string binaryFileName = Path.Combine(folderFileName, loadedClip.name + ".binaryAnim");

            if (File.Exists(binaryFileName))
            {
                File.Delete(binaryFileName);
            }

            // Read animation clip and save data in memory
            editorCurvesToWrite = AnimationUtility.GetCurveBindings(loadedClip);

            animatedCurvesToWrite = new AnimationCurve[editorCurvesToWrite.Length];
            for (int i = 0; i < editorCurvesToWrite.Length; i++)
            {
                animatedCurvesToWrite[i] = AnimationUtility.GetEditorCurve(loadedClip, editorCurvesToWrite[i]);
            }
            Debug.Log("<color=cyan>" + " Animated Curves to write: " + animatedCurvesToWrite.Length + "</color>");
            yield return new WaitForEndOfFrame();

            using (BinaryWriter bWritter = new BinaryWriter(File.Open(binaryFileName, FileMode.Create), Encoding.Default))
            {
                int nEditorCurves = editorCurvesToWrite.Length;

                Debug.Log("<color=cyan>" + "Total curves to write " + nEditorCurves + "</color>");

                bWritter.Write(nEditorCurves);

                for (int i = 0; i < nEditorCurves; ++i)
                {
                    Keyframe[] keys = animatedCurvesToWrite[i].keys;

                    Debug.Log("<color=cyan>" + "Curve: " + (i + 1) + "/" + nEditorCurves + " - (Keys:  " + keys.Length + ")" + "</color>");

                    bWritter.Write(editorCurvesToWrite[i].path);
                    bWritter.Write(editorCurvesToWrite[i].propertyName);

                    bWritter.Write(keys.Length);

                    for (int k = 0; k < keys.Length; ++k)
                    {
                        bWritter.Write(keys[k].time);
                        bWritter.Write(keys[k].value);
                    }
                }

                Debug.Log("<color=cyan>" + "Binary Completed: " + binaryFileName + " - Curves: " + nEditorCurves + binaryFileName + "</color>");
            }

            yield return new WaitForEndOfFrame();


        }

            // Update is called once per frame
        void Update()
        {
            if (readyToPlay)
            {
                if (Input.GetKeyDown(KeyCode.P))
                {
                    readyToPlay = false;

                    anim.Play(loadedClip.name);

                }
            }
        }
    }
}
