using System;
using Server;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;
using Server.Mobiles;
using Server.Commands;
using Server.Commands.Generic;

namespace Server.Mobiles
{
	[CorpseName( "WeeklyBoss's corpse" )]
	public class WeeklyBoss : BaseCreature
	{
		[Constructable]
		public WeeklyBoss () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "TEST";
			Title = "WEEKLY BOSS";

			Body = 716;
			BaseSoundID = 362;
			Resource = CraftResource.RedScales;

			SetStr( 896, 985 );
			SetDex( 86, 175 );
			SetInt( 586, 675 );

			SetHits( 558, 611 );

			SetDamage( 23, 30 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Fire, 25 );

			SetResistance( ResistanceType.Physical, 65, 75 );
			SetResistance( ResistanceType.Fire, 80, 90 );
			SetResistance( ResistanceType.Cold, 60, 70 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 60, 70 );

			SetSkill( SkillName.Psychology, 80.1, 100.0 );
			SetSkill( SkillName.Magery, 80.1, 100.0 );
			SetSkill( SkillName.Meditation, 52.5, 75.0 );
			SetSkill( SkillName.MagicResist, 100.5, 150.0 );
			SetSkill( SkillName.Tactics, 97.6, 100.0 );
			SetSkill( SkillName.FistFighting, 97.6, 100.0 );

			Fame = 20000;
			Karma = -20000;

			VirtualArmor = 60;
		}

		public override bool AutoDispel{ get{ return !Controlled; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override int Meat{ get{ return 22; } }
		public override int Hides{ get{ return 25; } }
		public override HideType HideType{ get{ return HideType.Draconic; } }
		public override int Scales{ get{ return 8; } }
		public override ScaleType ScaleType{ get{ return ResourceScales(); } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override bool CanAngerOnTame { get { return true; } }
		public override int Skin{ get{ return Utility.Random(5); } }
		public override SkinType SkinType{ get{ return SkinType.Dragon; } }
		public override int Skeletal{ get{ return Utility.Random(5); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Draco; } }


		public override int BreathPhysicalDamage{ get{ return 0; } }
		public override int BreathFireDamage{ get{ return 0; } }
		public override int BreathColdDamage{ get{ return 0; } }
		public override int BreathPoisonDamage{ get{ return 100; } }
		public override int BreathEnergyDamage{ get{ return 0; } }
		public override int BreathEffectHue{ get{ return 0x3F; } }
		public override int BreathEffectSound{ get{ return 0x658; } }
		public override bool ReacquireOnMovement{ get{ return !Controlled; } }
		public override bool HasBreath{ get{ return true; } }
		public override double BreathEffectDelay{ get{ return 0.1; } }
		public override void BreathDealDamage( Mobile target, int form ){ base.BreathDealDamage( target, 18 ); }

		int CanDie = 0;

		public void SpawnCreature( Mobile target )
		{
			Map map = this.Map;

			if ( map == null )
				return;

			int monsters = 0;

			foreach ( Mobile m in this.GetMobilesInRange( 4 ) )
			{
				if ( m is YoungDragon || m is BabyDragon )
					++monsters;
			}

			if ( monsters < 6 )
			{
				PlaySound( 0x216 );

				int newmonsters = Utility.RandomMinMax( 1, 3 );

				for ( int i = 0; i < newmonsters; ++i )
				{
					BaseCreature monster;

					switch ( Utility.Random( 2 ) )
					{
						default:
						case 0: monster = new FireWyrmling(); break;
						case 1: monster = new BabyDragon(); break;
					}

					monster.Team = this.Team;

					bool validLocation = false;
					Point3D loc = this.Location;

					for ( int j = 0; !validLocation && j < 10; ++j )
					{
						int x = X + Utility.Random( 3 ) - 1;
						int y = Y + Utility.Random( 3 ) - 1;
						int z = map.GetAverageZ( x, y );

						if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
							loc = new Point3D( x, y, Z );
						else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
							loc = new Point3D( x, y, z );
					}

					monster.IsTempEnemy = true;
					monster.MoveToWorld( loc, map );
					monster.Combatant = target;
				}
			}
		}

		public void SummonBabyDragon( Mobile target )
		{
			if ( target == null || target.Deleted ) //sanity
				return;

			if ( 0.5 >= Utility.RandomDouble() ) // 5% chance
				SpawnCreature( target );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );
			//DoSpecialAbility( attacker );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );
			SummonBabyDragon( defender );
		}

		public WeeklyBoss( Serial serial ) : base( serial )
		{
		}

		public override bool OnBeforeDeath()
		{
			

			if ( CanDie == 0 )
			{
				Say("Fool! You think I can be slain to easily?");
				this.Hits = this.HitsMax/2;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				CanDie = 1;
				return false;
			}
			else 
			{
				Effects.SendLocationParticles( EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
				this.PlaySound( 0x1FE );
				Say("I will return!");
			}


			return base.OnBeforeDeath();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
		
	}
}