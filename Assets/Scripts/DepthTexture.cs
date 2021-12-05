using UnityEngine;

namespace Assets.Scripts
{
    [ExecuteInEditMode]
    public class DepthTexture : MonoBehaviour
    {
        Camera camera;

        private void Start()
        {
            camera = GetComponent<Camera>();
            camera.depthTextureMode = DepthTextureMode.Depth;
        }
    }
}
