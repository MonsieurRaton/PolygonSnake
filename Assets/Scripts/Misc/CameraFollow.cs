using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour
{
    public bool snap;

    [Space(10)]
    public float smoothPosition = 3.5f;
    public float smoothRotation = 5.5f;
    public bool follow = false;
    public bool withOffsets = false;
    public bool locked = false;
    public bool followRotation = false;

    [Space(10)]
    public bool _2D;
    [Range(1, 64)]
    public int pixelToUnits = 1;

    public List<Transform> views;
    public List<Vector3> viewsOffsets;
    public int viewSelected = 0;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

	private Quaternion targetRotation;
	private float originalAngle;
	void Awake()
	{
        //views = new List<Transform>();
	
		targetRotation = new Quaternion ();

        originalPosition = transform.position;
        originalRotation = transform.rotation;
		originalAngle = originalRotation.eulerAngles.x;

        viewsOffsets.Clear();
        for (int i = 0; i < views.Count; i++) viewsOffsets.Add(transform.position - views[i].position);
    }

    void Update()
    {
        if (snap) return;
        if (!follow) return;

        if (_2D)
        {
            Follow2D();
            return;
        }

        if (!locked && viewSelected >= 0 && viewSelected < views.Count && views[viewSelected] != null)
        {
			Vector3 positionOffset = views[viewSelected].forward * viewsOffsets [viewSelected].z;
			positionOffset += views[viewSelected].right * viewsOffsets [viewSelected].x;
			positionOffset += views[viewSelected].up * viewsOffsets [viewSelected].y;

			transform.position = Vector3.Lerp(
				transform.position,
				views[viewSelected].position + (withOffsets ? positionOffset : Vector3.zero),
				smoothPosition * Time.deltaTime);

			/*
            transform.position = Vector3.Lerp(
                transform.position,
                views[viewSelected].position + (withOffsets ? viewsOffsets[viewSelected] : Vector3.zero),
                smoothPosition * Time.deltaTime);
                */

			if (followRotation) {
				targetRotation.Set (views [viewSelected].rotation.x, views [viewSelected].rotation.y, views [viewSelected].rotation.z, views [viewSelected].rotation.w);

				targetRotation *= Quaternion.Euler (originalAngle, 0, 0);

				transform.rotation = Quaternion.Lerp (
					transform.rotation,
					targetRotation,
					smoothRotation * Time.deltaTime);
				
				/*
				transform.rotation = Quaternion.Lerp (
					transform.rotation,
					views [viewSelected].rotation,
					smoothRotation * Time.deltaTime);
					*/
			}
        }
    }

    void FixedUpdate()
    {
        if (!snap) return;
        if (!locked && viewSelected >= 0 && viewSelected < views.Count && views[viewSelected] != null)
            transform.position = views[viewSelected].position + (withOffsets ? viewsOffsets[viewSelected] : Vector3.zero);
    }

    public void AddAndFollow(Transform target)
    {
        ClearViewsAndResetCamera();

        transform.position = target.position + originalPosition;

        views.Add(target);
        viewsOffsets.Add(transform.position - target.position);

        follow = true;
        withOffsets = true;
    }

    // --------------------------------------------------------

    public void SetCurrentAsOriginal()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public void ClearViewsAndResetCamera()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        views.Clear();
        viewsOffsets.Clear();
        viewSelected = 0;
    }

    public void AddView(Transform t)
    {
        views.Add(t);
    }

    public void ChangeView(int index)
    {
        if (views == null || views.Count == 0)
            return;

        if (index < 0) index = 0;
        if (index >= views.Count - 1) index = views.Count - 1;

        viewSelected = index;
    }

    public float distance;
    public float factor;
    public void Follow2D()
    {
        Vector3 position = transform.position;
        distance = Mathf.Abs(position.x - views[viewSelected].position.x);
        factor = Mathf.Clamp(distance * 1.5f, 1, 5);

        position = Follow2DHelper(position, Mathf.Floor(factor));

        transform.position = position;
    }

    private Vector3 Follow2DHelper(Vector3 position, float factor)
    {
        if (position.x < views[viewSelected].position.x) position.x += PixelToUnitsInc() * factor;
        if (position.x > views[viewSelected].position.x) position.x -= PixelToUnitsInc() * factor;
        return position;
    }

    public float PixelToUnitsInc() { return 1f / (pixelToUnits * 1f); }
}
