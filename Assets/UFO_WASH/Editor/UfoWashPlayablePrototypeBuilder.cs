#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.Build;

namespace UFOWASH.Editor
{
    public static class UfoWashPlayablePrototypeBuilder
    {
        [InitializeOnLoadMethod]
        static void AutoCreatePlayablePrototype()
        {
            EditorApplication.delayCall += () =>
            {
                if (!EditorApplication.isPlaying && !System.IO.File.Exists(ScenePath))
                    BuildPrototypeInternal(false);
            };
        }
        const string ScenePath="Assets/UFO_WASH/Scenes/UFO_WASH_Playable.unity";
        [MenuItem("UFO WASH/BUILD PLAYABLE CLEANING PROTOTYPE")]
        public static void BuildPrototype() { BuildPrototypeInternal(true); }
        static void BuildPrototypeInternal(bool showDialog)
        {
            var previousScene=SceneManager.GetActiveScene();
            var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Additive);
            SceneManager.SetActiveScene(scene);
            RenderSettings.ambientLight=new Color(.28f,.31f,.38f);
            Make("Floor",PrimitiveType.Cube,new Vector3(0,-.15f,2),new Vector3(22,.3f,28),new Color(.12f,.16f,.2f));
            Make("Back Wall",PrimitiveType.Cube,new Vector3(0,4,15),new Vector3(22,8,.5f),new Color(.07f,.1f,.16f));
            Make("Left Wall",PrimitiveType.Cube,new Vector3(-11,4,2),new Vector3(.5f,8,26),new Color(.09f,.13f,.2f));
            Make("Right Wall",PrimitiveType.Cube,new Vector3(11,4,2),new Vector3(.5f,8,26),new Color(.09f,.13f,.2f));
            Make("Cyan Header Strip",PrimitiveType.Cube,new Vector3(0,7.4f,14.7f),new Vector3(14,.15f,.1f),new Color(.02f,.8f,1f));
            Make("Wash Pad",PrimitiveType.Cube,new Vector3(0,.02f,2),new Vector3(9,.08f,10),new Color(.17f,.22f,.28f));
            var light=new GameObject("Bay Light"); var l=light.AddComponent<Light>(); l.type=LightType.Directional;l.intensity=1.35f;light.transform.rotation=Quaternion.Euler(50,-30,0);
            var player=new GameObject("Player"); player.transform.position=new Vector3(0,1,-7); player.AddComponent<CharacterController>().height=1.8f; player.GetComponent<CharacterController>().radius=.32f;
            var pivot=new GameObject("Camera Pivot");pivot.transform.SetParent(player.transform);pivot.transform.localPosition=new Vector3(0,.65f,0);
            var camObj=new GameObject("Player Camera");camObj.tag="MainCamera";camObj.transform.SetParent(pivot.transform);camObj.transform.localPosition=Vector3.zero;camObj.transform.localRotation=Quaternion.identity;var cam=camObj.AddComponent<Camera>();cam.fieldOfView=80;cam.nearClipPlane=.05f;camObj.AddComponent<AudioListener>();
            var controller=player.AddComponent<UfoWashFirstPersonController>(); Set(controller,"cameraPivot",pivot);
            var spray=player.AddComponent<UfoWashSprayTool>();spray.viewCamera=cam;
            // Saucer assembly: layered primitive meshes, each surface cleans independently.
            var ufo=new GameObject("DIRTY UFO - JOB 001");ufo.transform.position=new Vector3(0,1.5f,2.5f);
            DirtPart(ufo,"Lower Hull",PrimitiveType.Cylinder,new Vector3(0,0,0),new Vector3(4.5f,.32f,4.5f),1f);
            DirtPart(ufo,"Upper Dome",PrimitiveType.Sphere,new Vector3(0,.45f,0),new Vector3(2.3f,.85f,2.3f),.9f);
            DirtPart(ufo,"Cockpit",PrimitiveType.Sphere,new Vector3(0,.95f,0),new Vector3(1.1f,.55f,1.1f),.8f);
            for(int i=0;i<8;i++){float a=i*Mathf.PI/4;DirtPart(ufo,"Engine Rim "+i,PrimitiveType.Sphere,new Vector3(Mathf.Cos(a)*1.8f,-.2f,Mathf.Sin(a)*1.8f),new Vector3(.42f,.24f,.42f),1f);}
            var hud=new GameObject("HUD");hud.AddComponent<UfoWashHUD>();
            EditorSceneManager.SaveScene(scene,ScenePath);
            var list=new System.Collections.Generic.List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            list.RemoveAll(s=>s.path==ScenePath);list.Insert(0,new EditorBuildSettingsScene(ScenePath,true));EditorBuildSettings.scenes=list.ToArray();
            if(showDialog) EditorUtility.DisplayDialog("UFO WASH","Playable prototype scene created and added to Build Settings:\n"+ScenePath+"\n\nPress Play. WASD + mouse. Hold left mouse to clean. R resets job.", "Let's go");
            if(!showDialog)
            {
                if(previousScene.IsValid()) SceneManager.SetActiveScene(previousScene);
                EditorSceneManager.CloseScene(scene,true);
            }
            Debug.Log("UFO WASH: Playable cleaning prototype generated at "+ScenePath);
        }
        static void DirtPart(GameObject root,string name,PrimitiveType type,Vector3 pos,Vector3 scale,float dirt)
        {
            var go=GameObject.CreatePrimitive(type);go.name=name;go.transform.SetParent(root.transform);go.transform.localPosition=pos;go.transform.localScale=scale;
            var surf=go.AddComponent<UfoWashDirtSurface>();surf.dirt=dirt;
        }
        static GameObject Make(string name,PrimitiveType type,Vector3 pos,Vector3 scale,Color color)
        {
            var go=GameObject.CreatePrimitive(type);go.name=name;go.transform.position=pos;go.transform.localScale=scale;var m=new Material(Shader.Find("Universal Render Pipeline/Lit"));m.color=color;go.GetComponent<Renderer>().material=m;return go;
        }
        static void Set(Object obj,string field,Object value){var so=new SerializedObject(obj);so.FindProperty(field).objectReferenceValue=value;so.ApplyModifiedProperties();}
    }
}
#endif