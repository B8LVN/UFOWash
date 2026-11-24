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
        static Material matBasalt, matIce, matPlanet, matStarfield, matVesperModular;

        [MenuItem("UFO WASH/REBUILD GENERATED SCENE (MANUAL FALLBACK)", false, 0)]
        public static void Build()
        {
            if (Application.isBatchMode)
                throw new System.InvalidOperationException("Scene replacement requires an interactive confirmation.");
            if (!EditorUtility.DisplayDialog("Replace generated scene?",
                "This recreates the canonical PowerSlice scene. Use the separate QUALITY PREVIEW command to preserve it. Continue?",
                "Replace generated scene", "Cancel")) return;
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            BuildScene(!Application.isBatchMode);
        }

        [MenuItem("UFO WASH/Developer Tools/Create Separate Scene Preview", false, 1)]
        public static void BuildQualityPreview()
        {
            if (!Application.isBatchMode && !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            BuildScene(false, "Assets/UFO_WASH/Scenes/UFO_WASH_QualityPreview.unity");
        }

        internal static void AutoRebuildGeneratedScene()
        {
            Debug.Log("UFO WASH: source updated. Press Play for the main menu; existing scene files are preserved.");
        }

        static GameObject playModeDetails;
        static Material playModeModularMaterial;
        static Material playModeFloorMaterial, playModeWallMaterial, playModeBasaltMaterial;
        static Texture2D playModeFloorTexture, playModeWallTexture, playModeBasaltTexture;
        static bool hasSavedPlayModeLighting;
        static UnityEngine.Rendering.AmbientMode savedAmbientMode;
        static Color savedAmbientSky, savedAmbientEquator, savedAmbientGround, savedFogColor;
        static float savedAmbientIntensity, savedFogDensity;
        static bool savedFog;
        static FogMode savedFogMode;
        static readonly List<KeyValuePair<Renderer, Material[]>> playModeMaterialOverrides =
            new List<KeyValuePair<Renderer, Material[]>>();
        static readonly List<KeyValuePair<Transform, Vector3>> movedPlayModeProps =
            new List<KeyValuePair<Transform, Vector3>>();

        internal static void AddPlayModeServiceDetails(Scene scene)
        {
            if (!Application.isPlaying || scene.path != ScenePath || playModeDetails != null) return;
            // Reuse the authored materials without regenerating or modifying assets.
            matMetal = LoadPlayMaterial("PS_BrushedSteel");
            matDark = LoadPlayMaterial("PS_DarkMetal");
            matCyan = LoadPlayMaterial("PS_AlienTeal");
            matWhite = LoadPlayMaterial("PS_White");
            matBlue = LoadPlayMaterial("PS_BluePaint");
            matOrange = LoadPlayMaterial("PS_HazardOrange");
            matGlow = LoadPlayMaterial("PS_EmissiveCyan");
            if (matVesperModular == null)
                matVesperModular = CreatePlayModeModularMaterial();

            var originalRoots = new HashSet<GameObject>(scene.GetRootGameObjects());
            playModeDetails = new GameObject("PLAY MODE // SERVICE ACCESS") { hideFlags = HideFlags.DontSave };
            SceneManager.MoveGameObjectToScene(playModeDetails, scene);
            ApplyPlayModeVisualOverhaul();
            bool hasAuthoredAccess = false;
            foreach (var root in originalRoots)
                foreach (var child in root.GetComponentsInChildren<Transform>(true))
                {
                    if (child.name == "Upper hull service deck") hasAuthoredAccess = true;
                    if (child.name != "Chemical drum" && child.name != "Drum lid" && child.name != "Drum hazard stripe") continue;
                    Vector3 position = child.position;
                    // Older scenes placed these props across the new stair approach.
                    if (Mathf.Abs(Mathf.Abs(position.x) - 6.5f) > 0.1f || position.z < -6f) continue;
                    movedPlayModeProps.Add(new KeyValuePair<Transform, Vector3>(child, position));
                    child.position = new Vector3(position.x * 1.23f, position.y, position.z - 2.3f);
                }
            if (!hasAuthoredAccess)
            {
                BuildStarterBayServiceDetails();
                BuildServiceAccess();
            }
            // The normal owner workflow is Play -> main menu -> start the shift.
            // Keep progression and the first worker temporary; never rewrite the authored scene.
            if (Object.FindAnyObjectByType<UfoWashProgressSave>() == null)
                playModeDetails.AddComponent<UfoWashProgressSave>();
            if (Object.FindAnyObjectByType<UfoWashWashAttendant>() == null)
                BuildWashAttendant(new Vector3(-5.2f, 0f, 1.2f));
            if (Object.FindAnyObjectByType<UfoWashBayRecoveryStation>() == null)
                BuildBayRecoveryUpgrade();
            BuildPlayModeEquipmentProps();

            foreach (var root in scene.GetRootGameObjects())
                if (root != playModeDetails && !originalRoots.Contains(root))
                    root.transform.SetParent(playModeDetails.transform, true);
        }

        static Material LoadPlayMaterial(string name)
        {
            var material = AssetDatabase.LoadAssetAtPath<Material>("Assets/UFO_WASH/Art/Materials/" + name + ".mat");
            if (material == null) throw new System.InvalidOperationException("Missing authored service-bay material: " + name);
            return material;
        }

        internal static void RemovePlayModeServiceDetails()
        {
            foreach (var prop in movedPlayModeProps)
                if (prop.Key != null) prop.Key.position = prop.Value;
            movedPlayModeProps.Clear();
            for (int i = playModeMaterialOverrides.Count - 1; i >= 0; i--)
                if (playModeMaterialOverrides[i].Key != null)
                    playModeMaterialOverrides[i].Key.sharedMaterials = playModeMaterialOverrides[i].Value;
            playModeMaterialOverrides.Clear();
            if (hasSavedPlayModeLighting)
            {
                RenderSettings.ambientMode = savedAmbientMode;
                RenderSettings.ambientSkyColor = savedAmbientSky;
                RenderSettings.ambientEquatorColor = savedAmbientEquator;
                RenderSettings.ambientGroundColor = savedAmbientGround;
                RenderSettings.ambientIntensity = savedAmbientIntensity;
                RenderSettings.fog = savedFog;
                RenderSettings.fogMode = savedFogMode;
                RenderSettings.fogColor = savedFogColor;
                RenderSettings.fogDensity = savedFogDensity;
                hasSavedPlayModeLighting = false;
            }
            if (playModeDetails != null) Object.DestroyImmediate(playModeDetails);
            playModeDetails = null;
            DestroyPlayModeMaterial(ref playModeFloorMaterial, ref playModeFloorTexture);
            DestroyPlayModeMaterial(ref playModeWallMaterial, ref playModeWallTexture);
            DestroyPlayModeMaterial(ref playModeBasaltMaterial, ref playModeBasaltTexture);
            if (playModeModularMaterial != null)
            {
                if (matVesperModular == playModeModularMaterial) matVesperModular = null;
                Object.DestroyImmediate(playModeModularMaterial);
                playModeModularMaterial = null;
            }
        }

        static void ApplyPlayModeVisualOverhaul()
        {
            savedAmbientMode = RenderSettings.ambientMode;
            savedAmbientSky = RenderSettings.ambientSkyColor;
            savedAmbientEquator = RenderSettings.ambientEquatorColor;
            savedAmbientGround = RenderSettings.ambientGroundColor;
            savedAmbientIntensity = RenderSettings.ambientIntensity;
            savedFog = RenderSettings.fog;
            savedFogMode = RenderSettings.fogMode;
            savedFogColor = RenderSettings.fogColor;
            savedFogDensity = RenderSettings.fogDensity;
            hasSavedPlayModeLighting = true;

            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(.18f,.30f,.43f);
            RenderSettings.ambientEquatorColor = new Color(.31f,.34f,.35f);
            RenderSettings.ambientGroundColor = new Color(.075f,.065f,.09f);
            RenderSettings.ambientIntensity = 1.05f;
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogColor = new Color(.075f,.13f,.20f);
            RenderSettings.fogDensity = .00115f;

            playModeFloorMaterial = CreatePlayModeSurfaceMaterial("BAY FLOOR // TEXTURED DECK",
                new Color(.18f,.23f,.27f), .34f, .48f, false, 9101, out playModeFloorTexture);
            playModeWallMaterial = CreatePlayModeSurfaceMaterial("BAY WALL // RIBBED COMPOSITE",
                new Color(.115f,.18f,.23f), .22f, .34f, false, 9102, out playModeWallTexture);
            playModeBasaltMaterial = CreatePlayModeSurfaceMaterial("VESPER // TEXTURED BASALT",
                new Color(.19f,.23f,.31f), .08f, .18f, true, 9103, out playModeBasaltTexture);

            if (playModeFloorMaterial != null) OverrideNamedMaterial("Polished graphite floor", playModeFloorMaterial);
            if (playModeWallMaterial != null)
            {
                OverrideNamedMaterial("Left bulkhead", playModeWallMaterial);
                OverrideNamedMaterial("Right bulkhead", playModeWallMaterial);
                OverrideNamedMaterial("Rear bulkhead left", playModeWallMaterial);
                OverrideNamedMaterial("Rear bulkhead right", playModeWallMaterial);
            }
            if (playModeBasaltMaterial != null)
                OverrideNamedMaterial("VESPER BELT // BASALT APRON", playModeBasaltMaterial);

            AddPlayModeSpotlight("BAY KEY // WARM ARRIVAL", new Vector3(-6.5f,9.6f,5.3f),
                new Vector3(0f,2.3f,3.4f), new Color(1f,.64f,.38f), 9f, 19f, 54f);
            AddPlayModeSpotlight("BAY FILL // COOL CEILING", new Vector3(6.5f,9.4f,5.1f),
                new Vector3(0f,2.0f,3.3f), new Color(.28f,.72f,1f), 7.5f, 19f, 56f);
            AddPlayModeSpotlight("BAY RIM // SERVICE DECK", new Vector3(0f,10f,14f),
                new Vector3(0f,2.7f,4f), new Color(.12f,.72f,.9f), 6f, 18f, 48f);
        }

        static void OverrideNamedMaterial(string objectName, Material material)
        {
            var target = GameObject.Find(objectName);
            if (target == null) return;
            foreach (var renderer in target.GetComponentsInChildren<Renderer>(true))
            {
                if (renderer == null) continue;
                playModeMaterialOverrides.Add(new KeyValuePair<Renderer, Material[]>(renderer, renderer.sharedMaterials));
                renderer.sharedMaterial = material;
            }
        }

        static Material CreatePlayModeSurfaceMaterial(string materialName, Color baseColor,
            float metallic, float smoothness, bool basalt, int seed, out Texture2D texture)
        {
            texture = null;
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) return null;
            texture = CreatePlayModeSurfaceTexture(materialName, baseColor, basalt, seed);
            var material = new Material(shader) { name = materialName, hideFlags = HideFlags.DontSave };
            material.SetColor("_BaseColor", Color.white);
            material.SetTexture("_BaseMap", texture);
            material.SetFloat("_Metallic", metallic);
            material.SetFloat("_Smoothness", smoothness);
            material.SetTextureScale("_BaseMap", basalt ? new Vector2(12f,10f) : new Vector2(2.5f,2.5f));
            return material;
        }

        static Texture2D CreatePlayModeSurfaceTexture(string textureName, Color baseColor, bool basalt, int seed)
        {
            const int size = 256;
            const int tile = 64;
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false)
            {
                name = textureName,
                wrapMode = TextureWrapMode.Repeat,
                filterMode = FilterMode.Bilinear,
                anisoLevel = 4,
                hideFlags = HideFlags.DontSave
            };
            var random = new System.Random(seed);
            var noise = new float[size * size];
            for (int i = 0; i < noise.Length; i++)
                noise[i] = (float)random.NextDouble();
            var pixels = new Color[size * size];
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                float grain = (noise[y * size + x] - .5f) * (basalt ? .12f : .055f);
                float variation;
                if (basalt)
                {
                    float broad = Mathf.PerlinNoise(x / 38f, y / 38f);
                    variation = .72f + broad * .36f + grain;
                }
                else
                {
                    int localX = x % tile;
                    int localY = y % tile;
                    bool seam = localX < 3 || localY < 3 || localX > tile - 4 || localY > tile - 4;
                    bool edgeBand = localX < 6 || localY < 6 || localX > tile - 7 || localY > tile - 7;
                    variation = seam ? .46f : edgeBand ? 1.08f : .91f + grain;
                    int boltX = localX < tile / 2 ? 9 : tile - 10;
                    int boltY = localY < tile / 2 ? 9 : tile - 10;
                    if (Mathf.Abs(localX - boltX) <= 1 && Mathf.Abs(localY - boltY) <= 1)
                        variation = .52f;
                }
                pixels[y * size + x] = new Color(
                    Mathf.Clamp01(baseColor.r * variation),
                    Mathf.Clamp01(baseColor.g * variation),
                    Mathf.Clamp01(baseColor.b * variation), 1f);
            }
            texture.SetPixels(pixels);
            texture.Apply(false, false);
            return texture;
        }

        static void AddPlayModeSpotlight(string name, Vector3 position, Vector3 target,
            Color color, float intensity, float range, float angle)
        {
            var go = new GameObject(name) { hideFlags = HideFlags.DontSave };
            go.transform.SetParent(playModeDetails.transform, true);
            go.transform.position = position;
            go.transform.rotation = Quaternion.LookRotation(target - position);
            var light = go.AddComponent<Light>();
            light.type = LightType.Spot;
            light.color = color;
            light.intensity = intensity;
            light.range = range;
            light.spotAngle = angle;
            light.shadows = LightShadows.None;
        }

        static void DestroyPlayModeMaterial(ref Material material, ref Texture2D texture)
        {
            if (material != null) Object.DestroyImmediate(material);
            if (texture != null) Object.DestroyImmediate(texture);
            material = null;
            texture = null;
        }

        static void BuildPlayModeEquipmentProps()
        {
            AddPlayModeSpaceProp("machine_generatorLarge", "BAY 01 // SERVICE GENERATOR",
                new Vector3(-9.7f,0f,13.2f), Quaternion.Euler(0f,90f,0f), 2.8f);
            AddPlayModeSpaceProp("machine_generator", "BAY 01 // AUXILIARY PUMP",
                new Vector3(-9.3f,0f,16.6f), Quaternion.Euler(0f,90f,0f), 1.75f);
            AddPlayModeSpaceProp("machine_wireless", "BAY 01 // COMMS RELAY",
                new Vector3(-10.7f,0f,10.5f), Quaternion.identity, 1.55f);
            AddPlayModeSpaceProp("machine_barrelLarge", "BAY 01 // SEALED CHEMICAL DRUM",
                new Vector3(-8.7f,0f,10.7f), Quaternion.identity, .95f);
        }

        static void AddPlayModeSpaceProp(string assetName, string displayName,
            Vector3 floorPosition, Quaternion rotation, float targetFootprint)
        {
            var prop = SpawnImportedModel(assetName, floorPosition, rotation, 1f, false);
            if (prop == null) return;

            var renderers = prop.GetComponentsInChildren<Renderer>(true);
            if (renderers.Length > 0)
            {
                Bounds bounds = renderers[0].bounds;
                for (int i = 1; i < renderers.Length; i++) bounds.Encapsulate(renderers[i].bounds);
                float currentFootprint = Mathf.Max(bounds.size.x, bounds.size.z);
                float scale = currentFootprint > .001f ? targetFootprint / currentFootprint : 1f;
                prop.transform.localScale *= scale;
                bounds = renderers[0].bounds;
                for (int i = 1; i < renderers.Length; i++) bounds.Encapsulate(renderers[i].bounds);
                prop.transform.position += floorPosition - new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
            }

            prop.name = displayName;
            foreach (var collider in prop.GetComponentsInChildren<Collider>(true))
                collider.enabled = false;
        }

        static Material CreatePlayModeModularMaterial()
        {
            const string palettePath = "Assets/UFO_WASH/Art/Textures/Vesper_ModularSpace_Colormap.png";
            var palette = AssetDatabase.LoadAssetAtPath<Texture2D>(palettePath);
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            if (palette == null || shader == null)
            {
                Debug.LogWarning("UFO WASH: Vesper modular palette or URP/Lit is missing; recovery annex uses brushed steel.");
                return null;
            }

            playModeModularMaterial = new Material(shader) { hideFlags = HideFlags.DontSave };
            if (playModeModularMaterial.HasProperty("_BaseMap"))
                playModeModularMaterial.SetTexture("_BaseMap", palette);
            else if (playModeModularMaterial.HasProperty("_MainTex"))
                playModeModularMaterial.SetTexture("_MainTex", palette);
            return playModeModularMaterial;
        }

        static void BuildScene(bool showDialog, string outputPath = ScenePath)
        {
            // Invoked explicitly by a menu command, never by ordinary source import.
            // Candidate scenes use a separate output path and preserve Build Settings.
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            EnsureFoldersAndMaterials();
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(.48f,.52f,.62f);
            RenderSettings.fog = true; RenderSettings.fogColor = new Color(.08f,.12f,.2f); RenderSettings.fogDensity = .0018f;
            SetupPostProcessing();
            BuildOuterRimVista();
            BuildVesperLandingYard();

            // Main industrial hangar shell
            Cube("Polished graphite floor", new Vector3(0,-.25f,3), new Vector3(26,.5f,34), matFloor);
            Cube("Rear bulkhead left", new Vector3(-11,6,19.5f), new Vector3(4,12,.7f), matWall);
            Cube("Rear bulkhead right", new Vector3(11,6,19.5f), new Vector3(4,12,.7f), matWall);
            Cube("Rear bulkhead lintel", new Vector3(0,11,19.5f), new Vector3(18,2,.7f), matWall);
            Cube("Bay 01 pressure gate threshold", new Vector3(0,.13f,19.5f), new Vector3(18,.26f,.7f), matMetal);
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
            BuildStarterBayServiceDetails();
            BuildServiceAccess();
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
            CustomSaucerPart(ufo,"Primary hull - contaminated",new[]{new Vector2(0f,-.22f),new Vector2(1.0f,-.22f),new Vector2(2.7f,-.12f),new Vector2(3.35f,-.02f),new Vector2(3.45f,.12f),new Vector2(2.9f,.22f),new Vector2(2.1f,.34f),new Vector2(1.25f,.48f),new Vector2(.6f,.56f),new Vector2(0f,.58f)},1f,matMetal);
            Part(ufo,"Lower rim",PrimitiveType.Cylinder,new Vector3(0,-.3f,0),new Vector3(6.1f,.18f,6.1f),matCyan);
            CustomSaucerPart(ufo,"Upper saucer shell",new[]{new Vector2(0f,1.42f),new Vector2(.45f,1.38f),new Vector2(.95f,1.22f),new Vector2(1.35f,.94f),new Vector2(1.62f,.62f),new Vector2(1.7f,.42f),new Vector2(0f,.4f)},.92f,matMetal);
            Part(ufo,"Cockpit collar",PrimitiveType.Cylinder,new Vector3(0,.95f,0),new Vector3(2.35f,.2f,2.35f),matMetal);
            DirtPart(ufo,"Glass cockpit",PrimitiveType.Sphere,new Vector3(0,1.3f,0),new Vector3(2.05f,1.25f,2.05f),.72f,matGlass);
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
            // Keep the authored saucer as the customer vessel. The included space kit's
            // speeder is a fighter silhouette; using it here obscured the UFO identity and
            // replaced the cleanable layered hull with a wedge-shaped ship.
            ufo.AddComponent<UfoWashCustomerShip>();
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
            if (importedAlien != null)
            {
                alien.SetActive(false);
                AddCustomerInteraction(importedAlien, true);
            }
            else
            {
                AddCustomerInteraction(alien, false);
            }
            // Player starts facing the UFO; tool and interaction
            var player = new GameObject("PLAYER // CUSTOM SPACER SUIT");
            player.transform.position = new Vector3(0,0,-5.8f);
            var cc=player.AddComponent<CharacterController>(); cc.height=1.8f; cc.radius=.34f; cc.center=new Vector3(0,.9f,0);
            BuildPlayerCharacter(player.transform);
            var pivot=new GameObject("First Person Camera Pivot"); pivot.transform.SetParent(player.transform); pivot.transform.localPosition=new Vector3(0,1.62f,0); pivot.transform.localRotation=Quaternion.identity;
            var camGo=new GameObject("Main Camera"); camGo.tag="MainCamera"; camGo.transform.SetParent(pivot.transform); camGo.transform.localPosition=Vector3.zero;
            var cam=camGo.AddComponent<Camera>(); cam.fieldOfView=76; cam.nearClipPlane=.05f; cam.farClipPlane=120;
            // Hide the temporary third-person body mesh in first-person view and show a dedicated tool viewmodel.
            foreach (var bodyRenderer in player.GetComponentsInChildren<Renderer>(true)) bodyRenderer.enabled = false;
            BuildFirstPersonTool(camGo.transform);
            var cameraData=camGo.GetComponent<UniversalAdditionalCameraData>(); if(cameraData==null) cameraData=camGo.AddComponent<UniversalAdditionalCameraData>(); cameraData.renderPostProcessing=true;
            camGo.AddComponent<AudioListener>();
            var controller=player.AddComponent<UfoWashFirstPersonController>(); Set(controller,"cameraPivot",pivot);
            var spray=player.AddComponent<UfoWashSprayTool>(); spray.viewCamera=cam; spray.range=12; spray.cleanPerSecond=.32f;
            var interact=player.AddComponent<UfoWashPlayerInteraction>(); Set(interact,"viewCamera",cam);
            // Upgrade console with obvious label
            var terminal=Cube("SPRAYER UPGRADE CONSOLE",new Vector3(-8.8f,1.25f,-3.5f),new Vector3(1.7f,2.5f,1.1f),matBlue);
            terminal.AddComponent<UfoWashUpgradeStation>();
            Cube("Console screen",new Vector3(-8.8f,1.75f,-4.08f),new Vector3(1.35f,.8f,.06f),matGlow).GetComponent<Collider>().enabled=false;
            Sign("FAN NOZZLE // 200 CR",new Vector3(-8.8f,2.75f,-4.08f),.22f,matWhite);
            // Props: hose reels, barrels, crates and floor lamps
            for(int i=0;i<2;i++) {
                float x=i==0?-6.5f:6.5f;
                Cylinder("Chemical drum",new Vector3(x*1.23f,.8f,-7.8f),new Vector3(.55f,.8f,.55f),matBlue);
                Cylinder("Drum lid",new Vector3(x*1.23f,1.62f,-7.8f),new Vector3(.58f,.08f,.58f),matMetal);
                for(int y=0;y<3;y++) Cube("Drum hazard stripe",new Vector3(x*1.23f,.45f+y*.28f,-7.24f),new Vector3(.65f,.07f,.025f),matOrange);
            }
            // Lighting
            Light sun=new GameObject("Hangar key light").AddComponent<Light>(); sun.type=LightType.Directional; sun.intensity=1.0f; sun.color=new Color(.68f,.8f,1f); sun.transform.rotation=Quaternion.Euler(48,-28,0);
            AreaLight(new Vector3(0,10,1),new Color(.12f,.72f,1f),7);
            AreaLight(new Vector3(-8,7,10),new Color(.08f,.3f,1f),5);
            AreaLight(new Vector3(8,7,10),new Color(1f,.32f,.12f),5);
            AreaLight(new Vector3(0,7,17),new Color(.12f,.65f,1f),6);
            // Contract terminal and career loop: player must accept a customer order before spraying.
            var career = new GameObject("CAREER LOOP // CUSTOMER CONTRACTS");
            career.AddComponent<UfoWashProgressSave>();
            career.AddComponent<UfoWashCareerLoop>();
            var contractTerminal = Cube("CONTRACT TERMINAL // ACCEPT JOB", new Vector3(4.2f,1.25f,-3.7f), new Vector3(1.55f,2.5f,1.0f), matBlue);
            contractTerminal.AddComponent<UfoWashJobBoard>();
            var contractScreen = Cube("Contract terminal screen", new Vector3(4.2f,1.75f,-4.23f), new Vector3(1.25f,.82f,.045f), matGlow);
            contractScreen.GetComponent<Collider>().enabled = false;
            Sign("ACCEPT JOB", new Vector3(4.2f,2.42f,-4.25f), .27f, matWhite);
            Sign("CONTRACTS", new Vector3(4.2f,3.05f,-3.7f), .32f, matCyan);

            // Consumables are a real operating cost: refill water and foam chemical here.
            var supplyStation = Cube("SUPPLY STATION // WATER + FOAM", new Vector3(-4.2f,1.25f,-3.7f),
                new Vector3(1.55f,2.5f,1.0f), matBlue);
            supplyStation.AddComponent<UfoWashSupplyStation>();
            Cube("Supply station display", new Vector3(-4.2f,1.75f,-4.23f),
                new Vector3(1.25f,.82f,.045f), matGlow).GetComponent<Collider>().enabled = false;
            Sign("SUPPLIES // 65 CR", new Vector3(-4.2f,2.42f,-4.25f), .24f, matWhite);

            BuildWashAttendant(new Vector3(-5.2f, 0f, 1.2f));
            BuildBayRecoveryUpgrade();

            var hud=new GameObject("GAME HUD"); hud.AddComponent<UfoWashHUD>();
            var frontEnd = new GameObject("GAME FRONT END // STORY + TUTORIAL");
            frontEnd.AddComponent<UfoWashFrontEnd>();

            OrganizeHierarchy(scene);
            EditorSceneManager.SaveScene(scene,outputPath);
            if (outputPath == ScenePath)
            {
                var scenes=new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
                scenes.RemoveAll(s=>s.path==ScenePath); scenes.Insert(0,new EditorBuildSettingsScene(ScenePath,true)); EditorBuildSettings.scenes=scenes.ToArray();
            }
            Selection.activeObject=AssetDatabase.LoadAssetAtPath<SceneAsset>(outputPath);
            if (showDialog)
                EditorUtility.DisplayDialog("UFO WASH SCENE BUILT",
                    @"The UFO WASH scene is ready. Press Play.

WASD + mouse: move/look
E: interact / accept contract
1 / 2 / 3: pre-rinse / foam / final rinse
Left mouse: operate washer",
                    "PLAY");
            else
                Debug.Log("UFO WASH: preview generated at " + outputPath + ". Visual acceptance requires Play Mode evidence.");
        }

        static void BuildStarterBayServiceDetails()
        {
            // Restrained graphite / cyan service fixtures with amber access edges.
            // Low-profile details stay outside the walkable route around the ship.
            for (int side = -1; side <= 1; side += 2)
            {
                Cube("Service trench surround", new Vector3(side*5.8f,.025f,3.2f),new Vector3(.55f,.05f,9.2f),matDark);
                for (int i=0;i<23;i++)
                    Cube("Drain grate",new Vector3(side*5.8f,.055f,-1.2f+i*.4f),new Vector3(.45f,.025f,.055f),matMetal)
                        .GetComponent<Collider>().enabled=false;
                Cube("Fluid manifold",new Vector3(side*10.8f,.8f,7f),new Vector3(1.2f,1.6f,3f),matDark);
                for(int pipe=0;pipe<2;pipe++)
                {
                    var line=Cylinder("Water and foam riser",new Vector3(side*10.1f,2.6f,6.3f+pipe*1.4f),
                        new Vector3(.13f,1.9f,.13f),pipe==0?matCyan:matWhite);
                    line.GetComponent<Collider>().enabled=false;
                }
                Cube("Manifold service light",new Vector3(side*10.12f,1.15f,7f),new Vector3(.06f,.10f,2.3f),matGlow);
                for(int mark=0;mark<10;mark++)
                    Cube("Keep-clear amber marker",new Vector3(side*6.4f,.022f,-1.5f+mark),new Vector3(.22f,.025f,.32f),matOrange)
                        .GetComponent<Collider>().enabled=false;
            }
            Sign("01  PRE-RINSE    /    02  FOAM    /    03  RINSE",new Vector3(0,5.9f,19.02f),.27f,matWhite);
            Sign("DUST BASIN  /  INDEPENDENT SERVICE",new Vector3(0,5.25f,19.02f),.20f,matCyan);
        }

        static void BuildBayRecoveryUpgrade()
        {
            var console = Cube("BAY SYSTEMS // RECOVERY LOOP", new Vector3(8.8f,1.25f,-3.5f),
                new Vector3(1.7f,2.5f,1.1f), matBlue);
            Cube("Bay recovery console screen", new Vector3(8.8f,1.75f,-4.08f),
                new Vector3(1.35f,.8f,.06f), matGlow).GetComponent<Collider>().enabled = false;
            var consoleSign = Sign("BAY RECOVERY // UPGRADE", new Vector3(8.8f,2.75f,-4.08f), .22f, matWhite);
            consoleSign.transform.rotation = Quaternion.Euler(0f,180f,0f);

            var firstModule = new GameObject("VESPER ANNEX // WATER RECOVERY");
            firstModule.SetActive(false);
            var waterRoom = SpawnModularKitModel("room-small-variation",
                new Vector3(9.2f,-.25f,13.2f), Quaternion.identity, 6f);
            if (waterRoom == null)
                throw new System.InvalidOperationException("The included Kenney room-small-variation model is required for the bay recovery upgrade.");
            waterRoom.name = "VESPER ANNEX // WATER RECOVERY // CC0 KENNEY";
            waterRoom.transform.SetParent(firstModule.transform, true);
            var waterSign = Sign("WATER RECOVERY // BAY 01", new Vector3(9.2f,3.3f,10.1f), .25f, matCyan);
            waterSign.transform.rotation = Quaternion.Euler(0f,180f,0f);
            waterSign.transform.SetParent(firstModule.transform, true);

            var secondModule = new GameObject("VESPER ANNEX // DUAL-STAGE RECLAIMER");
            secondModule.SetActive(false);
            var dualStageRoom = SpawnModularKitModel("room-wide-variation",
                new Vector3(9.2f,-.25f,13.2f), Quaternion.identity, 7.4f);
            if (dualStageRoom == null)
                throw new System.InvalidOperationException("The included Kenney room-wide-variation model is required for the second bay recovery tier.");
            dualStageRoom.name = "VESPER ANNEX // DUAL-STAGE RECLAIMER // CC0 KENNEY";
            dualStageRoom.transform.SetParent(secondModule.transform, true);
            var dualStageSign = Sign("DUAL-STAGE RECOVERY // BAY 01", new Vector3(9.2f,3.3f,9.6f), .25f, matCyan);
            dualStageSign.transform.rotation = Quaternion.Euler(0f,180f,0f);
            dualStageSign.transform.SetParent(secondModule.transform, true);

            console.AddComponent<UfoWashBayRecoveryStation>().Configure(firstModule, secondModule);
        }

        static void BuildServiceAccess()
        {
            // Local washing needs real access above the canopy, not a global
            // progress shortcut. This candidate catwalk still requires an in-game traversal check.
            for(int step=1;step<=16;step++)
            {
                float height=step*.2f;
                Cube("Upper service stair "+step,new Vector3(6.2f,height*.5f,-6.4f+(step-.5f)*.35f),
                    new Vector3(1.6f,height,.35f),matMetal);
                Cube("Stair amber nosing",new Vector3(6.2f,height+.008f,-6.4f+(step-1)*.35f),
                    new Vector3(1.55f,.015f,.04f),matOrange).GetComponent<Collider>().enabled=false;
            }
            for(int side=-1;side<=1;side+=2)
            {
                Cube("Upper hull service deck",new Vector3(side*6.2f,3.1f,4.2f),new Vector3(1.6f,.2f,10f),matMetal);
                for(int support=0;support<3;support++)
                {
                    float z=-.3f+support*4.5f;
                    Cube("Deck support",new Vector3(side*6.8f,1.5f,z),new Vector3(.16f,3f,.16f),matDark);
                    Cube("Safety rail post",new Vector3(side*6.95f,3.7f,z),new Vector3(.08f,1f,.08f),matOrange);
                }
                Cube("Outer safety rail",new Vector3(side*6.95f,4.15f,4.2f),new Vector3(.08f,.10f,10f),matOrange);
                Cube("Deck cyan access edge",new Vector3(side*5.43f,3.22f,4.2f),new Vector3(.04f,.025f,9.8f),matCyan)
                    .GetComponent<Collider>().enabled=false;
            }
            Cube("Rear service bridge",new Vector3(0,3.1f,9.2f),new Vector3(14f,.2f,1.6f),matMetal);
            Cube("Rear bridge rail",new Vector3(0,4.15f,9.95f),new Vector3(14f,.10f,.08f),matOrange);
            Sign("UPPER HULL ACCESS",new Vector3(6.2f,1.1f,-6.7f),.22f,matWhite);
        }

        static void BuildWashAttendant(Vector3 position)
        {
            var attendant = new GameObject("WASH ATTENDANT // VESPER SERVICE TECH");
            attendant.transform.position = position;
            var capsule = attendant.AddComponent<CapsuleCollider>();
            capsule.height = 1.9f;
            capsule.radius = .34f;
            capsule.center = new Vector3(0f, .94f, 0f);

            if (!AttachImportedWashTech(attendant))
                BuildWashTechFallback(attendant);

            Part(attendant, "Tech tool // ion lance", PrimitiveType.Cylinder,
                new Vector3(.49f, .78f, .34f), new Vector3(.11f, .34f, .11f), matDark);
            Part(attendant, "Tech tool // status lens", PrimitiveType.Sphere,
                new Vector3(.49f, .96f, .34f), new Vector3(.14f, .12f, .14f), matGlow);
            Part(attendant, "Tech boot // left", PrimitiveType.Cube,
                new Vector3(-.2f, .12f, .1f), new Vector3(.3f, .22f, .43f), matDark);
            Part(attendant, "Tech boot // right", PrimitiveType.Cube,
                new Vector3(.2f, .12f, .1f), new Vector3(.3f, .22f, .43f), matDark);

            var worker = attendant.AddComponent<UfoWashWashAttendant>();
            Set(worker, "nozzle", attendant.transform.Find("Tech tool // ion lance"));
            Sign("VESPER SERVICE TECH // BAY 01",
                position + new Vector3(0f, 2.8f, 0f), .22f, matOrange);
        }

        static bool AttachImportedWashTech(GameObject attendant)
        {
            const string importedFolder = "Assets/UFO_WASH/Art/Imported/KenneySpaceKit";
            string[] guids = AssetDatabase.FindAssets("astronautA t:Model", new[] { importedFolder });
            string modelPath = null;
            for (int i = 0; i < guids.Length; i++)
            {
                string candidate = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (System.IO.Path.GetFileNameWithoutExtension(candidate)
                    .Equals("astronautA", System.StringComparison.OrdinalIgnoreCase))
                {
                    modelPath = candidate;
                    break;
                }
            }

            if (string.IsNullOrEmpty(modelPath)) return false;
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(modelPath);
            if (prefab == null) return false;
            var model = Object.Instantiate(prefab);
            if (model == null) return false;

            model.name = "KENNEY CC0 // ASTRONAUT WASH TECH";
            model.transform.SetParent(attendant.transform, false);
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;

            var renderers = model.GetComponentsInChildren<Renderer>(true);
            if (renderers.Length == 0)
            {
                Object.DestroyImmediate(model);
                return false;
            }

            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++) bounds.Encapsulate(renderers[i].bounds);
            float targetHeight = 1.9f;
            float scale = targetHeight / Mathf.Max(.01f, bounds.size.y);
            model.transform.localScale = Vector3.one * scale;
            bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++) bounds.Encapsulate(renderers[i].bounds);
            model.transform.position += attendant.transform.position +
                Vector3.up * (targetHeight * .5f) - bounds.center;

            for (int i = 0; i < renderers.Length; i++)
            {
                string partName = renderers[i].gameObject.name.ToLowerInvariant();
                Material replacement = partName.Contains("visor") || partName.Contains("glass")
                    ? matCyan : partName.Contains("dark") || partName.Contains("boot")
                        ? matDark : partName.Contains("blue") || partName.Contains("shoulder")
                            ? matBlue : matWhite;
                var source = renderers[i].sharedMaterials;
                var materials = new Material[Mathf.Max(1, source != null ? source.Length : 0)];
                for (int slot = 0; slot < materials.Length; slot++) materials[slot] = replacement;
                renderers[i].sharedMaterials = materials;
            }
            return true;
        }

        static void BuildWashTechFallback(GameObject attendant)
        {
            Part(attendant, "Tech torso // ceramic suit", PrimitiveType.Capsule,
                new Vector3(0f, .95f, 0f), new Vector3(.62f, .92f, .5f), matWhite);
            Part(attendant, "Tech head // sealed helmet", PrimitiveType.Sphere,
                new Vector3(0f, 1.72f, .02f), new Vector3(.58f, .5f, .48f), matMetal);
            Part(attendant, "Tech visor // cyan glass", PrimitiveType.Cube,
                new Vector3(0f, 1.75f, .25f), new Vector3(.43f, .17f, .055f), matCyan);
            Part(attendant, "Tech shoulder // left", PrimitiveType.Sphere,
                new Vector3(-.43f, 1.22f, 0f), new Vector3(.28f, .25f, .3f), matBlue);
            Part(attendant, "Tech shoulder // right", PrimitiveType.Sphere,
                new Vector3(.43f, 1.22f, 0f), new Vector3(.28f, .25f, .3f), matBlue);
            Part(attendant, "Tech arm // left", PrimitiveType.Capsule,
                new Vector3(-.46f, .82f, .12f), new Vector3(.19f, .52f, .2f), matWhite);
            Part(attendant, "Tech arm // right", PrimitiveType.Capsule,
                new Vector3(.46f, .82f, .12f), new Vector3(.19f, .52f, .2f), matWhite);
        }

        static void OrganizeHierarchy(Scene scene)
        {
            // Keep the generated scene readable in the Hierarchy. Preserve world transforms
            // while moving every generated root into a named, stable subsystem container.
            var roots = scene.GetRootGameObjects();
            Transform world = CreateGroup("00 // HANGAR ARCHITECTURE");
            Transform equipment = CreateGroup("01 // SERVICE EQUIPMENT & STORAGE");
            Transform vessel = CreateGroup("02 // CUSTOMER VESSEL");
            Transform customers = CreateGroup("03 // CUSTOMERS & NPC");
            Transform player = CreateGroup("04 // PLAYER");
            Transform systems = CreateGroup("05 // GAMEPLAY SYSTEMS");
            Transform lighting = CreateGroup("06 // LIGHTING & POST PROCESSING");
            Transform interfaceRoot = CreateGroup("07 // UI & FRONT END");

            foreach (var root in roots)
            {
                if (root == null) continue;
                string n = root.name.ToLowerInvariant();
                Transform target = world;
                if (n.StartsWith("player")) target = player;
                else if (n.Contains("global volume") || n.Contains("light")) target = lighting;
                else if (n.Contains("imported 3d model // craft") || n.Contains("customer ufo")) target = vessel;
                else if (n.Contains("alien customer") || n.Contains("imported 3d model // alien") || n.Contains("sign // customer")) target = customers;
                else if (n.Contains("game front end") || n.Contains("game hud") || n.Contains("front end")) target = interfaceRoot;
                else if (n.Contains("career loop") || n.Contains("contract terminal")) target = systems;
                else if (n.Contains("terminal") || n.Contains("supply") || n.Contains("upgrade") ||
                    n.Contains("chemical drum") || n.Contains("drum lid") || n.Contains("drum hazard") ||
                    n.Contains("attendant") ||
                    n.Contains("gantry") || n.Contains("spray nozzle") || n.Contains("nozzle hose") ||
                    n.Contains("washer") || n.Contains("hose reel")) target = equipment;

                root.transform.SetParent(target, true);
            }
        }

        static Transform CreateGroup(string groupName)
        {
            var group = new GameObject(groupName);
            return group.transform;
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

            {
                foreach (var renderer in renderers) {
                    // Imported meshes can have several material slots. Replacing only
                    // sharedMaterial fixes slot zero and leaves the remaining slots magenta.
                    Material[] sourceMaterials = renderer.sharedMaterials;
                    if (sourceMaterials == null || sourceMaterials.Length == 0)
                        sourceMaterials = new Material[] { matMetal };

                    var safeMaterials = new Material[sourceMaterials.Length];
                    for (int slot = 0; slot < sourceMaterials.Length; slot++) {
                        string materialName = sourceMaterials[slot] != null
                            ? sourceMaterials[slot].name.ToLowerInvariant() : string.Empty;
                        string partName = renderer.gameObject.name.ToLowerInvariant();
                        string semantic = materialName + " " + partName;

                        if (assetName == "alien") safeMaterials[slot] = semantic.Contains("eye") ? matDark : matCyan;
                        else if (semantic.Contains("glass") || semantic.Contains("window") ||
                            semantic.Contains("canopy") || semantic.Contains("cockpit"))
                            safeMaterials[slot] = matGlass;
                        else if (semantic.Contains("light") || semantic.Contains("engine") ||
                            semantic.Contains("thruster") || semantic.Contains("glow"))
                            safeMaterials[slot] = matGlow;
                        else if (semantic.Contains("orange") || semantic.Contains("warning") ||
                            semantic.Contains("red") || semantic.Contains("hazard"))
                            safeMaterials[slot] = matOrange;
                        else if (semantic.Contains("blue") || semantic.Contains("navy"))
                            safeMaterials[slot] = matBlue;
                        else if (semantic.Contains("dark") || semantic.Contains("black"))
                            safeMaterials[slot] = matDark;
                        else
                            safeMaterials[slot] = matMetal;
                    }
                    renderer.sharedMaterials = safeMaterials;
                }
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

        static void AddCustomerInteraction(GameObject customer, bool importedModel)
        {
            if (customer == null) return;
            customer.AddComponent<UfoWashCustomerNpc>();
            var capsule = customer.GetComponent<CapsuleCollider>();
            if (capsule == null) capsule = customer.AddComponent<CapsuleCollider>();
            capsule.direction = 1;
            capsule.radius = importedModel ? 0.46f : 0.55f;
            capsule.height = importedModel ? 2.15f : 2.5f;
            capsule.center = importedModel ? Vector3.zero : new Vector3(0f, 1.25f, 0f);
        }

        static void BuildPlayerCharacter(Transform root) {
            var suit = matWhite;
            var armor = matBlue;
            var visor = matGlass;
            var glow = matGlow;

            Part(root.gameObject,"Astronaut torso",PrimitiveType.Capsule,new Vector3(0,1.12f,0),new Vector3(.62f,.78f,.38f),suit);
            Part(root.gameObject,"Chest armor",PrimitiveType.Cube,new Vector3(0,1.22f,.205f),new Vector3(.43f,.48f,.12f),armor);
            Part(root.gameObject,"Helmet",PrimitiveType.Sphere,new Vector3(0,1.82f,.015f),new Vector3(.57f,.58f,.52f),suit);
            Part(root.gameObject,"Dark panoramic visor",PrimitiveType.Sphere,new Vector3(0,1.85f,.225f),new Vector3(.46f,.35f,.16f),visor);
            Part(root.gameObject,"Helmet comm left",PrimitiveType.Cylinder,new Vector3(-.3f,1.82f,0),new Vector3(.09f,.055f,.09f),glow);
            Part(root.gameObject,"Helmet comm right",PrimitiveType.Cylinder,new Vector3(.3f,1.82f,0),new Vector3(.09f,.055f,.09f),glow);
            Part(root.gameObject,"Life support backpack",PrimitiveType.Cube,new Vector3(0,1.12f,-.29f),new Vector3(.52f,.72f,.28f),armor);
            Part(root.gameObject,"Backpack status light",PrimitiveType.Cube,new Vector3(0,1.25f,-.445f),new Vector3(.25f,.12f,.025f),glow);
            Part(root.gameObject,"Left upper arm",PrimitiveType.Capsule,new Vector3(-.43f,1.2f,0),new Vector3(.22f,.48f,.23f),suit);
            Part(root.gameObject,"Right upper arm",PrimitiveType.Capsule,new Vector3(.43f,1.2f,0),new Vector3(.22f,.48f,.23f),suit);
            Part(root.gameObject,"Left glove",PrimitiveType.Sphere,new Vector3(-.46f,.83f,.08f),new Vector3(.23f,.2f,.23f),armor);
            Part(root.gameObject,"Right glove",PrimitiveType.Sphere,new Vector3(.46f,.83f,.08f),new Vector3(.23f,.2f,.23f),armor);
            Part(root.gameObject,"Ion washer pistol grip",PrimitiveType.Cube,new Vector3(.48f,.82f,.3f),new Vector3(.16f,.2f,.2f),matDark);
            Part(root.gameObject,"Ion washer barrel",PrimitiveType.Cylinder,new Vector3(.48f,.84f,.49f),new Vector3(.065f,.2f,.065f),matMetal);
            root.Find("Ion washer barrel").localRotation = Quaternion.Euler(90f,0f,0f);
            Part(root.gameObject,"Ion washer muzzle",PrimitiveType.Sphere,new Vector3(.48f,.84f,.68f),new Vector3(.12f,.12f,.12f),glow);
            Part(root.gameObject,"Left leg",PrimitiveType.Capsule,new Vector3(-.17f,.43f,0),new Vector3(.25f,.55f,.27f),suit);
            Part(root.gameObject,"Right leg",PrimitiveType.Capsule,new Vector3(.17f,.43f,0),new Vector3(.25f,.55f,.27f),suit);
            Part(root.gameObject,"Left boot",PrimitiveType.Cube,new Vector3(-.17f,.13f,.08f),new Vector3(.29f,.2f,.4f),armor);
            Part(root.gameObject,"Right boot",PrimitiveType.Cube,new Vector3(.17f,.13f,.08f),new Vector3(.29f,.2f,.4f),armor);
            Part(root.gameObject,"Chest insignia",PrimitiveType.Cube,new Vector3(0,1.34f,.275f),new Vector3(.16f,.09f,.025f),glow);

            // Disable decorative suit-part colliders only. Keep the root CharacterController active.
            foreach (var collider in root.GetComponentsInChildren<Collider>(true))
                if (!(collider is CharacterController)) collider.enabled = false;
        }

        static void BuildFirstPersonTool(Transform cameraRoot)
        {
            var root = new GameObject("FIRST PERSON ION WASHER VIEWMODEL");
            root.transform.SetParent(cameraRoot, false);
            root.transform.localPosition = Vector3.zero;
            Part(root, "Washer body", PrimitiveType.Cube, new Vector3(.36f,-.30f,.58f), new Vector3(.19f,.16f,.42f), matBlue);
            Part(root, "Washer upper housing", PrimitiveType.Cube, new Vector3(.36f,-.205f,.60f), new Vector3(.14f,.045f,.28f), matMetal);
            Part(root, "Washer grip", PrimitiveType.Cube, new Vector3(.36f,-.405f,.48f), new Vector3(.11f,.19f,.13f), matDark);
            Part(root, "Washer barrel", PrimitiveType.Cylinder, new Vector3(.36f,-.29f,.88f), new Vector3(.055f,.22f,.055f), matMetal);
            root.transform.Find("Washer barrel").localRotation = Quaternion.Euler(90f,0f,0f);
            Part(root, "Washer nozzle", PrimitiveType.Sphere, new Vector3(.36f,-.29f,1.00f), new Vector3(.105f,.105f,.105f), matGlow);
            Part(root, "Washer pressure gauge", PrimitiveType.Cylinder, new Vector3(.43f,-.205f,.56f), new Vector3(.045f,.025f,.045f), matOrange);
            foreach (var collider in root.GetComponentsInChildren<Collider>(true)) collider.enabled = false;
        }

        static void BuildOuterRimVista()
        {
            // A real vista beyond the bay gives the station a place in the Vesper Belt.
            // The skyfield and banded planet are original procedural assets; the remote
            // relays use only the included CC0 Kenney Space Kit.
            BuildStarfieldBackdrop();
            Cube("VESPER BELT // BASALT APRON", new Vector3(0,-1.15f,34), new Vector3(110,1.1f,96), matBasalt);

            // Sculpted, triangulated mesas replace the old row of rotated box silhouettes.
            for (int i = 0; i < 16; i++)
            {
                bool rightSide = i >= 8;
                int index = i % 8;
                float x = (rightSide ? 1f : -1f) * (13f + index * 3.9f);
                float z = 32f + (index % 4) * 6.4f;
                float width = 7f + (index % 3) * 2.2f;
                float height = 5f + Mathf.PerlinNoise(index * .53f, rightSide ? .91f : .19f) * 11f;
                float depth = 8f + (index % 4) * 1.6f;
                BuildLowPolyMesa("VESPER BELT // BASALT MESA", new Vector3(x, -.55f, z),
                    new Vector3(width, height, depth), 120 + i, (i % 3 == 0) ? matBlue : matBasalt);

                if (index % 2 == 0)
                {
                    Vector3 crystalBase = new Vector3(x + (rightSide ? -2.3f : 2.3f), -.55f + height * .58f, z - 1.2f);
                    BuildCrystalCluster(crystalBase, 210 + i);
                }
            }

            var planet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            planet.name = "VESPER // THE AMBER VEIL GAS GIANT";
            planet.transform.position = new Vector3(0f, 8.2f, 45f);
            planet.transform.localScale = new Vector3(18f, 14f, 7f);
            planet.transform.rotation = Quaternion.Euler(8f, 0f, -7f);
            planet.GetComponent<Renderer>().sharedMaterial = matPlanet;
            Object.DestroyImmediate(planet.GetComponent<Collider>());

            var ring = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            ring.name = "VESPER // ICE PARTICLE RING";
            ring.transform.position = new Vector3(0f, 8.2f, 49.6f);
            ring.transform.localScale = new Vector3(18.2f, .045f, 18.2f);
            ring.transform.rotation = Quaternion.Euler(68f, 0f, 13f);
            ring.GetComponent<Renderer>().sharedMaterial = matIce;
            Object.DestroyImmediate(ring.GetComponent<Collider>());

            var moon = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            moon.name = "VESPER // FROST MOON NARA";
            moon.transform.position = new Vector3(8.2f, 10.1f, 58f);
            moon.transform.localScale = new Vector3(4.3f, 4.3f, 2.8f);
            moon.GetComponent<Renderer>().sharedMaterial = matIce;
            Object.DestroyImmediate(moon.GetComponent<Collider>());

            SpawnImportedModel("satelliteDish_large", new Vector3(-11f, 3f, 42f), Quaternion.Euler(0f, 30f, 0f), 1f, false);
            SpawnImportedModel("satelliteDish_large", new Vector3(11f, 3f, 46f), Quaternion.Euler(0f, -35f, 0f), 1f, false);
            SpawnImportedModel("hangar_smallA", new Vector3(-20f, 3f, 55f), Quaternion.Euler(0f, 150f, 0f), 1f, false);
            SpawnImportedModel("hangar_smallB", new Vector3(20f, 3f, 58f), Quaternion.Euler(0f, 205f, 0f), 1f, false);

            for (int i = 0; i < 24; i++)
            {
                float x = -38 + (i * 13 % 76);
                float y = 2 + (i * 17 % 24);
                float z = 25 + (i * 19 % 48);
                var beacon = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                beacon.name = "VESPER SETTLEMENT // DISTANT BEACON";
                beacon.transform.position = new Vector3(x,y,z);
                beacon.transform.localScale = Vector3.one * (.12f + (i % 3) * .05f);
                beacon.GetComponent<Renderer>().sharedMaterial = (i % 4 == 0) ? matOrange : matGlow;
                Object.DestroyImmediate(beacon.GetComponent<Collider>());
            }
        }

        static void BuildVesperLandingYard()
        {
            // The bay is a real doorway onto a small service compound, not a painted vista.
            // This sloped apron joins the hangar floor to the lower basalt plateau.
            var apron = Cube("VESPER OUTPOST // RAMPED ARRIVAL CAUSEWAY",
                new Vector3(0f,-.29f,27.4f), new Vector3(10.5f,.30f,16.2f), matFloor);
            apron.transform.rotation = Quaternion.Euler(2f,0f,0f);
            for (int x = -4; x <= 4; x += 8)
            {
                var edge = Cube("VESPER OUTPOST // CAUSEWAY GUIDE LIGHT",
                    new Vector3(x,-.13f,27.4f), new Vector3(.09f,.035f,15.8f), matGlow);
                edge.transform.rotation = Quaternion.Euler(2f,0f,0f);
            }

            // CC0 Kenney modular rooms and corridors are recolored with UFO WASH's
            // graphite, Vesper teal and amber hazard palette before they enter the scene.
            SpawnModularKitModel("gate-door-window", new Vector3(0f,.10f,20.2f),
                Quaternion.identity, 13.5f);
            SpawnModularKitModel("corridor-wide", new Vector3(0f,-.18f,27.5f),
                Quaternion.identity, 10.5f);
            SpawnModularKitModel("room-wide", new Vector3(-14.8f,-.54f,39.5f),
                Quaternion.Euler(0f,18f,0f), 11f);
            SpawnModularKitModel("room-large-variation", new Vector3(14.7f,-.54f,41f),
                Quaternion.Euler(0f,-14f,0f), 11.5f);
            SpawnModularKitModel("room-small-variation", new Vector3(-22f,-.54f,48f),
                Quaternion.Euler(0f,28f,0f), 7.5f);
            SpawnModularKitModel("corridor-junction", new Vector3(0f,-.54f,39.5f),
                Quaternion.Euler(0f,90f,0f), 8.2f);

            // A named utility spine and landing marks give the outside space scale and use.
            Cube("VESPER OUTPOST // WATER AND POWER SPINE", new Vector3(-5.2f,.2f,38f),
                new Vector3(.36f,.38f,19f), matMetal);
            for (int i = 0; i < 7; i++)
            {
                float z = 22f + i * 3.1f;
                Cylinder("VESPER OUTPOST // CAUSEWAY BEACON", new Vector3(5.6f,.6f,z),
                    new Vector3(.2f,.8f,.2f), i % 2 == 0 ? matGlow : matOrange);
                Sphere("VESPER OUTPOST // BEACON LENS", new Vector3(5.6f,1.1f,z), .22f,
                    i % 2 == 0 ? matGlow : matOrange);
            }
            Sign("BAY 01 // CUSTOMER DOCK", new Vector3(0f,5.2f,20.1f), .55f, matCyan);
            Sign("VESPER BELT // INDEPENDENT SERVICE YARD", new Vector3(0f,4.2f,20.1f), .24f, matWhite);

            SpawnImportedModel("craft_miner", new Vector3(-24f,.2f,38f),
                Quaternion.Euler(0f,168f,0f), 1f, false);
            SpawnImportedModel("rover", new Vector3(21f,-.3f,45f),
                Quaternion.Euler(0f,-22f,0f), 1f, false);

            BuildSectorExpansionGate("crater-ridge", "CRATER RIDGE", new Vector3(0f,2.5f,63f),
                2500, 2.6f, new Vector3(0f,-.54f,76f), "room-large");
            BuildSectorExpansionGate("ion-flats", "ION FLATS", new Vector3(43f,2.5f,34f),
                9000, 3.5f, new Vector3(50f,-.54f,40f), "room-wide");
            BuildSectorExpansionGate("glasswake-reach", "GLASSWAKE REACH", new Vector3(-43f,2.5f,34f),
                25000, 4.2f, new Vector3(-50f,-.54f,40f), "room-large-variation");
        }

        static void BuildSectorExpansionGate(string key, string sectorName, Vector3 gateCenter,
            int cost, float reputation, Vector3 contentPosition, string roomAsset)
        {
            string title = sectorName.ToUpperInvariant();
            Cube("SECTOR GATE // " + title + " // LEFT PYLON",
                gateCenter + new Vector3(-5.2f,0f,0f), new Vector3(.7f,5.6f,.8f), matMetal);
            Cube("SECTOR GATE // " + title + " // RIGHT PYLON",
                gateCenter + new Vector3(5.2f,0f,0f), new Vector3(.7f,5.6f,.8f), matMetal);
            Cube("SECTOR GATE // " + title + " // CROWN",
                gateCenter + new Vector3(0f,2.75f,0f), new Vector3(11f,.75f,.9f), matDark);
            Cube("SECTOR GATE // " + title + " // STATUS STRIP",
                gateCenter + new Vector3(0f,2.25f,-.48f), new Vector3(8.8f,.12f,.07f), matGlow);
            Sign(title + " // LAND CLAIM", gateCenter + new Vector3(0f,3.8f,-.52f), .42f, matWhite);
            Sign(cost.ToString("N0") + " GC  //  " + reputation.ToString("0.0") + " REP",
                gateCenter + new Vector3(0f,-3.65f,-.52f), .26f, matOrange);

            var barrier = Cube("SECTOR GATE // " + title + " // CLAIM TO OPEN",
                gateCenter, new Vector3(9.4f,4.8f,.22f), matGlass);
            var sectorContent = new GameObject("SECTOR CONTENT // " + title + " // CLAIMED LAND");
            var room = SpawnModularKitModel(roomAsset, contentPosition, Quaternion.identity, 11f);
            if (room != null) room.transform.SetParent(sectorContent.transform, true);
            sectorContent.SetActive(false);

            var sectorGate = barrier.AddComponent<UfoWashSectorGate>();
            sectorGate.Configure(key, title, cost, reputation, barrier, sectorContent);
            EditorUtility.SetDirty(sectorGate);
        }

        static GameObject SpawnModularKitModel(string assetName, Vector3 position, Quaternion rotation, float targetFootprint)
        {
            const string modelFolder = "Assets/UFO_WASH/Art/Imported/KenneyModularSpaceKit/Models";
            string selectedPath = null;
            foreach (string guid in AssetDatabase.FindAssets("t:Model", new[] { modelFolder }))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (System.IO.Path.GetFileNameWithoutExtension(path).Equals(assetName,
                    System.StringComparison.OrdinalIgnoreCase))
                {
                    selectedPath = path;
                    break;
                }
            }

            if (string.IsNullOrEmpty(selectedPath))
            {
                Debug.LogWarning("UFO WASH: Kenney modular piece not found: " + assetName);
                return null;
            }

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(selectedPath);
            if (prefab == null) return null;
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            instance.name = "VESPER OUTPOST // MODULAR " + assetName.ToUpperInvariant();
            instance.transform.rotation = rotation;

            var renderers = instance.GetComponentsInChildren<Renderer>(true);
            if (renderers.Length == 0)
            {
                instance.transform.SetPositionAndRotation(position, rotation);
                return instance;
            }

            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++) bounds.Encapsulate(renderers[i].bounds);
            float footprint = Mathf.Max(bounds.size.x, bounds.size.z);
            float fitScale = footprint > .001f ? targetFootprint / footprint : 1f;
            instance.transform.localScale = Vector3.one * fitScale;
            bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++) bounds.Encapsulate(renderers[i].bounds);
            instance.transform.position += position - new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);

            foreach (var renderer in renderers)
            {
                int materialCount = renderer.sharedMaterials != null ? renderer.sharedMaterials.Length : 1;
                var materials = new Material[Mathf.Max(1, materialCount)];
                Material stationMaterial = matVesperModular != null ? matVesperModular : matMetal;
                for (int i = 0; i < materials.Length; i++) materials[i] = stationMaterial;
                renderer.sharedMaterials = materials;
            }
            return instance;
        }

        static void BuildStarfieldBackdrop()
        {
            var sky = new GameObject("VESPER BELT // AURORA SKYFIELD");
            sky.transform.position = new Vector3(0f, 19f, 94f);
            var mesh = new Mesh { name = "Vesper Skyfield Panorama Mesh" };
            mesh.vertices = new[]
            {
                new Vector3(-110f, -70f, 0f), new Vector3(110f, -70f, 0f),
                new Vector3(110f, 70f, 0f), new Vector3(-110f, 70f, 0f)
            };
            mesh.uv = new[] { new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(0f, 1f) };
            mesh.triangles = new[] { 0, 2, 1, 0, 3, 2 }; // front face points toward the player at -Z
            mesh.RecalculateNormals();
            sky.AddComponent<MeshFilter>().sharedMesh = mesh;
            sky.AddComponent<MeshRenderer>().sharedMaterial = matStarfield;
        }

        static void BuildLowPolyMesa(string objectName, Vector3 center, Vector3 size, int seed, Material material)
        {
            const int sides = 9;
            var random = new System.Random(seed);
            var rings = new Vector3[3][];
            float[] radius = { 1f, .78f, .54f };
            float[] height = { 0f, .32f, .76f };
            float angleOffset = (float)random.NextDouble() * Mathf.PI * 2f;
            for (int ring = 0; ring < rings.Length; ring++)
            {
                rings[ring] = new Vector3[sides];
                for (int i = 0; i < sides; i++)
                {
                    float angle = angleOffset + i * Mathf.PI * 2f / sides;
                    float jitter = .82f + (float)random.NextDouble() * .36f;
                    float vertical = height[ring] + ((float)random.NextDouble() - .5f) * (ring == 0 ? .04f : .14f);
                    rings[ring][i] = new Vector3(
                        Mathf.Cos(angle) * size.x * .5f * radius[ring] * jitter,
                        size.y * vertical,
                        Mathf.Sin(angle) * size.z * .5f * radius[ring] * jitter);
                }
            }

            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            for (int ring = 0; ring < rings.Length - 1; ring++)
            {
                for (int i = 0; i < sides; i++)
                {
                    int next = (i + 1) % sides;
                    AddTriangle(vertices, triangles, rings[ring][i], rings[ring + 1][i], rings[ring][next]);
                    AddTriangle(vertices, triangles, rings[ring][next], rings[ring + 1][i], rings[ring + 1][next]);
                }
            }
            Vector3 crown = new Vector3(0f, size.y * .8f, 0f);
            for (int i = 0; i < sides; i++)
                AddTriangle(vertices, triangles, crown, rings[2][(i + 1) % sides], rings[2][i]);

            var mesh = new Mesh { name = objectName + " Mesh" };
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            var mesa = new GameObject(objectName);
            mesa.transform.position = center;
            mesa.AddComponent<MeshFilter>().sharedMesh = mesh;
            mesa.AddComponent<MeshRenderer>().sharedMaterial = material;
        }

        static void BuildCrystalCluster(Vector3 basePosition, int seed)
        {
            var random = new System.Random(seed);
            int count = 3 + seed % 3;
            for (int i = 0; i < count; i++)
            {
                float angle = (float)random.NextDouble() * Mathf.PI * 2f;
                float radius = .2f + (float)random.NextDouble() * .95f;
                float height = 1.1f + (float)random.NextDouble() * 2.5f;
                float width = .25f + (float)random.NextDouble() * .38f;
                Vector3 position = basePosition + new Vector3(Mathf.Cos(angle) * radius, height * .5f, Mathf.Sin(angle) * radius);
                BuildCrystal("VESPER BELT // LUMEN CRYSTAL", position, width, height, seed + i * 41);
            }
        }

        static void BuildCrystal(string objectName, Vector3 center, float width, float height, int seed)
        {
            const int sides = 6;
            float angleOffset = (seed % 360) * Mathf.Deg2Rad;
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            Vector3[] baseRing = new Vector3[sides];
            Vector3[] shoulderRing = new Vector3[sides];
            for (int i = 0; i < sides; i++)
            {
                float angle = angleOffset + i * Mathf.PI * 2f / sides;
                float jagged = .86f + ((seed * (i + 3) % 17) / 100f);
                baseRing[i] = new Vector3(Mathf.Cos(angle) * width * jagged, -height * .5f, Mathf.Sin(angle) * width * jagged);
                shoulderRing[i] = new Vector3(Mathf.Cos(angle) * width * .65f, height * .22f, Mathf.Sin(angle) * width * .65f);
            }
            Vector3 tip = new Vector3(Mathf.Sin(seed) * width * .18f, height * .5f, Mathf.Cos(seed) * width * .18f);
            for (int i = 0; i < sides; i++)
            {
                int next = (i + 1) % sides;
                AddTriangle(vertices, triangles, baseRing[i], shoulderRing[i], baseRing[next]);
                AddTriangle(vertices, triangles, baseRing[next], shoulderRing[i], shoulderRing[next]);
                AddTriangle(vertices, triangles, shoulderRing[i], tip, shoulderRing[next]);
            }
            var mesh = new Mesh { name = objectName + " Mesh" };
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            var crystal = new GameObject(objectName);
            crystal.transform.position = center;
            crystal.AddComponent<MeshFilter>().sharedMesh = mesh;
            crystal.AddComponent<MeshRenderer>().sharedMaterial = matIce;
        }

        static void AddTriangle(List<Vector3> vertices, List<int> triangles, Vector3 a, Vector3 b, Vector3 c)
        {
            int first = vertices.Count;
            vertices.Add(a); vertices.Add(b); vertices.Add(c);
            triangles.Add(first); triangles.Add(first + 1); triangles.Add(first + 2);
        }
        static void EnsureFoldersAndMaterials() {
            foreach(string f in new[]{"Assets/UFO_WASH/Art/Materials","Assets/UFO_WASH/Art/Textures","Assets/UFO_WASH/Scenes"}) if(!AssetDatabase.IsValidFolder(f)) System.IO.Directory.CreateDirectory(f);
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
            matBasalt=MakeMat("PS_VesperBasalt",new Color(.075f,.105f,.18f),.22f);
            matIce=MakeMat("PS_VesperLumenCrystal",new Color(.18f,.68f,.82f),.34f);
            matIce.EnableKeyword("_EMISSION"); matIce.SetColor("_EmissionColor",new Color(.035f,.22f,.34f));
            matPlanet=MakeMat("PS_VesperPlanetSurface",Color.white,.48f);
            matPlanet.SetTexture("_BaseMap",EnsureProceduralTexture("Assets/UFO_WASH/Art/Textures/Vesper_AmberVeil_Albedo.png",true));
            matPlanet.EnableKeyword("_EMISSION"); matPlanet.SetColor("_EmissionColor",new Color(.08f,.07f,.10f));
            matStarfield=MakeUnlitTextureMat("PS_VesperAuroraSkyfield",EnsureProceduralTexture("Assets/UFO_WASH/Art/Textures/Vesper_Aurora_Skyfield.png",false));
            matVesperModular=MakeMat("PS_VesperModularStation",Color.white,.36f);
            var modularPalette=AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/UFO_WASH/Art/Textures/Vesper_ModularSpace_Colormap.png");
            if(modularPalette!=null)
            {
                if(matVesperModular.HasProperty("_BaseMap")) matVesperModular.SetTexture("_BaseMap",modularPalette);
                else if(matVesperModular.HasProperty("_MainTex")) matVesperModular.SetTexture("_MainTex",modularPalette);
                EditorUtility.SetDirty(matVesperModular);
            }
        }

        static Material MakeUnlitTextureMat(string name, Texture2D texture)
        {
            string path="Assets/UFO_WASH/Art/Materials/"+name+".mat";
            var material=AssetDatabase.LoadAssetAtPath<Material>(path);
            var shader=Shader.Find("Universal Render Pipeline/Unlit");
            if(shader==null) throw new System.InvalidOperationException("URP/Unlit shader is unavailable.");
            if(material==null){material=new Material(shader);AssetDatabase.CreateAsset(material,path);}
            else if(material.shader!=shader) material.shader=shader;
            if(material.HasProperty("_BaseMap")) material.SetTexture("_BaseMap",texture);
            if(material.HasProperty("_BaseColor")) material.SetColor("_BaseColor",Color.white);
            material.renderQueue=(int)RenderQueue.Geometry;
            EditorUtility.SetDirty(material);
            return material;
        }

        static Texture2D EnsureProceduralTexture(string path, bool gasGiant)
        {
            var existing=AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if(existing!=null) return existing;

            const int width=1024;
            const int height=512;
            var texture=new Texture2D(width,height,TextureFormat.RGBA32,true,false);
            texture.name=System.IO.Path.GetFileNameWithoutExtension(path);
            var pixels=new Color[width*height];
            for(int y=0;y<height;y++)
            {
                float v=y/(float)(height-1);
                for(int x=0;x<width;x++)
                {
                    float u=x/(float)(width-1);
                    Color color;
                    if(gasGiant)
                    {
                        float turbulence=Mathf.PerlinNoise(u*5.7f+2.2f,v*8.5f+1.4f);
                        float warp=(Mathf.PerlinNoise(u*3.1f+8f,v*4.2f+6f)-.5f)*.11f;
                        float bands=.5f+.5f*Mathf.Sin((v+warp)*Mathf.PI*2f*13f);
                        Color deep=new Color(.045f,.095f,.19f);
                        Color teal=new Color(.12f,.36f,.47f);
                        Color copper=new Color(.58f,.23f,.20f);
                        Color sand=new Color(.87f,.55f,.34f);
                        color=Color.Lerp(deep,teal,Mathf.SmoothStep(.12f,.78f,bands));
                        float storm=Mathf.SmoothStep(.48f,.78f,turbulence)*Mathf.SmoothStep(.2f,.8f,1f-bands);
                        color=Color.Lerp(color,copper,storm*.72f);
                        color=Color.Lerp(color,sand,Mathf.SmoothStep(.82f,.98f,bands)*(.38f+turbulence*.35f));
                        color*=.78f+turbulence*.42f;
                    }
                    else
                    {
                        Color deep=new Color(.006f,.012f,.04f);
                        Color upper=new Color(.025f,.05f,.13f);
                        color=Color.Lerp(deep,upper,Mathf.SmoothStep(0f,1f,v));
                        float cloud=Mathf.PerlinNoise(u*5.2f+3.3f,v*7.4f+9.1f);
                        float ridge=.49f+.08f*Mathf.Sin(u*15f+cloud*5f);
                        float aurora=Mathf.Exp(-Mathf.Pow((v-ridge)/.16f,2f))*Mathf.SmoothStep(.26f,.76f,cloud);
                        color=Color.Lerp(color,new Color(.025f,.24f,.33f),aurora*.72f);
                        float violet=Mathf.Exp(-Mathf.Pow((v-(.72f+.05f*Mathf.Sin(u*9f)))/.09f,2f))*cloud;
                        color=Color.Lerp(color,new Color(.25f,.065f,.22f),violet*.62f);
                    }
                    pixels[y*width+x]=new Color(color.r,color.g,color.b,1f);
                }
            }

            if(!gasGiant)
            {
                var random=new System.Random(2189);
                for(int star=0;star<1100;star++)
                {
                    int sx=random.Next(0,width);
                    int sy=random.Next(0,height);
                    int radius=random.NextDouble()>.94 ? 2 : 1;
                    float strength=.45f+(float)random.NextDouble()*.55f;
                    Color tint=random.NextDouble()>.84 ? new Color(.46f,.78f,1f) : Color.white;
                    for(int oy=-radius;oy<=radius;oy++)
                    for(int ox=-radius;ox<=radius;ox++)
                    {
                        int px=sx+ox,py=sy+oy;
                        if(px<0||px>=width||py<0||py>=height) continue;
                        float falloff=1f/(1f+ox*ox+oy*oy);
                        Color starColor=tint*(strength*falloff);
                        int pixelIndex=py*width+px;
                        Color existingPixel=pixels[pixelIndex];
                        pixels[pixelIndex]=new Color(Mathf.Max(existingPixel.r,starColor.r),Mathf.Max(existingPixel.g,starColor.g),Mathf.Max(existingPixel.b,starColor.b),1f);
                    }
                }
            }

            texture.SetPixels(pixels);
            texture.Apply(true,false);
            string absolute=System.IO.Path.GetFullPath(path);
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(absolute));
            System.IO.File.WriteAllBytes(absolute,texture.EncodeToPNG());
            Object.DestroyImmediate(texture);
            AssetDatabase.ImportAsset(path,ImportAssetOptions.ForceUpdate);
            var importer=AssetImporter.GetAtPath(path) as TextureImporter;
            if(importer!=null)
            {
                importer.textureType=TextureImporterType.Default;
                importer.sRGBTexture=true;
                importer.wrapMode=TextureWrapMode.Repeat;
                importer.filterMode=FilterMode.Bilinear;
                importer.mipmapEnabled=true;
                importer.maxTextureSize=2048;
                importer.SaveAndReimport();
            }
            return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
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
            // Existing generated materials may have been created with Built-in/Standard shaders.
            // Reassign URP/Lit every build; changing only the color does not repair a pink shader.
            var shader=Shader.Find("Universal Render Pipeline/Lit");
            if(shader==null)
                throw new System.InvalidOperationException("URP/Lit shader is unavailable. Check that URP 17.6 is installed and assigned in Project Settings > Graphics.");
            if(m==null){m=new Material(shader); AssetDatabase.CreateAsset(m,path);}
            else if(m.shader != shader){m.shader=shader; Debug.Log("UFO WASH: repaired incompatible material shader: "+name);}
            if(m.HasProperty("_BaseColor")) m.SetColor("_BaseColor",c);
            else m.color=c;
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
            var g=new GameObject("SIGN // "+text);g.transform.position=p;g.transform.rotation=Quaternion.identity;var tm=g.AddComponent<TextMesh>();tm.text=text;tm.fontSize=64;tm.characterSize=size/64f;tm.anchor=TextAnchor.MiddleCenter;tm.alignment=TextAlignment.Center;tm.color=m.color;tm.fontStyle=FontStyle.Bold;
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

    // Import is never permission to replace an authored scene.
    public sealed class UfoWashSceneAutoBuilder : AssetPostprocessor
    {
        const string BuilderPath = "Assets/UFO_WASH/Editor/UfoWashPowerSliceBuilder.cs";
        const string BuilderScenePath = "Assets/UFO_WASH/Scenes/UFO_WASH_PowerSlice.unity";
        static bool rebuildQueued;

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (rebuildQueued || Application.isBatchMode) return;
            for (int i = 0; i < importedAssets.Length; i++)
            {
                if (importedAssets[i] != BuilderPath) continue;
                rebuildQueued = true;
                EditorApplication.delayCall += RebuildAfterImport;
                break;
            }
        }

        static void RebuildAfterImport()
        {
            rebuildQueued = false;
            if (EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorApplication.delayCall += RebuildAfterImport;
                return;
            }
            UfoWashPowerSliceBuilder.AutoRebuildGeneratedScene();
        }
    }
}
#endif











