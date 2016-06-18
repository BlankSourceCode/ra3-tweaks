using UnityEngine;
using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using LitJson;

namespace RA3Tweaks.Tweaks
{
	public class Plugin : MonoBehaviour
	{
        public static void Initialize()
        {
            Debug.Log("RA3Injection::Plugin Initialized\n");
          
            ComponentInfoList.CreateFromFileOrResource(Application.dataPath + "/Components.json", "Databases/Components");

            GameObject go = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("ComponentPrefabs/CompSawBlade60"));
            TweakHelpers.LogTransform(go.transform, "");

            string jsonString = System.IO.File.ReadAllText(@"D:\components.json");
		    System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
		    JsonData jsonData = JsonMapper.ToObject(jsonString);
            Debug.Log(jsonString);

            ComponentInfo[] items = new ComponentInfo[jsonData.Count];
            for (int i = 0; i < jsonData.Count; i++)
		    {
                int mSubCategory = -1;
                int mCategory = -1;
                items[i] = new ComponentInfo();
                jsonData[i].Read("active", ref items[i].mActive);
                jsonData[i].Read("displaymass", ref items[i].mDisplayMass);
                jsonData[i].Read("name", ref items[i].mName);
                jsonData[i].Read("description", ref items[i].mDescription);
                jsonData[i].Read("thumbnail", ref items[i].mThumbnailName);
                jsonData[i].Read("componentprefabname", ref items[i].mPrefabName);
                jsonData[i].Read("attchassis", ref items[i].mAttChassis);
                jsonData[i].Read("mirrorx", ref items[i].mMirrorX);
                jsonData[i].Read("mirrory", ref items[i].mMirrorY);
                jsonData[i].Read("mirrorz", ref items[i].mMirrorZ);
                jsonData[i].Read("mass", ref items[i].mMass);
                jsonData[i].Read("elecsupply", ref items[i].mElecSupply);
                jsonData[i].Read("elecdraw", ref items[i].mElecDraw);
                jsonData[i].Read("airsupply", ref items[i].mAirSupply);
                jsonData[i].Read("airdraw", ref items[i].mAirDraw);
                jsonData[i].Read("dpiblunt", ref items[i].mDpiBlunt);
                jsonData[i].Read("dpipierce", ref items[i].mDpiPierce);
                jsonData[i].Read("dpiflame", ref items[i].mDpiFlame);
                jsonData[i].Read("dpielec", ref items[i].mDpiElec);
                jsonData[i].Read("dpsblunt", ref items[i].mDpsBlunt);
                jsonData[i].Read("dpspierce", ref items[i].mDpsPierce);
                jsonData[i].Read("dpsflame", ref items[i].mDpsFlame);
                jsonData[i].Read("dpselec", ref items[i].mDpsElec);
                jsonData[i].Read("hpfactblunt", ref items[i].mHpFactBlunt);
                jsonData[i].Read("hpfactpierce", ref items[i].mHpFactPierce);
                jsonData[i].Read("hpfactflame", ref items[i].mHpFactFlame);
                jsonData[i].Read("hpfactelec", ref items[i].mHpFactElec);
                jsonData[i].Read("hitpoints", ref items[i].mHitPoints);
                jsonData[i].Read("uispecial", ref items[i].mUISpecial);
                jsonData[i].Read("attachsound", ref items[i].mAttachSound);
                jsonData[i].Read("spinupsound", ref items[i].mSpinUpSound);
                jsonData[i].Read("spinupvol", ref items[i].mSpinUpVol);
                jsonData[i].Read("spindownsound", ref items[i].mSpinDownSound);
                jsonData[i].Read("spindownvol", ref items[i].mSpinDownVol);
                jsonData[i].Read("loopsound", ref items[i].mLoopSound);
                jsonData[i].Read("looppitchmin", ref items[i].mLoopPitchMin);
                jsonData[i].Read("looppitchmax", ref items[i].mLoopPitchMax);
                jsonData[i].Read("loopvolmin", ref items[i].mLoopVolMin);
                jsonData[i].Read("loopvolmax", ref items[i].mLoopVolMax);
                jsonData[i].Read("active", ref items[i].mActive);
                jsonData[i].Read("category", ref mCategory);
                jsonData[i].Read("subcatagory", ref mSubCategory);
                jsonData[i].Read("displaymass", ref items[i].mDisplayMass);
                jsonData[i].Read("name", ref items[i].mName);
                jsonData[i].Read("description", ref items[i].mDescription);
                jsonData[i].Read("thumbnail", ref items[i].mThumbnailName);
                jsonData[i].Read("componentprefabname", ref items[i].mPrefabName);
                jsonData[i].Read("attchassis", ref items[i].mAttChassis);
                jsonData[i].Read("mirrorx", ref items[i].mMirrorX);
                jsonData[i].Read("mirrory", ref items[i].mMirrorY);
                jsonData[i].Read("mirrorz", ref items[i].mMirrorZ);
                jsonData[i].Read("mass", ref items[i].mMass);
                jsonData[i].Read("elecsupply", ref items[i].mElecSupply);
                jsonData[i].Read("elecdraw", ref items[i].mElecDraw);
                jsonData[i].Read("airsupply", ref items[i].mAirSupply);
                jsonData[i].Read("airdraw", ref items[i].mAirDraw);
                jsonData[i].Read("dpiblunt", ref items[i].mDpiBlunt);
                jsonData[i].Read("dpipierce", ref items[i].mDpiPierce);
                jsonData[i].Read("dpiflame", ref items[i].mDpiFlame);
                jsonData[i].Read("dpielec", ref items[i].mDpiElec);
                jsonData[i].Read("dpsblunt", ref items[i].mDpsBlunt);
                jsonData[i].Read("dpspierce", ref items[i].mDpsPierce);
                jsonData[i].Read("dpsflame", ref items[i].mDpsFlame);
                jsonData[i].Read("dpselec", ref items[i].mDpsElec);
                jsonData[i].Read("hpfactblunt", ref items[i].mHpFactBlunt);
                jsonData[i].Read("hpfactpierce", ref items[i].mHpFactPierce);
                jsonData[i].Read("hpfactflame", ref items[i].mHpFactFlame);
                jsonData[i].Read("hpfactelec", ref items[i].mHpFactElec);
                jsonData[i].Read("hitpoints", ref items[i].mHitPoints);
                jsonData[i].Read("uispecial", ref items[i].mUISpecial);
                jsonData[i].Read("attachsound", ref items[i].mAttachSound);
                jsonData[i].Read("spinupsound", ref items[i].mSpinUpSound);
                jsonData[i].Read("spinupvol", ref items[i].mSpinUpVol);
                jsonData[i].Read("spindownsound", ref items[i].mSpinDownSound);
                jsonData[i].Read("spindownvol", ref items[i].mSpinDownVol);
                jsonData[i].Read("loopsound", ref items[i].mLoopSound);
                jsonData[i].Read("looppitchmin", ref items[i].mLoopPitchMin);
                jsonData[i].Read("looppitchmax", ref items[i].mLoopPitchMax);
                jsonData[i].Read("loopvolmin", ref items[i].mLoopVolMin);
                jsonData[i].Read("loopvolmax", ref items[i].mLoopVolMax);
                items[i].mCategory = (ComponentInfo.CATEGORY)mCategory;
                items[i].mSubCategory = (ComponentInfo.CATEGORY)mSubCategory;
            }

            Type type = typeof(ComponentInfoList);
            FieldInfo info = type.GetField("_instance", BindingFlags.NonPublic | BindingFlags.Static);
            ComponentInfoList infoList = info.GetValue(null) as ComponentInfoList;
            Debug.Log(infoList);

            ComponentInfo[] oldItems = infoList.mItems;
            infoList.mItems = new ComponentInfo[oldItems.Length + items.Length];
            for (int i = 0; i < infoList.mItems.Length; i++) 
            {
                if (i < oldItems.Length)
                {
                    infoList.mItems[i] = oldItems[i];
                }
                else
                {
                    infoList.mItems[i] = items[i - oldItems.Length];
                }
            }
        }

        void Start()
        {
        }

        void Update()
        {
        }
    }
}