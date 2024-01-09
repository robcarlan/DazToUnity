namespace Daz3D
{
    public class CleanedUp
    {
        public static void DForceComment()
        {
            //DForceMaterial dforceMat = _dforceMap.Map[key];
            //GameObject parent = renderer.gameObject;
            //SkinnedMeshRenderer skinned = parent.GetComponent<SkinnedMeshRenderer>();
            //Cloth cloth;
            //// add Unity Cloth Physics component to gameobject parent of the renderer
            //if (parent.GetComponent<Cloth>() == null)
            //{
            //    cloth = parent.AddComponent<Cloth>();
            //    // assign values from dtuMat
            //    cloth.stretchingStiffness = dforceMat.dtuMaterial.Get("Stretch Stiffness").Float;
            //    cloth.bendingStiffness = dforceMat.dtuMaterial.Get("Bend Stiffness").Float;
            //    cloth.damping = dforceMat.dtuMaterial.Get("Damping").Float;
            //    cloth.friction = dforceMat.dtuMaterial.Get("Friction").Float;

            //    // fix SkinnedMeshRenderer boundaries bug
            //    skinned.updateWhenOffscreen = true;

            //    // Add G8F cloth collision rig
            //    var searchResult = workingInstance.transform.Find("Cloth Collision Rig");
            //    GameObject collision_instance = (searchResult != null) ? searchResult.gameObject : null;
            //    if (collision_instance == null)
            //    {
            //        GameObject collision_prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Daz3D/Resources/G8F Collision Rig.prefab");
            //        collision_instance = Instantiate<GameObject>(collision_prefab);
            //        collision_instance.name = "Cloth Collision Rig";
            //        collision_instance.transform.parent = workingInstance.transform;
            //        // merge cloth collision rig to figure root bone
            //        collision_instance.GetComponent<ClothCollisionAssigner>().mergeRig(skinned.rootBone);
            //    }
            //    ClothCollisionAssigner.ClothConfig clothConfig = new ClothCollisionAssigner.ClothConfig();
            //    clothConfig.m_ClothToManage = cloth;
            //    clothConfig.m_UpperBody = true;
            //    clothConfig.m_LowerBody = true;
            //    collision_instance.GetComponent<ClothCollisionAssigner>().addClothConfig(clothConfig);

            //}
            //else
            //{
            //    cloth = parent.GetComponent<Cloth>();
            //}

            //// add clothtools to gameobject parent of renderer
            //ClothTools clothTools;
            //if (parent.GetComponent<ClothTools>() == null)
            //{
            //    clothTools = parent.AddComponent<ClothTools>();
            //    clothTools.GenerateLookupTables();
            //}
            //else
            //{
            //    clothTools = parent.GetComponent<ClothTools>();
            //}

            //int matIndex = Array.IndexOf(skinned.sharedMaterials, keyMat);
            //// get vertex list for this material's submesh
            //if (matIndex >= 0)
            //{
            //    float simulation_strength;
            //    //// map the materical's submesh's vertices to the correct "Dynamics Strength"
            //    simulation_strength = dforceMat.dtuMaterial.Get("Dynamics Strength").Float;
            //    Debug.Log("DEBUG INFO: simulation strength: " + simulation_strength);
            //    //// DEBUG line to map simulation strength to material index
            //    //simulation_strength = matIndex;

            //    //// Tiered scaling function
            //    float adjusted_simulation_strength;
            //    //float strength_max = 1.0f;
            //    //float strength_min = 0.0f;
            //    float strength_scale_threshold = 0.5f;
            //    if (simulation_strength <= strength_scale_threshold)
            //    {
            //        //// stronger compression of values below threshold
            //        float scale = 0.075f;
            //        float offset = 0.2f;
            //        adjusted_simulation_strength = (simulation_strength - offset) * scale;
            //    }
            //    else
            //    {
            //        float offset = (strength_scale_threshold - 0.2f) * 0.075f; // offset = (threshold - previous tier's offset) * previous teir's scale
            //        float scale = 0.2f;
            //        adjusted_simulation_strength = (simulation_strength - offset) / (1 - offset); // apply offset, then normalize to 1.0
            //        adjusted_simulation_strength *= scale;
            //    }
            //    //// clamp to 0.0f to 0.2f
            //    float coeff_min = 0.0f;
            //    float coeff_max = 0.2f;
            //    adjusted_simulation_strength = (adjusted_simulation_strength > coeff_min) ? adjusted_simulation_strength : coeff_min;
            //    adjusted_simulation_strength = (adjusted_simulation_strength < coeff_max) ? adjusted_simulation_strength : coeff_max;
            //    //// Debug line for no scaling
            //    //adjusted_simulation_strength = simulation_strength;
            //    clothTools.SetSubMeshWeights(matIndex, adjusted_simulation_strength);
            //}
        }

        public static void RendererMaterialRemapComment()
        {
            /****
             ** Everything below is old and broken.
             ** Fbx exported from DazBridges no longer embed textures.
             ****
            var shader = Shader.Find("HDRP/Lit");

            if (shader == null)
            {
                Debug.LogWarning("couldn't find HDRP/Lit shader");
                continue;
            }

            nuMat = new Material(shader);
            nuMat.CopyPropertiesFromMaterial(keyMat);

            // just copy the textures, colors and scalars that are appropriate given the base material type
            //DazMaterialPropertiesInfo info = new DazMaterialPropertiesInfo();
            //CustomizeMaterial(ref nuMat, info);

            var matPath = Path.GetDirectoryName(modelPath);
            matPath = Path.Combine(matPath, fbxPrefab.name + "Daz3D_Materials");
            matPath = AssetDatabase.GenerateUniqueAssetPath(matPath);

            if (!Directory.Exists(matPath))
                Directory.CreateDirectory(matPath);

            //Debug.Log("obj path " + path);
            AssetDatabase.CreateAsset(nuMat, matPath + "/Daz3D_" + keyMat.name + ".mat");
            */
        }
    }
}