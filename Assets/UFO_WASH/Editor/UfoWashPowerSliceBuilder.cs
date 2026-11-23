#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

namespace UFOWASH.Editor
{
    public static class UfoWashPowerSliceBuilder
    {
        const string ScenePath = "Assets/UFO_WASH/Scenes/UFO_WASH_PowerSlice.unity";
        static Material matFloor, matWall, matDark, matMetal, matWhite, matCyan, matBlue, matOrange, matGlass, matDirt, matGlow;

        [MenuItem("UFO WASH/BUILD POWER SLICE - FULL ART PASS", false, 0)]
        public static void Build()
        {
            EnsureFoldersAndMaterials();
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(.38f,.43f,.52f);
            RenderSettings.fog = true; RenderSettings.fogColor = new Color(.075f,.095f,.13f); RenderSettings.fogDensity = .006f;
            SetupPostProcessing();

            // Main industrial hangar shell
            Cube("Polished graphite floor", new Vector3(0,-.25f,3), new Vector3(26,.5f,34), matFloor);
            Cube("Rear bulkhead", new Vector3(0,6,19.5f), new Vector3(26,12,.7f), matWall);
            Cube("Left bulkhead", new Vector3(-13,6,3), new Vector3(.7f,12,33), matWall);
            Cube("Right bulkhead", new Vector3(13,6,3), new Vector3(.7f,12,33), matWall);
            Cube("Ceiling", new Vector3(0,12,3), new Vector3(26,.5f,33), matDark);
            // Architectural ribs and luminous accents
            for(int z=-10; z<=18; z+=4) {
                Cube("Ceiling structural rib", new Vector3(0,11.55f,z), new Vector3(25,.32f,.42f), matMetal);
                Cube("Rib light", new Vector3(0,11.35f,z-.25f), new Vector3(20,.08f,.12f), matGlow);
            }
            for(int x=-12; x<=12; x+=4) {
                Cube("Wall vertical support", new Vector3(x,6,19.05f), new Vector3(.22f,11,.18f), matMetal);
            }
            // Floor lanes, drains and service markings
            for(int x=-10; x<=10; x+=5) Cube("Yellow lane marking", new Vector3(x,.012f,2), new Vector3(.055f,.025f,29), matOrange);
            for(int z=-12; z<=17; z+=3) Cube("Drain channel", new Vector3(8,.015f,z), new Vector3(.18f,.03f,2.5f), matDark);
            Cube("Wash platform", new Vector3(0,.08f,3.2f), new Vector3(10,.18f,11), matMetal);
            Cube("Platform cyan perimeter front", new Vector3(0,.19f,-2.35f), new Vector3(10,.06f,.12f), matGlow);
            Cube("Platform cyan perimeter rear", new Vector3(0,.19f,8.75f), new Vector3(10,.06f,.12f), matGlow);
            // Back wall branding and service displays
            Sign("UFO WASH", new Vector3(0,8.2f,19.05f), 2.0f, matCyan);
            Sign("INTERGALACTIC DETAILING // BAY 01", new Vector3(0,7.1f,19.03f), .42f, matWhite);
            DisplayPanel("JOB CONTROL", new Vector3(-8.7f,4.2f,18.45f));
            DisplayPanel("WASH STATUS", new Vector3(8.7f,4.2f,18.45f));
            // Side service cabinets
            for(int i=0;i<3;i++) {
                float z=-7+i*5;
                Cube("Equipment locker", new Vector3(-11.8f,1.8f,z), new Vector3(1.2f,3.6f,2.2f), matDark);
                Cube("Locker cyan display", new Vector3(-11.17f,2.3f,z), new Vector3(.035f,.65f,1.15f), matGlow);
                for(int k=0;k<3;k++) Sphere("Locker bolt", new Vector3(-11.12f,1.1f+k*.7f,z-.75f), .07f, matOrange);
            }
            // Overhead wash gantry and hanging nozzles
            for(int x=-4;x<=4;x+=8) {
                Cube("Gantry column", new Vector3(x,5,3.2f), new Vector3(.35f,10,.35f), matMetal);
                Cube("Gantry foot", new Vector3(x, .4f,3.2f), new Vector3(1,.4f,1), matDark);
            }
            Cube("Overhead wash gantry", new Vector3(0,9.8f,3.2f), new Vector3(8.5f,.5f,.65f), matMetal);
            Cube("Gantry light strip", new Vector3(0,9.5f,2.85f), new Vector3(7.5f,.08f,.08f), matGlow);
            for(int x=-3;x<=3;x+=2) {
                Cylinder("Spray nozzle", new Vector3(x,8.9f,3.2f), new Vector3(.13f,.5f,.13f), matCyan);
                Cylinder("Nozzle hose", new Vector3(x,9.3f,3.2f), new Vector3(.07f,.55f,.07f), matDark);
            }
            // UFO: layered custom-looking saucer with ring, portholes, cockpit and engine glow
            var ufo = new GameObject("CUSTOMER UFO // MUDDY CLASS-IV SCOUT");
            ufo.transform.position = new Vector3(0,2.65f,3.4f);
            CustomSaucerPart(ufo,"Primary hull - contaminated",new[]{new Vector2(0f,-.22f),new Vector2(1.0f,-.22f),new Vector2(2.7f,-.12f),new Vector2(3.35f,-.02f),new Vector2(3.45f,.12f),new Vector2(2.9f,.22f),new Vector2(2.1f,.34f),new Vector2(1.25f,.48f),new Vector2(.6f,.56f),new Vector2(0f,.58f)},1f,matDirt);
            Part(ufo,"Lower rim",PrimitiveType.Cylinder,new Vector3(0,-.3f,0),new Vector3(6.1f,.18f,6.1f),matCyan);
            CustomSaucerPart(ufo,"Upper saucer shell",new[]{new Vector2(0f,1.42f),new Vector2(.45f,1.38f),new Vector2(.95f,1.22f),new Vector2(1.35f,.94f),new Vector2(1.62f,.62f),new Vector2(1.7f,.42f),new Vector2(0f,.4f)},.92f,matDirt);
            Part(ufo,"Cockpit collar",PrimitiveType.Cylinder,new Vector3(0,.95f,0),new Vector3(2.35f,.2f,2.35f),matMetal);
            Part(ufo,"Glass cockpit",PrimitiveType.Sphere,new Vector3(0,1.3f,0),new Vector3(2.05f,1.25f,2.05f),matGlass);
            Part(ufo,"Cockpit crown",PrimitiveType.Sphere,new Vector3(0,1.88f,0),new Vector3(.75f,.4f,.75f),matCyan);
            for(int i=0;i<12;i++) {
                float a=i*Mathf.PI/6;
                Vector3 p=new Vector3(Mathf.Cos(a)*2.75f,.18f,Mathf.Sin(a)*2.75f);
                Part(ufo,"Hull porthole "+i,PrimitiveType.Sphere,p,new Vector3(.28f,.18f,.28f),matGlow);
                Part(ufo,"Landing pod "+i,PrimitiveType.Sphere,new Vector3(Mathf.Cos(a)*2.45f,-.42f,Mathf.Sin(a)*2.45f),new Vector3(.38f,.28f,.38f),matOrange);
            }
            for(int i=0;i<6;i++) {
                float a=i*Mathf.PI/3;
                Part(ufo,"Engine thruster "+i,PrimitiveType.Cylinder,new Vector3(Mathf.Cos(a)*2.1f,-.48f,Mathf.Sin(a)*2.1f),new Vector3(.48f,.18f,.48f),matGlow);
            }
            // Replace the placeholder saucer with a real imported spacecraft when the CC0 pack is present.
            var importedShip = SpawnImportedModel("craft_speederA", new Vector3(0,2.65f,3.4f), Quaternion.Euler(0,180,0), 3.2f, true);
            if (importedShip != null) ufo.SetActive(false);
            // Alien customer waiting at reception marker
            var alien = new GameObject("ALIEN CUSTOMER // ZORP");
            alien.transform.position = new Vector3(7.5f,0,-3.5f);
            Part(alien,"Body",PrimitiveType.Capsule,new Vector3(0,1.15f,0),new Vector3(.8f,1.1f,.55f),matCyan);
            Part(alien,"Head",PrimitiveType.Sphere,new Vector3(0,2.25f,0),new Vector3(1.05f,.85f,.8f),matCyan);
            Part(alien,"Left eye",PrimitiveType.Sphere,new Vector3(-.28f,2.32f,.36f),new Vector3(.22f,.32f,.12f),matDark);
            Part(alien,"Right eye",PrimitiveType.Sphere,new Vector3(.28f,2.32f,.36f),new Vector3(.22f,.32f,.12f),matDark);
            Part(alien,"Left arm",PrimitiveType.Capsule,new Vector3(-.62f,1.2f,0),new Vector3(.2f,.75f,.2f),matCyan);
            Part(alien,"Right arm",PrimitiveType.Capsule,new Vector3(.62f,1.2f,0),new Vector3(.2f,.75f,.2f),matCyan);
            Sign("CUSTOMER",new Vector3(7.5f,3.2f,-3.5f),.35f,matOrange);
            // Replace the capsule alien with Kenney's actual alien character model.
            var importedAlien = SpawnImportedModel("alien", new Vector3(7.5f,0,-3.5f), Quaternion.identity, 2.4f, false);
            if (importedAlien != null) alien.SetActive(false);
            // Player starts facing the UFO; tool and interaction
            var player = new GameObject("PLAYER // FIRST PERSON");
            player.transform.position = new Vector3(0,1,-12.5f);
            var cc=player.AddComponent<CharacterController>(); cc.height=1.8f; cc.radius=.32f; cc.center=new Vector3(0,.9f,0);
            var pivot=new GameObject("Camera Pivot"); pivot.transform.SetParent(player.transform); pivot.transform.localPosition=new Vector3(0,1.05f,0); pivot.transform.localRotation=Quaternion.Euler(-3f,0f,0f);
            var camGo=new GameObject("Main Camera"); camGo.tag="MainCamera"; camGo.transform.SetParent(pivot.transform); camGo.transform.localPosition=Vector3.zero;
            var cam=camGo.AddComponent<Camera>(); cam.fieldOfView=78; cam.nearClipPlane=.05f; cam.farClipPlane=120;
            var cameraData=camGo.GetComponent<UniversalAdditionalCameraData>(); if(cameraData==null) cameraData=camGo.AddComponent<UniversalAdditionalCameraData>(); cameraData.renderPostProcessing=true;
            camGo.AddComponent<AudioListener>();
            var controller=player.AddComponent<UfoWashFirstPersonController>(); Set(controller,"cameraPivot",pivot);
            var spray=player.AddComponent<UfoWashSprayTool>(); spray.viewCamera=cam; spray.range=12; spray.cleanPerSecond=.32f;
            var interact=player.AddComponent<UfoWashPlayerInteraction>(); Set(interact,"viewCamera",cam);
            // Upgrade console with obvious label
            var terminal=Cube("SPRAYER UPGRADE CONSOLE",new Vector3(-8.8f,1.25f,-3.5f),new Vector3(1.7f,2.5f,1.1f),matBlue);
            terminal.AddComponent<UfoWashUpgradeStation>();
            Cube("Console screen",new Vector3(-8.8f,1.75f,-2.92f),new Vector3(1.35f,.8f,.06f),matGlow);
            Sign("UPGRADE",new Vector3(-8.8f,2.35f,-2.88f),.3f,matWhite);
            // Props: hose reels, barrels, crates and floor lamps
            for(int i=0;i<2;i++) {
                float x=i==0?-6.5f:6.5f;
                Cylinder("Chemical drum",new Vector3(x,.8f,-5.5f),new Vector3(.55f,.8f,.55f),matBlue);
                Cylinder("Drum lid",new Vector3(x,1.62f,-5.5f),new Vector3(.58f,.08f,.58f),matMetal);
                for(int y=0;y<3;y++) Cube("Drum hazard stripe",new Vector3(x,.45f+y*.28f,-4.94f),new Vector3(.65f,.07f,.025f),matOrange);
            }
            // Lighting
            Light sun=new GameObject("Hangar key light").AddComponent<Light>(); sun.type=LightType.Directional; sun.intensity=1.0f; sun.color=new Color(.68f,.8f,1f); sun.transform.rotation=Quaternion.Euler(48,-28,0);
            AreaLight(new Vector3(0,10,1),new Color(.12f,.72f,1f),7);
            AreaLight(new Vector3(-8,7,10),new Color(.08f,.3f,1f),5);
            AreaLight(new Vector3(8,7,10),new Color(1f,.32f,.12f),5);
            AreaLight(new Vector3(0,7,17),new Color(.12f,.65f,1f),6);
            // Contract terminal and career loop: player must accept a customer order before spraying.
            var career = new GameObject("CAREER LOOP // CUSTOMER CONTRACTS");
            career.AddComponent<UfoWashCareerLoop>();
            var contractTerminal = Cube("CONTRACT TERMINAL // ACCEPT JOB", new Vector3(2.2f,1.25f,-10.2f), new Vector3(1.55f,2.5f,1.0f), matBlue);
            contractTerminal.AddComponent<UfoWashJobBoard>();
            var contractScreen = Cube("Contract terminal screen", new Vector3(2.2f,1.75f,-10.73f), new Vector3(1.25f,.82f,.045f), matGlow);
            contractScreen.GetComponent<Collider>().enabled = false;
            Sign("ACCEPT JOB", new Vector3(2.2f,2.42f,-10.75f), .27f, matWhite);
            Sign("CONTRACTS", new Vector3(2.2f,3.05f,-10.2f), .32f, matCyan);

            var hud=new GameObject("GAME HUD"); hud.AddComponent<UfoWashHUD>();
            EditorSceneManager.SaveScene(scene,ScenePath);
            var scenes=new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            scenes.RemoveAll(s=>s.path==ScenePath); scenes.Insert(0,new EditorBuildSettingsScene(ScenePath,true)); EditorBuildSettings.scenes=scenes.ToArray();
            Selection.activeObject=AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath);
            EditorUtility.DisplayDialog("UFO WASH POWER SLICE BUILT","A new full art-pass scene was created. Open UFO_WASH_PowerSlice and press Play.

WASD + mouse: move/look
Find the CONTRACTS terminal and press E to accept a customer
Hold left mouse: spray and clean the UFO
Aim at upgrade console + E: buy sprayer upgrades
R: reset the active contract","LET'S PLAY");
        }

        static GameObject SpawnImportedModel(string assetName, Vector3 position, Quaternion rotation, float scale, bool washable) {
            const string importedFolder = "Assets/UFO_WASH/Art/Imported/KenneySpaceKit";
            string[] guids = AssetDatabase.FindAssets(assetName + " t:Model", new[] { importedFolder });
            string selectedPath = guids.Length > 0 ? AssetDatabase.GUIDToAssetPath(guids[0]) : null;

            // Kenney pack filenames differ between releases. Fall back to a semantic filename scan
            // instead of silently requiring one guessed filename.
            if (string.IsNullOrEmpty(selectedPath)) {
                string[] allModels = AssetDatabase.FindAssets("t:Model", new[] { importedFolder });
                int bestScore = 0;
                foreach (string guid in allModels) {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    string name = System.IO.Path.GetFileNameWithoutExtension(path).ToLowerInvariant();
                    int score = 0;
                    if (assetName == "alien") {
                        if (name.Contains("alien")) score = 1000;
                        else if (name.Contains("character")) score = 180;
                        else if (name.Contains("robot")) score = 80;
                    } else {
                        if (name.Contains("ufo")) score = 1000;
                        else if (name.Contains("spaceship") || name.Contains("space_ship")) score = 950;
                        else if (name.Contains("speeder")) score = 900;
                        else if (name.Contains("starfighter") || name.Contains("fighter")) score = 850;
                        else if (name.Contains("ship")) score = 800;
                        else if (name.Contains("craft")) score = 700;
                        else if (name.Contains("vehicle")) score = 600;
                        if (name.Contains("station") || name.Contains("building") || name.Contains("asteroid") ||
                            name.Contains("rock") || name.Contains("turret") || name.Contains("weapon")) score = 0;
                    }
                    if (score > bestScore) {
                        bestScore = score;
                        selectedPath = path;
                    }
                }
            }

            if (string.IsNullOrEmpty(selectedPath)) {
                Debug.LogWarning("UFO WASH: no suitable imported model found for " + assetName +
                    ". The built-in fallback remains active. Check the imported model filenames in " + importedFolder + ".");
                return null;
            }

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(selectedPath);
            if (prefab == null) return null;
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            instance.name = "IMPORTED 3D MODEL // " + System.IO.Path.GetFileNameWithoutExtension(selectedPath);
            instance.transform.rotation = rotation;

            var renderers = instance.GetComponentsInChildren<Renderer>(true);
            if (renderers.Length > 0) {
                Bounds bounds = renderers[0].bounds;
                for (int i = 1; i < renderers.Length; i++) bounds.Encapsulate(renderers[i].bounds);
                bool isAlien = assetName == "alien";
                float currentSize = isAlien ? bounds.size.y : Mathf.Max(bounds.size.x, bounds.size.z);
                float targetSize = isAlien ? 2.2f : 5.8f;
                float fitScale = currentSize > .001f ? targetSize / currentSize : scale;
                instance.transform.localScale = Vector3.one * fitScale;

                bounds = renderers[0].bounds;
                for (int i = 1; i < renderers.Length; i++) bounds.Encapsulate(renderers[i].bounds);
                Vector3 targetCenter = position + (isAlien ? Vector3.up * 1.1f : Vector3.zero);
                instance.transform.position += targetCenter - bounds.center;
            } else {
                instance.transform.position = position;
                instance.transform.localScale = Vector3.one * scale;
            }

            if (washable) {
                foreach (var renderer in renderers) {
                    var go = renderer.gameObject;
                    if (go.GetComponent<Collider>() == null) {
                        var filter = go.GetComponent<MeshFilter>();
                        if (filter != null && filter.sharedMesh != null) {
                            var meshCollider = go.AddComponent<MeshCollider>();
                            meshCollider.sharedMesh = filter.sharedMesh;
                        }
                    }
                    if (go.GetComponent<UfoWashDirtSurface>() == null) {
                        var dirt = go.AddComponent<UfoWashDirtSurface>();
                        dirt.dirt = 1f;
                    }
                }
            }

            Debug.Log("UFO WASH: selected imported model " + selectedPath + " for " + assetName);
            return instance;
        }

        static void EnsureFoldersAndMaterials() {
            foreach(string f in new[]{"Assets/UFO_WASH/Art/Materials","Assets/UFO_WASH/Scenes"}) if(!AssetDatabase.IsValidFolder(f)) System.IO.Directory.CreateDirectory(f);
            matFloor=MakeMat("PS_Floor",new Color(.055f,.075f,.105f),.82f);
            matWall=MakeMat("PS_Wall",new Color(.09f,.13f,.19f),.72f);
            matDark=MakeMat("PS_DarkMetal",new Color(.018f,.027f,.045f),.65f);
            matMetal=MakeMat("PS_BrushedSteel",new Color(.24f,.32f,.4f),.78f);
            matWhite=MakeMat("PS_White",new Color(.75f,.85f,.92f),.5f);
            matCyan=MakeMat("PS_AlienTeal",new Color(.025f,.48f,.55f),.45f);
            matBlue=MakeMat("PS_BluePaint",new Color(.035f,.14f,.3f),.48f);
            matOrange=MakeMat("PS_HazardOrange",new Color(1f,.27f,.035f),.48f);
            matGlass=MakeMat("PS_CockpitGlass",new Color(.025f,.22f,.32f),.22f);
            matDirt=MakeMat("PS_ContaminatedHull",new Color(.26f,.16f,.085f),.92f);
            matGlow=MakeMat("PS_EmissiveCyan",new Color(.02f,.8f,1f),.3f);
            matGlow.EnableKeyword("_EMISSION"); matGlow.SetColor("_EmissionColor",new Color(.02f,1.2f,1.8f));
        }
        static void SetupPostProcessing() {
            const string profilePath = "Assets/UFO_WASH/Art/Materials/UfoWash_CinematicVolume.asset";
            var profile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(profilePath);
            if (profile == null) {
                profile = ScriptableObject.CreateInstance<VolumeProfile>();
                AssetDatabase.CreateAsset(profile, profilePath);
            }

            if (!profile.TryGet<Bloom>(out var bloom)) bloom = profile.Add<Bloom>(true);
            bloom.active = true;
            bloom.threshold.Override(1.05f);
            bloom.intensity.Override(.72f);
            bloom.scatter.Override(.68f);

            if (!profile.TryGet<Vignette>(out var vignette)) vignette = profile.Add<Vignette>(true);
            vignette.active = true;
            vignette.intensity.Override(.22f);
            vignette.smoothness.Override(.72f);
            vignette.rounded.Override(true);

            if (!profile.TryGet<ColorAdjustments>(out var grade)) grade = profile.Add<ColorAdjustments>(true);
            grade.active = true;
            grade.postExposure.Override(.18f);
            grade.contrast.Override(14f);
            grade.saturation.Override(8f);

            if (!profile.TryGet<Tonemapping>(out var tone)) tone = profile.Add<Tonemapping>(true);
            tone.active = true;
            tone.mode.Override(TonemappingMode.ACES);

            EditorUtility.SetDirty(profile);
            var volumeObject = new GameObject("GLOBAL VOLUME // SCI-FI CINEMA");
            var volume = volumeObject.AddComponent<Volume>();
            volume.isGlobal = true;
            volume.priority = 10f;
            volume.weight = 1f;
            volume.sharedProfile = profile;
        }

        static Material MakeMat(string name,Color c,float smooth) {
            string path="Assets/UFO_WASH/Art/Materials/"+name+".mat";
            var m=AssetDatabase.LoadAssetAtPath<Material>(path);
            if(m==null){var shader=Shader.Find("Universal Render Pipeline/Lit"); if(shader==null)shader=Shader.Find("Standard"); m=new Material(shader); AssetDatabase.CreateAsset(m,path);}
            m.color=c;
            if(m.HasProperty("_Smoothness")) m.SetFloat("_Smoothness",smooth);
            if(m.HasProperty("_Metallic")) m.SetFloat("_Metallic", name=="PS_Floor" ? .52f : (name=="PS_BrushedSteel" ? .68f : .08f));
            if(name=="PS_CockpitGlass") {
                Color glassColor = new Color(c.r,c.g,c.b,.38f);
                if(m.HasProperty("_BaseColor")) m.SetColor("_BaseColor",glassColor);
                else m.color=glassColor;
                if(m.HasProperty("_Surface")) m.SetFloat("_Surface",1f);
                if(m.HasProperty("_Blend")) m.SetFloat("_Blend",0f);
                if(m.HasProperty("_ZWrite")) m.SetFloat("_ZWrite",0f);
                if(m.HasProperty("_SrcBlend")) m.SetFloat("_SrcBlend",(float)BlendMode.SrcAlpha);
                if(m.HasProperty("_DstBlend")) m.SetFloat("_DstBlend",(float)BlendMode.OneMinusSrcAlpha);
                m.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                m.renderQueue=(int)RenderQueue.Transparent;
            }
            EditorUtility.SetDirty(m); return m;
        }
        static GameObject Cube(string n,Vector3 p,Vector3 s,Material m) {
            var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=n;g.transform.position=p;g.transform.localScale=s;g.GetComponent<Renderer>().sharedMaterial=m;return g;
        }
        static GameObject Sphere(string n,Vector3 p,float r,Material m) {
            var g=GameObject.CreatePrimitive(PrimitiveType.Sphere);g.name=n;g.transform.position=p;g.transform.localScale=Vector3.one*r;g.GetComponent<Renderer>().sharedMaterial=m;return g;
        }
        static GameObject Cylinder(string n,Vector3 p,Vector3 s,Material m) {
            var g=GameObject.CreatePrimitive(PrimitiveType.Cylinder);g.name=n;g.transform.position=p;g.transform.localScale=s;g.GetComponent<Renderer>().sharedMaterial=m;return g;
        }
        static void CustomSaucerPart(GameObject root,string n,Vector2[] profile,float dirt,Material m) {
            const int segments=64;
            var vertices=new List<Vector3>(); var triangles=new List<int>();
            for(int r=0;r<profile.Length;r++) for(int i=0;i<segments;i++) {
                float a=(float)i/segments*Mathf.PI*2f;
                vertices.Add(new Vector3(Mathf.Cos(a)*profile[r].x,profile[r].y,Mathf.Sin(a)*profile[r].x));
            }
            for(int r=0;r<profile.Length-1;r++) for(int i=0;i<segments;i++) {
                int a=r*segments+i, b=r*segments+(i+1)%segments, c=(r+1)*segments+i, d=(r+1)*segments+(i+1)%segments;
                triangles.Add(a); triangles.Add(c); triangles.Add(b); triangles.Add(b); triangles.Add(c); triangles.Add(d);
            }
            var mesh=new Mesh(); mesh.name=n+" Mesh"; mesh.SetVertices(vertices); mesh.SetTriangles(triangles,0); mesh.RecalculateNormals(); mesh.RecalculateBounds();
            var go=new GameObject(n); go.transform.SetParent(root.transform,false);
            var filter=go.AddComponent<MeshFilter>(); filter.sharedMesh=mesh;
            var renderer=go.AddComponent<MeshRenderer>(); renderer.sharedMaterial=m;
            var collider=go.AddComponent<MeshCollider>(); collider.sharedMesh=mesh;
            var surface=go.AddComponent<UfoWashDirtSurface>(); surface.dirt=dirt;
        }
        static void Part(GameObject root,string n,PrimitiveType t,Vector3 p,Vector3 s,Material m) {
            var g=GameObject.CreatePrimitive(t);g.name=n;g.transform.SetParent(root.transform,false);g.transform.localPosition=p;g.transform.localScale=s;g.GetComponent<Renderer>().sharedMaterial=m;
        }
        static void DirtPart(GameObject root,string n,PrimitiveType t,Vector3 p,Vector3 s,float dirt,Material m) {
            Part(root,n,t,p,s,m);var g=root.transform.Find(n).gameObject;var d=g.AddComponent<UfoWashDirtSurface>();d.dirt=dirt;
        }
        static void Sign(string text,Vector3 p,float size,Material m) {
            var g=new GameObject("SIGN // "+text);g.transform.position=p;g.transform.rotation=Quaternion.identity;var tm=g.AddComponent<TextMesh>();tm.text=text;tm.fontSize=64;tm.characterSize=size/4f;tm.anchor=TextAnchor.MiddleCenter;tm.alignment=TextAlignment.Center;tm.color=m.color;tm.fontStyle=FontStyle.Bold;
        }
        static void DisplayPanel(string label,Vector3 p) {
            Cube(label+" housing",p,new Vector3(3.2f,2.1f,.3f),matDark);
            Cube(label+" glass",p+new Vector3(0,0,-.18f),new Vector3(2.85f,1.65f,.05f),matBlue);
            Sign(label,p+new Vector3(0,.45f,-.23f),.55f,matGlow);
            for(int i=0;i<4;i++) Cube("Display data line",p+new Vector3(0,.05f-i*.22f,-.24f),new Vector3(2.1f-i*.25f,.035f,.025f),matCyan);
        }
        static void AreaLight(Vector3 p,Color c,float intensity) {
            var go=new GameObject("Neon bay light");go.transform.position=p;var l=go.AddComponent<Light>();l.type=LightType.Point;l.color=c;l.intensity=intensity;l.range=15;
        }
        static void Set(Object obj,string field,Object value) {
            var so=new SerializedObject(obj);var prop=so.FindProperty(field);if(prop!=null){prop.objectReferenceValue=value;so.ApplyModifiedProperties();}
        }
    }
}
#endif
