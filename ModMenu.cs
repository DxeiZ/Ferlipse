using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ModMenuSpace
{
	// Token: 0x02001605 RID: 5637
	public class ModMenu : MonoBehaviour
	{
		// Token: 0x06009432 RID: 37938 RVA: 0x002C8B54 File Offset: 0x002C6D54
		public static void DrawMenu()
		{
			ModMenu.buttonStyle = new GUIStyle(GUI.skin.button);
			ModMenu.buttonStyle.fontSize = 14;
			ModMenu.buttonStyle.alignment = TextAnchor.MiddleCenter;
			ModMenu.buttonStyle.normal.textColor = Color.white;
			ModMenu.buttonStyle.normal.background = ModMenu.MakeTexture(2, 2, new Color(0f, 0.5f, 1f, 0.7f));
			ModMenu.buttonStyle.hover.background = ModMenu.MakeTexture(2, 2, new Color(0f, 0.6f, 1f, 0.9f));
			ModMenu.labelStyle = new GUIStyle();
			ModMenu.labelStyle.fontSize = 15;
			ModMenu.labelStyle.alignment = TextAnchor.MiddleCenter;
			ModMenu.labelStyle.normal.textColor = Color.white;
			ModMenu.labelStyle.normal.background = ModMenu.MakeTexture(2, 2, new Color(0f, 0.5f, 1f, 0.7f));
			ModMenu.labelStyle.hover.background = ModMenu.MakeTexture(2, 2, new Color(0f, 0.6f, 1f, 0.9f));
			ModMenu.buttonTexture = ModMenu.MakeTexture(2, 2, new Color(0f, 0.5f, 1f, 0.7f));
			GUI.Label(new Rect(10f, 10f, 510f, 30f), ModMenu.MenuLabel, ModMenu.labelStyle);
			if (GUI.Button(new Rect(10f, 50f, 250f, 30f), ModMenu.NoclipLabel, ModMenu.buttonStyle))
			{
				ModMenu.NoclipActive = !ModMenu.NoclipActive;
				Rigidbody component = T17NetView.Find<Player>(2974).gameObject.GetComponent<Rigidbody>();
				if (ModMenu.NoclipActive)
				{
					ModMenu.NoclipLabel = "Noclip AÇIK";
					component.detectCollisions = false;
				}
				else
				{
					ModMenu.NoclipLabel = "Noclip KAPALI";
					component.detectCollisions = true;
				}
			}
			if (GUI.Button(new Rect(10f, 90f, 250f, 30f), ModMenu.GodModeLabel, ModMenu.buttonStyle))
			{
				ModMenu.GodModeActive = !ModMenu.GodModeActive;
				CharacterStats component2 = T17NetView.Find<Player>(2974).gameObject.GetComponent<CharacterStats>();
				if (ModMenu.GodModeActive)
				{
					ModMenu.GodModeLabel = "GodMode AÇIK";
					component2.GetType().GetField("m_bInfinitePlayerEnergyOn", ModMenu.fieldBindingFlags).SetValue(component2, true);
					component2.GetType().GetField("m_bInfinitePlayerHealthOn", ModMenu.fieldBindingFlags).SetValue(component2, true);
				}
				else
				{
					ModMenu.GodModeLabel = "GodMode KAPALI";
					component2.GetType().GetField("m_bInfinitePlayerEnergyOn", ModMenu.fieldBindingFlags).SetValue(component2, false);
					component2.GetType().GetField("m_bInfinitePlayerHealthOn", ModMenu.fieldBindingFlags).SetValue(component2, false);
				}
			}
			if (GUI.Button(new Rect(10f, 130f, 250f, 30f), ModMenu.SpeedHackLabel, ModMenu.buttonStyle))
			{
				ModMenu.SpeedHackActive = !ModMenu.SpeedHackActive;
				CharacterMovement component3 = T17NetView.Find<Player>(2974).gameObject.GetComponent<CharacterMovement>();
				if (ModMenu.SpeedHackActive)
				{
					ModMenu.SpeedHackLabel = "SpeedHack AÇIK";
					component3.m_fMaxSpeed = 15f;
				}
				else
				{
					ModMenu.SpeedHackLabel = "SpeedHack KAPALI";
					component3.m_fMaxSpeed = 4.7f;
				}
			}
			if (GUI.Button(new Rect(10f, 170f, 250f, 30f), ModMenu.UnlockDoorsLabel, ModMenu.buttonStyle))
			{
				ModMenu.UnlockDoorsLabel = "Tüm Kapıları Açın";
				foreach (Door door in UnityEngine.Object.FindObjectsOfType<Door>())
				{
					T17NetView.Find<Player>(2974).AddAllowedDoor(door, null);
					door.SetForceOpen(true);
					DoorManager.GetInstance().SetUpCharacterKeys(T17NetView.Find<Player>(2974));
				}
			}
			if (GUI.Button(new Rect(10f, 210f, 250f, 30f), ModMenu.LockdownLabel, ModMenu.buttonStyle))
			{
				ModMenu.LockdownActive = !ModMenu.LockdownActive;
				PrisonAlertnessManager instance = PrisonAlertnessManager.GetInstance();
				if (instance != null)
				{
					if (ModMenu.LockdownActive)
					{
						try
						{
							SolitaryManager.GetInstance().TriggerLockdown(false);
							Debug.LogWarning("[Trainer] Kilitlemeyi Etkin olarak değiştirin: " + instance.GetCurrentAlertness().ToString());
						}
						catch (Exception ex)
						{
							Debug.LogError("[Trainer] Kilitleme HATASI: " + ex.Message);
						}
					}
					try
					{
						SolitaryManager.GetInstance().GetType().GetMethod("OnAlarm_LockdownEnd", ModMenu.defaultBindingFlags).Invoke(SolitaryManager.GetInstance(), null);
						SolitaryManager.GetInstance().GetType().GetField("m_bIsLockdownActive", ModMenu.fieldBindingFlags).SetValue(SolitaryManager.GetInstance(), false);
						SolitaryManager.GetInstance().GetType().GetField("m_LockdownTimer", ModMenu.fieldBindingFlags).SetValue(SolitaryManager.GetInstance(), null);
						instance.StopCoroutine("HUD.Routine.Lockdown");
						Debug.LogWarning("[Trainer] Kilitlemeyi Etkin Değil olarak değiştirin: " + instance.GetCurrentAlertness().ToString());
					}
					catch (Exception ex2)
					{
						Debug.LogError("[Trainer] Kilitleme HATASI: " + ex2.Message);
					}
				}
			}
			if (GUI.Button(new Rect(270f, 50f, 250f, 30f), ModMenu.DogLabel, ModMenu.buttonStyle))
			{
				DogEventManager dogEventManager = UnityEngine.Object.FindObjectOfType<DogEventManager>();
				if (dogEventManager != null)
				{
					foreach (AICharacter aicharacter in NPCManager.GetInstance().m_Doggies)
					{
						AICharacter_Dog aicharacter_Dog = (AICharacter_Dog)aicharacter;
						aicharacter_Dog.ForgetEvent(dogEventManager.GetAttackingAIEvent());
						aicharacter_Dog.ForgetEvent(dogEventManager.GetBoundAIEvent());
						aicharacter_Dog.ForgetEvent(dogEventManager.GetKnockedOutAIEvent());
						aicharacter_Dog.m_Character.SetIsAttacking(false);
						aicharacter_Dog.m_Character.PauseMovement(1000f, true);
						aicharacter_Dog.m_Character.m_CharacterOpinions.IncreaseOpinionOf(T17NetView.Find<Player>(2974), 1000);
					}
					dogEventManager.IsAttacking(false);
					dogEventManager.IsBound(false, T17NetView.Find<Player>(2974));
					Debug.Log("[Trainer] Köpekleri Çağırın");
				}
			}
			if (GUI.Button(new Rect(270f, 90f, 250f, 30f), ModMenu.HittableDogsLabel, ModMenu.buttonStyle))
			{
				ModMenu.HittableDogsActive = !ModMenu.HittableDogsActive;
				UnityEngine.Object.FindObjectOfType<DogEventManager>();
				if (ModMenu.HittableDogsActive)
				{
					ModMenu.HittableDogsLabel = "Vurulabilir Köpekler AÇIK";
					using (List<AICharacter>.Enumerator enumerator2 = NPCManager.GetInstance().m_Doggies.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							AICharacter aicharacter2 = enumerator2.Current;
							((AICharacter_Dog)aicharacter2).gameObject.GetComponent<AIPlayer>().m_CharacterRole = CharacterRole.Inmate;
						}
						return;
					}
				}
				ModMenu.HittableDogsLabel = "Vurulabilir Köpekler KAPALI";
				foreach (AICharacter aicharacter3 in NPCManager.GetInstance().m_Doggies)
				{
					((AICharacter_Dog)aicharacter3).gameObject.GetComponent<AIPlayer>().m_CharacterRole = CharacterRole.Dog;
				}
			}
			if (GUI.Button(new Rect(270f, 130f, 250f, 30f), ModMenu.ElectricFencesLabel, ModMenu.buttonStyle))
			{
				ModMenu.ElectricFencesActive = !ModMenu.ElectricFencesActive;
				PrisonPowerManager instance2 = PrisonPowerManager.GetInstance();
				if (instance2 != null)
				{
					List<PrisonPowerManager.GeneratorData> generators = instance2.m_Generators;
					for (int j = 0; j < generators.Count; j++)
					{
						List<ElectricFence> electricFences = generators.ElementAt(j).m_ElectricFences;
						for (int k = 0; k < electricFences.Count; k++)
						{
							ElectricFence electricFence = electricFences.ElementAt(k);
							electricFence.SetEnabled(ModMenu.ElectricFencesActive);
							Debug.LogWarning("[Trainer] Elektrikli Çit Durum Değişikliği: " + electricFence.gameObject.name);
						}
					}
				}
			}
			if (GUI.Button(new Rect(270f, 170f, 250f, 30f), "Tüm Kapıların Kilidini Açın", ModMenu.buttonStyle))
			{
				foreach (Door door2 in UnityEngine.Object.FindObjectsOfType<Door>())
				{
					door2.gameObject.GetComponent<BoxCollider>().enabled = false;
					T17NetView.Find<Player>(2974).AddAllowedDoor(door2, null);
				}
			}
			if (GUI.Button(new Rect(270f, 210f, 250f, 30f), "Tüm NPC'ler Vurulabilir", ModMenu.buttonStyle))
			{
				AIPlayer[] array2 = UnityEngine.Object.FindObjectsOfType<AIPlayer>();
				for (int l = 0; l < array2.Length; l++)
				{
					array2[l].m_CharacterRole = CharacterRole.Inmate;
				}
			}
			if (GUI.Button(new Rect(10f, 250f, 510f, 30f), ModMenu.MaxPlayerStatsLabel, ModMenu.buttonStyle))
			{
				ConfigManager instance3 = ConfigManager.GetInstance();
				CharacterStats component4 = T17NetView.Find<Player>(2974).gameObject.GetComponent<CharacterStats>();
				component4.SetHealth(1000f);
				component4.SetEnergy(1000f);
				component4.SetHeat(0f, CharacterStats.MessageGameplayReasons.Unassigned);
				component4.IncreaseMoney(1000000f);
				component4.IncreaseIntellectRPC(1000f);
				component4.IncreaseStrengthRPC(1000f);
				component4.IncreaseCardioRPC(1000f);
				component4.IncreaseEnergyRPC(1000f);
				component4.IncreaseHealthRPC(1000f);
				if (instance3 != null)
				{
					CharacterConfig playerConfig = instance3.playerConfig;
					playerConfig.m_HealthBaseLine = 1000f;
					playerConfig.m_StrengthBaseLine = 1000f;
					playerConfig.m_CardioBaseLine = 1000f;
					playerConfig.m_IntellectBaseLine = 1000f;
					playerConfig.m_EnergyBaseLine = 1000f;
					playerConfig.m_MoneyBaseLine = 99999f;
					playerConfig.m_HealthRestoreRate = 100f;
					playerConfig.m_EnergyRestoreRate = 100f;
					playerConfig.m_EnergyRestoreRateBlocking = 100f;
					playerConfig.m_StrengthDecayRate = 0f;
					playerConfig.m_IntellectDecayRate = 0f;
					playerConfig.m_CardioDecayRate = 0f;
					playerConfig.m_HeatDecayRate = 1000f;
				}
			}
		}

		// Token: 0x06009434 RID: 37940 RVA: 0x002C959C File Offset: 0x002C779C
		static ModMenu()
		{
			ModMenu.MenuLabel = "Ferlipse";
			ModMenu.MenuVisible = false;
			ModMenu.MaxPlayerStatsLabel = "Maksimum Oyuncu İstatistikleri";
			ModMenu.NoclipActive = false;
			ModMenu.GodModeActive = false;
			ModMenu.NoclipLabel = "Noclip OFF";
			ModMenu.SpeedHackActive = false;
			ModMenu.SpeedHackLabel = "SpeedHack KAPALI";
			ModMenu.UnlockDoorsLabel = "Tüm Kapıları Aç";
			ModMenu.LockdownLabel = "Lockdown OFF";
			ModMenu.GodModeLabel = "GodMode KAPALI";
			ModMenu.LockdownActive = false;
			ModMenu.DogLabel = "Köpekleri Devre Dışı Bırak";
			ModMenu.GiveLabel = "Duvarı Yok Etme Aracı Ver";
			ModMenu.GiveStaffKeyLabel = "Personel Anahtarı Ver";
			ModMenu.HittableDogsLabel = "Vurulabilir Köpekler KAPALI";
			ModMenu.ElectricFencesActive = false;
			ModMenu.ElectricFencesLabel = "Elektrikli Çitler";
		}

		// Token: 0x06009435 RID: 37941 RVA: 0x002C9660 File Offset: 0x002C7860
		private static Texture2D MakeTexture(int width, int height, Color color)
		{
			Color[] array = new Color[width * height];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = color;
			}
			Texture2D texture2D = new Texture2D(width, height);
			texture2D.SetPixels(array);
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x040070FE RID: 28926
		public static string MenuLabel;

		// Token: 0x040070FF RID: 28927
		public static bool MenuVisible;

		// Token: 0x04007100 RID: 28928
		public static string MaxPlayerStatsLabel;

		// Token: 0x04007101 RID: 28929
		public static bool NoclipActive;

		// Token: 0x04007102 RID: 28930
		public static string NoclipLabel;

		// Token: 0x04007103 RID: 28931
		public static bool SpeedHackActive;

		// Token: 0x04007104 RID: 28932
		public static string SpeedHackLabel;

		// Token: 0x04007105 RID: 28933
		public static string UnlockDoorsLabel;

		// Token: 0x04007106 RID: 28934
		public static string LockdownLabel;

		// Token: 0x04007107 RID: 28935
		public static bool LockdownActive;

		// Token: 0x04007108 RID: 28936
		public static BindingFlags defaultBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		// Token: 0x04007109 RID: 28937
		public static BindingFlags fieldBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField;

		// Token: 0x0400710A RID: 28938
		public static BindingFlags propertyBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty;

		// Token: 0x0400710B RID: 28939
		public static string DogLabel;

		// Token: 0x0400710C RID: 28940
		public static string HittableDogsLabel;

		// Token: 0x0400710D RID: 28941
		public static bool HittableDogsActive;

		// Token: 0x0400710E RID: 28942
		public static string GiveLabel;

		// Token: 0x0400710F RID: 28943
		public static bool GaveItem;

		// Token: 0x04007110 RID: 28944
		public static string GiveStaffKeyLabel;

		// Token: 0x04007111 RID: 28945
		public static string ElectricFencesLabel;

		// Token: 0x04007112 RID: 28946
		public static bool ElectricFencesActive;

		// Token: 0x04007113 RID: 28947
		public static int Test;

		// Token: 0x04007114 RID: 28948
		public static bool GodModeActive;

		// Token: 0x04007115 RID: 28949
		public static string GodModeLabel;

		// Token: 0x04007116 RID: 28950
		private static GUIStyle titleStyle;

		// Token: 0x04007117 RID: 28951
		private static GUIStyle buttonStyle;

		// Token: 0x04007118 RID: 28952
		private static GUIStyle labelStyle;

		// Token: 0x04007119 RID: 28953
		private static Texture2D buttonTexture;

		// Token: 0x0400711A RID: 28954
		private static bool menuVisible;
	}
}
