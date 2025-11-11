using System;
using Server;

namespace Server.Items
{
    public class Artifact_TheShadowWall : GiftRoyalShield
	{
		public override int InitMinHits{ get{ return 250; } }
		public override int InitMaxHits{ get{ return 255; } }

        public override int BasePhysicalResistance{ get{ return 10; } }
        public override int BaseColdResistance{ get{ return 10; } }
        public override int BaseFireResistance{ get{ return 10; } }
        public override int BaseEnergyResistance{ get{ return 10; } }
        public override int BasePoisonResistance{ get{ return 10; } }

        [Constructable]
        public Artifact_TheShadowWall()
        {
            Name = "The Shadow Wall";
            Hue = 0x2FBA;
            StrRequirement = 125;
            Attributes.BonusDex = 5;
			Attributes.AttackChance = 5;
            ArmorAttributes.SelfRepair = 5;
            Attributes.SpellDamage = 20;
            Attributes.SpellChanneling = 1;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, 10, "" );
		}

        public Artifact_TheShadowWall(Serial serial) : base( serial )
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
