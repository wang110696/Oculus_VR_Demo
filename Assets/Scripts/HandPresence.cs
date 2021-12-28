using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public bool isShowController = false;
    public InputDeviceCharacteristics controllerCharacteristics;
    public List<GameObject> controllerPrefabs;
    private InputDevice targetDevice;
    private GameObject spawnedController;
    public GameObject handModelPrefab;
    private GameObject spawnedHandModel;

    public Animator handAnimator;

    void Start()
    {
        TryInitialize();
    }

    void TryInitialize()
    {
        // 将设备放入这里
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        Debug.Log(devices.Count + "name:" + devices[0].name);
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            Debug.Log(targetDevice.name);
            //注意Controller的name和device的name需要匹配
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            //如果匹配到了那么久实例化一个prefab
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);
            }
            else
            {
                Debug.LogError("don`t find the correct controller model");
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }

            spawnedHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
    }

    void UpdateHandAnimation()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) &&
        //     primaryButtonValue)
        // {
        //     Debug.Log("pressing Primary buton");
        // }
        //
        // if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerButtonValue) &&
        //     triggerButtonValue > 0.1f)
        // {
        //     Debug.Log("trigger!");
        // }
        //
        // if (targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue) &&
        //     primary2DAxisValue != Vector2.zero)
        // {
        //     Debug.Log("2D Axis moving" + primary2DAxisValue);
        // }

        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        else
        {
            if (isShowController)
            {
                spawnedHandModel.SetActive(false);
                spawnedController.SetActive(true);
            }
            else
            {
                spawnedHandModel.SetActive(true);
                spawnedController.SetActive(false);
                UpdateHandAnimation();
            }
        }
    }
}