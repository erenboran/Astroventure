using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class ThirdPersonShooterController : MonoBehaviour {

    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;

    private PlayerController playerController;
    private PlayerInputHandler playerInputHandler;
    private Animator animator;

    private void Awake() {
        playerController = GetComponent<PlayerController>();
        playerInputHandler = GetComponent<PlayerInputHandler>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        Transform hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask)) {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
        }

        if (playerInputHandler.aim) {
            aimVirtualCamera.gameObject.SetActive(true);
            playerController.SetSensitivity(aimSensitivity);
            playerController.SetRotateOnMove(false);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 13f));

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        } else {
            aimVirtualCamera.gameObject.SetActive(false);
            playerController.SetSensitivity(normalSensitivity);
            playerController.SetRotateOnMove(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 13f));
        }


        if (playerInputHandler.shoot) {
            /*
            // Hit Scan Shoot
            if (hitTransform != null) {
                // Hit something
                if (hitTransform.GetComponent<BulletTarget>() != null) {
                    // Hit target
                    Instantiate(vfxHitGreen, mouseWorldPosition, Quaternion.identity);
                } else {
                    // Hit something else
                    Instantiate(vfxHitRed, mouseWorldPosition, Quaternion.identity);
                }
            }
            //*/
            //*
            // Projectile Shoot
            Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
            Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            //*/
            playerInputHandler.shoot = false;
        }

    }

}