using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation.Samples
{
    /// <summary>
    /// Moves the XROrigin in such a way that it makes the given content appear to be
    /// at a given location acquired via a raycast.
    /// </summary>
    [RequireComponent(typeof(XROrigin))]
    [RequireComponent(typeof(ARRaycastManager))]
    public class MakeAppearOnPlane : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("A transform which should be made to appear to be at the touch point.")]
        Transform m_Content;

        /// <summary>
        /// A transform which should be made to appear to be at the touch point.
        /// </summary>
        public Transform content
        {
            get { return m_Content; }
            set { m_Content = value; }
        }

        [SerializeField]
        [Tooltip("The rotation the content should appear to have.")]
        Quaternion m_Rotation;

        /// <summary>
        /// The rotation the content should appear to have.
        /// </summary>
        public Quaternion rotation
        {
            get { return m_Rotation; }
            set
            {
                m_Rotation = value;
                if (m_XROrigin != null)
                    MakeContentAppearAt(content, content.transform.position, m_Rotation);
            }
        }

        void Awake()
        {
            m_XROrigin = GetComponent<XROrigin>();
            m_RaycastManager = GetComponent<ARRaycastManager>();
        }

        void Update()
        {
            if (Input.touchCount == 0 || m_Content == null)
                return;

            var touch = Input.GetTouch(0);

            if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one
                // will be the closest hit.
                var hitPose = s_Hits[0].pose;

                // This does not move the content; instead, it moves and orients the XROrigin
                // such that the content appears to be at the raycast hit position.
                MakeContentAppearAt(content, hitPose.position, m_Rotation);
            }
        }

        // Ported from ARSessionOrigin.MakeContentAppearAt, which was removed in AR Foundation 5.
        void MakeContentAppearAt(Transform content, Vector3 position, Quaternion rotation)
        {
            if (content == null)
                return;

            var originTransform = m_XROrigin.transform;

            // Adjust the "point of interest" transform to account
            // for the actual position we want the content to appear at.
            contentOffsetTransform.position += originTransform.position - position;

            // The XROrigin's position needs to match the content's pivot. This is so
            // the entire origin rotates around the content (so the impression is that
            // the content is rotating, not the rig).
            originTransform.position = content.position;

            // Since we aren't rotating the content, we need to perform the inverse
            // operation on the XROrigin. For example, if we want the
            // content to appear to be rotated 90 degrees on the Y axis, we should
            // rotate our rig -90 degrees on the Y axis.
            originTransform.rotation = Quaternion.Inverse(rotation) * content.rotation;
        }

        Transform contentOffsetTransform
        {
            get
            {
                if (m_ContentOffsetGameObject == null)
                {
                    // Insert a GameObject directly below the rig
                    m_ContentOffsetGameObject = new GameObject("Content Placement Offset");
                    m_ContentOffsetGameObject.transform.SetParent(transform, false);

                    // Re-parent any children of the XROrigin
                    for (var i = 0; i < transform.childCount; ++i)
                    {
                        var child = transform.GetChild(i);
                        if (child != m_ContentOffsetGameObject.transform)
                        {
                            child.SetParent(m_ContentOffsetGameObject.transform, true);
                            --i; // Decrement because childCount is also one less.
                        }
                    }
                }

                return m_ContentOffsetGameObject.transform;
            }
        }

        GameObject m_ContentOffsetGameObject;

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        XROrigin m_XROrigin;

        ARRaycastManager m_RaycastManager;
    }
}
