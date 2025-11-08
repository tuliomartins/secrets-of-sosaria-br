using System;
using Server;

namespace Server.Items
{
	public class Artifact_Nimbus : GiftAssassinSpike
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override bool CanFortify{ get{ return false; } }

		[Constructable]
		public Artifact_Nimbus()
		{
			Name = "Nimbus";
			Hue = 0xB54;

			WeaponAttributes.HitLightning = 100;
			AosElementDamages.Energy = 100;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, 10, "" );
		}

		public Artifact_Nimbus( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			ArtifactLevel = 2;

			int version = reader.ReadInt();
		}
	}
}
