﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000004 RID: 4
[Serializable]
public class Attack
{
	// Token: 0x06000020 RID: 32 RVA: 0x00003547 File Offset: 0x00001747
	public bool StartDraw(Humanoid character, ItemDrop.ItemData weapon)
	{
		if (!Attack.HaveAmmo(character, weapon))
		{
			return false;
		}
		Attack.EquipAmmoItem(character, weapon);
		return true;
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00003560 File Offset: 0x00001760
	public bool Start(Humanoid character, Rigidbody body, ZSyncAnimation zanim, CharacterAnimEvent animEvent, VisEquipment visEquipment, ItemDrop.ItemData weapon, Attack previousAttack, float timeSinceLastAttack, float attackDrawPercentage)
	{
		if (this.m_attackAnimation == "")
		{
			return false;
		}
		this.m_character = character;
		this.m_baseAI = this.m_character.GetComponent<BaseAI>();
		this.m_body = body;
		this.m_zanim = zanim;
		this.m_animEvent = animEvent;
		this.m_visEquipment = visEquipment;
		this.m_weapon = weapon;
		this.m_attackDrawPercentage = attackDrawPercentage;
		if (Attack.m_attackMask == 0)
		{
			Attack.m_attackMask = LayerMask.GetMask(new string[]
			{
				"Default",
				"static_solid",
				"Default_small",
				"piece",
				"piece_nonsolid",
				"character",
				"character_net",
				"character_ghost",
				"hitbox",
				"character_noenv",
				"vehicle"
			});
			Attack.m_attackMaskTerrain = LayerMask.GetMask(new string[]
			{
				"Default",
				"static_solid",
				"Default_small",
				"piece",
				"piece_nonsolid",
				"terrain",
				"character",
				"character_net",
				"character_ghost",
				"hitbox",
				"character_noenv",
				"vehicle"
			});
			Attack.m_harvestRayMask = LayerMask.GetMask(new string[]
			{
				"piece",
				"piece_nonsolid",
				"item"
			});
		}
		if (this.m_requiresReload && (!this.m_character.IsWeaponLoaded() || this.m_character.InMinorAction()))
		{
			return false;
		}
		if (this.m_cantUseInDungeon && this.m_character.InInterior())
		{
			Player player = this.m_character as Player;
			if (player != null)
			{
				player.Message(MessageHud.MessageType.Center, "$msg_blocked", 0, null);
				return false;
			}
		}
		float attackStamina = this.GetAttackStamina();
		if (attackStamina > 0f && !character.HaveStamina(attackStamina + 0.1f))
		{
			if (character.IsPlayer())
			{
				Hud.instance.StaminaBarEmptyFlash();
			}
			return false;
		}
		if (!character.TryUseEitr(this.GetAttackEitr()))
		{
			return false;
		}
		float attackHealth = this.GetAttackHealth();
		if (attackHealth > 0f && !character.HaveHealth(attackHealth + 0.1f) && this.m_attackHealthLowBlockUse && character.IsPlayer())
		{
			Hud.instance.FlashHealthBar();
		}
		if (!Attack.HaveAmmo(character, this.m_weapon))
		{
			return false;
		}
		Attack.EquipAmmoItem(character, this.m_weapon);
		if (this.m_attackChainLevels > 1)
		{
			if (previousAttack != null && previousAttack.m_attackAnimation == this.m_attackAnimation)
			{
				this.m_currentAttackCainLevel = previousAttack.m_nextAttackChainLevel;
			}
			if (this.m_currentAttackCainLevel >= this.m_attackChainLevels || timeSinceLastAttack > 0.2f)
			{
				this.m_currentAttackCainLevel = 0;
			}
			this.m_zanim.SetTrigger(this.m_attackAnimation + this.m_currentAttackCainLevel.ToString());
		}
		else if (this.m_attackRandomAnimations >= 2)
		{
			int num = UnityEngine.Random.Range(0, this.m_attackRandomAnimations);
			this.m_zanim.SetTrigger(this.m_attackAnimation + num.ToString());
		}
		else
		{
			this.m_zanim.SetTrigger(this.m_attackAnimation);
		}
		if (character.IsPlayer() && this.m_attackType != Attack.AttackType.None && this.m_currentAttackCainLevel == 0 && (Player.m_localPlayer == null || !Player.m_localPlayer.AttackTowardsPlayerLookDir || this.m_attackType == Attack.AttackType.Projectile))
		{
			character.transform.rotation = character.GetLookYaw();
			this.m_body.rotation = character.transform.rotation;
		}
		weapon.m_lastAttackTime = Time.time;
		this.m_animEvent.ResetChain();
		return true;
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00003908 File Offset: 0x00001B08
	private float GetAttackStamina()
	{
		if (this.m_attackStamina <= 0f)
		{
			return 0f;
		}
		float num = this.m_attackStamina;
		float skillFactor = this.m_character.GetSkillFactor(this.m_weapon.m_shared.m_skillType);
		Player player = this.m_character as Player;
		if (player != null)
		{
			if (this.m_isHomeItem)
			{
				num *= 1f + player.GetEquipmentHomeItemModifier();
			}
			else
			{
				num *= 1f + player.GetEquipmentAttackStaminaModifier();
			}
		}
		this.m_character.GetSEMan().ModifyAttackStaminaUsage(num, ref num, true);
		num -= num * 0.33f * skillFactor;
		if (this.m_staminaReturnPerMissingHP > 0f)
		{
			num -= (this.m_character.GetMaxHealth() - this.m_character.GetHealth()) * this.m_staminaReturnPerMissingHP;
		}
		return num;
	}

	// Token: 0x06000023 RID: 35 RVA: 0x000039D4 File Offset: 0x00001BD4
	private float GetAttackEitr()
	{
		if (this.m_attackEitr <= 0f)
		{
			return 0f;
		}
		float attackEitr = this.m_attackEitr;
		float skillFactor = this.m_character.GetSkillFactor(this.m_weapon.m_shared.m_skillType);
		return attackEitr - attackEitr * 0.33f * skillFactor;
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00003A20 File Offset: 0x00001C20
	private float GetAttackHealth()
	{
		if (this.m_attackHealth <= 0f && this.m_attackHealthPercentage <= 0f)
		{
			return 0f;
		}
		float num = this.m_attackHealth + this.m_character.GetHealth() * this.m_attackHealthPercentage / 100f;
		float skillFactor = this.m_character.GetSkillFactor(this.m_weapon.m_shared.m_skillType);
		return num - num * 0.33f * skillFactor;
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00003A94 File Offset: 0x00001C94
	public void Update(float dt)
	{
		if (this.m_attackDone)
		{
			return;
		}
		this.m_time += dt;
		bool flag = this.m_character.InAttack();
		if (flag)
		{
			if (!this.m_wasInAttack)
			{
				BaseAI baseAI = this.m_character.GetBaseAI();
				if (baseAI != null)
				{
					baseAI.ChargeStop();
				}
				if (this.m_attackType != Attack.AttackType.Projectile || !this.m_perBurstResourceUsage)
				{
					this.m_character.UseStamina(this.GetAttackStamina());
					this.m_character.UseEitr(this.GetAttackEitr());
					this.m_character.UseHealth(Mathf.Min(this.m_character.GetHealth() - 1f, this.GetAttackHealth()));
				}
				Transform attackOrigin = this.GetAttackOrigin();
				this.m_weapon.m_shared.m_startEffect.Create(attackOrigin.position, this.m_character.transform.rotation, attackOrigin, 1f, -1);
				this.m_startEffect.Create(attackOrigin.position, this.m_character.transform.rotation, attackOrigin, 1f, -1);
				this.m_character.AddNoise(this.m_attackStartNoise);
				this.m_nextAttackChainLevel = this.m_currentAttackCainLevel + 1;
				if (this.m_nextAttackChainLevel >= this.m_attackChainLevels)
				{
					this.m_nextAttackChainLevel = 0;
				}
				this.m_wasInAttack = true;
			}
			if (this.m_isAttached)
			{
				this.UpdateAttach(dt);
			}
		}
		this.UpdateProjectile(dt);
		if ((!flag && this.m_wasInAttack) || this.m_abortAttack)
		{
			this.Stop();
		}
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00003C0F File Offset: 0x00001E0F
	public bool IsDone()
	{
		return this.m_attackDone;
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00003C18 File Offset: 0x00001E18
	public void Stop()
	{
		if (this.m_attackDone)
		{
			return;
		}
		if (this.m_loopingAttack)
		{
			this.m_zanim.SetTrigger("attack_abort");
		}
		if (this.m_isAttached)
		{
			this.m_zanim.SetTrigger("detach");
			this.m_isAttached = false;
			this.m_attachTarget = null;
		}
		if (this.m_wasInAttack)
		{
			if (this.m_visEquipment)
			{
				this.m_visEquipment.SetWeaponTrails(false);
			}
			this.m_wasInAttack = false;
		}
		this.m_attackDone = true;
		if (this.m_attackKillsSelf)
		{
			HitData hitData = new HitData();
			hitData.m_point = this.m_character.GetCenterPoint();
			hitData.m_damage.m_damage = 9999999f;
			hitData.m_hitType = HitData.HitType.Self;
			this.m_character.ApplyDamage(hitData, false, true, HitData.DamageModifier.Normal);
		}
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00003CE1 File Offset: 0x00001EE1
	public void Abort()
	{
		this.m_abortAttack = true;
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00003CEC File Offset: 0x00001EEC
	public void OnAttackTrigger()
	{
		if (!this.UseAmmo(out this.m_lastUsedAmmo))
		{
			return;
		}
		switch (this.m_attackType)
		{
		case Attack.AttackType.Horizontal:
		case Attack.AttackType.Vertical:
			this.DoMeleeAttack();
			break;
		case Attack.AttackType.Projectile:
			this.ProjectileAttackTriggered();
			break;
		case Attack.AttackType.None:
			this.DoNonAttack();
			break;
		case Attack.AttackType.Area:
			this.DoAreaAttack();
			break;
		}
		if (this.m_toggleFlying)
		{
			if (this.m_character.IsFlying())
			{
				this.m_character.Land();
			}
			else
			{
				this.m_character.TakeOff();
			}
		}
		if (this.m_recoilPushback != 0f)
		{
			this.m_character.ApplyPushback(-this.m_character.transform.forward, this.m_recoilPushback);
		}
		if (this.m_selfDamage > 0)
		{
			HitData hitData = new HitData();
			hitData.m_damage.m_damage = (float)this.m_selfDamage;
			this.m_character.Damage(hitData);
		}
		if (this.m_consumeItem)
		{
			this.ConsumeItem();
		}
		if (this.m_requiresReload)
		{
			this.m_character.ResetLoadedWeapon();
		}
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00003DF8 File Offset: 0x00001FF8
	private void ConsumeItem()
	{
		if (this.m_weapon.m_shared.m_maxStackSize > 1 && this.m_weapon.m_stack > 1)
		{
			this.m_weapon.m_stack--;
			return;
		}
		this.m_character.UnequipItem(this.m_weapon, false);
		this.m_character.GetInventory().RemoveItem(this.m_weapon);
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00003E64 File Offset: 0x00002064
	private static ItemDrop.ItemData FindAmmo(Humanoid character, ItemDrop.ItemData weapon)
	{
		if (string.IsNullOrEmpty(weapon.m_shared.m_ammoType))
		{
			return null;
		}
		ItemDrop.ItemData itemData = character.GetAmmoItem();
		if (itemData != null && (!character.GetInventory().ContainsItem(itemData) || itemData.m_shared.m_ammoType != weapon.m_shared.m_ammoType))
		{
			itemData = null;
		}
		if (itemData == null)
		{
			itemData = character.GetInventory().GetAmmoItem(weapon.m_shared.m_ammoType, null);
		}
		return itemData;
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00003ED8 File Offset: 0x000020D8
	private static bool EquipAmmoItem(Humanoid character, ItemDrop.ItemData weapon)
	{
		Attack.FindAmmo(character, weapon);
		if (!string.IsNullOrEmpty(weapon.m_shared.m_ammoType))
		{
			ItemDrop.ItemData ammoItem = character.GetAmmoItem();
			if (ammoItem != null && character.GetInventory().ContainsItem(ammoItem) && ammoItem.m_shared.m_ammoType == weapon.m_shared.m_ammoType)
			{
				return true;
			}
			ItemDrop.ItemData ammoItem2 = character.GetInventory().GetAmmoItem(weapon.m_shared.m_ammoType, null);
			if (ammoItem2.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Ammo || ammoItem2.m_shared.m_itemType == ItemDrop.ItemData.ItemType.AmmoNonEquipable)
			{
				return character.EquipItem(ammoItem2, true);
			}
		}
		return true;
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00003F78 File Offset: 0x00002178
	private static bool HaveAmmo(Humanoid character, ItemDrop.ItemData weapon)
	{
		if (string.IsNullOrEmpty(weapon.m_shared.m_ammoType))
		{
			return true;
		}
		ItemDrop.ItemData itemData = character.GetAmmoItem();
		if (itemData != null && (!character.GetInventory().ContainsItem(itemData) || itemData.m_shared.m_ammoType != weapon.m_shared.m_ammoType))
		{
			itemData = null;
		}
		if (itemData == null)
		{
			itemData = character.GetInventory().GetAmmoItem(weapon.m_shared.m_ammoType, null);
		}
		if (itemData == null)
		{
			character.Message(MessageHud.MessageType.Center, "$msg_outof " + weapon.m_shared.m_ammoType, 0, null);
			return false;
		}
		return itemData.m_shared.m_itemType != ItemDrop.ItemData.ItemType.Consumable || character.CanConsumeItem(itemData, false);
	}

	// Token: 0x0600002E RID: 46 RVA: 0x0000402C File Offset: 0x0000222C
	private bool UseAmmo(out ItemDrop.ItemData ammoItem)
	{
		this.m_ammoItem = null;
		ammoItem = null;
		if (string.IsNullOrEmpty(this.m_weapon.m_shared.m_ammoType))
		{
			return true;
		}
		ammoItem = this.m_character.GetAmmoItem();
		if (ammoItem != null && (!this.m_character.GetInventory().ContainsItem(ammoItem) || ammoItem.m_shared.m_ammoType != this.m_weapon.m_shared.m_ammoType))
		{
			ammoItem = null;
		}
		if (ammoItem == null)
		{
			ammoItem = this.m_character.GetInventory().GetAmmoItem(this.m_weapon.m_shared.m_ammoType, null);
		}
		if (ammoItem == null)
		{
			this.m_character.Message(MessageHud.MessageType.Center, "$msg_outof " + this.m_weapon.m_shared.m_ammoType, 0, null);
			return false;
		}
		if (ammoItem.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Consumable)
		{
			bool flag = this.m_character.ConsumeItem(this.m_character.GetInventory(), ammoItem, false);
			if (flag)
			{
				this.m_ammoItem = ammoItem;
			}
			return flag;
		}
		this.m_character.GetInventory().RemoveItem(ammoItem, 1);
		this.m_ammoItem = ammoItem;
		return true;
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00004150 File Offset: 0x00002350
	private void ProjectileAttackTriggered()
	{
		Vector3 basePos;
		Vector3 forward;
		this.GetProjectileSpawnPoint(out basePos, out forward);
		this.m_weapon.m_shared.m_triggerEffect.Create(basePos, Quaternion.LookRotation(forward), null, 1f, -1);
		this.m_triggerEffect.Create(basePos, Quaternion.LookRotation(forward), null, 1f, -1);
		if (this.m_weapon.m_shared.m_useDurability && this.m_character.IsPlayer())
		{
			this.m_weapon.m_durability -= this.m_weapon.m_shared.m_useDurabilityDrain;
		}
		if (this.m_projectileBursts == 1)
		{
			this.FireProjectileBurst();
			return;
		}
		this.m_projectileAttackStarted = true;
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00004200 File Offset: 0x00002400
	private void UpdateProjectile(float dt)
	{
		if (this.m_projectileAttackStarted && this.m_projectileBurstsFired < this.m_projectileBursts)
		{
			this.m_projectileFireTimer -= dt;
			if (this.m_projectileFireTimer <= 0f)
			{
				this.m_projectileFireTimer = this.m_burstInterval;
				this.FireProjectileBurst();
				this.m_projectileBurstsFired++;
			}
		}
	}

	// Token: 0x06000031 RID: 49 RVA: 0x0000425E File Offset: 0x0000245E
	private Transform GetAttackOrigin()
	{
		if (this.m_attackOriginJoint.Length > 0)
		{
			return Utils.FindChild(this.m_character.GetVisual().transform, this.m_attackOriginJoint, Utils.IterativeSearchType.DepthFirst);
		}
		return this.m_character.transform;
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00004298 File Offset: 0x00002498
	private void GetProjectileSpawnPoint(out Vector3 spawnPoint, out Vector3 aimDir)
	{
		Transform attackOrigin = this.GetAttackOrigin();
		Transform transform = this.m_character.transform;
		spawnPoint = attackOrigin.position + transform.up * this.m_attackHeight + transform.forward * this.m_attackRange + transform.right * this.m_attackOffset;
		aimDir = this.m_character.GetAimDir(spawnPoint);
		if (this.m_baseAI)
		{
			Character targetCreature = this.m_baseAI.GetTargetCreature();
			if (targetCreature)
			{
				Vector3 normalized = (targetCreature.GetCenterPoint() - spawnPoint).normalized;
				aimDir = Vector3.RotateTowards(this.m_character.transform.forward, normalized, 1.5707964f, 1f);
			}
		}
		if (this.m_useCharacterFacing)
		{
			Vector3 forward = Vector3.forward;
			if (this.m_useCharacterFacingYAim)
			{
				forward.y = aimDir.y;
			}
			aimDir = transform.TransformDirection(forward);
		}
	}

	// Token: 0x06000033 RID: 51 RVA: 0x000043B4 File Offset: 0x000025B4
	private void FireProjectileBurst()
	{
		if (this.m_perBurstResourceUsage)
		{
			float attackStamina = this.GetAttackStamina();
			if (attackStamina > 0f)
			{
				if (!this.m_character.HaveStamina(attackStamina))
				{
					this.Stop();
					return;
				}
				this.m_character.UseStamina(attackStamina);
			}
			float attackEitr = this.GetAttackEitr();
			if (attackEitr > 0f)
			{
				if (!this.m_character.HaveEitr(attackEitr))
				{
					this.Stop();
					return;
				}
				this.m_character.UseEitr(attackEitr);
			}
			float attackHealth = this.GetAttackHealth();
			if (attackHealth > 0f)
			{
				if (!this.m_character.HaveHealth(attackHealth) && this.m_attackHealthLowBlockUse)
				{
					this.Stop();
					return;
				}
				this.m_character.UseHealth(Mathf.Min(this.m_character.GetHealth() - 1f, attackHealth));
			}
		}
		ItemDrop.ItemData ammoItem = this.m_ammoItem;
		GameObject attackProjectile = this.m_attackProjectile;
		float num = this.m_projectileVel;
		float num2 = this.m_projectileVelMin;
		float num3 = this.m_projectileAccuracy;
		float num4 = this.m_projectileAccuracyMin;
		float num5 = this.m_attackHitNoise;
		AnimationCurve drawVelocityCurve = this.m_drawVelocityCurve;
		if (ammoItem != null && ammoItem.m_shared.m_attack.m_attackProjectile)
		{
			attackProjectile = ammoItem.m_shared.m_attack.m_attackProjectile;
			num += ammoItem.m_shared.m_attack.m_projectileVel;
			num2 += ammoItem.m_shared.m_attack.m_projectileVelMin;
			num3 += ammoItem.m_shared.m_attack.m_projectileAccuracy;
			num4 += ammoItem.m_shared.m_attack.m_projectileAccuracyMin;
			num5 += ammoItem.m_shared.m_attack.m_attackHitNoise;
			drawVelocityCurve = ammoItem.m_shared.m_attack.m_drawVelocityCurve;
		}
		float num6 = this.m_character.GetRandomSkillFactor(this.m_weapon.m_shared.m_skillType);
		if (this.m_bowDraw)
		{
			num3 = Mathf.Lerp(num4, num3, Mathf.Pow(this.m_attackDrawPercentage, 0.5f));
			num6 *= this.m_attackDrawPercentage;
			num = Mathf.Lerp(num2, num, drawVelocityCurve.Evaluate(this.m_attackDrawPercentage));
			Game.instance.IncrementPlayerStat(PlayerStatType.ArrowsShot, 1f);
		}
		else if (this.m_skillAccuracy)
		{
			float skillFactor = this.m_character.GetSkillFactor(this.m_weapon.m_shared.m_skillType);
			num3 = Mathf.Lerp(num4, num3, skillFactor);
		}
		Vector3 vector;
		Vector3 vector2;
		this.GetProjectileSpawnPoint(out vector, out vector2);
		if (this.m_launchAngle != 0f)
		{
			Vector3 axis = Vector3.Cross(Vector3.up, vector2);
			vector2 = Quaternion.AngleAxis(this.m_launchAngle, axis) * vector2;
		}
		if (this.m_burstEffect.HasEffects())
		{
			this.m_burstEffect.Create(vector, Quaternion.LookRotation(vector2), null, 1f, -1);
		}
		for (int i = 0; i < this.m_projectiles; i++)
		{
			if (this.m_destroyPreviousProjectile && this.m_weapon.m_lastProjectile)
			{
				ZNetScene.instance.Destroy(this.m_weapon.m_lastProjectile);
				this.m_weapon.m_lastProjectile = null;
			}
			Vector3 vector3 = vector2;
			if (!this.m_bowDraw && this.m_randomVelocity)
			{
				num = UnityEngine.Random.Range(num2, num);
			}
			Vector3 axis2 = Vector3.Cross(vector3, Vector3.up);
			Quaternion rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(-num3, num3), Vector3.up);
			if (this.m_circularProjectileLaunch && !this.m_distributeProjectilesAroundCircle)
			{
				rotation = Quaternion.AngleAxis(UnityEngine.Random.value * 360f, Vector3.up);
			}
			else if (this.m_circularProjectileLaunch && !this.m_distributeProjectilesAroundCircle)
			{
				rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(-num3, num3) + (float)(i * (360 / this.m_projectiles)), Vector3.up);
			}
			vector3 = Quaternion.AngleAxis(UnityEngine.Random.Range(-num3, num3), axis2) * vector3;
			vector3 = rotation * vector3;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(attackProjectile, vector, Quaternion.LookRotation(vector3));
			HitData hitData = new HitData();
			hitData.m_toolTier = (short)this.m_weapon.m_shared.m_toolTier;
			hitData.m_pushForce = this.m_weapon.m_shared.m_attackForce * this.m_forceMultiplier;
			hitData.m_backstabBonus = this.m_weapon.m_shared.m_backstabBonus;
			hitData.m_staggerMultiplier = this.m_staggerMultiplier;
			hitData.m_damage.Add(this.m_weapon.GetDamage(), 1);
			hitData.m_statusEffectHash = ((this.m_weapon.m_shared.m_attackStatusEffect && (this.m_weapon.m_shared.m_attackStatusEffectChance == 1f || UnityEngine.Random.Range(0f, 1f) < this.m_weapon.m_shared.m_attackStatusEffectChance)) ? this.m_weapon.m_shared.m_attackStatusEffect.NameHash() : 0);
			hitData.m_skillLevel = this.m_character.GetSkillLevel(this.m_weapon.m_shared.m_skillType);
			hitData.m_itemLevel = (short)this.m_weapon.m_quality;
			hitData.m_itemWorldLevel = (byte)this.m_weapon.m_worldLevel;
			hitData.m_blockable = this.m_weapon.m_shared.m_blockable;
			hitData.m_dodgeable = this.m_weapon.m_shared.m_dodgeable;
			hitData.m_skill = this.m_weapon.m_shared.m_skillType;
			hitData.m_skillRaiseAmount = this.m_raiseSkillAmount;
			hitData.SetAttacker(this.m_character);
			hitData.m_hitType = ((hitData.GetAttacker() is Player) ? HitData.HitType.PlayerHit : HitData.HitType.EnemyHit);
			hitData.m_healthReturn = this.m_attackHealthReturnHit;
			if (ammoItem != null)
			{
				hitData.m_damage.Add(ammoItem.GetDamage(), 1);
				hitData.m_pushForce += ammoItem.m_shared.m_attackForce;
				if (ammoItem.m_shared.m_attackStatusEffect != null && (ammoItem.m_shared.m_attackStatusEffectChance == 1f || UnityEngine.Random.Range(0f, 1f) < ammoItem.m_shared.m_attackStatusEffectChance))
				{
					hitData.m_statusEffectHash = ammoItem.m_shared.m_attackStatusEffect.NameHash();
				}
				if (!ammoItem.m_shared.m_blockable)
				{
					hitData.m_blockable = false;
				}
				if (!ammoItem.m_shared.m_dodgeable)
				{
					hitData.m_dodgeable = false;
				}
			}
			hitData.m_pushForce *= num6;
			this.ModifyDamage(hitData, num6);
			this.m_character.GetSEMan().ModifyAttack(this.m_weapon.m_shared.m_skillType, ref hitData);
			IProjectile component = gameObject.GetComponent<IProjectile>();
			if (component != null)
			{
				component.Setup(this.m_character, vector3 * num, num5, hitData, this.m_weapon, this.m_lastUsedAmmo);
			}
			this.m_weapon.m_lastProjectile = gameObject;
			if (this.m_spawnOnHitChance > 0f && this.m_spawnOnHit)
			{
				Projectile projectile = component as Projectile;
				if (projectile != null)
				{
					projectile.m_spawnOnHit = this.m_spawnOnHit;
					projectile.m_spawnOnHitChance = this.m_spawnOnHitChance;
				}
			}
		}
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00004AC4 File Offset: 0x00002CC4
	private void ModifyDamage(HitData hitData, float damageFactor = 1f)
	{
		if (this.m_damageMultiplier != 1f)
		{
			hitData.m_damage.Modify(this.m_damageMultiplier);
		}
		if (damageFactor != 1f)
		{
			hitData.m_damage.Modify(damageFactor);
		}
		hitData.m_damage.Modify(this.GetLevelDamageFactor());
		if (this.m_damageMultiplierPerMissingHP > 0f)
		{
			hitData.m_damage.Modify(1f + (this.m_character.GetMaxHealth() - this.m_character.GetHealth()) * this.m_damageMultiplierPerMissingHP);
		}
		if (this.m_damageMultiplierByTotalHealthMissing > 0f)
		{
			hitData.m_damage.Modify(1f + (1f - this.m_character.GetHealthPercentage()) * this.m_damageMultiplierByTotalHealthMissing);
		}
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00004B88 File Offset: 0x00002D88
	private void DoNonAttack()
	{
		if (this.m_weapon.m_shared.m_useDurability && this.m_character.IsPlayer())
		{
			this.m_weapon.m_durability -= this.m_weapon.m_shared.m_useDurabilityDrain;
		}
		Transform attackOrigin = this.GetAttackOrigin();
		this.m_weapon.m_shared.m_triggerEffect.Create(attackOrigin.position, this.m_character.transform.rotation, attackOrigin, 1f, -1);
		this.m_triggerEffect.Create(attackOrigin.position, this.m_character.transform.rotation, attackOrigin, 1f, -1);
		if (this.m_weapon.m_shared.m_consumeStatusEffect)
		{
			this.m_character.GetSEMan().AddStatusEffect(this.m_weapon.m_shared.m_consumeStatusEffect, true, 0, 0f);
		}
		this.m_character.AddNoise(this.m_attackHitNoise);
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00004C89 File Offset: 0x00002E89
	private float GetLevelDamageFactor()
	{
		return 1f + (float)Mathf.Max(0, this.m_character.GetLevel() - 1) * 0.5f;
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00004CAC File Offset: 0x00002EAC
	private void DoAreaAttack()
	{
		Transform transform = this.m_character.transform;
		Transform attackOrigin = this.GetAttackOrigin();
		Vector3 vector = attackOrigin.position + Vector3.up * this.m_attackHeight + transform.forward * this.m_attackRange + transform.right * this.m_attackOffset;
		this.m_weapon.m_shared.m_triggerEffect.Create(vector, transform.rotation, attackOrigin, 1f, -1);
		this.m_triggerEffect.Create(vector, transform.rotation, attackOrigin, 1f, -1);
		int num = 0;
		Vector3 vector2 = Vector3.zero;
		bool flag = false;
		float randomSkillFactor = this.m_character.GetRandomSkillFactor(this.m_weapon.m_shared.m_skillType);
		int layerMask = this.m_hitTerrain ? Attack.m_attackMaskTerrain : Attack.m_attackMask;
		Collider[] array = Physics.OverlapSphere(vector, this.m_attackRayWidth, layerMask, QueryTriggerInteraction.UseGlobal);
		HashSet<GameObject> hashSet = new HashSet<GameObject>();
		foreach (Collider collider in array)
		{
			if (!(collider.gameObject == this.m_character.gameObject))
			{
				GameObject gameObject = Projectile.FindHitObject(collider);
				if (!(gameObject == this.m_character.gameObject) && !hashSet.Contains(gameObject))
				{
					hashSet.Add(gameObject);
					Vector3 vector3;
					if (collider is MeshCollider)
					{
						vector3 = collider.ClosestPointOnBounds(vector);
					}
					else
					{
						vector3 = collider.ClosestPoint(vector);
					}
					IDestructible component = gameObject.GetComponent<IDestructible>();
					if (component != null)
					{
						Vector3 vector4 = vector3 - vector;
						vector4.y = 0f;
						Vector3 vector5 = vector3 - transform.position;
						if (Vector3.Dot(vector5, vector4) < 0f)
						{
							vector4 = vector5;
						}
						vector4.Normalize();
						HitData hitData = new HitData();
						hitData.m_toolTier = (short)this.m_weapon.m_shared.m_toolTier;
						hitData.m_statusEffectHash = ((this.m_weapon.m_shared.m_attackStatusEffect && (this.m_weapon.m_shared.m_attackStatusEffectChance == 1f || UnityEngine.Random.Range(0f, 1f) < this.m_weapon.m_shared.m_attackStatusEffectChance)) ? this.m_weapon.m_shared.m_attackStatusEffect.NameHash() : 0);
						hitData.m_skillLevel = this.m_character.GetSkillLevel(this.m_weapon.m_shared.m_skillType);
						hitData.m_itemLevel = (short)this.m_weapon.m_quality;
						hitData.m_itemWorldLevel = (byte)this.m_weapon.m_worldLevel;
						hitData.m_pushForce = this.m_weapon.m_shared.m_attackForce * randomSkillFactor * this.m_forceMultiplier;
						hitData.m_backstabBonus = this.m_weapon.m_shared.m_backstabBonus;
						hitData.m_staggerMultiplier = this.m_staggerMultiplier;
						hitData.m_dodgeable = this.m_weapon.m_shared.m_dodgeable;
						hitData.m_blockable = this.m_weapon.m_shared.m_blockable;
						hitData.m_skill = this.m_weapon.m_shared.m_skillType;
						hitData.m_skillRaiseAmount = this.m_raiseSkillAmount;
						hitData.m_damage.Add(this.m_weapon.GetDamage(), 1);
						hitData.m_point = vector3;
						hitData.m_dir = vector4;
						hitData.m_hitCollider = collider;
						hitData.SetAttacker(this.m_character);
						hitData.m_hitType = ((hitData.GetAttacker() is Player) ? HitData.HitType.PlayerHit : HitData.HitType.EnemyHit);
						hitData.m_healthReturn = this.m_attackHealthReturnHit;
						this.ModifyDamage(hitData, randomSkillFactor);
						this.SpawnOnHit(gameObject);
						if (this.m_attackChainLevels > 1 && this.m_currentAttackCainLevel == this.m_attackChainLevels - 1 && this.m_lastChainDamageMultiplier > 1f)
						{
							hitData.m_damage.Modify(this.m_lastChainDamageMultiplier);
							hitData.m_pushForce *= 1.2f;
						}
						this.m_character.GetSEMan().ModifyAttack(this.m_weapon.m_shared.m_skillType, ref hitData);
						Character character = component as Character;
						bool flag2 = false;
						if (character)
						{
							flag2 = (BaseAI.IsEnemy(this.m_character, character) || (character.GetBaseAI() && character.GetBaseAI().IsAggravatable() && this.m_character.IsPlayer()));
							if (((!this.m_hitFriendly || this.m_character.IsTamed()) && !this.m_character.IsPlayer() && !flag2) || (!this.m_weapon.m_shared.m_tamedOnly && this.m_character.IsPlayer() && !this.m_character.IsPVPEnabled() && !flag2) || (this.m_weapon.m_shared.m_tamedOnly && !character.IsTamed()))
							{
								goto IL_566;
							}
							if (hitData.m_dodgeable && character.IsDodgeInvincible())
							{
								goto IL_566;
							}
						}
						else if (this.m_weapon.m_shared.m_tamedOnly)
						{
							goto IL_566;
						}
						if (this.m_attackHealthReturnHit > 0f && this.m_character && flag2)
						{
							this.m_character.Heal(this.m_attackHealthReturnHit, true);
						}
						component.Damage(hitData);
						if ((component.GetDestructibleType() & this.m_skillHitType) != DestructibleType.None)
						{
							flag = true;
						}
					}
					num++;
					vector2 += vector3;
				}
			}
			IL_566:;
		}
		if (num > 0)
		{
			vector2 /= (float)num;
			this.m_weapon.m_shared.m_hitEffect.Create(vector2, Quaternion.identity, null, 1f, -1);
			this.m_hitEffect.Create(vector2, Quaternion.identity, null, 1f, -1);
			if (this.m_weapon.m_shared.m_useDurability && this.m_character.IsPlayer())
			{
				this.m_weapon.m_durability -= 1f;
			}
			this.m_character.AddNoise(this.m_attackHitNoise);
			if (flag)
			{
				this.m_character.RaiseSkill(this.m_weapon.m_shared.m_skillType, this.m_raiseSkillAmount);
			}
		}
		if (this.m_spawnOnTrigger)
		{
			IProjectile component2 = UnityEngine.Object.Instantiate<GameObject>(this.m_spawnOnTrigger, vector, Quaternion.identity).GetComponent<IProjectile>();
			if (component2 != null)
			{
				component2.Setup(this.m_character, this.m_character.transform.forward, -1f, null, null, this.m_lastUsedAmmo);
			}
		}
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00005340 File Offset: 0x00003540
	private void GetMeleeAttackDir(out Transform originJoint, out Vector3 attackDir)
	{
		originJoint = this.GetAttackOrigin();
		Vector3 forward = this.m_character.transform.forward;
		Vector3 aimDir = this.m_character.GetAimDir(originJoint.position);
		aimDir.x = forward.x;
		aimDir.z = forward.z;
		aimDir.Normalize();
		attackDir = Vector3.RotateTowards(this.m_character.transform.forward, aimDir, 0.017453292f * this.m_maxYAngle, 10f);
	}

	// Token: 0x06000039 RID: 57 RVA: 0x000053C8 File Offset: 0x000035C8
	private void AddHitPoint(List<Attack.HitPoint> list, GameObject go, Collider collider, Vector3 point, float distance, bool multiCollider)
	{
		Attack.HitPoint hitPoint = null;
		for (int i = list.Count - 1; i >= 0; i--)
		{
			if ((!multiCollider && list[i].go == go) || (multiCollider && list[i].collider == collider))
			{
				hitPoint = list[i];
				break;
			}
		}
		if (hitPoint == null)
		{
			hitPoint = new Attack.HitPoint();
			hitPoint.go = go;
			hitPoint.collider = collider;
			hitPoint.firstPoint = point;
			list.Add(hitPoint);
		}
		hitPoint.avgPoint += point;
		hitPoint.count++;
		if (distance < hitPoint.closestDistance)
		{
			hitPoint.closestPoint = point;
			hitPoint.closestDistance = distance;
		}
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00005488 File Offset: 0x00003688
	private void DoMeleeAttack()
	{
		Transform transform;
		Vector3 vector;
		this.GetMeleeAttackDir(out transform, out vector);
		Vector3 point = this.m_character.transform.InverseTransformDirection(vector);
		Quaternion quaternion = Quaternion.LookRotation(vector, Vector3.up);
		this.m_weapon.m_shared.m_triggerEffect.Create(transform.position, quaternion, transform, 1f, -1);
		this.m_triggerEffect.Create(transform.position, quaternion, transform, 1f, -1);
		Vector3 vector2 = transform.position + Vector3.up * this.m_attackHeight + this.m_character.transform.right * this.m_attackOffset;
		float num = this.m_attackAngle / 2f;
		float num2 = 4f;
		float attackRange = this.m_attackRange;
		List<Attack.HitPoint> list = new List<Attack.HitPoint>();
		HashSet<Skills.SkillType> hashSet = new HashSet<Skills.SkillType>();
		int layerMask = this.m_hitTerrain ? Attack.m_attackMaskTerrain : Attack.m_attackMask;
		for (float num3 = -num; num3 <= num; num3 += num2)
		{
			Quaternion rotation = Quaternion.identity;
			if (this.m_attackType == Attack.AttackType.Horizontal)
			{
				rotation = Quaternion.Euler(0f, -num3, 0f);
			}
			else if (this.m_attackType == Attack.AttackType.Vertical)
			{
				rotation = Quaternion.Euler(num3, 0f, 0f);
			}
			Vector3 vector3 = this.m_character.transform.TransformDirection(rotation * point);
			Debug.DrawLine(vector2, vector2 + vector3 * attackRange);
			RaycastHit[] array;
			if (this.m_attackRayWidth > 0f)
			{
				array = Physics.SphereCastAll(vector2, this.m_attackRayWidth, vector3, Mathf.Max(0f, attackRange - this.m_attackRayWidth), layerMask, QueryTriggerInteraction.Ignore);
			}
			else
			{
				array = Physics.RaycastAll(vector2, vector3, attackRange, layerMask, QueryTriggerInteraction.Ignore);
			}
			Array.Sort<RaycastHit>(array, (RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance));
			foreach (RaycastHit raycastHit in array)
			{
				if (!(raycastHit.collider.gameObject == this.m_character.gameObject))
				{
					Vector3 vector4 = raycastHit.point;
					if (raycastHit.normal == -vector3 && raycastHit.point == Vector3.zero)
					{
						if (raycastHit.collider is MeshCollider)
						{
							vector4 = vector2 + vector3 * attackRange;
						}
						else
						{
							vector4 = raycastHit.collider.ClosestPoint(vector2);
						}
					}
					if (this.m_attackAngle >= 180f || Vector3.Dot(vector4 - vector2, vector) > 0f)
					{
						GameObject gameObject = Projectile.FindHitObject(raycastHit.collider);
						if (!(gameObject == this.m_character.gameObject))
						{
							Vagon component = gameObject.GetComponent<Vagon>();
							if (!component || !component.IsAttached(this.m_character))
							{
								Character component2 = gameObject.GetComponent<Character>();
								if (component2 != null)
								{
									bool flag = BaseAI.IsEnemy(this.m_character, component2) || (component2.GetBaseAI() && component2.GetBaseAI().IsAggravatable() && this.m_character.IsPlayer());
									if (((!this.m_hitFriendly || this.m_character.IsTamed()) && !this.m_character.IsPlayer() && !flag) || (!this.m_weapon.m_shared.m_tamedOnly && this.m_character.IsPlayer() && !this.m_character.IsPVPEnabled() && !flag) || (this.m_weapon.m_shared.m_tamedOnly && !component2.IsTamed()))
									{
										goto IL_41E;
									}
									if (this.m_weapon.m_shared.m_dodgeable && component2.IsDodgeInvincible())
									{
										goto IL_41E;
									}
								}
								else if (this.m_weapon.m_shared.m_tamedOnly)
								{
									goto IL_41E;
								}
								bool multiCollider = this.m_pickaxeSpecial && (gameObject.GetComponent<MineRock5>() || gameObject.GetComponent<MineRock>());
								this.AddHitPoint(list, gameObject, raycastHit.collider, vector4, raycastHit.distance, multiCollider);
								if (!this.m_hitThroughWalls)
								{
									break;
								}
							}
						}
					}
				}
				IL_41E:;
			}
		}
		int num4 = 0;
		Vector3 vector5 = Vector3.zero;
		bool flag2 = false;
		Character character = null;
		bool flag3 = false;
		foreach (Attack.HitPoint hitPoint in list)
		{
			GameObject go = hitPoint.go;
			Vector3 vector6 = hitPoint.avgPoint / (float)hitPoint.count;
			Vector3 vector7 = vector6;
			switch (this.m_hitPointtype)
			{
			case Attack.HitPointType.Closest:
				vector7 = hitPoint.closestPoint;
				break;
			case Attack.HitPointType.Average:
				vector7 = vector6;
				break;
			case Attack.HitPointType.First:
				vector7 = hitPoint.firstPoint;
				break;
			}
			num4++;
			vector5 += vector6;
			this.m_weapon.m_shared.m_hitEffect.Create(vector7, Quaternion.identity, null, 1f, -1);
			this.m_hitEffect.Create(vector7, Quaternion.identity, null, 1f, -1);
			IDestructible component3 = go.GetComponent<IDestructible>();
			if (component3 != null)
			{
				DestructibleType destructibleType = component3.GetDestructibleType();
				Skills.SkillType skillType = this.m_weapon.m_shared.m_skillType;
				if (this.m_specialHitSkill != Skills.SkillType.None && (destructibleType & this.m_specialHitType) != DestructibleType.None)
				{
					skillType = this.m_specialHitSkill;
					hashSet.Add(this.m_specialHitSkill);
				}
				else if ((destructibleType & this.m_skillHitType) != DestructibleType.None)
				{
					hashSet.Add(skillType);
				}
				float num5 = this.m_character.GetRandomSkillFactor(skillType);
				if (this.m_multiHit && this.m_lowerDamagePerHit && list.Count > 1)
				{
					num5 /= (float)list.Count * 0.75f;
				}
				HitData hitData = new HitData();
				hitData.m_toolTier = (short)this.m_weapon.m_shared.m_toolTier;
				hitData.m_statusEffectHash = ((this.m_weapon.m_shared.m_attackStatusEffect && (this.m_weapon.m_shared.m_attackStatusEffectChance == 1f || UnityEngine.Random.Range(0f, 1f) < this.m_weapon.m_shared.m_attackStatusEffectChance)) ? this.m_weapon.m_shared.m_attackStatusEffect.NameHash() : 0);
				hitData.m_skillLevel = this.m_character.GetSkillLevel(this.m_weapon.m_shared.m_skillType);
				hitData.m_itemLevel = (short)this.m_weapon.m_quality;
				hitData.m_itemWorldLevel = (byte)this.m_weapon.m_worldLevel;
				hitData.m_pushForce = this.m_weapon.m_shared.m_attackForce * num5 * this.m_forceMultiplier;
				hitData.m_backstabBonus = this.m_weapon.m_shared.m_backstabBonus;
				hitData.m_staggerMultiplier = this.m_staggerMultiplier;
				hitData.m_dodgeable = this.m_weapon.m_shared.m_dodgeable;
				hitData.m_blockable = this.m_weapon.m_shared.m_blockable;
				hitData.m_skill = skillType;
				hitData.m_skillRaiseAmount = this.m_raiseSkillAmount;
				hitData.m_damage = this.m_weapon.GetDamage();
				hitData.m_point = vector7;
				hitData.m_dir = (vector7 - vector2).normalized;
				hitData.m_hitCollider = hitPoint.collider;
				hitData.SetAttacker(this.m_character);
				hitData.m_hitType = ((hitData.GetAttacker() is Player) ? HitData.HitType.PlayerHit : HitData.HitType.EnemyHit);
				hitData.m_healthReturn = this.m_attackHealthReturnHit;
				this.ModifyDamage(hitData, num5);
				this.SpawnOnHit(go);
				if (this.m_attackChainLevels > 1 && this.m_currentAttackCainLevel == this.m_attackChainLevels - 1)
				{
					hitData.m_damage.Modify(2f);
					hitData.m_pushForce *= 1.2f;
				}
				this.m_character.GetSEMan().ModifyAttack(skillType, ref hitData);
				if (component3 is Character)
				{
					character = (component3 as Character);
				}
				component3.Damage(hitData);
				if (this.m_attackHealthReturnHit > 0f && this.m_character && character)
				{
					this.m_character.Heal(this.m_attackHealthReturnHit, true);
				}
				if ((destructibleType & this.m_resetChainIfHit) != DestructibleType.None)
				{
					this.m_nextAttackChainLevel = 0;
				}
				if (!this.m_multiHit)
				{
					break;
				}
			}
			if (go.GetComponent<Heightmap>() != null && !flag2 && (!this.m_pickaxeSpecial || !flag3))
			{
				flag2 = true;
				this.m_weapon.m_shared.m_hitTerrainEffect.Create(vector7, quaternion, null, 1f, -1);
				this.m_hitTerrainEffect.Create(vector7, quaternion, null, 1f, -1);
				if (this.m_weapon.m_shared.m_spawnOnHitTerrain)
				{
					Attack.SpawnOnHitTerrain(vector7, this.m_weapon.m_shared.m_spawnOnHitTerrain, this.m_character, this.m_attackHitNoise, this.m_weapon, this.m_lastUsedAmmo, false);
				}
				if (!this.m_multiHit)
				{
					break;
				}
				if (this.m_pickaxeSpecial)
				{
					break;
				}
			}
			else
			{
				flag3 = true;
			}
		}
		if (num4 > 0)
		{
			vector5 /= (float)num4;
			if (this.m_weapon.m_shared.m_useDurability && this.m_character.IsPlayer())
			{
				this.m_weapon.m_durability -= this.m_weapon.m_shared.m_useDurabilityDrain;
			}
			this.m_character.AddNoise(this.m_attackHitNoise);
			this.m_character.FreezeFrame(0.15f);
			if (this.m_weapon.m_shared.m_spawnOnHit)
			{
				IProjectile component4 = UnityEngine.Object.Instantiate<GameObject>(this.m_weapon.m_shared.m_spawnOnHit, vector5, quaternion).GetComponent<IProjectile>();
				if (component4 != null)
				{
					component4.Setup(this.m_character, Vector3.zero, this.m_attackHitNoise, null, this.m_weapon, this.m_lastUsedAmmo);
				}
			}
			foreach (Skills.SkillType skill in hashSet)
			{
				this.m_character.RaiseSkill(skill, this.m_raiseSkillAmount * ((character != null) ? 1.5f : 1f));
			}
			if (this.m_attach && !this.m_isAttached && character)
			{
				this.TryAttach(character, vector5);
			}
		}
		if (this.m_spawnOnTrigger)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.m_spawnOnTrigger, vector2, Quaternion.identity);
			IProjectile component5 = gameObject2.GetComponent<IProjectile>();
			if (component5 != null)
			{
				component5.Setup(this.m_character, this.m_character.transform.forward, -1f, null, this.m_weapon, this.m_lastUsedAmmo);
			}
			Piece component6 = gameObject2.GetComponent<Piece>();
			if (component6 != null)
			{
				Player player = this.m_character as Player;
				if (player != null)
				{
					player.PlacePiece(component6, vector2 + vector * this.m_attackRange, player.transform.rotation, false);
				}
			}
		}
		if (this.m_harvest && this.m_character == Player.m_localPlayer)
		{
			Vector3 position = vector2 + vector * this.m_attackRange;
			float skillFactor = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Farming);
			float radius = Mathf.Lerp(this.m_harvestRadius, this.m_harvestRadiusMaxLevel, skillFactor);
			int num6 = Physics.OverlapSphereNonAlloc(position, radius, Attack.s_pieceColliders, Attack.m_harvestRayMask);
			for (int j = 0; j < num6; j++)
			{
				GameObject gameObject3 = Attack.s_pieceColliders[j].gameObject;
				Pickable component7 = gameObject3.GetComponent<Pickable>();
				if (component7 != null && component7.m_harvestable && component7.CanBePicked())
				{
					component7.Interact(Player.m_localPlayer, false, false);
				}
				else
				{
					Plant component8 = gameObject3.GetComponent<Plant>();
					if (component8 != null && component8.GetStatus() != Plant.Status.Healthy)
					{
						Destructible component9 = gameObject3.GetComponent<Destructible>();
						if (component9 != null)
						{
							component9.Destroy(null);
						}
					}
				}
			}
		}
	}

	// Token: 0x0600003B RID: 59 RVA: 0x000060DC File Offset: 0x000042DC
	private void SpawnOnHit(GameObject target)
	{
		if (this.m_spawnOnHitChance > 0f && this.m_spawnOnHit && UnityEngine.Random.Range(0f, 1f) < this.m_spawnOnHitChance)
		{
			IProjectile componentInChildren = UnityEngine.Object.Instantiate<GameObject>(this.m_spawnOnHit, target.transform.position, target.transform.rotation).GetComponentInChildren<IProjectile>();
			if (componentInChildren != null)
			{
				componentInChildren.Setup(this.m_character, this.m_character.transform.forward, -1f, null, this.m_weapon, this.m_lastUsedAmmo);
			}
		}
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00006174 File Offset: 0x00004374
	private bool TryAttach(Character hitCharacter, Vector3 hitPoint)
	{
		if (hitCharacter.IsDodgeInvincible())
		{
			return false;
		}
		if (hitCharacter.IsBlocking())
		{
			Vector3 lhs = hitCharacter.transform.position - this.m_character.transform.position;
			lhs.y = 0f;
			lhs.Normalize();
			if (Vector3.Dot(lhs, hitCharacter.transform.forward) < 0f)
			{
				return false;
			}
		}
		this.m_isAttached = true;
		this.m_attachTarget = hitCharacter.transform;
		float num = hitCharacter.GetRadius() + this.m_character.GetRadius() + 0.1f;
		Vector3 a = hitCharacter.transform.position - this.m_character.transform.position;
		a.y = 0f;
		a.Normalize();
		this.m_attachDistance = num;
		Vector3 position = hitCharacter.GetCenterPoint() - a * num;
		this.m_attachOffset = this.m_attachTarget.InverseTransformPoint(position);
		hitPoint.y = Mathf.Clamp(hitPoint.y, hitCharacter.transform.position.y + hitCharacter.GetRadius(), hitCharacter.transform.position.y + hitCharacter.GetHeight() - hitCharacter.GetRadius() * 1.5f);
		this.m_attachHitPoint = this.m_attachTarget.InverseTransformPoint(hitPoint);
		this.m_zanim.SetTrigger("attach");
		return true;
	}

	// Token: 0x0600003D RID: 61 RVA: 0x000062E0 File Offset: 0x000044E0
	private void UpdateAttach(float dt)
	{
		if (this.m_attachTarget)
		{
			Character component = this.m_attachTarget.GetComponent<Character>();
			if (component != null)
			{
				if (component.IsDead())
				{
					this.Stop();
					return;
				}
				this.m_detachTimer += dt;
				if (this.m_detachTimer > 0.3f)
				{
					this.m_detachTimer = 0f;
					if (component.IsDodgeInvincible())
					{
						this.Stop();
						return;
					}
				}
			}
			Vector3 b = this.m_attachTarget.TransformPoint(this.m_attachOffset);
			Vector3 a = this.m_attachTarget.TransformPoint(this.m_attachHitPoint);
			Vector3 b2 = Vector3.Lerp(this.m_character.transform.position, b, 0.1f);
			Vector3 vector = a - b2;
			vector.Normalize();
			Quaternion rotation = Quaternion.LookRotation(vector);
			Vector3 position = a - vector * this.m_character.GetRadius();
			this.m_character.transform.position = position;
			this.m_character.transform.rotation = rotation;
			this.m_character.GetComponent<Rigidbody>().velocity = Vector3.zero;
			return;
		}
		this.Stop();
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00006404 File Offset: 0x00004604
	public bool IsAttached()
	{
		return this.m_isAttached;
	}

	// Token: 0x0600003F RID: 63 RVA: 0x0000640C File Offset: 0x0000460C
	public bool GetAttachData(out ZDOID parent, out string attachJoint, out Vector3 relativePos, out Quaternion relativeRot, out Vector3 relativeVel)
	{
		attachJoint = "";
		parent = ZDOID.None;
		relativePos = Vector3.zero;
		relativeRot = Quaternion.identity;
		relativeVel = Vector3.zero;
		if (!this.m_isAttached || !this.m_attachTarget)
		{
			return false;
		}
		ZNetView component = this.m_attachTarget.GetComponent<ZNetView>();
		if (!component)
		{
			return false;
		}
		parent = component.GetZDO().m_uid;
		relativePos = component.transform.InverseTransformPoint(this.m_character.transform.position);
		relativeRot = Quaternion.Inverse(component.transform.rotation) * this.m_character.transform.rotation;
		relativeVel = Vector3.zero;
		return true;
	}

	// Token: 0x06000040 RID: 64 RVA: 0x000064E8 File Offset: 0x000046E8
	public static GameObject SpawnOnHitTerrain(Vector3 hitPoint, GameObject prefab, Character character, float attackHitNoise, ItemDrop.ItemData weapon, ItemDrop.ItemData ammo, bool randomRotation = false)
	{
		TerrainModifier componentInChildren = prefab.GetComponentInChildren<TerrainModifier>();
		if (componentInChildren)
		{
			if (!PrivateArea.CheckAccess(hitPoint, componentInChildren.GetRadius(), true, false))
			{
				return null;
			}
			if (Location.IsInsideNoBuildLocation(hitPoint))
			{
				return null;
			}
		}
		TerrainOp componentInChildren2 = prefab.GetComponentInChildren<TerrainOp>();
		if (componentInChildren2)
		{
			if (!PrivateArea.CheckAccess(hitPoint, componentInChildren2.GetRadius(), true, false))
			{
				return null;
			}
			if (Location.IsInsideNoBuildLocation(hitPoint))
			{
				return null;
			}
		}
		TerrainModifier.SetTriggerOnPlaced(true);
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, hitPoint, randomRotation ? Quaternion.Euler(0f, (float)UnityEngine.Random.Range(0, 360), 0f) : ((character != null) ? Quaternion.LookRotation(character.transform.forward) : Quaternion.identity));
		TerrainModifier.SetTriggerOnPlaced(false);
		IProjectile component = gameObject.GetComponent<IProjectile>();
		if (component != null)
		{
			component.Setup(character, Vector3.zero, attackHitNoise, null, weapon, ammo);
		}
		return gameObject;
	}

	// Token: 0x06000041 RID: 65 RVA: 0x000065BB File Offset: 0x000047BB
	public Attack Clone()
	{
		return base.MemberwiseClone() as Attack;
	}

	// Token: 0x06000042 RID: 66 RVA: 0x000065C8 File Offset: 0x000047C8
	public ItemDrop.ItemData GetWeapon()
	{
		return this.m_weapon;
	}

	// Token: 0x06000043 RID: 67 RVA: 0x000065D0 File Offset: 0x000047D0
	public bool CanStartChainAttack()
	{
		return this.m_nextAttackChainLevel > 0 && this.m_animEvent.CanChain();
	}

	// Token: 0x06000044 RID: 68 RVA: 0x000065E8 File Offset: 0x000047E8
	public void OnTrailStart()
	{
		if (this.m_attackType == Attack.AttackType.Projectile)
		{
			Transform attackOrigin = this.GetAttackOrigin();
			this.m_weapon.m_shared.m_trailStartEffect.Create(attackOrigin.position, this.m_character.transform.rotation, this.m_character.transform, 1f, -1);
			this.m_trailStartEffect.Create(attackOrigin.position, this.m_character.transform.rotation, this.m_character.transform, 1f, -1);
			return;
		}
		Transform transform;
		Vector3 forward;
		this.GetMeleeAttackDir(out transform, out forward);
		Quaternion baseRot = Quaternion.LookRotation(forward, Vector3.up);
		this.m_weapon.m_shared.m_trailStartEffect.Create(transform.position, baseRot, this.m_character.transform, 1f, -1);
		this.m_trailStartEffect.Create(transform.position, baseRot, this.m_character.transform, 1f, -1);
	}

	// Token: 0x06000045 RID: 69 RVA: 0x000066DE File Offset: 0x000048DE
	public override string ToString()
	{
		return string.Format("{0}: {1}, {2}", "Attack", this.m_attackAnimation, this.m_attackType);
	}

	// Token: 0x0400005E RID: 94
	[Header("Common")]
	public Attack.AttackType m_attackType;

	// Token: 0x0400005F RID: 95
	public string m_attackAnimation = "";

	// Token: 0x04000060 RID: 96
	public string m_chargeAnimationBool = "";

	// Token: 0x04000061 RID: 97
	public int m_attackRandomAnimations;

	// Token: 0x04000062 RID: 98
	public int m_attackChainLevels;

	// Token: 0x04000063 RID: 99
	public bool m_loopingAttack;

	// Token: 0x04000064 RID: 100
	public bool m_consumeItem;

	// Token: 0x04000065 RID: 101
	public bool m_hitTerrain = true;

	// Token: 0x04000066 RID: 102
	public bool m_hitFriendly;

	// Token: 0x04000067 RID: 103
	public bool m_isHomeItem;

	// Token: 0x04000068 RID: 104
	public float m_attackStamina = 20f;

	// Token: 0x04000069 RID: 105
	public float m_attackEitr;

	// Token: 0x0400006A RID: 106
	public float m_attackHealth;

	// Token: 0x0400006B RID: 107
	[Range(0f, 100f)]
	public float m_attackHealthPercentage;

	// Token: 0x0400006C RID: 108
	public bool m_attackHealthLowBlockUse = true;

	// Token: 0x0400006D RID: 109
	public float m_attackHealthReturnHit;

	// Token: 0x0400006E RID: 110
	public bool m_attackKillsSelf;

	// Token: 0x0400006F RID: 111
	public float m_speedFactor = 0.2f;

	// Token: 0x04000070 RID: 112
	public float m_speedFactorRotation = 0.2f;

	// Token: 0x04000071 RID: 113
	public float m_attackStartNoise = 10f;

	// Token: 0x04000072 RID: 114
	public float m_attackHitNoise = 30f;

	// Token: 0x04000073 RID: 115
	public float m_damageMultiplier = 1f;

	// Token: 0x04000074 RID: 116
	[global::Tooltip("For each missing health point, increase damage this much.")]
	public float m_damageMultiplierPerMissingHP;

	// Token: 0x04000075 RID: 117
	[global::Tooltip("At 100% missing HP the damage will increase by this much, and gradually inbetween.")]
	public float m_damageMultiplierByTotalHealthMissing;

	// Token: 0x04000076 RID: 118
	[global::Tooltip("For each missing health point, return one stamina point.")]
	public float m_staminaReturnPerMissingHP;

	// Token: 0x04000077 RID: 119
	public float m_forceMultiplier = 1f;

	// Token: 0x04000078 RID: 120
	public float m_staggerMultiplier = 1f;

	// Token: 0x04000079 RID: 121
	public float m_recoilPushback;

	// Token: 0x0400007A RID: 122
	public int m_selfDamage;

	// Token: 0x0400007B RID: 123
	[Header("Misc")]
	public string m_attackOriginJoint = "";

	// Token: 0x0400007C RID: 124
	public float m_attackRange = 1.5f;

	// Token: 0x0400007D RID: 125
	public float m_attackHeight = 0.6f;

	// Token: 0x0400007E RID: 126
	public float m_attackOffset;

	// Token: 0x0400007F RID: 127
	public GameObject m_spawnOnTrigger;

	// Token: 0x04000080 RID: 128
	public bool m_toggleFlying;

	// Token: 0x04000081 RID: 129
	public bool m_attach;

	// Token: 0x04000082 RID: 130
	public bool m_cantUseInDungeon;

	// Token: 0x04000083 RID: 131
	[Header("Loading")]
	public bool m_requiresReload;

	// Token: 0x04000084 RID: 132
	public string m_reloadAnimation = "";

	// Token: 0x04000085 RID: 133
	public float m_reloadTime = 2f;

	// Token: 0x04000086 RID: 134
	public float m_reloadStaminaDrain;

	// Token: 0x04000087 RID: 135
	public float m_reloadEitrDrain;

	// Token: 0x04000088 RID: 136
	[Header("Draw")]
	public bool m_bowDraw;

	// Token: 0x04000089 RID: 137
	public float m_drawDurationMin;

	// Token: 0x0400008A RID: 138
	public float m_drawStaminaDrain;

	// Token: 0x0400008B RID: 139
	public float m_drawEitrDrain;

	// Token: 0x0400008C RID: 140
	public string m_drawAnimationState = "";

	// Token: 0x0400008D RID: 141
	public AnimationCurve m_drawVelocityCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x0400008E RID: 142
	[Header("Melee/AOE")]
	public float m_attackAngle = 90f;

	// Token: 0x0400008F RID: 143
	public float m_attackRayWidth;

	// Token: 0x04000090 RID: 144
	public float m_maxYAngle;

	// Token: 0x04000091 RID: 145
	public bool m_lowerDamagePerHit = true;

	// Token: 0x04000092 RID: 146
	public Attack.HitPointType m_hitPointtype;

	// Token: 0x04000093 RID: 147
	public bool m_hitThroughWalls;

	// Token: 0x04000094 RID: 148
	public bool m_multiHit = true;

	// Token: 0x04000095 RID: 149
	public bool m_pickaxeSpecial;

	// Token: 0x04000096 RID: 150
	public float m_lastChainDamageMultiplier = 2f;

	// Token: 0x04000097 RID: 151
	[BitMask(typeof(DestructibleType))]
	public DestructibleType m_resetChainIfHit;

	// Token: 0x04000098 RID: 152
	[Header("Spawn on hit")]
	public GameObject m_spawnOnHit;

	// Token: 0x04000099 RID: 153
	public float m_spawnOnHitChance;

	// Token: 0x0400009A RID: 154
	[Header("Skill settings")]
	public float m_raiseSkillAmount = 1f;

	// Token: 0x0400009B RID: 155
	[BitMask(typeof(DestructibleType))]
	public DestructibleType m_skillHitType = DestructibleType.Character;

	// Token: 0x0400009C RID: 156
	public Skills.SkillType m_specialHitSkill;

	// Token: 0x0400009D RID: 157
	[BitMask(typeof(DestructibleType))]
	public DestructibleType m_specialHitType;

	// Token: 0x0400009E RID: 158
	[Header("Projectile")]
	public GameObject m_attackProjectile;

	// Token: 0x0400009F RID: 159
	public float m_projectileVel = 10f;

	// Token: 0x040000A0 RID: 160
	public float m_projectileVelMin = 2f;

	// Token: 0x040000A1 RID: 161
	[global::Tooltip("When not using Draw, randomize velocity between Velocity and Velocity Min")]
	public bool m_randomVelocity;

	// Token: 0x040000A2 RID: 162
	public float m_projectileAccuracy = 10f;

	// Token: 0x040000A3 RID: 163
	public float m_projectileAccuracyMin = 20f;

	// Token: 0x040000A4 RID: 164
	public bool m_circularProjectileLaunch;

	// Token: 0x040000A5 RID: 165
	public bool m_distributeProjectilesAroundCircle;

	// Token: 0x040000A6 RID: 166
	public bool m_skillAccuracy;

	// Token: 0x040000A7 RID: 167
	public bool m_useCharacterFacing;

	// Token: 0x040000A8 RID: 168
	public bool m_useCharacterFacingYAim;

	// Token: 0x040000A9 RID: 169
	[FormerlySerializedAs("m_useCharacterFacingAngle")]
	public float m_launchAngle;

	// Token: 0x040000AA RID: 170
	public int m_projectiles = 1;

	// Token: 0x040000AB RID: 171
	public int m_projectileBursts = 1;

	// Token: 0x040000AC RID: 172
	public float m_burstInterval;

	// Token: 0x040000AD RID: 173
	public bool m_destroyPreviousProjectile;

	// Token: 0x040000AE RID: 174
	public bool m_perBurstResourceUsage;

	// Token: 0x040000AF RID: 175
	[Header("Harvest")]
	public bool m_harvest;

	// Token: 0x040000B0 RID: 176
	public float m_harvestRadius;

	// Token: 0x040000B1 RID: 177
	public float m_harvestRadiusMaxLevel;

	// Token: 0x040000B2 RID: 178
	private static readonly Collider[] s_pieceColliders = new Collider[200];

	// Token: 0x040000B3 RID: 179
	[Header("Attack-Effects")]
	public EffectList m_hitEffect = new EffectList();

	// Token: 0x040000B4 RID: 180
	public EffectList m_hitTerrainEffect = new EffectList();

	// Token: 0x040000B5 RID: 181
	public EffectList m_startEffect = new EffectList();

	// Token: 0x040000B6 RID: 182
	public EffectList m_triggerEffect = new EffectList();

	// Token: 0x040000B7 RID: 183
	public EffectList m_trailStartEffect = new EffectList();

	// Token: 0x040000B8 RID: 184
	public EffectList m_burstEffect = new EffectList();

	// Token: 0x040000B9 RID: 185
	protected static int m_attackMask = 0;

	// Token: 0x040000BA RID: 186
	protected static int m_attackMaskTerrain = 0;

	// Token: 0x040000BB RID: 187
	protected static int m_harvestRayMask = 0;

	// Token: 0x040000BC RID: 188
	private Humanoid m_character;

	// Token: 0x040000BD RID: 189
	private BaseAI m_baseAI;

	// Token: 0x040000BE RID: 190
	private Rigidbody m_body;

	// Token: 0x040000BF RID: 191
	private ZSyncAnimation m_zanim;

	// Token: 0x040000C0 RID: 192
	private CharacterAnimEvent m_animEvent;

	// Token: 0x040000C1 RID: 193
	[NonSerialized]
	private ItemDrop.ItemData m_weapon;

	// Token: 0x040000C2 RID: 194
	private VisEquipment m_visEquipment;

	// Token: 0x040000C3 RID: 195
	[NonSerialized]
	private ItemDrop.ItemData m_lastUsedAmmo;

	// Token: 0x040000C4 RID: 196
	private float m_attackDrawPercentage;

	// Token: 0x040000C5 RID: 197
	private const float m_freezeFrameDuration = 0.15f;

	// Token: 0x040000C6 RID: 198
	private const float m_chainAttackMaxTime = 0.2f;

	// Token: 0x040000C7 RID: 199
	private int m_nextAttackChainLevel;

	// Token: 0x040000C8 RID: 200
	private int m_currentAttackCainLevel;

	// Token: 0x040000C9 RID: 201
	private bool m_wasInAttack;

	// Token: 0x040000CA RID: 202
	private float m_time;

	// Token: 0x040000CB RID: 203
	private bool m_abortAttack;

	// Token: 0x040000CC RID: 204
	private bool m_attackTowardsCameraDir = true;

	// Token: 0x040000CD RID: 205
	private bool m_projectileAttackStarted;

	// Token: 0x040000CE RID: 206
	private float m_projectileFireTimer = -1f;

	// Token: 0x040000CF RID: 207
	private int m_projectileBurstsFired;

	// Token: 0x040000D0 RID: 208
	[NonSerialized]
	private ItemDrop.ItemData m_ammoItem;

	// Token: 0x040000D1 RID: 209
	private bool m_attackDone;

	// Token: 0x040000D2 RID: 210
	private bool m_isAttached;

	// Token: 0x040000D3 RID: 211
	private Transform m_attachTarget;

	// Token: 0x040000D4 RID: 212
	private Vector3 m_attachOffset;

	// Token: 0x040000D5 RID: 213
	private float m_attachDistance;

	// Token: 0x040000D6 RID: 214
	private Vector3 m_attachHitPoint;

	// Token: 0x040000D7 RID: 215
	private float m_detachTimer;

	// Token: 0x02000220 RID: 544
	private class HitPoint
	{
		// Token: 0x04001F0E RID: 7950
		public GameObject go;

		// Token: 0x04001F0F RID: 7951
		public Vector3 avgPoint = Vector3.zero;

		// Token: 0x04001F10 RID: 7952
		public int count;

		// Token: 0x04001F11 RID: 7953
		public Vector3 firstPoint;

		// Token: 0x04001F12 RID: 7954
		public Collider collider;

		// Token: 0x04001F13 RID: 7955
		public Dictionary<Collider, Vector3> allHits = new Dictionary<Collider, Vector3>();

		// Token: 0x04001F14 RID: 7956
		public Vector3 closestPoint;

		// Token: 0x04001F15 RID: 7957
		public float closestDistance = 999999f;
	}

	// Token: 0x02000221 RID: 545
	public enum AttackType
	{
		// Token: 0x04001F17 RID: 7959
		Horizontal,
		// Token: 0x04001F18 RID: 7960
		Vertical,
		// Token: 0x04001F19 RID: 7961
		Projectile,
		// Token: 0x04001F1A RID: 7962
		None,
		// Token: 0x04001F1B RID: 7963
		Area,
		// Token: 0x04001F1C RID: 7964
		TriggerProjectile
	}

	// Token: 0x02000222 RID: 546
	public enum HitPointType
	{
		// Token: 0x04001F1E RID: 7966
		Closest,
		// Token: 0x04001F1F RID: 7967
		Average,
		// Token: 0x04001F20 RID: 7968
		First
	}
}
