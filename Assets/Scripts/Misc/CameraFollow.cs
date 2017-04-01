using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour {

    public static CameraFollow main;
    
    public enum CameraMode { Stopped, Follow, Follow2D}
    
    public CameraMode currentMode;
    public bool followRotation;
    [Space(10)]
    public float smoothPosition = 3.5f;
    public float smoothRotation = 5.5f;
    [SerializeField] private Vector3 followOffset;

    private Transform currentTarget;
	private Quaternion targetRotation;


    void Awake() {
        if (main == null) { 
            main = this;
        }
    }

    void Update() {

        if (currentMode == CameraMode.Follow) {
            Vector3 positionOffset = currentTarget.forward * followOffset.z;
            positionOffset += currentTarget.right * followOffset.x;
            positionOffset += currentTarget.up * followOffset.y;
            transform.position = Vector3.Lerp(transform.position,
                                               currentTarget.position + positionOffset,
                                               smoothPosition * Time.deltaTime);
            if (followRotation) {
                targetRotation = new Quaternion(currentTarget.rotation.x, currentTarget.rotation.y,
                                                currentTarget.rotation.z, currentTarget.rotation.w);

                targetRotation *= Quaternion.Euler(45, 0, 0);

                transform.rotation = Quaternion.Lerp( transform.rotation,
                                                      targetRotation,
                                                      smoothRotation * Time.deltaTime);
            }
        } else {
            transform.position = Vector3.Lerp(transform.position,
                                   currentTarget.position + Vector3.up * 10,
                                   smoothPosition * Time.deltaTime);
            if (followRotation) {
                targetRotation = new Quaternion(currentTarget.rotation.x, currentTarget.rotation.y,
                                currentTarget.rotation.z, currentTarget.rotation.w);

                targetRotation *= Quaternion.Euler(90, 0, 0);

                transform.rotation = Quaternion.Lerp(transform.rotation,
                                                      targetRotation,
                                                      smoothRotation * Time.deltaTime);
            }
        }


    }

    public void SetFollow(Transform target) {
        enabled = true;
        currentTarget = target;
        currentMode = CameraMode.Follow;
        transform.position = currentTarget.position + followOffset;
        transform.localEulerAngles = Vector3.right * 50;
    }
    public void SetFollow(Transform target, bool _2D) {
        enabled = true;
        currentTarget = target;
        if (_2D) {
            currentMode = CameraMode.Follow2D;
            transform.position = currentTarget.position + Vector3.up * 5;
            transform.localEulerAngles = Vector3.right*90;
        } else {
            SetFollow(target);
        }
    }

    public void StopCamera() {
        currentMode = CameraMode.Stopped;
        currentTarget = null;
        enabled = false;
    }
    

}
